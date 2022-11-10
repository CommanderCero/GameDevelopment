using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    public Canvas GUICanvas;

    void Start()
    {

    }

    void OnTriggerEnter2D (Collider2D col)
    {
       // if (col.gameObject.CompareTag("Player"))
        {
            GUICanvas.gameObject.SetActive(true);
        }
    }

    void Update()
    {

    }
}
