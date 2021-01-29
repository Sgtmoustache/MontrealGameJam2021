using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collecting : Interactable {

    [SerializeField] int objectID;
    
    public override void Interact(GameObject player){
        if(!(Inventory.HasObject())){
            Debug.Log("PLAYER GRAB");
            photonView.RPC("SetLock", RpcTarget.All, true);
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            Inventory.setObject(objectID, this.gameObject);

            this.gameObject.transform.SetParent(player.transform);
            this.gameObject.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
        }
    }
    public override void beInteractable(){
        photonView.RPC("SetLock", RpcTarget.All, false);
    }
}
