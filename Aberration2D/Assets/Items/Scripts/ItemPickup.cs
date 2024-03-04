using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;
    [SerializeField] LayerMask playerLayer;

    Vector3 startPos;
    float tick = 0;

    void Start()
    {
        tick = Random.Range(0, 1.5f);
        item = AssignItem(itemDrop);
        startPos = transform.position;
    }

    void Update()
    {
        tick += Time.deltaTime;
        transform.localPosition = new Vector3(startPos.x, startPos.y + (Mathf.Sin(tick) * 0.2f), 0);
        if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, Vector2.zero, 0, playerLayer))
        {
            Player.instance.GetComponent<PlayerItems>().AddItem(item);
            Destroy(this.gameObject);
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
            default:
                return new Loudener();
        }
    }
}