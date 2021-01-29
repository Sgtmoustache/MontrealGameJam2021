using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Bounds MapBounds;
    [SerializeField] private int numberOfBots = 50;
    [SerializeField] private CinemachineVirtualCamera camera;
    
    [SerializeField] private List<Transform> botsSpawns = new List<Transform>();
    [SerializeField] private List<Transform> playerSpawns = new List<Transform>();

    public static GameObject LocalPlayer;
    
    public void Start()
    {
        SpawnPlayers();
        SpawnBots();

        camera.Follow = LocalPlayer.transform;
    }
    
    private void SpawnPlayers()
    {
        if (PhotonNetwork.IsConnected)
            LocalPlayer = PhotonNetwork.Instantiate("Prefabs/PlayerWithLight", playerSpawns[photonView.ViewID-1].position, botsSpawns[photonView.ViewID-1].rotation);
        else
            LocalPlayer = (GameObject) Instantiate(Resources.Load("Prefabs/PlayerWithLight"), playerSpawns[photonView.ViewID-1].position, botsSpawns[photonView.ViewID-1].rotation);

        Debug.LogWarning("Spawning player!");
        
        LocalPlayer.GetComponent<PlayerMovement>().Camera = MainCamera;
        LocalPlayer.gameObject.layer = 6;
        LocalPlayer.GetComponentInChildren<Light>().enabled = true;
        LocalPlayer.GetComponent<VisibilityHandler>().enabled = false;
        LocalPlayer.gameObject.tag = "Player";
    }

    private void SpawnBots()
    {
        List<GameObject> bots = new List<GameObject>();
        
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add(PhotonNetwork.Instantiate("Prefabs/BotWithVisibility",  botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }
        else if (!PhotonNetwork.IsConnected)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add((GameObject) Instantiate(Resources.Load("Prefabs/BotWithVisibility"), botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }

        foreach (GameObject bot in bots)
        {
            bot.GetComponent<BotMovement>()._bounds = MapBounds;
        }
    }
}