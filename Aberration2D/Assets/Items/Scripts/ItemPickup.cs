using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public Item item;
    public ItemObject itemDrop;
    [SerializeField] LayerMask playerLayer;

    [Space]

    [Header("Outlines")]
    [SerializeField] Material commonOutline;
    [SerializeField] Material specialOutline;
    [SerializeField] Material exoticOutline;
    [SerializeField] Material fabledOutline;
    [SerializeField] Material selectedOutline;

    Material rarityMaterial;

    Vector3 startPos;
    float tick = 0;

    void Start()
    {
        rarityMaterial = SetMaterial(itemDrop.itemRarity);
        transform.GetChild(0).GetComponent<SpriteRenderer>().material = rarityMaterial;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemDrop.icon;

        tick = Random.Range(0, 1.5f);
        item = AssignItem(itemDrop.item);
        startPos = transform.position;
    }

    void Update()
    {
        tick += Time.deltaTime;
        transform.localPosition = new Vector3(startPos.x, startPos.y + (Mathf.Sin(tick) * 0.1f), 0);
        if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, Vector2.zero, 0, playerLayer))
        {
            Player.instance.GetComponent<PlayerItems>().AddItem(item);
            ItemInfoDisplay.instance.ItemPickedUp(itemDrop);
            GameObject.Find("Inventory").GetComponent<InventoryDisplay>().CreateInventoryItems(Player.instance.GetComponent<PlayerItems>().items);
            Destroy(this.gameObject);
        }
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
                return commonOutline;
        }
    }

    public static Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
        {
            case Items.Loudener:
                return new Loudener();
            case Items.FleshBoots:
                return new FleshBoots();
            case Items.MutantBullets:
                return new MutantBullets();
            case Items.AmmoCache:
                return new AmmoCache();
            case Items.BurstSwitch:
                return new BurstSwitch();
            case Items.SaltedOrgans:
                return new SaltedOrgans();
            case Items.NosferatuBlood:
                return new NosferatuBlood();
            case Items.SpringReload:
                return new SpringReload();
            case Items.DemonicBlood:
                return new DemonicBlood();
            case Items.SteelHeart:
                return new SteelHeart();
            case Items.AllSeeingBullets:
                return new AllSeeingBullets();
            case Items.AcrobaticMuscle:
                return new AcrobaticMuscle();
            case Items.BionicFinger:
                return new BionicFinger();
            case Items.AutoTrigger:
                return new AutoTrigger();
            default:
                return new Loudener();
        }
    }
}