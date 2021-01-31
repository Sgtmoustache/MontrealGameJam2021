using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropping : MonoBehaviour
{

    private const float RayHeight = 1.0f;
    private const float Range = 2;

    private bool triggerExit = false;

    Transform arrow;

    private void start(){
        arrow = this.gameObject.transform.Find("ArrowDirection");
    }

    // Update is called once per frame
    void Update()
    {
        Inventory inventory = this.gameObject.GetComponent<Inventory>();
        if(inventory.HasItem()){
            if (Input.GetKeyDown(KeyCode.Q)){
                GameObject obj = inventory.GetItemGameObject();
                inventory.ClearItem();
                Vector3 vec = this.gameObject.transform.localPosition;
                Collecting collect = obj.GetComponent<Collecting>();
                collect.beInteractable();
                obj.transform.SetParent(null);
                obj.transform.localPosition = new Vector3(vec.x, 2.0f, vec.z);
                if(this.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore += 20 ; 
            }
            if(this.gameObject.GetComponent<PlayerInfo>()?.PlayerType == "Student"){
                Vector3 targetPosition = new Vector3(10f, -1f , -26f);
                targetPosition.y = transform.position.y;
                arrow.gameObject.SetActive(true);
                arrow.LookAt(targetPosition);
            }
            else{
                Vector3 targetPosition = new Vector3(-19f, 0f , 0f);
                targetPosition.y = transform.position.y;
                arrow.gameObject.SetActive(true);
                arrow.LookAt(targetPosition);
            }
        }
        else
            arrow.gameObject.SetActive(false);

        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log(GameManager.StudentScore);
            Debug.Log(GameManager.TeacherScore);
        }

        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, RayHeight, 0);
        Vector3 direction = this.gameObject.transform.forward;
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin, direction, out hit, Range, LayerMask.GetMask("Water")))
        {
            PlaceHolder placeHolder = hit.collider.gameObject.GetComponent<PlaceHolder>();
            TextMeshProUGUI Description = this.gameObject.GetComponent<PlayerInfo>().Display;
            if(!(placeHolder.HasItem() && inventory.HasItem())){
                if(placeHolder.HasItem())
                    Description.SetText("Take");
                else if(inventory.HasItem())
                    Description.SetText("Place");

                if (Input.GetKeyDown(KeyCode.E)){
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.Log("RaycastTest");
                    Interactable interact = hit.collider.gameObject.GetComponent<Interactable>();
                    interact.Interact(this.gameObject);
                }
            }
            else{
                Description.SetText("");
            }
            triggerExit = true;
        }
        else{
            if(triggerExit){
                triggerExit = false;
                TextMeshProUGUI Description = this.gameObject.GetComponent<PlayerInfo>().Display;
                Description.SetText("");
            }
        }
    }
}
