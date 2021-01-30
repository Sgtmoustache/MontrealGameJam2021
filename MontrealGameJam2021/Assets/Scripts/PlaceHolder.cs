using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : Interactable
{
    [SerializeField] Collectibles objectType;

    public bool canBePlace = true;   
    public bool canBePick = false;  
    GameObject storeItem = null;

    public IEnumerator bufferPlace(){
        canBePick = false;
        yield return new WaitForSeconds(2);
        canBePick = true;
    }
    public IEnumerator bufferGrab(){
        canBePlace = false;
        yield return new WaitForSeconds(2);
        canBePlace = true;
    }

    public override void Interact(GameObject player){
        Inventory inventory = player.GetComponent<Inventory>();
        if(canBePlace && inventory.HasItem() && player.layer == 6 && inventory.GetTypeOfItem() == objectType){
            Debug.Log("PLAYER PLACE");
            storeItem = inventory.GetItemGameObject();
            
            storeItem.transform.SetParent(this.gameObject.transform);
            storeItem.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);

            inventory.ClearItem();
            canBePlace = false;
            
            StartCoroutine(bufferPlace());

        }
        else if(canBePick){
            Debug.Log("PLAYER GRAB FROM TABLE");
            inventory.SetItem(objectType, storeItem);
            
            storeItem.transform.SetParent(player.transform);
            storeItem.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
            storeItem = null;
            canBePick = false;

            StartCoroutine(bufferGrab());
        }
    }
    public override void beInteractable(){
        canBePlace = true;
    }
}
