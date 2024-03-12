using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> songs = new List<AudioClip>();
    
    IEnumerator Start()
    {
        AudioSource src = GetComponent<AudioSource>();
        src.clip = songs[Random.Range(0, songs.Count)];
        src.Play();
        float startVolume = src.volume;
        src.volume = 0;

        float duration = 0;
        while (duration < 2f)
        {
            duration += Time.deltaTime;
            src.volume = (duration / 2) * startVolume;
            yield return null;
        }
    }
}
