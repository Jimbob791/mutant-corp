using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemObject : ScriptableObject
{
    public Items item;
    public Sprite icon;
    public string itemName;
    public string itemRarity;
    [TextArea(3,5)] public string itemDescription;
}
