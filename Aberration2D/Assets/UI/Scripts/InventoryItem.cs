using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public ItemObject item;

    public void MouseOver()
    {
        InventoryMouseInfo.instance.display = true;
        InventoryMouseInfo.instance.item = item;
    }

    public void MouseExit()
    {
        InventoryMouseInfo.instance.display = false;
    }
}
