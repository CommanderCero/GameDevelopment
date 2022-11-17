using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextPulse : MonoBehaviour
{
    public TMP_Text Text;
    public float PulseDurationSeconds = 5;

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time, PulseDurationSeconds);
        // Reverse t since we want to start from 1 go to 0 and then back to 1
        Text.alpha = 1 - t;
    }
}
