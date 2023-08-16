using SFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;

public class FSInputManager : SSingleton<FSInputManager>
{
    //Ϊ�˱�֤����ͳһ��������ϵͳͳһ�������
    protected const string axisHorizontal = "Horizontal";
    protected const string axisVertical = "Vertical";

    public Vector2 PrimaryMovement { get { return _primaryMovement; } }
    private Vector2 _primaryMovement = Vector2.zero;
    public bool SmoothMovement = true;
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

    // Update is called once per frame
    void Update()
    {
        SetMovement();
    }
}
