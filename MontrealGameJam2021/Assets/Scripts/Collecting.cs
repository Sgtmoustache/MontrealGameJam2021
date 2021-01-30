using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collecting : Interactable {

    [SerializeField] Collectibles itemType;

    public override void Interact(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory)
        {
            if (!(inventory.HasItem()))
            {
                Debug.Log("PLAYER GRAB");
                photonView.RPC("SetLock", RpcTarget.All, true);
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                inventory.SetItem(itemType, gameObject);

                gameObject.transform.SetParent(PlayerSpawner.LocalPlayer.transform);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                gameObject.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
            }
        }
    }

    public override void beInteractable(){
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        photonView.RPC("SetLock", RpcTarget.All, false);
    }
}
