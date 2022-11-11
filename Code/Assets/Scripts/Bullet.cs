using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Start;
    public Vector2 End;
    public float time = 0.5f;

    private float totalTime = 0;
    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        transform.position = Vector2.Lerp(Start, End, totalTime / time);

        if(totalTime >= time)
        {
            Destroy(this.gameObject);
        }
    }
}
