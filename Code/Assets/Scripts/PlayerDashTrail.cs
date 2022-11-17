using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerDashTrail : MonoBehaviour
{
    public TrailRenderer DashTrailRenderer;
    private PlayerController controller;

    void Start()
    {
        DashTrailRenderer.emitting = false;
        RespawnManager.Instance.OnCheckpointLoaded += Instance_OnCheckpointLoaded;
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        DashTrailRenderer.emitting = controller.IsDashing;
    }

    private void Instance_OnCheckpointLoaded(Checkpoint loadedCheckpoint)
    {
        DashTrailRenderer.emitting = false;
    }
}
