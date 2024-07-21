using Cinemachine;
using SFramework.Tools;
using UnityEngine;

namespace SFramework.GameCamera
{
    public class CVCameraSystem3D : CVCameraSystem
    {
        protected override void HandleCameraMovement()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                inputDir.z = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDir.z = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDir.x = 1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        protected override void HandleCameraMovementEdgeScrolling()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.mousePosition.x < edgeScrollSize)
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < edgeScrollSize)
            {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - edgeScrollSize)
            {
                inputDir.x = 1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollSize)
            {
                inputDir.z = 1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        protected override void HandleCameraMovementDragPan()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragPanMoveActive = true;
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragPanMoveActive = false;
            }

            if (dragPanMoveActive)
            {
                Vector3 m1 = MapTools.Screen2WorldPos(Input.mousePosition);
                Vector3 m2 = MapTools.Screen2WorldPos(lastMousePosition);
                Vector3 mouseMovementDelta = m2 - m1;
                if (mouseMovementDelta.magnitude > dragThreshold)
                {
                    //使用偏差值计算瞬时速度
                    Vector3 inputDir = mouseMovementDelta / Time.deltaTime;
                    inputDir.z = 0;
                    Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
                    transform.position += moveDir * moveSpeed * Time.deltaTime;
                }
                lastMousePosition = Input.mousePosition;
            }
        }

        protected override void HandleCameraRotation()
        {
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                rotateDir = 1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotateDir = -1f;
            }

            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        }

        protected override void HandleCameraZoom_FieldOfView()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                targetFieldOfView += fieldOfViewStep;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                targetFieldOfView -= fieldOfViewStep;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

            if (virtualCamera != null)
            {
                virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
                    virtualCamera.m_Lens.FieldOfView,
                    targetFieldOfView,
                    Time.deltaTime * zoomSpeed
                );
            }
        }

        protected override void HandleCameraZoom_MoveForward()
        {
            Vector3 zoomDir = followOffset.normalized;
            if (Input.mouseScrollDelta.y > 0)
            {
                followOffset += zoomDir * zoomSpeed;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                followOffset -= zoomDir * zoomSpeed;
            }

            if (followOffset.magnitude < followOffsetMin)
            {
                followOffset = zoomDir * followOffsetMin;
            }

            if (followOffset.magnitude > followOffsetMax)
            {
                followOffset = zoomDir * followOffsetMax;
            }

            if (virtualCamera != null)
            {
                Vector3.Lerp(
                    virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset,
                    followOffset,
                    Time.deltaTime * zoomSpeed
                );

                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                    followOffset;
            }
        }
    }
}
