using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Tuple<Collectibles, GameObject> Item = null;

    public Collectibles GetTypeOfItem() => Item.Item1;
    public GameObject GetItemGameObject() => Item.Item2;
    
    public void SetItem(Collectibles type, GameObject obj)
    {
        Item = new Tuple<Collectibles, GameObject>(type, obj);
    }

    public void ClearItem()
    {
        Item = new Tuple<Collectibles, GameObject>(Collectibles.None, null);        
    }

    public bool HasItem() => Item != null;
}
