using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
[RequireComponent(typeof(PhotonView))]
public class PlaceHolder : Interactable
{
    public Collectibles itemType;
    [SerializeField] Transform itemDropPosition;
    [SerializeField] bool hidingSpot;
    [SerializeField] bool lostAndFound;
    

    public bool canBePlace = true;   
    public bool canBePick = false;  
    public GameObject storeItem = null;

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
                bool isPlayer = (player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student");

                if(!isPlayer && hidingSpot) return;

                storeItem = inventory.GetItemGameObject();
                inventory.ClearItem();

                photonView.RPC("AddItem", RpcTarget.All, storeItem.name);

                TextMeshProUGUI Description = player.GetComponent<PlayerInfo>().Display;
                

                
                if(hidingSpot)
                    Description.SetText("Search");
                else
                    Description.SetText("Take");

                if(isPlayer){
                    if(hidingSpot)//test
                        GameManager.StudentScore += 20 ;
                    else if(lostAndFound)
                        GameManager.StudentScore += 100 ;
                    else if(!lostAndFound)
                        GameManager.TeacherScore += 100 ;

                }
                else{
                    if(lostAndFound)
                        GameManager.StudentScore += 100 ;
                    else if(!lostAndFound)
                        GameManager.TeacherScore += 100 ;
                }

                

            }
            else if(canBePick){
                Collecting collect = storeItem.GetComponent<Collecting>();
                collect.Enable();
                collect.Interact(player);
                
                 photonView.RPC("RemoveItem", RpcTarget.All);


                if(player.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore += 20 ; 
                if(hidingSpot)
                    GameManager.StudentScore -= 20 ;
                else if(lostAndFound)
                    GameManager.StudentScore -= 100 ;
                else if(!lostAndFound)
                    GameManager.TeacherScore -= 100 ;

                
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory) return;

        TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
        bool isPlayer = (player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student");
        if(storeItem && !inventory.HasItem() && !hidingSpot) 
            Description.SetText("Take");
        else if(!inventory.HasItem() && hidingSpot)
            Description.SetText("Search");
        else if(inventory.HasItem() && isPlayer && hidingSpot)
                Description.SetText("Hide");
        else if(inventory.HasItem() && !hidingSpot)
                Description.SetText("Place");
        
    }

    private void OnTriggerExit(Collider player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (!inventory) return;
        if (player.gameObject.GetComponent<PlayerInfo>().PlayerType != "Student") return;
        
        TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
        Description.SetText("");
    }
    public override void beInteractable(){
        canBePlace = true;
    }
    
    public void BroadcastName(string name)
    {
        photonView.RPC("AddItem", RpcTarget.All, name);
    }

    [PunRPC]
    public void AddItem(string itemName) => AddItem(GameObject.Find(itemName));

    private void AddItem(GameObject item){
        canBePlace = false;
        storeItem = item;

        storeItem.transform.SetParent(this.gameObject.transform);
        storeItem.transform.position = itemDropPosition.position;
        storeItem.transform.rotation = itemDropPosition.rotation;
        Collecting collect = storeItem.GetComponent<Collecting>();
        collect.Disable();
        StartCoroutine(bufferPlace());
    }

    [PunRPC]
     public void RemoveItem(){
        canBePick = false;
        storeItem = null;
        StartCoroutine(bufferGrab());
    }

    public bool HasItem() => storeItem != null;
}
