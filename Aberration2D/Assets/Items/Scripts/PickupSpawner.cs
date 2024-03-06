using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Spawner Info")]
    [SerializeField] List<Transform> spawnPosList = new List<Transform>();
    [SerializeField] List<Chest> interactables = new List<Chest>();

    List<ItemObject> itemList = new List<ItemObject>();

    List<ItemObject> commons = new List<ItemObject>();
    List<ItemObject> specials = new List<ItemObject>();
    List<ItemObject> exotics = new List<ItemObject>();
    List<ItemObject> fabled = new List<ItemObject>();

    int credits;
    Chest chosenInteractable;

    void Start()
    {
        chosenInteractable = interactables[0];
        itemList = AllItems.instance.items;
        SortItems();

        credits = Random.Range(50, 100);

        int sumWeights = 0;
        for (int i = 0; i < interactables.Count; i++)
        {
            sumWeights += interactables[i].weight;
        }

        while (credits > 0)
        {
            int chosenWeight = Random.Range(0, sumWeights);
            for (int i = 0; i < interactables.Count; i++)
            {
                if (interactables[i].weight < chosenWeight)
                {
                    chosenInteractable = interactables[i];
                    break;
                }
                
                chosenWeight -= interactables[i].weight;
            }

            credits -= chosenInteractable.cost;

            int randPosIndex = Random.Range(0, spawnPosList.Count);
            Vector3 spawnPos = spawnPosList[randPosIndex].position;
            spawnPosList.RemoveAt(randPosIndex);

            GameObject newInteractable = Instantiate(chosenInteractable.prefab, spawnPos, Quaternion.identity);
            newInteractable.GetComponent<LevelChest>().item = ChooseItem(chosenInteractable);
        }
    }

    private ItemObject ChooseItem(Chest chosen)
    {
        int rand = Random.Range(0, 100);
        if (rand < chosen.commonChance)
        {
            return commons[Random.Range(0, commons.Count)];
        }
        if (rand < chosen.specialChance)
        {
            return specials[Random.Range(0, specials.Count)];
        }
        if (rand < chosen.exoticChance)
        {
            return exotics[Random.Range(0, exotics.Count)];
        }
        return exotics[Random.Range(0, exotics.Count)]; //return fabled[Random.Range(0, fabled.Count)];
    }

    private void SortItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            switch (itemList[i].itemRarity)
            {
                case "Common":
                    commons.Add(itemList[i]);
                    break;
                case "Special":
                    specials.Add(itemList[i]);
                    break;
                case "Exotic":
                    exotics.Add(itemList[i]);
                    break;
                case "Fabled":
                    fabled.Add(itemList[i]);
                    break;
                case "Mutation":
                    // mutation
                    break;
                default:
                    break;
            }
        }
    }
}

[System.Serializable]
public class Interactable
{
    public GameObject prefab;
    public int cost;
    public int weight;
}

[System.Serializable]
public class Chest : Interactable
{
    public float commonChance;
    public float specialChance;
    public float exoticChance;
    public float fabledChance;
}