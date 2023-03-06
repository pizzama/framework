using UnityEngine;
using UnityEngine.AI;

//使用unity的character控制器来控制玩家运动

public class NavMeshAgentProxy : MoveProxy<NavMeshAgent>
{
    public float Gravity = -9.8f;

    public Vector3 GetWorldPositionByRay(Vector3 point, int layerMask)
    {
        RaycastHit rayCastHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(point), out rayCastHit, 1000, layerMask);
        return rayCastHit.point;
    }
    public override void MoveByTarget(Vector3 worldPosition)
    {
        //需要通过射线与平面相交来确定鼠标点击的位置
        movementVelocity = worldPosition;
        movementVelocity.Normalize();
        if (movementVelocity != Vector3.zero)
        {
            playerControl.transform.rotation = Quaternion.LookRotation(movementVelocity);
            // //判断是否需要重力
            // if (playerControl.isGrounded == false)
            //     movementVelocity += Gravity * Vector3.up;
            playerControl.SetDestination(movementVelocity);
        }
    }

    public override void MoveByInput(float inputX, float inputY)
    {
        if (!Mathf.Approximately(inputX, 0f) || !Mathf.Approximately(inputY, 0f))
        {
            movementVelocity.Set(inputX, 0, inputY);
            movementVelocity.Normalize();
            Vector3 mv = movementVelocity * speed * Time.deltaTime;
            if (movementVelocity != Vector3.zero)
                playerControl.transform.rotation = Quaternion.LookRotation(mv);

            // 几种位移方法
            playerControl.Move(mv); //2
        }
        else
        {
            movementVelocity = Vector3.zero;
        }
    }

}