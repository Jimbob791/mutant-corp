using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public Item item;
    public ItemObject itemDrop;
    [SerializeField] LayerMask playerLayer;
    public int fabricatorPosition = -1;
    [SerializeField] GameObject itemSFX;

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
        startPos = transform.localPosition;
    }

    void Update()
    {
        tick += Time.deltaTime;
        transform.localPosition = new Vector3(startPos.x, startPos.y + (Mathf.Sin(tick) * 0.1f), 0);
        if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, Vector2.zero, 0, playerLayer))
        {
            Instantiate(itemSFX);
            GameManager.instance.numItems += 1;
            Player.instance.GetComponent<PlayerItems>().AddItem(item);
            ItemInfoDisplay.instance.ItemPickedUp(itemDrop);
            GameObject.Find("Inventory").GetComponent<InventoryDisplay>().CreateInventoryItems(Player.instance.GetComponent<PlayerItems>().items);
            if (fabricatorPosition == -1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i != fabricatorPosition)
                    {
                        Destroy(transform.parent.transform.parent.transform.GetChild(i).transform.GetChild(0).gameObject);
                    }
                }
                Destroy(this.gameObject);
            }
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
            case Items.RocketBoots:
                return new RocketBoots();
            case Items.ThumperGL:
                return new ThumperGL();
            case Items.SniperScope:
                return new SniperScope();
            case Items.Silencer:
                return new Silencer();
            case Items.GreenMaple:
                return new GreenMaple();
            case Items.ShotgunCells:
                return new ShotgunCells();
            case Items.Hourglass:
                return new Hourglass();
            case Items.PlasticC4:
                return new PlasticC4();
            case Items.JavelinSL:
                return new JavelinSL();
            case Items.PhoenixTalon:
                return new PhoenixTalon();
            case Items.DataLicense:
                return new DataLicense();
            case Items.RustedSickle:
                return new RustedSickle();
            case Items.GuardDogs:
                return new GuardDogs();
            case Items.Bacteriophage:
                return new Bacteriophage();
            case Items.TeslaShield:
                return new TeslaShield();
            case Items.Dewdrop:
                return new Dewdrop();
            case Items.HealingCrystal:
                return new HealingCrystal();
            case Items.BipodHands:
                return new BipodHands();
            case Items.Nectar:
                return new Nectar();
            case Items.SealOfApproval:
                return new SealOfApproval();
            case Items.MysteryVial:
                return new MysteryVial();
            case Items.SpeedUp:
                return new SpeedUp();
            case Items.RadarLock:
                return new RadarLock();
            default:
                return new Loudener();
        }
    }
}