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
                items[i].item.OnPickup(Player.instance, items[i].stacks);
                return;
            }
        }
        items.Add(new ItemList(item, item.GetName(), 1));
        items[items.Count - 1].item.OnPickup(Player.instance, 1);
    }
}
