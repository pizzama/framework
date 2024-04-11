using UnityEngine;

namespace SFramework.CameraUtils
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private SEnum.MouseClick _mouseClick = SEnum.MouseClick.Left;
        public Transform target;

        public float XSpeed = 200;
        public float YSpeed = 200;
        public float MSpeed = 10;
        public float YMinLimit = -50;
        public float YMaxLimit = 50;
        public float Distance = 10;
        public float MinDistance = 2;
        public float MaxDistance = 30;
        public bool NeedDamping = true;
        public float Damping = 5;
        public float X = 0;
        public float Y = 0;

        private void Start()
        {
            Vector3 angles = transform.eulerAngles;
            X = angles.x;
            Y = angles.y;
        }

        private void LateUpdate()
        {
            if (target)
            {
                if (Input.GetMouseButton((int)_mouseClick))
                {
                    X += Input.GetAxis("Mouse X") * XSpeed * 0.02f;
                    Y -= Input.GetAxis("Mouse Y") * XSpeed * 0.02f;
                    Y = clampAngle(Y, YMinLimit, YMaxLimit);
                }

                Distance -= Input.GetAxis("Mouse ScrollWheel") * MSpeed;
                Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
                Quaternion rotation = Quaternion.Euler(Y, X, 0.0f);
                Vector3 disVector = new Vector3(0.0f, 0.0f, -Distance);
                Vector3 position = rotation * disVector + target.position;
                // adjust the camera
                if (NeedDamping)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Damping);
                    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * Damping);
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }

        private float clampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}