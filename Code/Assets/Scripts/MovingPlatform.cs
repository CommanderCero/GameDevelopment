using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public float TripDuration;

    private Rigidbody2D platform;
    private Rigidbody2D player;

    private void OnValidate()
    {
        transform.position = (Vector3)StartPosition;
    }

    private void Start()
    {
        platform = GetComponent<Rigidbody2D>();
        transform.position = (Vector3)StartPosition;
    }

    void FixedUpdate()
    {
        float tripTime = Mathf.Repeat(Time.time, TripDuration * 2);
        float t;
        if (tripTime < TripDuration)
        {
            // tripTime is between 0 and TripDuration
            t = (tripTime / TripDuration);
        }
        else
        {
            // tripTime is between TripDuration and 2 * TripDuration
            t = 2.0f - (tripTime / TripDuration);
        }

        var newPosition = new Vector2(Mathf.SmoothStep(StartPosition.x, EndPosition.x, t), Mathf.SmoothStep(StartPosition.y, EndPosition.y, t));
        var delta = newPosition - (Vector2)transform.position;
        platform.MovePosition(newPosition);
        //if(player != null)
        //{
        //    Debug.Log("AddForce");
        //    player.Move
        //    player.AddForce(delta * 10);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartPosition, EndPosition);
    }
}
