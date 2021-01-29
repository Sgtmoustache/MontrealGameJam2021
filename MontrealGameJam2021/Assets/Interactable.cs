using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Interactable: MonoBehaviour{

    public abstract void beInteractable();
    public abstract void Interact(GameObject player);


    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E)){
            Interact(other.gameObject);
        }
    }

}


