using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



[Serializable]
public struct InputData
{
    public float hMovement;
    public float vMovement;
    public float verticalMouse;
    public float horizontalMouse;
    public bool dash;
    public bool jump;
    public bool press;

    public void GetInput()
    {
        hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");
        horizontalMouse = Input.GetAxis("Mouse Y");
        horizontalMouse = Input.GetAxis("Mouse X");
        dash = Input.GetButton("Dash");
        jump = Input.GetButtonDown("Jump");
        press = Input.GetButtonDown("Press");

    }
}

