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
    LunchBox,
    CoffreCrayon,
    GlassesGreen,
    GlassesYellow,
    WalletLeather,
    WalletBlack,
    IdCard,
}

public class ItemInfo : MonoBehaviour
{
    public Collectibles Collectibles = Collectibles.None;
}
