using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform GroundCheck;
    public LayerMask GroundLayers;

    public float WalkSpeed = 10f;
    public float RunSpeed = 15f;

    public float JumpHeight = 2f;
    public float JumpTimeSeconds = 2f;

    private Rigidbody2D rig;
    private Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        RespawnManager.Instance.OnCheckpointLoaded += OnCheckpointLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        Move(Input.GetAxis("Horizontal"));

        //bool grounded = GroundCollider.
        ////Debug.Log(grounded);
        //
        //var deltaX = Input.GetAxis("Horizontal");
        //var deltaY = -9.81f;
        //if(Input.GetKeyUp(KeyCode.Space) && GroundCollider.IsColliding)
        //{
        //    // Jump
        //    rig.velocity += new Vector2(0, 10);
        //    Debug.Log($"You Jumped {deltaY}");
        //}
        //
        //rig.velocity = new Vector2(deltaX * WalkSpeed, rig.velocity.y + deltaY * Time.deltaTime);
    }

    private void Move(float moveDir)
    {
        bool isGrounded = false;
        foreach(var collider in Physics2D.OverlapCircleAll(GroundCheck.position, 0.5f, GroundLayers))
        {
            if (collider.gameObject != gameObject)
            {
                isGrounded = true;
                break;
            }
        }
        if(!isGrounded)
        {
            return;
        }

        Vector2 targetVelocity = new Vector2(WalkSpeed * moveDir, rig.velocity.y);
        // Smoothing
        targetVelocity = Vector2.SmoothDamp(rig.velocity, targetVelocity, ref velocity, 0.05f);
        rig.velocity = targetVelocity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpForce = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * rig.gravityScale * JumpHeight);
            rig.velocity += new Vector2(0f, jumpForce);
        }
    }

    // Respawning
    private void OnDestroy()
    {
        RespawnManager.Instance.OnCheckpointLoaded -= OnCheckpointLoaded;
    }

    private void OnCheckpointLoaded(Checkpoint loadedCheckpoint)
    {
        rig.velocity = Vector2.zero;
        transform.position = loadedCheckpoint.transform.position;
    }
}
