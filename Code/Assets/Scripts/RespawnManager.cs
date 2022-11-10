using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }
    public Checkpoint CurrentCheckpoint;

    // Event for loading a checkpoint
    public delegate void CheckpointLoadedHandler(Checkpoint loadedCheckpoint);
    public event CheckpointLoadedHandler OnCheckpointLoaded;

    private Checkpoint[] Checkpoints;

    // Start is called before the first frame update
    void Awake()
    {
        // Singletone
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        Checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();
        // Sort checkpoints and assign ID's for easy identification
        Array.Sort(Checkpoints, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        for(int i = 0; i < Checkpoints.Length; i++)
        {
            Checkpoints[i].ID = i;
        }

        CurrentCheckpoint = Checkpoints[0];
    }

    public void LoadCurrentCheckpoint()
    {
        if(OnCheckpointLoaded != null)
            OnCheckpointLoaded(CurrentCheckpoint);
    }

    public void ActivateCheckpoint(Checkpoint c)
    {
        Debug.Log($"Activated Checkpoint {c.ID}");
        // Disable so it won't get activated again
        CurrentCheckpoint.gameObject.SetActive(false);
        CurrentCheckpoint = c;
    }
}
