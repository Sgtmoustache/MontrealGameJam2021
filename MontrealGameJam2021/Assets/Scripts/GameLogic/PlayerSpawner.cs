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

    public void SpawnPlayers()
    {
        int randomValue = Random.Range(0, playerSpawns.Count-1);

        if (PhotonNetwork.IsConnected)
            LocalPlayer = PhotonNetwork.Instantiate("Prefabs/Student", playerSpawns[randomValue].position, playerSpawns[randomValue].rotation);
        else
            LocalPlayer = (GameObject) Instantiate(Resources.Load("Prefabs/Student"), playerSpawns[randomValue].position, playerSpawns[randomValue].rotation);

        Debug.LogWarning("Spawning player!");
        
        LocalPlayer.GetComponent<PlayerMovement>().Camera = MainCamera;
        LocalPlayer.gameObject.layer = 6;
        LocalPlayer.GetComponentInChildren<Light>().enabled = true;
        LocalPlayer.GetComponent<VisibilityHandler>().enabled = false;
        LocalPlayer.gameObject.tag = "Player";
        
        camera.Follow = LocalPlayer.transform;
    }

    public void RespawnPlayer(Transform forceLocation = null)
    {
        if (forceLocation == null)
        {
            Debug.LogWarning("Respawning player");
            int randomValue = Random.Range(0, playerSpawns.Count-1);
        
            LocalPlayer.transform.position = playerSpawns[randomValue].position;
            LocalPlayer.transform.rotation = playerSpawns[randomValue].rotation;
        }
        else
        {
            Debug.LogWarning("Going to specific location");
            LocalPlayer.transform.position = forceLocation.position;
            LocalPlayer.transform.rotation = forceLocation.rotation;
        }
    }

    public void SpawnBots()
    {
        List<GameObject> bots = new List<GameObject>();
        
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add(PhotonNetwork.Instantiate("Prefabs/StudentBot",  botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }
        else if (!PhotonNetwork.IsConnected)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add((GameObject) Instantiate(Resources.Load("Prefabs/StudentBot"), botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }

        foreach (GameObject bot in bots)
        {
            bot.GetComponent<BotMovement>()._bounds = MapBounds;
        }
    }
}