using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowVisibility : MonoBehaviour
{
    private bool _isOnItem;

    [SerializeField] private GameObject arrow;

    public GameObject targetToFollow;

    public int height = 3;

    private Collectibles _itemType;
    public bool isTeacherTarget = false;
    private Inventory _inventory;
    private PlaceHolder _placeHolder;
    private ItemInfo _itemInfo;
    private bool isHidingSpot = false;
    private void Start()
    {
        if (!targetToFollow)
            Destroy(this.gameObject);
        
        _itemInfo = targetToFollow.GetComponent<ItemInfo>();
        _placeHolder = targetToFollow.GetComponent<PlaceHolder>();
        _inventory = PlayerSpawner.LocalPlayer.GetComponent<Inventory>();
        
        if (targetToFollow.GetComponent<Collecting>() != null)
        {
            _isOnItem = true;
            _itemType = targetToFollow.GetComponent<ItemInfo>().Collectibles;
        }
        else
        {
            _isOnItem = false;
            _itemType = targetToFollow.GetComponent<PlaceHolder>().itemType;
        }
        
        if (_placeHolder != null)
        {
            isHidingSpot = _placeHolder.hidingSpot;
            height = 6;
            arrow.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetToFollow.transform.position + new Vector3(0, height, 0);
        
        if (isHidingSpot)
            return;
        
        bool playerHasItem = _inventory.HasItem();
        
        if (PlayerSpawner.Instance.IsTeacher)
        {
            if (!playerHasItem && _isOnItem && _itemInfo.currentPlaceHolder == null)
            {
                arrow.SetActive(true);
            }
            else if (playerHasItem && !_isOnItem && _inventory.GetTypeOfItem() == _itemType && isTeacherTarget == PlayerSpawner.Instance.IsTeacher)
            {
                arrow.SetActive(true);
            }
            else if (!playerHasItem && _isOnItem)
            {
                arrow.SetActive(_itemInfo.currentPlaceHolder != null && _itemInfo.currentPlaceHolder.lostAndFound);
            }
            else if (playerHasItem && !_isOnItem &&_inventory.GetTypeOfItem() == _itemType && !_placeHolder.HasItem())
            {
                arrow.SetActive(true);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
        else
        {
            if (!playerHasItem && _isOnItem && _itemInfo.currentPlaceHolder == null)
            {
                arrow.SetActive(true);
            }
            else if (playerHasItem && !_isOnItem && _itemType == Collectibles.None && isTeacherTarget == PlayerSpawner.Instance.IsTeacher)
            {
                arrow.SetActive(true);
            }
            else if (!playerHasItem && _isOnItem)
            {
                arrow.SetActive(_itemInfo.currentPlaceHolder != null && !_itemInfo.currentPlaceHolder.lostAndFound);
            }
            else if (playerHasItem && !_isOnItem &&_itemType == Collectibles.None && !_placeHolder.HasItem())
            {
                arrow.SetActive(true);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }
}
