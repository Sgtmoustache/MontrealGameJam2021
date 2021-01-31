using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Collecting : Interactable {

    public override void Interact(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory  && !Locked)
        {
            if (!(inventory.HasItem()))
            {
                bool isPlayer = (player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student");
                base.Interact(player);
                inventory.SetItem(gameObject.GetComponent<ItemInfo>().Collectibles, gameObject);
                Vector3 vec = player.transform.position;
                gameObject.transform.SetParent(PlayerSpawner.LocalPlayer.transform);
                
                photonView.RPC("SetRigibodyConstraint", RpcTarget.All, RigidbodyConstraints.FreezeAll);
                
                gameObject.transform.position = new Vector3(vec.x, (vec.y + (isPlayer? 2.5f : 3.5f)), vec.z);

                TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
                    Description.SetText("");
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
    }

    public void Disable() {
        photonView.RPC("SetLock", RpcTarget.All, true);
        photonView.RPC("SetRigibodyConstraint", RpcTarget.All, RigidbodyConstraints.None);
    }

    public void Enable() => photonView.RPC("SetLock", RpcTarget.All, false);



    private void OnTriggerEnter(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory || Locked) return;

        TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
        Description.SetText("Place");
    }

    private void OnTriggerExit(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory || Locked) return;    
        TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
        Description.SetText("");
    }
}
