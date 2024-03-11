using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMouseInfo : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] float height;
    public ItemObject item;
    public bool display;

    public static InventoryMouseInfo instance;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] TextMeshProUGUI rarity;
    [SerializeField] TextMeshProUGUI rarityBack;
    [SerializeField] Image icon;

    Image img;

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

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        img.enabled = display;
        title.enabled = display;
        desc.enabled = display;
        rarity.enabled = display;
        rarityBack.enabled = display;
        icon.enabled = display;
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= 1920 / 2;
        mousePos.y -= 1080 / 2;
        mousePos.z = 0;

        if (mousePos.x > 400)
            mousePos.x -= width / 2;
        else
            mousePos.x += width / 2;
        mousePos.y -= height / 2;

        transform.localPosition = mousePos;

        if (item == null)
        {
            return;
        }

        title.text = item.itemName;
        desc.text = item.itemDescription;
        rarity.text = item.itemRarity.ToUpper();
        rarityBack.text = item.itemRarity.ToUpper();
        icon.sprite = item.icon;
    }
}
