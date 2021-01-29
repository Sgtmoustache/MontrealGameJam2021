using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static int objectID;
    public static GameObject objet;

    public static void setObjectId(int value)
    {
        objectID = value;
    }
    public static void setItem(GameObject value)
    {
        objet = value;
    }
    public static GameObject getItem()
    {
        return objet;
    }
    public static int getObjectValue(){ 
        return objectID;
    }

    public static bool HasObject()
    {
        //nightlight = !nightlight;
        return (objectID > 0)? true : false;
    }
}
