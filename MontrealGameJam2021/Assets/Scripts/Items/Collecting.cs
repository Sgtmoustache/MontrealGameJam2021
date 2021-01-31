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
                base.Interact(player);
                inventory.SetItem(gameObject.GetComponent<ItemInfo>().Collectibles, gameObject);
                Vector3 vec = player.transform.position;
                gameObject.transform.SetParent(PlayerSpawner.LocalPlayer.transform);
                
                photonView.RPC("SetRigibodyConstraint", RpcTarget.All, RigidbodyConstraints.FreezeAll);
                
                gameObject.transform.position = new Vector3(vec.x, (vec.y + 2.2f), vec.z);
                if(player.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore -= 20 ; 
            }
        }
    }

    [PunRPC]
    public void SetRigibodyConstraint(RigidbodyConstraints constraints)
    {
        gameObject.GetComponent<Rigidbody>().constraints = constraints;
    }

    public override void beInteractable(){
        photonView.RPC("SetLock", RpcTarget.All, false);
        photonView.RPC("SetRigibodyConstraint", RpcTarget.All, RigidbodyConstraints.None);
        canBePick = true;
    }

    public void Disable() {
        photonView.RPC("SetLock", RpcTarget.All, false);
        photonView.RPC("SetRigibodyConstraint", RpcTarget.All, RigidbodyConstraints.None);
        canBePick = false;
    }

    public void Enable() => canBePick = true;
}
