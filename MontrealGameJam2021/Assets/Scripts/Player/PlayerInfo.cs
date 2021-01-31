using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Display;

    [SerializeField] public string PlayerType;

    public bool isLocal = false;
}
