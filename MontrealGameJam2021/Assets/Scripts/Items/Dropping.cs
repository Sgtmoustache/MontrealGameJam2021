using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropping : MonoBehaviour
{


    [SerializeField] Transform arrow;
    [SerializeField] Canvas FloatingText;

    private PlayerInfo playerInfo;

    private Transform PlaceHolderOutsideLocation;

    private void Start()
    {
        FloatingText.gameObject.SetActive(true);
        PlaceHolderOutsideLocation = null;
    }
    // Update is called once per frame
    void Update()
    {
        Inventory inventory = this.gameObject.GetComponent<Inventory>();
        if(inventory.HasItem()){
            if (Input.GetKeyDown(KeyCode.Q)){
                bool isPlayer = (this.gameObject.GetComponent<PlayerInfo>().PlayerType == "Student");
                GameObject obj = inventory.GetItemGameObject();
                inventory.ClearItem();
                Vector3 vec = this.gameObject.transform.localPosition;
                Collecting collect = obj.GetComponent<Collecting>();
                collect.beInteractable();
                obj.transform.SetParent(null);
                obj.transform.localPosition = new Vector3(vec.x, (vec.y + (isPlayer? 2.5f : 3.5f)), vec.z);
            }
            if(this.gameObject.GetComponent<PlayerInfo>()?.PlayerType == "Student"){
                Vector3 targetPosition = new Vector3(20f, -14f , 21f);
                targetPosition.y = arrow.transform.position.y;
                arrow.gameObject.SetActive(true);
                arrow.LookAt(targetPosition);
            }
            else{
                if(!PlaceHolderOutsideLocation)
                {
                    List<PlaceHolder> OutsidePlaceHolders = GameManager._Instance.ItemManager.OutsidePlaceHolders;
                    var find =  OutsidePlaceHolders.FirstOrDefault(element => element.itemType == inventory.GetTypeOfItem());
                    PlaceHolderOutsideLocation = find.gameObject.transform;
                }
                
                Vector3 targetPosition = PlaceHolderOutsideLocation.position;
                targetPosition.y = arrow.transform.position.y;
                arrow.gameObject.SetActive(true);
                arrow.LookAt(targetPosition);
            }
        }
        else{
            arrow.gameObject.SetActive(false);
            PlaceHolderOutsideLocation = null;
        }
    }
}
