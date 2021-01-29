using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class placeObject : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField] int objectID;
    [SerializeField] Text pickupText;

    public bool canBePlace = true;
    public bool canBePick = true;
    public void OnInteract(GameObject player){
        Debug.Log("test");
        if(Inventory.HasObject() && Inventory.getObjectValue() == objectID){
            Debug.Log("PLACE ITEM");

            GameObject obj = Inventory.getItem();
            obj.transform.SetParent(this.transform);
            obj.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);

            Inventory.setItem(null);
            Inventory.setObjectId(0);

            canBePick = true;
            canBePlace = false;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {   
        if(canBePlace){
            Debug.Log("Can place object");
            InteractionManager manager = collider.GetComponent<InteractionManager>();
            manager.pickup(true, this.gameObject);
        }
        if(canBePick){
            Debug.Log("Can place pick");
            GameObject obj = this.gameObject.transform.GetChild(1).gameObject;
            canBePlace = true;
            canBePick = false;
            InteractionManager manager = collider.GetComponent<InteractionManager>();
            manager.pickup(true, this.gameObject);
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        Debug.Log("You LEAVE the paceHolder");
        InteractionManager manager = collider.GetComponent<InteractionManager>();
        manager.pickup(false, null);
    }

    public void disable(){
        canBePlace = true;
    }
}
