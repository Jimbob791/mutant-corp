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
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject choiceParent;
    [SerializeField] int numChoices = 3;

    [SerializeField] List<MutationObject> mutations = new List<MutationObject>();

    List<GameObject> options = new List<GameObject>();
    List<MutationObject> chosenMutations = new List<MutationObject>();

    int selected = 0;
    bool confirmed = true;

    void Start()
    {
        StartCoroutine(TitleIntro());
        StartCoroutine(ChooseMutationOptions());
    }

    void Update()
    {
        if (confirmed)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selected += 1;
            selected = selected >= numChoices ? 0 : selected;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            selected -= 1;
            selected = selected < 0 ? numChoices - 1 : selected;
        }

        selector.transform.localPosition = new Vector3(0, selected * -210, 0);
    }

    IEnumerator ChooseMutationOptions()
    {
        for (int i = 0; i < numChoices; i++)
        {
            yield return new WaitForSeconds(0.8f);

            MutationObject chosen = mutations[Random.Range(0, mutations.Count)];
            chosenMutations.Add(chosen);
            mutations.Remove(chosen);

            GameObject newChoice = Instantiate(prefab, choiceParent.transform);
            newChoice.transform.localPosition = new Vector3(0, i * -210, 0);
            newChoice.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = chosen.mutationName;
            newChoice.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = chosen.description;
            //newChoice.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = chosen.icon;
            options.Add(newChoice);
        }
        confirmed = false;
    }

    public void Confirm()
    {
        confirmed = true;
        title.text = "Mutation Confirmed";
        StartCoroutine(EnablePod());
        cover.GetComponent<Animator>().enabled = true;

        AddMutation(chosenMutations[selected]);

        options.RemoveAt(selected);
        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponent<Animator>().enabled = true;
            options[i].GetComponent<Animator>().speed = Random.Range(0.5f, 0.8f);
        }
        StartCoroutine(CallExit());
    }

    private IEnumerator EnablePod()
    {
        yield return new WaitForSeconds(1);
        pod.GetComponent<Animator>().SetTrigger("select");
    }

    private IEnumerator CallExit()
    {
        yield return new WaitForSeconds(4);
        GameManager.instance.ExitMutations();
    }

    IEnumerator TitleIntro()
    {
        string t = " Select a Mutation";

        for (int i = 0; i < t.Length; i++)
        {
            title.text = title.text + t[i];
            yield return new WaitForSeconds(0.1f);
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

    private void AddMutation(MutationObject mutation)
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
}