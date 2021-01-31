using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropping : MonoBehaviour
{


    [SerializeField] Transform arrow;
    [SerializeField] Canvas FloatingText;

    private PlayerInfo playerInfo;

    private void Start()
    {
        FloatingText.gameObject.SetActive(true);
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
                targetPosition.y = arrow.transform.position.y;
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
        else{
            arrow.gameObject.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log(GameManager.StudentScore);
            Debug.Log(GameManager.TeacherScore);
        }

    }
}
