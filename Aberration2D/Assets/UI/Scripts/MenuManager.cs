using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    [SerializeField] RawImage background;
    [SerializeField] GameObject hoverSFX;
    [SerializeField] GameObject clickSFX;

    [Space]

    [SerializeField] GameObject mainCanvas;
    [SerializeField] TextMeshProUGUI easyTime;
    [SerializeField] TextMeshProUGUI medTime;
    [SerializeField] TextMeshProUGUI hardTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("EasyTime"))
        {
            easyTime.text = ConvertTimeToString(PlayerPrefs.GetFloat("EasyTime"));
        }
        else
        {
            easyTime.text = "No Runs";
        }
        if (PlayerPrefs.HasKey("MedTime"))
        {
            medTime.text = ConvertTimeToString(PlayerPrefs.GetFloat("MedTime"));
        }
        else
        {
            medTime.text = "No Runs";
        }
        if (PlayerPrefs.HasKey("HardTime"))
        {
            hardTime.text = ConvertTimeToString(PlayerPrefs.GetFloat("HardTime"));
        }
        else
        {
            hardTime.text = "No Runs";
        }


    }

    private string ConvertTimeToString(float time)
    {
        float hours = Mathf.FloorToInt(time / 3600);
        float minutes = Mathf.FloorToInt((time / 60) % 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliSeconds = Mathf.FloorToInt((time % 1) * 1000);
 
        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliSeconds.ToString("00");
    }

    void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(scrollSpeed, 0) * Time.deltaTime, background.uvRect.size);
    }

    public void NewGame()
    {
        Instantiate(clickSFX);
        
        mainCanvas.GetComponent<Animator>().SetBool("choosing", true);
    }

    public void ExitNew()
    {
        Instantiate(clickSFX);
        
        mainCanvas.GetComponent<Animator>().SetBool("choosing", false);
    }

    public void Easy()
    {
        Instantiate(clickSFX);
        GameManager.instance.mode = "EasyTime";       
        GameManager.instance.LoadMutations();
    }

    public void Med()
    {
        Instantiate(clickSFX);
        GameManager.instance.mode = "MedTime";       
        GameManager.instance.LoadMutations();
    }

    public void Hard()
    {
        Instantiate(clickSFX);
        GameManager.instance.mode = "HardTime";       
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
