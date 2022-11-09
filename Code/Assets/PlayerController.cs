using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Grounded GroundCollider;

    public float WalkSpeed = 20f;
    public float RunSpeed = 20f;

    public float JumpHeight = 2f;
    public float JumpTimeSeconds = 2f;

    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //bool grounded = GroundCollider.
        ////Debug.Log(grounded);
        //
        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = -9.81f;
        if(Input.GetKeyUp(KeyCode.Space) && GroundCollider.IsColliding)
        {
            // Jump
            rig.velocity += new Vector2(0, 10);
            Debug.Log($"You Jumped {deltaY}");
        }

        rig.velocity = new Vector2(deltaX * WalkSpeed, rig.velocity.y + deltaY * Time.deltaTime);
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    RemoveOverlap(collision);
    //}
    //
    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    RemoveOverlap(collision);
    //}
    //
    //void RemoveOverlap(Collision2D collision)
    //{
    //    Debug.Log("I collided");
    //    // If we're filtering out the collider we hit then ignore it.
    //    //if (m_MovementFilter.IsFilteringLayerMask(collision.collider.gameObject))
    //    //    return;
    //
    //    // Calculate the collider distance.
    //    var colliderDistance = Physics2D.Distance(collision.otherCollider, collision.collider);
    //
    //    // If we're overlapped then remove the overlap.
    //    // NOTE: We could also ensure we move out of overlap by the contact offset here.
    //    if (colliderDistance.isOverlapped)
    //        collision.otherRigidbody.position += colliderDistance.normal * colliderDistance.distance;
    //}
}
