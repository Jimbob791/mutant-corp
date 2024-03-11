using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public int data;
    [SerializeField] TextMeshProUGUI display;

    int currentData = 0;

    void FixedUpdate()
    {
        if (currentData < data)
        {
            currentData += 1;
        }
        else if (currentData > data)
        {
            currentData -= 1;
        }
        display.text = currentData.ToString();
    }

    public void AddData(int amount)
    {
        data += amount;
    }
}
