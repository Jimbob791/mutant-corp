using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoHelper : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] TextMeshProUGUI helpText;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 ratios = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height);

        mousePos.x = (ratios.x * 1920) - 960;
        mousePos.y = (ratios.y * 1080) - 540;

        mousePos.z = 0;
        mousePos.y -= 30;
        mousePos.x -= 100;

        transform.localPosition = mousePos;
    }

    public void SetText(string text)
    {
        helpText.text = text;
        GetComponent<Image>().enabled = true;
    }
    public void Hide()
    {
        helpText.text = "";
        GetComponent<Image>().enabled = false;
    }
}
