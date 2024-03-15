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
        Vector2 ratios = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height);

        mousePos.x = (ratios.x * 1920) - 960;
        mousePos.y = (ratios.y * 1080) - 540;

        mousePos.z = 0;

        if (mousePos.x > 360)
            mousePos.x -= width / 2;
        else
            mousePos.x += width / 2;

        mousePos.y -= 100;

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
