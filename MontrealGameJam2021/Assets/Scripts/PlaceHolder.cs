using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : Interactable
{
    [SerializeField] int objectID;

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
        if(canBePlace && Inventory.HasObject() && player.layer == 6 && Inventory.getObjectID() == objectID){
            Debug.Log("PLAYER PLACE");
            storeItem = Inventory.getItem();
            
            storeItem.transform.SetParent(this.gameObject.transform);
            storeItem.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);

            Inventory.setObject(0, null);
            canBePlace = false;
            
            StartCoroutine(bufferPlace());

        }
        else if(canBePick){
            Debug.Log("PLAYER GRAB FROM TABLE");
            Inventory.setObject(objectID, storeItem);
            
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
