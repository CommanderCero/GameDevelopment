using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public ParticleSystem DeathParticlesPrefab;
    public AudioClip DeathSoundEffect;
    public float DeathVolume;
    public PlayerController PlayerInstance;

    public void Awake()
    {
        // Singletone
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void KillPlayer()
    {
        StartCoroutine(nameof(KillCoroutine));
    }

    public void CompleteLevel()
    {
    }

    private IEnumerator KillCoroutine()
    {
        PlayerInstance.gameObject.SetActive(false);
        var particles = Instantiate(DeathParticlesPrefab, PlayerInstance.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayOneShot(DeathSoundEffect, DeathVolume);

        // Wait for animation to end
        while(particles.isPlaying)
        {
            yield return null;
        }
        Destroy(particles.gameObject);

        PlayerInstance.gameObject.SetActive(true);
        RespawnManager.Instance.LoadCurrentCheckpoint();
    }
}
