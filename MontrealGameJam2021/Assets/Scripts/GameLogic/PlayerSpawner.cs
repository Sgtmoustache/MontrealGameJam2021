using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    //Todo : Change to personnal camera
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Bounds MapBounds;
    public void Start()
    {
        SpawnPlayers();
        SpawnBots();
    }

    private void SpawnPlayers()
    {
        Object player;

        if (PhotonNetwork.IsConnected)
            player = PhotonNetwork.Instantiate("Prefabs/Player", new Vector3(0, 0, 0), Quaternion.identity);
        else
            player = Instantiate(Resources.Load("Prefabs/Player"));

        ((GameObject) player).GetComponent<PlayerMovement>().Camera = MainCamera;
    }

    private void SpawnBots()
    {
        Object bot;
        
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i <= 50; i++)
            {
                bot = PhotonNetwork.Instantiate("Prefabs/Bot", new Vector3(0, 0, 0), Quaternion.identity);
                ((GameObject) bot).GetComponent<BotMovement>()._bounds = MapBounds;
            }
        }
        else if (!PhotonNetwork.IsConnected)
        {
            for (int i = 0; i <= 50; i++)
            {
                bot = Instantiate(Resources.Load("Prefabs/Bot"));
                ((GameObject) bot).GetComponent<BotMovement>()._bounds = MapBounds;            
            }
        }
    }
}