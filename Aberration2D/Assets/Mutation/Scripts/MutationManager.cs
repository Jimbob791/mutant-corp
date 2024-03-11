using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MutationManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] GameObject pod;
    [SerializeField] GameObject selector;
    [SerializeField] GameObject cover;
    [SerializeField] GameObject[] prefabs = new GameObject[3];
    [SerializeField] GameObject choiceParent;
    [SerializeField] GameObject selectSFX;
    [SerializeField] GameObject confirmSFX;
    [SerializeField] GameObject createSFX;
    [SerializeField] GameObject rollSFX;
    [SerializeField] GameObject slamSFX;
    [SerializeField] GameObject hurtSFX;

    List<ItemObject> items = new List<ItemObject>();
    List<ItemObject> mutations = new List<ItemObject>();

    List<GameObject> options = new List<GameObject>();
    List<ItemObject> chosenMutations = new List<ItemObject>();

    int selected = 0;
    bool confirmed = true;

    void Start()
    {
        items = AllItems.instance.items;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemRarity == "Mutation")
            {
                mutations.Add(items[i]);
            }
        }

        StartCoroutine(TitleIntro());
        StartCoroutine(ChooseMutationOptions());
    }

    void Update()
    {
        if (confirmed)
        {
            return;
        }

        selector.transform.localPosition = new Vector3(0, selected * -210, 0);
    }

    IEnumerator ChooseMutationOptions()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(createSFX);

            ItemObject chosen = mutations[Random.Range(0, mutations.Count)];
            chosenMutations.Add(chosen);
            mutations.Remove(chosen);

            GameObject newChoice = prefabs[i];
            newChoice.transform.localPosition = new Vector3(0, i * -210, 0);
            newChoice.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
            newChoice.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = chosen.itemName;
            newChoice.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = chosen.itemDescription;
            //newChoice.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = chosen.icon;
            options.Add(newChoice);
        }
        confirmed = false;
    }

    public void Confirm()
    {
        if (confirmed)
        {
            return;
        }

        confirmed = true;
        title.text = "Mutation Confirmed";
        cover.GetComponent<Animator>().enabled = true;

        AddMutation(chosenMutations[selected]);

        options.RemoveAt(selected);
        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponent<Animator>().enabled = true;
            options[i].GetComponent<Animator>().speed = Random.Range(0.5f, 0.8f);
        }
        Instantiate(confirmSFX);
        StartCoroutine(EnablePod());
        StartCoroutine(CallExit());
    }

    private IEnumerator EnablePod()
    {
        yield return new WaitForSeconds(1);
        Instantiate(hurtSFX);
        pod.GetComponent<Animator>().SetTrigger("select");
        yield return new WaitForSeconds(1.64f);
        Instantiate(slamSFX);
    }

    private IEnumerator CallExit()
    {
        yield return new WaitForSeconds(3);
        GameManager.instance.ExitMutations();
    }

    IEnumerator TitleIntro()
    {
        yield return new WaitForSeconds(0.7f);
        string t = " Select a Mutation";

        for (int i = 0; i < t.Length; i++)
        {
            title.text = title.text + t[i];
            yield return new WaitForSeconds(0.08f);
        }

        StartCoroutine(TitleFlicker());
    }

    IEnumerator TitleFlicker()
    {
        title.enabled = true;
        yield return new WaitForSeconds(0.5f);
        title.enabled = false;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(TitleFlicker());
    }

    private void AddMutation(ItemObject mutation)
    {
        Item item = ItemPickup.AssignItem(mutation.item);

        for (int i = 0; i < ItemStorage.instance.items.Count; i++)
        {
            if (ItemStorage.instance.items[i].name == item.GetName())
            {
                ItemStorage.instance.items[i].stacks += 1;
                item.OnPickup(ItemStorage.instance.items[i].stacks);
                return;
            }
        }
        ItemStorage.instance.items.Add(new ItemList(item, item.GetName(), 1));
        item.OnPickup(1);
    }

    public void Selected1()
    {
        selected = 0;
        Instantiate(selectSFX);
    }

    public void Selected2()
    {
        selected = 1;
        Instantiate(selectSFX);
    }

    public void Selected3()
    {
        selected = 2;
        Instantiate(selectSFX);
    }
}