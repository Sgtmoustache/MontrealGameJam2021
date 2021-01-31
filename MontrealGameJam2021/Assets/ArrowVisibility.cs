using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowVisibility : MonoBehaviour
{
    public bool IsOnItem;

    [SerializeField] private GameObject arrow;

    public GameObject targetToFollow;

    public int height = 3;

    private void Start()
    {

        if (!targetToFollow)
            return;
            
        if (targetToFollow.GetComponent<Collecting>() != null)
        {
            IsOnItem = true;
        }
        else
        {
            IsOnItem = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetToFollow)
            return;
        
        transform.position = targetToFollow.transform.position + new Vector3(0, height, 0);
        
        bool playerHasItem = PlayerSpawner.LocalPlayer.GetComponent<Inventory>().HasItem();
        
        if (playerHasItem && !IsOnItem)
        {
            arrow.SetActive(true);
        }
        else if (!playerHasItem && IsOnItem)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
