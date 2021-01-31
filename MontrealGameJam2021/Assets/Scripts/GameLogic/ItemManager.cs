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
    
    public void Start()
    {
        ItemsPrefabs = Resources.LoadAll<GameObject>("Prefabs/Item/").ToList();
        Debug.LogWarning($"Found {ItemsPrefabs.Count} in Item folder");

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
                Debug.LogWarning($"NO DROP SPOT OUTSIDE FOR {prefab.name} ({collectibles})");
            }
        }

        ItemsPrefabs = ToKeep;
        Debug.LogWarning($"Kept {ItemsPrefabs.Count} item prefab");
    }

    private void Randomize()
    {
        OutsidePlaceHolders = OutsidePlaceHolders.OrderBy((item) => Random.Range(0, OutsidePlaceHolders.Count)).ToList();

        LostAndFoundPlaceHolders = LostAndFoundPlaceHolders.OrderBy((item) => Random.Range(0, LostAndFoundPlaceHolders.Count)).ToList();
        
        ItemsPrefabs = ItemsPrefabs.OrderBy((item) => Random.Range(0, ItemsPrefabs.Count)).ToList();
    }

    public void RefreshItems()
    {
        photonView.RPC("ClearPlaceHolders", RpcTarget.All);
        
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
                Debug.LogWarning($"({lostAndFoundCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {selectedPlaceholder.transform.parent.name + "/" + selectedPlaceholder.name}");

            }
            //OutsidePlaceHolders
            else
            {
                selectedPlaceholder = OutsidePlaceHolders.FirstOrDefault(b => b.itemType == item.GetComponent<ItemInfo>().Collectibles);
                if(selectedPlaceholder == null)
                    Debug.LogWarning($"!!!NO PLACE TO PLACE OBJECT {item.name}!!!");
                
                Debug.LogWarning($"({outsideCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {selectedPlaceholder.transform.parent.name + "/" + selectedPlaceholder.name}");
            }
            
            GameObject spawnedItem = PhotonNetwork.Instantiate("Prefabs/Item/" + item.name,  Vector3.zero, Quaternion.identity);
            selectedPlaceholder.BroadcastName(spawnedItem.name);
            SpawnedItems.Add(spawnedItem);
                
            lostAndFoundCount++;
            itemSpawnedCount++;
        }
    }

    [PunRPC]
    private void ClearPlaceHolders()
    {
        OutsidePlaceHolders.ForEach(b => b.RemoveItem());
        LostAndFoundPlaceHolders.ForEach(b => b.RemoveItem());
        SpawnedItems.ForEach(Destroy);
    }
}
