using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputShow : MonoBehaviour
{
    public Image leftShoulder, rightShoulder, south, north, west, east;
    public Color defaultColor, PressColor;

    public void PressJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            south.color = PressColor;
    }

    public void ReleaseJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            south.color = defaultColor;
    }

    public void PressDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            west.color = PressColor;
    }

    public void ReleaseDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            west.color = defaultColor;
    }
    public void PressInteract(InputAction.CallbackContext context)
    {
       
        if (context.performed)
        {
             Debug.Log("PressInteract");
            north.color = PressColor;

        }    
    }

    public void ReleaseInteract(InputAction.CallbackContext context)
    {
       
        if (context.performed)
        {
             Debug.Log("ReleaseInteract");
             north.color = defaultColor;
        }
            
    }
    public void PressSquat(InputAction.CallbackContext context)
    {
        if (context.performed)
            east.color = PressColor;
    }

    public void ReleaseSquat(InputAction.CallbackContext context)
    {
        if (context.performed)
            east.color = defaultColor;
    }
    public void PressDashShoulder(InputAction.CallbackContext context)
    {
        if (context.performed)
            rightShoulder.color = PressColor;
    }

    public void ReleaseDashShoulder(InputAction.CallbackContext context)
    {
        if (context.performed)
            rightShoulder.color = defaultColor;
    }
    public void PressClimb(InputAction.CallbackContext context)
    {
        if (context.performed)
            leftShoulder.color = PressColor;
    }

    public void ReleaseClimb(InputAction.CallbackContext context)
    {
        if (context.performed)
            leftShoulder.color = defaultColor;
    }
}
