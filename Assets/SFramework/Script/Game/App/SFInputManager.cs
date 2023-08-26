using SFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;

public class FSInputManager : SSingleton<FSInputManager>
{
    //为了保证对内统一不论新老系统统一输入规则
    protected const string axisHorizontal = "Horizontal";
    protected const string axisVertical = "Vertical";

    public Vector2 PrimaryMovement { get { return _primaryMovement; } }
    private Vector2 _primaryMovement = Vector2.zero;
    public bool SmoothMovement = true;
    [Tooltip("set this to false to prevent the InputManager from reading input")]
    public bool InputDetectionActive = true;

    public virtual void SetMovement()
    {
        if (InputDetectionActive)
        {
            if (SmoothMovement)
            {
                _primaryMovement.x = Input.GetAxis(axisHorizontal);
                _primaryMovement.y = Input.GetAxis(axisVertical);
            }
            else
            {
                _primaryMovement.x = Input.GetAxisRaw(axisHorizontal);
                _primaryMovement.y = Input.GetAxisRaw(axisVertical);
            }
        }
    }
    public virtual void SetMovement(Vector2 movement)
    {
        if (InputDetectionActive)
        {
            _primaryMovement.x = movement.x;
            _primaryMovement.y = movement.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetMovement();
    }
}
