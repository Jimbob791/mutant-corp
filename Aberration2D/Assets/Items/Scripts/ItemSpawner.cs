using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] Vector3 spawnPos;

    List<Item> items = new List<Item>();

    void Start()
    {
        SpawnPickups(1);
    }

    private void SpawnPickups(int amount)
    {
        GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
    }
}
