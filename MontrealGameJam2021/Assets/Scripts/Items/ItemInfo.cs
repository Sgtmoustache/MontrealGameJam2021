using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Collectibles
{
    None,
    All,
    TessBook,
    Laptop,
    Cellphone,
}

public class ItemInfo : MonoBehaviour
{
    public Collectibles Collectibles = Collectibles.None;
}
