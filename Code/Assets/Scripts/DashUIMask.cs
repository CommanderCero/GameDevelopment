using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUIMask : MonoBehaviour
{
    public Image DashMask;
    public PlayerController Player;

    void Update()
    {
        DashMask.fillAmount = Player.DashCooldownTimer / Player.DashCooldown;        
    }
}
