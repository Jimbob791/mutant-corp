using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject cover;
    [SerializeField] GameObject resume;
    [SerializeField] GameObject leave;

    bool open = false;

    void Start()
    {
        cover.SetActive(false);
        resume.SetActive(false);
        leave.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            open = !open;
            if (open)
                OpenPause();
            else
                ClosePause();
        }  
    }

    public void OpenPause()
    {
        Time.timeScale = 0;
        if (GameObject.FindGameObjectsWithTag("Laser").Length != 0)
        {
            GameObject.FindGameObjectsWithTag("Laser")[0].GetComponent<AudioSource>().Pause();
        }
        cover.SetActive(true);
        resume.SetActive(true);
        leave.SetActive(true);
    }

    public void ClosePause()
    {
        Time.timeScale = 1;
        if (GameObject.FindGameObjectsWithTag("Laser").Length != 0)
        {
            GameObject.FindGameObjectsWithTag("Laser")[0].GetComponent<AudioSource>().UnPause();
        }
        cover.SetActive(false);
        resume.SetActive(false);
        leave.SetActive(false);
    }

    public void Leave()
    {
        Time.timeScale = 1;
        GameManager.instance.LoadScore(false);
    }
}
