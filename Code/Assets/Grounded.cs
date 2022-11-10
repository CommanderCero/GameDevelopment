using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    public bool IsColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        IsColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        IsColliding = false;
    }
}
