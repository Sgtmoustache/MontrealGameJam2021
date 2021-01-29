using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : Interactable {

    [SerializeField] int objectID;
    public Text pickupText;
    
    public override void SetKey()
    {
        key = KeyCode.Q;
    }
    public override void Interact(GameObject player){
        
        

    }
}
