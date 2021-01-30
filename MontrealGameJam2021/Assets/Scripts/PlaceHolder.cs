using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlaceHolder : Interactable
{
    [SerializeField] Collectibles itemType;
    [SerializeField] Transform itemDropPosition;

    [SerializeField] bool hidingSpot;
    [SerializeField] bool lostAndFound;
    

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
            Debug.Log(inventory.GetTypeOfItem().ToString());
            if(canBePlace && inventory.HasItem() && player.layer == 6 && ((itemType != Collectibles.None) ? (inventory.GetTypeOfItem() == itemType) : true)){
                canBePlace = false;
                Debug.Log("PLAYER PLACE");
                storeItem = inventory.GetItemGameObject();

                storeItem.transform.SetParent(this.gameObject.transform);
                storeItem.transform.position = itemDropPosition.position;
                storeItem.transform.rotation = itemDropPosition.rotation;
                Collecting collect = storeItem.GetComponent<Collecting>();
                collect.Disable();
                inventory.ClearItem();
                if(player.GetComponent<PlayerInfo>().PlayerType == "Student"){
                    TextMeshProUGUI Description = player.GetComponent<PlayerInfo>().Display;
                    Description.SetText("Take");

                    if(hidingSpot)
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
                

                StartCoroutine(bufferPlace());

                

            }
            else if(canBePick){
                Debug.Log("PLAYER GRAB FROM TABLE");
                Collecting collect = storeItem.GetComponent<Collecting>();
                collect.Enable();
                collect.Interact(player);
                if(player.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore += 20 ; 
                if(hidingSpot)
                    GameManager.StudentScore -= 20 ;
                else if(lostAndFound)
                    GameManager.StudentScore -= 100 ;
                else if(!lostAndFound)
                    GameManager.TeacherScore -= 100 ;

                storeItem = null;
                canBePick = false;



                StartCoroutine(bufferGrab());
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("Enter");
        Inventory inventory = player.GetComponent<Inventory>();
        if(inventory){
            if(player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student"){
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
        Debug.Log("Leave");
        Inventory inventory = player.GetComponent<Inventory>();
        if(inventory){
            if(player.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student"){
                TextMeshProUGUI Description = player.gameObject.GetComponent<PlayerInfo>().Display;
                Description.SetText("");
            }
        }
    }
    public override void beInteractable(){
        canBePlace = true;
    }
}
