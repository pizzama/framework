using Cinemachine;
using SFramework.Tools;
using UnityEngine;

namespace SFramework.GameCamera
{
    public class CVCameraSystem3D : CVCameraSystem
    {
        protected override void handleCameraMovement()
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

        protected override void handleCameraMovementEdgeScrolling()
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

        protected override void handleCameraMovementDragPan()
        {
            if (zoomMoveActive || Input.touchCount > 1)
            {
                return;
            }
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
                Vector3 m1 = MapTools.ScreenToWorldPos(Input.mousePosition);
                Vector3 m2 = MapTools.ScreenToWorldPos(lastMousePosition);
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

        protected override void handleCameraRotation()
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

        protected override void handleCameraZoomOnByTouch()
        {
            if (Input.touchCount > 1)
            {
                if (targetFieldOfView == -1)
                {
                    targetFieldOfView = virtualCamera.m_Lens.FieldOfView;
                }
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                if (newTouch2.phase == TouchPhase.Began) //.Phase描述触摸的阶段  .Began当一个手指触碰了屏幕的时候
                {
                    zoomMoveActive = true;
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                float offset = newDistance - oldDistance;
                Debug.Log("camerazoom:" + offset + ";" + newDistance + ";" + oldDistance);

                //放大因子，一个像素按0.1倍来计算（范围10）
                float speed = offset / Time.deltaTime;

                if (speed != 0)
                {
                    targetFieldOfView -= speed;
                    targetFieldOfView = Mathf.Clamp(
                        targetFieldOfView,
                        fieldOfViewMin,
                        fieldOfViewMax
                    );
                    if (virtualCamera != null)
                    {
                        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
                            virtualCamera.m_Lens.FieldOfView,
                            targetFieldOfView,
                            Time.deltaTime * zoomSpeed
                        );
                    }
                }

                //记住新的触摸点，下次使用
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }

            if (zoomMoveActive)
            {
                Touch newTouch1 = Input.GetTouch(0);
                if (newTouch1.phase == TouchPhase.Ended)
                {
                    zoomMoveActive = false;
                }
            }
        }

        protected override void handleCameraZoom_FieldOfView()
        {
            if (targetFieldOfView == -1)
            {
                targetFieldOfView = virtualCamera.m_Lens.FieldOfView;
            }

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

        protected override void handleCameraZoom_MoveForward()
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