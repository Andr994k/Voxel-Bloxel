using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action OnLeftMouseClick, OnRightMouseClick;
    //get; private set; makes the variable publicly readable, but dissallows editing from outside the class of which it is contained in
    //This means that only PlayerInput can change the value of it
    public bool RunningPressed { get; private set; }
    public Vector3 MovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool IsJumping { get; private set; }

    void Update()
    {
        GetMouseClick();
        GetMousePosition();
        GetMovementInput();
        GetJumpInput();
        GetRunInput();
    }
    private void GetMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            OnLeftMouseClick?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseClick?.Invoke();
        }
    }
    private void GetMousePosition()
    {
        MousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void GetMovementInput()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void GetJumpInput()
    {
        IsJumping = Input.GetButton("Jump");
    }
    private void GetRunInput()
    {
        RunningPressed = Input.GetKey(KeyCode.LeftShift);
    }

}
