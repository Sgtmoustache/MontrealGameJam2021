using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropping : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
                Debug.Log("Drop Item");
                Collecting collect = obj.GetComponent<Collecting>();
                collect.beInteractable();
                obj.transform.SetParent(null);
                obj.transform.localPosition = new Vector3(vec.x, 2.0f, vec.z);
                if(this.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student")
                    GameManager.TeacherScore += 20 ; 
            }
        }

        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log(GameManager.StudentScore);
            Debug.Log(GameManager.TeacherScore);
        }
    }
}
