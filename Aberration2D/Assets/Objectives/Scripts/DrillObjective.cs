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
    [SerializeField] bool tutorial = false;
    [SerializeField] GameObject startSFX;
    [SerializeField] GameObject loopSFX;
    [SerializeField] GameObject endSFX;
    GameObject loop;

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
                if (tutorial)
                    GameManager.instance.LoadMenu();
                else
                {
                    Player.instance.GetComponent<PlayerHealth>().exiting = true;
                    GameManager.instance.LoadMutations();
                }
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
            GetComponent<ObjectiveSpawner>().SpawnWave();
            GameObject.Find("MainMusic").GetComponent<AudioSource>().clip = GameManager.instance.drillMusic.GetComponent<MainMusic>().songs[0];
            GameObject.Find("MainMusic").GetComponent<AudioSource>().Play();
            active = true;
            Instantiate(startSFX);
            StartCoroutine(DrillSound());
            dust.Play();
            GetComponent<Animator>().enabled = true;
        }
    }

    IEnumerator DrillSound()
    {
        yield return new WaitForSeconds(startSFX.GetComponent<AudioSource>().clip.length);
        loop = Instantiate(loopSFX);
        while (complete == false)
        {
            yield return null;
        }
        Destroy(loop);
        Instantiate(endSFX);
    }
}
