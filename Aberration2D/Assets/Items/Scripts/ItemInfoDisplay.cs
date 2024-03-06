using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoDisplay : MonoBehaviour
{
    public static ItemInfoDisplay instance;

    [SerializeField] float displayTime;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] TextMeshProUGUI rarity;
    [SerializeField] TextMeshProUGUI rarityBack;
    [SerializeField] GameObject cover;
    [SerializeField] Image icon;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ItemPickedUp(ItemObject item)
    {
        title.text = item.itemName;
        desc.text = item.itemDescription;
        rarity.text = item.itemRarity.ToUpper();
        rarityBack.text = item.itemRarity.ToUpper();
        icon.sprite = item.icon;

        GetComponent<Animator>().SetBool("display", true);
        cover.GetComponent<Animator>().SetBool("display", true);
        StartCoroutine(DisplayTime());
    }

    IEnumerator DisplayTime()
    {
        yield return new WaitForSeconds(displayTime);
        GetComponent<Animator>().SetBool("display", false);
        cover.GetComponent<Animator>().SetBool("display", false);
    }
}
