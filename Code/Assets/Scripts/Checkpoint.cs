using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int ID = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ugly workaround to prevent triggering the method twice (for two colliders)
        if(RespawnManager.Instance.CurrentCheckpoint.ID != ID && collision.gameObject.tag == "Player")
        {
            RespawnManager.Instance.ActivateCheckpoint(this);
        }
        
    }
}
