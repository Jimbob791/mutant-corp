using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public List<ItemList> items = new List<ItemList>();

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == item.GetName())
            {
                items[i].stacks += 1;
                items[i].item.OnPickup(items[i].stacks);
                PlayerStats.instance.SetPlayerStats();
                return;
            }
        }
        items.Add(new ItemList(item, item.GetName(), 1));
        items[items.Count - 1].item.OnPickup(1);
        PlayerStats.instance.SetPlayerStats();
    }

    public void Reload()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].item.OnReload(items[i].stacks);
        }
    }
}
