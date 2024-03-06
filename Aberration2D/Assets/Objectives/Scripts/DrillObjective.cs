using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrillObjective : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objective;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Slider slider;
    [SerializeField] float timeToActivate;
    [SerializeField] ParticleSystem dust;

    float progress = 0;
    bool active = false;
    public bool complete = false;
    Vector3 pos;

    void Start()
    {
        objective.text = "Locate Broken Drill";
        dust.Stop();
        pos = transform.position;
    }

    void Update()
    {
        if (complete)
        {
            objective.text = "Enter Drill to Proceed";
            if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1.5f), 0, Vector2.zero, 0, playerLayer))
            {
                GameManager.instance.LoadMutations();
            }
            return;
        }

        if (active)
        {
            progress += Time.deltaTime;
            transform.localPosition = pos + new Vector3(Random.Range(-0.05f, 0.05f), 0, 0);

            if (progress > timeToActivate)
            {
                complete = true;
                return;
            }

            slider.value = progress / timeToActivate;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1.5f), 0, Vector2.zero, 0, playerLayer))
        {
            objective.text = "Drilling - Stay Alive";
            active = true;
            dust.Play();
            GetComponent<Animator>().enabled = true;
        }
    }
}
