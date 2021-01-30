using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collecting : Interactable {

    private bool canBePick = true;

    public override void Interact(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory  && canBePick)
        {
            if (!(inventory.HasItem()))
            {
                ;
                base.Interact(player);
                inventory.SetItem(gameObject.GetComponent<ItemInfo>().Collectibles, gameObject);

                gameObject.transform.SetParent(PlayerSpawner.LocalPlayer.transform);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                gameObject.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
            }
        }
    }

    public override void beInteractable(){
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        photonView.RPC("SetLock", RpcTarget.All, false);
        canBePick = true;
    }

    public void Disable() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        photonView.RPC("SetLock", RpcTarget.All, false);
        canBePick = false;
    }

    public void Enable() => canBePick = true;
}
