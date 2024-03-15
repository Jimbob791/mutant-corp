using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numKilled;
    [SerializeField] TextMeshProUGUI damageDone;
    [SerializeField] TextMeshProUGUI stagesDone;
    [SerializeField] TextMeshProUGUI numItems;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI timeTextBacking;
    [SerializeField] TextMeshProUGUI win;
    [SerializeField] TextMeshProUGUI winBacking;

    public void LoadMenu()
    {
        GameManager.instance.LoadMenu();
    }

    void Start()
    {
        numKilled.text = "Enemies Killed: " + GameManager.instance.numEnemiesKilled.ToString();
        damageDone.text = "Damage Dealt: " + GameManager.instance.damageDealt.ToString();
        if (GameManager.instance.won)
            stagesDone.text = "Stages Completed: " + (GameManager.instance.difficulty).ToString();
        else
            stagesDone.text = "Stages Completed: " + (GameManager.instance.difficulty - 1).ToString();
        numItems.text = "Items Picked Up: " + GameManager.instance.numItems.ToString();
        timeText.text = ConvertTimeToString(GameManager.instance.globalTimer);
        timeTextBacking.text = ConvertTimeToString(GameManager.instance.globalTimer);
        win.text = GameManager.instance.won ? "SUCCESS" : "FAILURE";
        winBacking.text = GameManager.instance.won ? "SUCCESS" : "FAILURE";
    }

    private string ConvertTimeToString(float time)
    {
        float hours = Mathf.FloorToInt(time / 3600);
        float minutes = Mathf.FloorToInt((time / 60) % 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliSeconds = Mathf.FloorToInt((time % 1) * 1000);
 
        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliSeconds.ToString("00");
    }
}
