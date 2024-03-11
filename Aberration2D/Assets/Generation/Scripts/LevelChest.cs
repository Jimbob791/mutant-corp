using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChest : MonoBehaviour
{
    public ItemObject item;
    public int dataCost;
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject openSFX;

    bool opened = false;
    bool touching;

    void Update()
    {
        if (Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, Vector2.zero, 0, playerLayer))
            touching = true;
        else
            touching = false;

        if (!opened)
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = touching;
        else
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (Input.GetKeyDown(KeyCode.E) && !opened && touching && Player.instance.GetComponent<PlayerData>().data >= dataCost)
        {
            Player.instance.GetComponent<PlayerData>().data -= dataCost;
            GetComponent<Animator>().enabled = true;
            opened = true;
            SpawnPickup(item);
        }
    }

    private void SpawnPickup(ItemObject itemToSpawn)
    {
        Instantiate(openSFX);
        GameObject newPickup = Instantiate(pickupPrefab, transform.position + spawnOffset, Quaternion.identity);
        newPickup.GetComponent<ItemPickup>().itemDrop = itemToSpawn;
    }
}
