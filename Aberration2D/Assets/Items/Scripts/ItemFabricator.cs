using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFabricator : MonoBehaviour
{
    [SerializeField] string rarity;
    [SerializeField] int commonChance;
    [SerializeField] int specialChance;
    [SerializeField] int exoticChance;
    public List<GameObject> pickupBases = new List<GameObject>();
    [SerializeField] GameObject pickupPrefab;

    List<ItemObject> itemList = new List<ItemObject>();

    void Start()
    {
        if (rarity == "")
        {
            int r = Random.Range(0, 100);
            if (r < commonChance)
            {
                rarity = "Common";
            }
            else if (r < specialChance)
            {
                rarity = "Special";
            }
            else
            {
                rarity = "Exotic";
            }
        }

        itemList = AllItems.instance.items;
        List<ItemObject> sorted = new List<ItemObject>();
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemRarity == rarity)
            {
                sorted.Add(itemList[i]);
            }
        }

        for (int i = 0; i < pickupBases.Count; i++)
        {
            GameObject newPickup = Instantiate(pickupPrefab, pickupBases[i].transform.position, Quaternion.identity, pickupBases[i].transform);
            newPickup.GetComponent<ItemPickup>().fabricatorPosition = i;
            newPickup.GetComponent<ItemPickup>().itemDrop = sorted[Random.Range(0, sorted.Count)];
        }
    }
}
