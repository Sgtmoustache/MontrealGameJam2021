using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlaceHolder : Interactable
{
    public Collectibles itemType;
    [SerializeField] Transform itemDropPosition;
    

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
        if(inventory){
            if(canBePlace && inventory.HasItem() && player.layer == 6 && ((itemType != Collectibles.None) ? (inventory.GetTypeOfItem() == itemType) : true)){
                canBePlace = false;
                storeItem = inventory.GetItemGameObject();

                storeItem.transform.SetParent(this.gameObject.transform);
                storeItem.transform.position = itemDropPosition.position;
                storeItem.transform.rotation = itemDropPosition.rotation;
                Collecting collect = storeItem.GetComponent<Collecting>();
                collect.Disable();
                inventory.ClearItem();
                TextMeshProUGUI Description = player.GetComponent<PlayerInfo>().Display;
                Description.SetText("Take");

                StartCoroutine(bufferPlace());

            }
            else if(canBePick){
                Collecting collect = storeItem.GetComponent<Collecting>();
                collect.Enable();
                collect.Interact(player);

                storeItem = null;
                canBePick = false;

                StartCoroutine(bufferGrab());
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if(player.gameObject.GetComponent<PlayerInfo>()?.PlayerType == "Student"){
            if(inventory){
                TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
                if(storeItem)
                    Description.SetText("Take");
                else
                    Description.SetText("Hide");
            }
        }
    }

    private void OnTriggerExit(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if(player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student"){
            if(inventory){
                TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
                Description.SetText("");
            }
        }
    }
    public override void beInteractable(){
        canBePlace = true;
    }

    public void addItem(GameObject item){
        canBePlace = false;
        canBePick = true;
        storeItem = item;
        Collecting collect = storeItem.GetComponent<Collecting>();
        collect.Disable();
    }

     public void removeItem(){
        canBePlace = true;
        canBePick = false;
        storeItem = null;
    }

    public bool HasItem() => storeItem != null;
}
