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
                Vector3 vec = player.transform.position;
                gameObject.transform.SetParent(PlayerSpawner.LocalPlayer.transform);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                gameObject.transform.position = new Vector3(vec.x, 8.0f, vec.z);
                if(player.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore -= 20 ; 
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
