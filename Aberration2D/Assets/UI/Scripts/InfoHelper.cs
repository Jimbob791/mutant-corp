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
        mousePos.z = 0;

        mousePos.x -= width / 2;
        mousePos.y -= height / 2;

        transform.position = mousePos;
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
