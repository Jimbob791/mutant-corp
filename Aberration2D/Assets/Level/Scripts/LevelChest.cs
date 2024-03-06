using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChest : MonoBehaviour
{
    public ItemObject item;
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] LayerMask playerLayer;

    bool opened = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !opened && Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, Vector2.zero, 0, playerLayer))
        {
            GetComponent<Animator>().enabled = true;
            opened = true;
            SpawnPickup(item);
        }
    }

    private void SpawnPickup(ItemObject itemToSpawn)
    {
        GameObject newPickup = Instantiate(pickupPrefab, transform.position + spawnOffset, Quaternion.identity);
        newPickup.GetComponent<ItemPickup>().itemDrop = itemToSpawn;
    }
}
