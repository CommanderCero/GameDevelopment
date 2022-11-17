using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.WSA;
using static Unity.VisualScripting.Member;

public class TurretController : MonoBehaviour
{
    public PlayerHealth Target;
    public Bullet Projectile;
    public float projectileSpeed;

    [Header("Vision")]
    public float VisionRadiusDegrees = 45;
    public float VisionRange = 10f;
    public Light2D LightCone;

    [Header("Patrol")]
    public float angleStart = 45;
    public float angleEnd = 130;
    public float durationSeconds = 5;
    public float startTime;

    [Header("Attack")]
    public float TurnRate = 0.1f;
    public float ShootDelay = 0.05f;
    public float WarningTime = 0.5f;
    public Color WarningColor;
    public Color AttackColor;

    [Header("Sound")]
    public AudioClip ShotSound;
    public float ShotVol;
    public AudioClip AlarmClip;
    public float AlarmVol;

    private void Start()
    {
        startTime = Time.time;
        StartCoroutine(nameof(PatrolCoroutine));
    }

    IEnumerator PatrolCoroutine()
    {
        float deltaAngle = Mathf.Abs(angleEnd - angleStart) / durationSeconds;
        while(!CanSeeTarget())
        {
            float delta = Time.time - startTime;
            // Limit value between 0 - 2 * durationSeconds
            float tripTime = Mathf.Repeat(delta, durationSeconds * 2.0f);

            float t;
            if(tripTime < durationSeconds)
            {
                // tripTime is between 0 and durationSeconds
                t = (tripTime / durationSeconds);
            }
            else
            {
                // tripTime is between durationSeconds and 2 * durationSeconds
                t = 2.0f - (tripTime / durationSeconds);
            }
            float newAngle = Mathf.SmoothStep(angleStart, angleEnd, t);
            float angleStepSize = Mathf.Abs(newAngle - transform.rotation.eulerAngles.z);
            float angleStepDir = Mathf.Sign(newAngle - transform.rotation.eulerAngles.z);
            transform.rotation *= Quaternion.Euler(0, 0, angleStepDir * Mathf.Min(deltaAngle * Time.deltaTime, angleStepSize));

            yield return null;
        }

        StartCoroutine(nameof(AttackCoroutine));
    }

    IEnumerator AttackCoroutine()
    {
        LightCone.color = WarningColor;
        AudioManager.Instance.PlayOneShot(AlarmClip, AlarmVol);
        float timePassed = 0;
        while(timePassed <= WarningTime)
        {
            transform.up = Vector2.Lerp(transform.up, Target.transform.position - transform.position, TurnRate * Time.deltaTime);
            yield return null;
            if(!CanSeeTarget())
            {
                LightCone.color = Color.white;
                StartCoroutine(nameof(PatrolCoroutine));
                yield break;
            }
            timePassed += Time.deltaTime;
        }

        LightCone.color = AttackColor;
        float shootTimer = ShootDelay;
        while(CanSeeTarget())
        {
            transform.up = Vector2.Lerp(transform.up, Target.transform.position - transform.position, TurnRate * Time.deltaTime);

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                AudioManager.Instance.PlayOneShot(ShotSound, ShotVol);
                Shoot();
                shootTimer = ShootDelay;
            }
            
            yield return null;
        }
        LightCone.color = Color.white;

        StartCoroutine(nameof(PatrolCoroutine));
    }

    public bool CanSeeTarget()
    {
        Vector2 dir = Target.transform.position - transform.position;
        var angle = Mathf.Abs(Vector2.Angle(dir, transform.up));
        if (angle <= VisionRadiusDegrees / 2 && dir.magnitude <= VisionRange)
        {
            // Target is inside the cone, check if we can see it using a raycast
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, dir, VisionRange);
            if (rayInfo)
            {
                if (rayInfo.collider.gameObject == Target.gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Shoot()
    {
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, transform.up);
        if(rayInfo)
        {
            if (rayInfo.collider.gameObject == Target.gameObject)
            {
                Target.Damage();
            }

            var bullet = Instantiate(Projectile, transform.position, Quaternion.identity);
            bullet.Start = transform.position;
            bullet.End = rayInfo.point;
        }
    }

    private void DrawDebugLines()
    {
        Vector3 leftArcEnd = Quaternion.Euler(0, 0, VisionRadiusDegrees / 2f) * transform.up;
        Vector3 rightArcEnd = Quaternion.Euler(0, 0, -VisionRadiusDegrees / 2f) * transform.up;

        // Draw the outside of the arc + Our forward vector
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);
        Debug.DrawLine(transform.position, transform.position + leftArcEnd * VisionRange, Color.green);
        Debug.DrawLine(transform.position, transform.position + rightArcEnd * VisionRange, Color.green);
    }
}