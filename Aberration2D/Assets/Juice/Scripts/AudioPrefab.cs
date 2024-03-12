using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPrefab : MonoBehaviour
{
    [SerializeField] float pitchMin = 1;
    [SerializeField] float pitchMax = 1;

    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(pitchMin, pitchMax);
        Destroy(this.gameObject, GetComponent<AudioSource>().clip.length + 1f);
    }
}
