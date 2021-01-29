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
    [SerializeField] private int numberOfBots = 50;
    public void Start()
    {
        SpawnPlayers();
        SpawnBots();
    }

    private void SpawnPlayers()
    {
        Object player;
        
        if (PhotonNetwork.IsConnected && photonView.IsMine)
            player = PhotonNetwork.Instantiate("Prefabs/PlayerWithLight", new Vector3(0, 0, 0), Quaternion.identity);
        else
            player = Instantiate(Resources.Load("Prefabs/PlayerWithLight"));

        ((GameObject) player).GetComponent<PlayerMovement>().Camera = MainCamera;
    }

    private void SpawnBots()
    {
        Object bot;
        
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i <= numberOfBots; i++)
            {
                bot = PhotonNetwork.Instantiate("Prefabs/BotWithLight", new Vector3(0, 0, 0), Quaternion.identity);
                ((GameObject) bot).GetComponent<BotMovement>()._bounds = MapBounds;
            }
        }
        else if (!PhotonNetwork.IsConnected)
        {
            for (int i = 0; i <= numberOfBots; i++)
            {
                bot = Instantiate(Resources.Load("Prefabs/BotWithVisibility"));
                ((GameObject) bot).GetComponent<BotMovement>()._bounds = MapBounds;            
            }
        }
    }
}