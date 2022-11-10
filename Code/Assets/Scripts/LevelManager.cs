using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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
        RespawnManager.Instance.LoadCurrentCheckpoint();
    }

    public void CompleteLevel()
    {
    }
}
