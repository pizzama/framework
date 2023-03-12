using UnityEngine;

public class TransformProxy : MoveProxy<Transform>
{
    public float Gravity = -9.8f;
    public override void MoveByTarget(Vector3 mousePosition)
    {
        movementVelocity = mousePosition;
        if (movementVelocity != Vector3.zero)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(movementVelocity);
            playerControl.position = Vector3.MoveTowards(playerControl.position, position, speed * Time.deltaTime);
            Vector3 relativePos = position - playerControl.position;
            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            playerControl.rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);
        }
    }

    public override void MoveByInput(float inputX, float inputY)
    {
        // float inputX = Input.GetAxisRaw("Horizontal");// JoystickManager.Instance.Horizontal;
        // float inputY = Input.GetAxisRaw("Vertical");//JoystickManager.Instance.Vertical;
        if (!Mathf.Approximately(inputX, 0f) || !Mathf.Approximately(inputY, 0f))
        {
            movementVelocity.Set(inputX, 0, inputY);
            float inputMagnitue = Mathf.Clamp01(movementVelocity.magnitude);
            movementVelocity.Normalize();

            // 几种位移方法
            // transform.position += anim.deltaPosition; //1
            playerControl.Translate(movementVelocity * speed * inputMagnitue * Time.deltaTime, Space.World); //2

            // transform.position = Vector3.MoveTowards(transform.position, anim.velocity, currentSpeed * Time.deltaTime); //3.以固定的速度到达
            // Vector3 refvector = new Vector3(anim.velocity.x, anim.velocity.y, anim.velocity.z);//4.以固定的速度到达， 平滑
            // transform.position = Vector3.SmoothDamp(transform.position, anim.velocity, ref refvector, Time.deltaTime, currentSpeed); 
            // _rid.velocity = new Vector3(anim.velocity.x, _rid.velocity.y, anim.velocity.z); // 5.

            // 几种旋转的方法 需要对x轴座关注旋转 不能用LookRotation 或LookAt
            // 1.由于人的起始位置不是0，0 方向。需要先旋转 y -90度
            // float angle = Mathf.Atan2(_movementVelocity.y, _movementVelocity.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // 2.另一种处理方式
            if (movementVelocity != Vector3.zero)
            {
                //朝向向量也可以通过forward = Vector3.Cross(Vector3.right, Vector3.up);
                Quaternion toRatation = Quaternion.LookRotation(movementVelocity, Vector3.up);
                // Quaternion toRatation = Quaternion.FromToRotation(Vector3.up, _movementVelocity);
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRatation, rotateSpeed * Time.deltaTime);
                playerControl.rotation = Quaternion.Lerp(playerControl.rotation, toRatation, rotateSpeed * Time.deltaTime);
            }
        }
    }
}