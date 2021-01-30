using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{ 
    private List<GameObject> ItemsPrefabs = new List<GameObject>();
    [SerializeField] private List<PlaceHolder> OutsidePlaceHolders;
    [SerializeField] private List<PlaceHolder> LostAndFoundPlaceHolders;
    private List<GameObject> Items;

    private List<GameObject> SpawnedItems = new List<GameObject>();
    
    public void Start()
    {
        ItemsPrefabs = Resources.LoadAll<GameObject>("Prefabs/Item/").ToList();
        Debug.LogWarning($"Found {ItemsPrefabs.Count} in Item folder");
    }

    private void Randomize()
    {
        OutsidePlaceHolders = OutsidePlaceHolders.OrderBy((item) => Random.Range(0, OutsidePlaceHolders.Count)).ToList();

        LostAndFoundPlaceHolders = LostAndFoundPlaceHolders.OrderBy((item) => Random.Range(0, LostAndFoundPlaceHolders.Count)).ToList();
        
        ItemsPrefabs = ItemsPrefabs.OrderBy((item) => Random.Range(0, ItemsPrefabs.Count)).ToList();
    }

    public void RefreshItems()
    {
        ClearPlaceHolders();

        Randomize();
        
        int itemSpawnedCount = 0;
        int lostAndFoundCount = 0;
        int outsideCount = 0;
        
        foreach (var item in ItemsPrefabs)
        {
            //LostAndFoundPlaceOlder
            if (itemSpawnedCount < ItemsPrefabs.Count / 2)
            {
                Transform wantedPosition = LostAndFoundPlaceHolders[lostAndFoundCount].transform;
                Debug.LogWarning($"({lostAndFoundCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {wantedPosition.parent.name + "/" + wantedPosition.name}");

                //TODO ADD ITEM TO PLACEHOLDER SCRIPT
                SpawnedItems.Add(Instantiate(item, wantedPosition.position, wantedPosition.rotation));
                lostAndFoundCount++;
            }
            //OutsidePlaceHolders
            else
            {
                Transform wantedPosition = OutsidePlaceHolders.FirstOrDefault(b => b.itemType == item.GetComponent<ItemInfo>().Collectibles)?.transform;
                Debug.LogWarning($"({outsideCount}/{ItemsPrefabs.Count}) Spawning {item.name} at {wantedPosition.parent.name + "/" + wantedPosition.name}");

                //TODO ADD ITEM TO PLACEHOLDER SCRIPT
                
                
                SpawnedItems.Add(Instantiate(item, wantedPosition.position, wantedPosition.rotation));
                outsideCount++;
            }
            
            itemSpawnedCount++;
        }
    }

    private void ClearPlaceHolders()
    {
        //TODO REMOVE FROM PLACEHOLDERS
        SpawnedItems.ForEach(Destroy);
    }
}
