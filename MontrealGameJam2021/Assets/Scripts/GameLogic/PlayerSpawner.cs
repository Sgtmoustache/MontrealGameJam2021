using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] private Bounds MapBounds;
    [SerializeField] private int numberOfBots = 50;
    [HideInInspector] public CinemachineVirtualCamera camera;
    
    [SerializeField] private List<Transform> botsSpawns = new List<Transform>();
    [SerializeField] private List<Transform> playerSpawns = new List<Transform>();

    public static GameObject LocalPlayer;

    [PunRPC]
    public void SpawnPlayers()
    {
        Debug.LogWarning("Spawning player");

        if(PhotonNetwork.IsMasterClient)
            photonView.RPC("SpawnPlayers", RpcTarget.Others);
        
        int randomValue = Random.Range(0, playerSpawns.Count-1);

        if (PhotonNetwork.IsConnected)
            LocalPlayer = PhotonNetwork.Instantiate("Prefabs/PlayerWithLight", playerSpawns[randomValue].position, playerSpawns[randomValue].rotation);
        else
            LocalPlayer = (GameObject) Instantiate(Resources.Load("Prefabs/PlayerWithLight"), playerSpawns[randomValue].position, playerSpawns[randomValue].rotation);
        
        
        LocalPlayer.GetComponent<PlayerMovement>().Camera = GameManager.CameraPosition;
        LocalPlayer.gameObject.layer = 6;
        LocalPlayer.GetComponentInChildren<Light>().enabled = true;
        LocalPlayer.GetComponent<VisibilityHandler>().enabled = false;
        LocalPlayer.gameObject.tag = "Player";
        
        camera.Follow = LocalPlayer.transform;
    }

    [PunRPC]
    public void RespawnPlayer(Vector3 forceLocation)
    {
        if (forceLocation == Vector3.zero)
        {
            Debug.LogWarning("Respawning player");
            int randomValue = Random.Range(0, playerSpawns.Count-1);
        
            LocalPlayer.transform.position = playerSpawns[randomValue].position;
            LocalPlayer.transform.rotation = playerSpawns[randomValue].rotation;
        }
        else
        {
            Debug.LogWarning("Going to specific location");
            LocalPlayer.transform.position = forceLocation;
        }
        
        LocalPlayer.GetComponent<Inventory>().ClearItem();
    }

    public void SpawnBots()
    {
        Debug.LogWarning("Spawning Bots");
        
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