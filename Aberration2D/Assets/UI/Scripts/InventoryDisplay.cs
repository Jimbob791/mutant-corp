using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject invPrefab;
    [SerializeField] GameObject backingParent;

    [Space]

    [Header("Outlines")]
    [SerializeField] Material commonOutline;
    [SerializeField] Material specialOutline;
    [SerializeField] Material exoticOutline;
    [SerializeField] Material fabledOutline;
    [SerializeField] Material mutationOutline;

    List<GameObject> objects = new List<GameObject>();

    Vector3 offset;

    void Start()
    {
        offset = transform.position - Player.instance.transform.position;
    }

    private Material SetMaterial(string rarity)
    {
        switch (rarity)
        {
            case "Common":
                return commonOutline;
            case "Special":
                return specialOutline;
            case "Exotic":
                return exoticOutline;
            case "Fabled":
                return fabledOutline;
            default:
                return mutationOutline;
        }
    }

    public void CreateInventoryItems(List<ItemList> items)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            Destroy(objects[i]);
        }

        List<InventoryObject> invList = new List<InventoryObject>();
        List<ItemObject> allItems = AllItems.instance.items;

        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < allItems.Count; k++)
            {
                if (allItems[k].itemName == items[i].name)
                {
                    InventoryObject newObj = new InventoryObject();
                    newObj.obj = allItems[k];
                    newObj.stacks = items[i].stacks;
                    invList.Add(newObj);
                }
            }
        }

        for (int i = 0; i < invList.Count; i++)
        {
            GameObject spawned;
            spawned = Instantiate(invPrefab, transform.position, Quaternion.identity, backingParent.transform);

            spawned.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = invList[i].obj.icon;
            
            spawned.GetComponent<InventoryItem>().item = invList[i].obj;

            for (int t = 0; t < 5; t++)
            {
                spawned.GetComponent<InventoryItem>().texts[t].text = invList[i].stacks.ToString();
            }

            if (i < 20)
                spawned.transform.localPosition = new Vector3(-19f + (i * 2f), 1.1f, 0f);
            else
                spawned.transform.localPosition = new Vector3(-19f + ((i - 20) * 2f), -0.9f, 0f);
                
            objects.Add(spawned);
        }
    }
}

struct InventoryObject
{
    public ItemObject obj;
    public int stacks;
}