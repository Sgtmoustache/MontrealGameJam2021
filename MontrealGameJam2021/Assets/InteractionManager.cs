using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    private bool canPickObject = false;
    private GameObject objectKey = null; 

    private void Update(){
        if(canPickObject){
            if (Input.GetKeyDown(KeyCode.E)){
                if(objectKey){
                    IInteractable collect = objectKey.GetComponent<IInteractable>();
                    collect.OnInteract(this.gameObject);
                    canPickObject = false;
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Q)){//Drop
            if(Inventory.HasObject()){
                Debug.Log("DROP");  
                
                IInteractable collect = Inventory.getItem().GetComponent<IInteractable>();
                collect.disable();

                objectKey = null; 
                canPickObject = false;  
                
            }
        }
    }

    public void pickup(bool canIt, GameObject ob){
        canPickObject = canIt;
        objectKey = ob;
    }

}
