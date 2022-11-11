using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class TurretController : MonoBehaviour
{
    public Transform Target;
    public Bullet Projectile;
    public Transform Agent; // turrret
    public float projectileSpeed;


    public float TurnRate = 0.5f;
    public float VisionRadiusDegrees = 45;
    public float VisionRange = 10f;
    bool Detected = false; 



    // Update is called once per frame
    void Update()
    {

        // Stuff for raycasting (later)
        Vector2 targetPos = Target.position;
  

        // Rotate the turret such that it's UP-Vector points to the target
        transform.up = Vector2.Lerp(transform.up, Target.position - transform.position, TurnRate * Time.deltaTime);

        // Check if the target is inside the cone
        Vector2 dir = Target.position - transform.position;
        var angle = Mathf.Abs(Vector2.Angle(dir, transform.up));
        Detected = false;
        if (angle <= VisionRadiusDegrees / 2 && dir.magnitude <= VisionRange)
        {

            // The target is inside the cone!
            Debug.Log($"Inside the cone {angle}");

            // Raycastto Character 
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, dir, VisionRange);


            // determine if in alarmed/firing state or resting state 
            if (rayInfo)
            {
                if (rayInfo.collider.gameObject.tag == "Player")
                {
                    Detected = true;
                }
            }

        }

        //Shooting Player

        if (Detected)
        {
            RaycastHit2D rayInfo2 = Physics2D.Raycast(transform.position, transform.up);
            if (rayInfo2.collider.gameObject.tag == "Player") {
                Debug.Log($"I'm dead lol");
            }

            else
            {
               // Debug.Log($"I'm not dead");
            }

            var bullet = Instantiate(Projectile, Agent.position, Agent.rotation) as Bullet;
            bullet.Start = transform.position;
            bullet.End = rayInfo2.point;
        }
            

        DrawDebugLines();
    }

    private void DrawDebugLines()
    {
        var color = Detected ? Color.red : Color.green;

        Vector3 leftArcEnd = Quaternion.Euler(0, 0, VisionRadiusDegrees / 2f) * transform.up;
        Vector3 rightArcEnd = Quaternion.Euler(0, 0, -VisionRadiusDegrees / 2f) * transform.up;

        // Draw the outside of the arc + Our forward vector
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);
        Debug.DrawLine(transform.position, transform.position + leftArcEnd * VisionRange, color);
        Debug.DrawLine(transform.position, transform.position + rightArcEnd * VisionRange, color);
    }
}