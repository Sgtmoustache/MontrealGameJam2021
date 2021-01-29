using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collecting : Interactable {

    [SerializeField] int objectID;

    public bool canBePick = true;
    
    public override void Interact(GameObject player){
        if(canBePick && !(Inventory.HasObject())){
            Debug.Log("PLAYER GRAB");
            GetComponent<PhotonView>().RequestOwnership();
            Inventory.setObject(objectID, this.gameObject);
            
            this.gameObject.transform.SetParent(player.transform);
            this.gameObject.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);

            canBePick = false;
        }
    }
    public override void beInteractable(){
        canBePick = true;
    }
}
