using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNum : MonoBehaviour
{
    public int damage;
    public TextMeshProUGUI display;
    public float displayTime;

    void Start()
    {
        display.text = damage.ToString();
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-20.5f, 20.5f));
        Destroy(this.gameObject, displayTime);
    }
}
