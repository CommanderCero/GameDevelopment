using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int HealthPoints { get; private set; }
    public int MaxHealthPoints = 3;
    public float RecoverTime = 1f;

    private float recoverTimer = 0;

    // Update is called once per frame
    void Update()
    {
        recoverTimer -= Time.deltaTime;
        if(recoverTimer < 0)
        {
            HealthPoints = MaxHealthPoints;
            if(recoverTimer > 0.01)
            {
                Debug.Log("Recovered");
            }
        }
    }

    public void Damage()
    {
        HealthPoints--;
        recoverTimer = RecoverTime;
        Debug.Log(HealthPoints);
        if (HealthPoints == 0)
        {
            LevelManager.Instance.KillPlayer();
        }
    }
}
