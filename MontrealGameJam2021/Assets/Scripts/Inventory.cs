using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int objectID;
    public GameObject objet;

    public void setObject(int value, GameObject ob)
    {
        objectID = value;
        objet = ob;
    }
    public GameObject getItem()
    {
        return objet;
    }
    public int getObjectID(){ 
        return objectID;
    }

    public bool HasObject()
    {
        //nightlight = !nightlight;
        return (objectID > 0)? true : false;
    }
}
