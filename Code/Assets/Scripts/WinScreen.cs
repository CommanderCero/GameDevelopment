using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour

{
    public Canvas GUICanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButtonPress()
    {
        GUICanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
