using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public abstract class Interactable: MonoBehaviourPun{

    public abstract void beInteractable();
    public abstract void Interact(GameObject player);


    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E)){
            Interact(other.gameObject);
        }
    }

}


