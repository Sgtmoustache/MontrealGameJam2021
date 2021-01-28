using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    //Todo : Change to personnal camera
    [SerializeField] private Transform MainCamera;
    
    public void Start()
    {
        Object player;
        
        if (PhotonNetwork.IsConnected && photonView.IsMine)
            player = PhotonNetwork.Instantiate("Prefabs/Player", new Vector3(0, 0, 0), Quaternion.identity);
        else
            player = Instantiate(Resources.Load("Prefabs/Player"));

        ((GameObject) player).GetComponent<PlayerMovement>().Camera = MainCamera;
    }
}
