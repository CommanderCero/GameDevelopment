using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource source;

    public void Awake()
    {
        // Singletone
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        source.PlayOneShot(clip, volume);
    }
}
