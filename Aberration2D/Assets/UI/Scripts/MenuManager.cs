using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    [SerializeField] RawImage background;
    [SerializeField] GameObject hoverSFX;
    [SerializeField] GameObject clickSFX;

    void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(scrollSpeed, 0) * Time.deltaTime, background.uvRect.size);
    }

    public void NewGame()
    {
        Instantiate(clickSFX);
        GameManager.instance.LoadMutations();
    }

    public void Tutorial()
    {
        Instantiate(clickSFX);
        GameManager.instance.LoadTutorial();
    }

    public void Exit()
    {
        Instantiate(clickSFX);
        GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
        StartCoroutine(ExitCor());
    }

    IEnumerator ExitCor()
    {
        Instantiate(clickSFX);
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }

    public void Hover()
    {
        Instantiate(hoverSFX);
    }
}
