using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Interactable: MonoBehaviour{

    public KeyCode key =  KeyCode.E;
    public void Start()
    {
        SetKey();
    }
    public abstract void SetKey();
    public abstract void Interact(GameObject player);

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(key)){
            Interact(other.gameObject);
        }
    }
}


