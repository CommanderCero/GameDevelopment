using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBoxTrigger : MonoBehaviour
{
    public Canvas GUICanvas;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
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