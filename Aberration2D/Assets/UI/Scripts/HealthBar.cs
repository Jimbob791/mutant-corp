using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float rate;
    [SerializeField] int shakeDuration;
    [SerializeField] float shakeStrength;

    float val;

    Vector3 pos;
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        pos = transform.localPosition;
    }

    void Update()
    {
        slider.value = slider.value + (val - slider.value) * rate;
    }

    public void SetValues(int max, int current)
    {
        slider.maxValue = max;
        val = current;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        for (int i = 0; i < shakeDuration; i++)
        {
            yield return new WaitForSeconds(1/60);
            transform.localPosition = new Vector3(pos.x + Random.Range(-shakeStrength, shakeStrength), pos.y + Random.Range(-shakeStrength, shakeStrength), 0);
        }

        transform.localPosition = pos;
    }
}
