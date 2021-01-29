using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static int objectID;
    public static GameObject objet;

    public static void setObject(int value, GameObject ob)
    {
        objectID = value;
        objet = ob;
    }
    public static GameObject getItem()
    {
        return objet;
    }
    public static int getObjectID(){ 
        return objectID;
    }

    public static bool HasObject()
    {
        //nightlight = !nightlight;
        return (objectID > 0)? true : false;
    }
}
