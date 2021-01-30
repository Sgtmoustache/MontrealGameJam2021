using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public abstract class Interactable: MonoBehaviourPun{
    public abstract void beInteractable();
    public virtual void Interact(GameObject player){
        photonView.RPC("SetLock", RpcTarget.All, true);
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    protected bool Locked = false;
    
    [PunRPC]
    public void SetLock(bool value)
    {
        Locked = value;
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && !Locked){
            Interact(other.gameObject);
        }
    }

}


