using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    public List<ItemList> items = new List<ItemList>();

    public static ItemStorage instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveItems(List<ItemList> itemsToSave)
    {
        items = itemsToSave;
    }

    public void LoadItems()
    {
        Player.instance.GetComponent<PlayerItems>().items = items;
    }
}
