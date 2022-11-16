using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public bool prevIsGrounded { get; private set; }
    public bool IsDashing { get; private set; }

    [Header("Movement")]
    public float RunMaxSpeed = 10f;
    public float AccelerationForce = 7;
    public float AirAccelerationRate = 0.5f;
    public float FrictionAmount = 0.2f;

    [Header("Jumping")]
    public float GravityScale = 3f;
    public float MaxJumpHeight = 5f;
    public float JumpCacheTime = 0.15f;
    public float CoyoteTime = 0.15f;

    [Header("Ground Check")]
    public Vector2 GroundOffset;
    public Vector2 GroundCheckSize;
    public LayerMask GroundLayers;

    [Header("Dash")]
    public float DashSpeed = 25f;
    public float DashTime = 0.15f;
    public float DashCoyoteTime = 0.15f;
    public float DashCooldown = 3f;

    //Sounds 
    [Header("Sound")]
    public float DashVol; // volume for sound effects                
    public AudioClip DashSound; // get audioclip 
    public float JumpVol;
    public AudioClip JumpSound; // get audioclip 
    public float LandVol;
    public AudioClip LandSound; // get audioclip 
    public float FootVol;
    //private AudioSource sounds = GameObject.GetComponents<AudioSource>();


    private Rigidbody2D rig;
    // Cache results from Update for use in FixedUpdate
    private float horizontalInput = 0;
    private float groundTimer = -1;
    private float jumpTimer = -1;
    private float dashTimer = -1;
    private bool canDash = true;
    
    private AudioSource source;
   


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = GravityScale;
        RespawnManager.Instance.OnCheckpointLoaded += OnCheckpointLoaded;

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsDashing)
        {
            return;
        }

        // Check if we are grounded
        groundTimer -= Time.deltaTime;
        prevIsGrounded = IsGrounded; //store previous grounded var
        IsGrounded = false;
        if (Mathf.Abs(rig.velocity.y) <= 0.01f)
        {
            foreach (var collider in Physics2D.OverlapBoxAll(GroundOffset + (Vector2)transform.position, GroundCheckSize, GroundLayers))
            {
                if (collider.gameObject != gameObject)
                {
                    IsGrounded = true;
                    groundTimer = CoyoteTime;
                    break;
                }
            }
        }

        if (IsGrounded == true && prevIsGrounded == false)
        {
            source.PlayOneShot(LandSound, LandVol);
            Debug.Log("Landing");
        }

        // Collect inputs
        horizontalInput = Input.GetAxis("Horizontal");

        jumpTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = JumpCacheTime;
        }

        dashTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTimer = DashCoyoteTime;
        }
    }

    void FixedUpdate()
    {
        if(IsDashing)
        {
            return;
        }

        Move();
        TryJump();
        TryDash();
        ApplyGroundFriction();
    }

    private void Move()
    {
        // Compute target speed
        float targetSpeed = horizontalInput * RunMaxSpeed;

        // How much should we accelerate?
        float accelRate = IsGrounded ? AccelerationForce : AccelerationForce * AirAccelerationRate;
        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rig.velocity.x;
        //Calculate force along x-axis to apply to the player
        float movement = speedDif * accelRate;
        //Convert this to a vector and apply to rigidbody
        rig.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void TryJump()
    {
        if (groundTimer > 0 && jumpTimer > 0)
        {
            source.PlayOneShot(JumpSound, JumpVol);
            float jumpForce = Mathf.Sqrt(MaxJumpHeight * Physics.gravity.y * rig.gravityScale * -2f);
            rig.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

            jumpTimer = 0f;
            IsGrounded = false;
        }
    }

    private void TryDash()
    {
        if (canDash && dashTimer >= 0 && Mathf.Abs(horizontalInput) >= 0.01f)
        {
            source.PlayOneShot(DashSound, DashVol); 
            dashTimer = 0;
            IsDashing = true;

            canDash = false;
            StartCoroutine(nameof(DashCoroutine), Vector2.right * horizontalInput);
        }
    }

    private IEnumerator DashCoroutine(Vector2 dir)
    {
        dir = dir.normalized;

        // Dash in the desired direction
        rig.gravityScale = 0;
        float dashStart = Time.time;
        while (Time.time - dashStart <= DashTime)
        {
            rig.velocity = dir * DashSpeed;
            yield return null;
        }

        // Deactivate dash
        rig.gravityScale = GravityScale;
        IsDashing = false;

        // Wait for cooldown
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    private void ApplyGroundFriction()
    {
        if (IsGrounded && Mathf.Abs(horizontalInput) <= 0.01f)
        {
            float frictionAmount = Mathf.Min(Mathf.Abs(rig.velocity.x), FrictionAmount);
            frictionAmount *= Mathf.Sign(rig.velocity.x);
            rig.AddForce(-frictionAmount * Vector2.right, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Display ground check area in editor
        UnityEditor.Handles.color = IsGrounded ? Color.red : Color.blue;
        UnityEditor.Handles.DrawWireCube((Vector2)transform.position + GroundOffset, GroundCheckSize);
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
