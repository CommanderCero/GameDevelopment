using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int ID = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RespawnManager.Instance.ActivateCheckpoint(this);
    }
}
