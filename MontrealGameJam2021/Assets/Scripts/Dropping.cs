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
        Inventory inventory = this.GetComponent<Inventory>();
        if(inventory.HasObject()){
            if (Input.GetKeyDown(KeyCode.Q)){
                GameObject obj = inventory.getItem();
                Vector3 vec = this.gameObject.transform.localPosition;

                Collecting collect = obj.GetComponent<Collecting>();
                collect.beInteractable();
                obj.transform.SetParent(null);
                obj.transform.localPosition = new Vector3(vec.x, 2.0f, vec.z);
                inventory.setObject(0, null);
            }
        }
    }
}
