using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ItemManager : MonoBehaviourPun
{ 
    private List<GameObject> ItemsPrefabs = new List<GameObject>();
    [SerializeField] public List<PlaceHolder> OutsidePlaceHolders;
    [SerializeField] public List<PlaceHolder> LostAndFoundPlaceHolders;
    [SerializeField] public List<PlaceHolder> HiddenSpotPlaceHolders;
    
    private List<GameObject> Items;

    private List<GameObject> SpawnedItems = new List<GameObject>();
    private List<GameObject> Arrows = new List<GameObject>();
    
    public void Start()
    {
        ItemsPrefabs = Resources.LoadAll<GameObject>("Prefabs/Item/").ToList();
        Debug.Log($"Found {ItemsPrefabs.Count} in Item folder");

        List<GameObject> ToKeep = new List<GameObject>();  
        
        foreach (var prefab in ItemsPrefabs)
        {
            var collectibles = prefab.GetComponent<ItemInfo>().Collectibles;

            if (OutsidePlaceHolders.Select(b => b.itemType).Contains(collectibles))
            {
                ToKeep.Add(prefab);
            }
            else
            {
                Debug.Log($"NO DROP SPOT OUTSIDE FOR {prefab.name} ({collectibles})");
            }
        }

        ItemsPrefabs = ToKeep;
        Debug.Log($"Kept {ItemsPrefabs.Count} item prefab");
    }

    private void Randomize()
    {
        OutsidePlaceHolders = OutsidePlaceHolders.OrderBy((item) => Random.Range(0, OutsidePlaceHolders.Count)).ToList();

        LostAndFoundPlaceHolders = LostAndFoundPlaceHolders.OrderBy((item) => Random.Range(0, LostAndFoundPlaceHolders.Count)).ToList();
        
        ItemsPrefabs = ItemsPrefabs.OrderBy((item) => Random.Range(0, ItemsPrefabs.Count)).ToList();
    }

    public void RefreshItems(bool clearPlaceHolders)
    {
        if(clearPlaceHolders)
            photonView.RPC("ClearPlaceHoldersFunction", RpcTarget.All);
        
        Randomize();
        
        int itemSpawnedCount = 0;
        int lostAndFoundCount = 0;
        int outsideCount = 0;
        
        foreach (var item in ItemsPrefabs)
        {
            PlaceHolder selectedPlaceholder;
            
            //LostAndFoundPlaceOlder
            if (itemSpawnedCount < ItemsPrefabs.Count / 2)
            {
                selectedPlaceholder = LostAndFoundPlaceHolders[lostAndFoundCount];
                Debug.Log($"({lostAndFoundCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {selectedPlaceholder.transform.parent.name + "/" + selectedPlaceholder.name}");

            }
            //OutsidePlaceHolders
            else
            {
                selectedPlaceholder = OutsidePlaceHolders.FirstOrDefault(b => b.itemType == item.GetComponent<ItemInfo>().Collectibles);
                Debug.Log($"({outsideCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {selectedPlaceholder.transform.parent.name + "/" + selectedPlaceholder.name}");
            }

            if (selectedPlaceholder == null)
            {
                throw new MissingComponentException($"NO PLACE TO PLACE OBJECT {item.name}");
            }

            //string tag = "[" + GameManager._Instance.CurrentRound + "]";
            
            GameObject spawnedItem = PhotonNetwork.Instantiate("Prefabs/Item/" + item.name,  selectedPlaceholder.itemDropPosition.position, selectedPlaceholder.itemDropPosition.rotation);
            //spawnedItem.name = spawnedItem.name + tag;
            selectedPlaceholder.BroadcastName(spawnedItem.name);
            SpawnedItems.Add(spawnedItem);
                
            lostAndFoundCount++;
            itemSpawnedCount++;
        }
        
        photonView.RPC("SpawnArrows", RpcTarget.All);
    }

    [PunRPC]
    private void SpawnArrows()
    {
        foreach (var arrow in Arrows)
        {
            Destroy(arrow);
        }
        
        foreach (var item in SpawnedItems)
        {
             GameObject obj = (GameObject) Instantiate(Resources.Load($"Prefabs/ObjectArrow"), Vector3.zero, Quaternion.identity);
             obj.GetComponent<ArrowVisibility>().targetToFollow = item;
             Arrows.Add(obj);
        }
    }

    [PunRPC]
    public void ClearPlaceHoldersFunction(){
        StartCoroutine(ClearPlaceHolders());
    }
    public IEnumerator ClearPlaceHolders()
    {
        Debug.Log("*****Clearing objects!");
        
        PlayerSpawner.LocalPlayer.GetComponent<Inventory>().ClearItem();
        OutsidePlaceHolders.ForEach(b => b.RemoveItem());
        LostAndFoundPlaceHolders.ForEach(b => b.RemoveItem());
        HiddenSpotPlaceHolders.ForEach(b => b.RemoveItem());

        yield return new WaitForSeconds(5);
        
        if(PhotonNetwork.IsMasterClient)
            SpawnedItems.ForEach(PhotonNetwork.Destroy);
        
        Debug.Log("*****End clear objects!");

    }
}
