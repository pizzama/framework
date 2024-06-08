using Cinemachine;
using UnityEngine;

namespace SFramework.GameCamera
{
    public class CVCameraSystem2D : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        [SerializeField]
        private bool _useEdgeScrolling = false;

        [SerializeField]
        private bool _useDragPan = false;

        [SerializeField]
        private float _moveSpeed = 50f;

        [SerializeField]
        private int _edgeScrollSize = 20;

        [SerializeField]
        private float _rotateSpeed = 100f;

        [SerializeField]
        private float _fieldOfViewMax = 50f;

        [SerializeField]
        private float _fieldOfViewMin = 10f;

        [SerializeField]
        private float _fieldOfViewStep = 5f;

        [SerializeField]
        private float _followOffsetMax = 50f;

        [SerializeField]
        private float _followOffsetMin = 10f;

        [SerializeField]
        private float _zoomSpeed = 10f;

        [SerializeField]
        private float _dragSpeed = 1f;

        [SerializeField]
        private float _dragThreshold = 0f;

        [SerializeField]
        private Vector2 _cameraMoveLimitX;

        [SerializeField]
        private Vector2 _cameraMoveLimitY;

        [SerializeField]
        private Vector2 _cameraMoveLimitZ;
        private bool _dragPanMoveActive;
        private float _targetFieldOfView = 20;
        private Vector2 _lastMousePosition;
        private Vector3 _followOffset;

        private void Update()
        {
            HandleCameraMovement();
            if (_useEdgeScrolling)
                HandleCameraMovementEdgeScrolling();
            if (_useDragPan)
                HandleCameraMovementDragPan();
            HandleCameraRotation();
            HandleCameraZoom_FieldOfView();
            limitCameraMove();
        }

        private void HandleCameraMovement()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                inputDir.y = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDir.y = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDir.x = 1f;
            }

            Vector3 moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementEdgeScrolling()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.mousePosition.x < _edgeScrollSize)
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < _edgeScrollSize)
            {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - _edgeScrollSize)
            {
                inputDir.x = 1f;
            }
            if (Input.mousePosition.y > Screen.height - _edgeScrollSize)
            {
                inputDir.z = 1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementDragPan()
        {
            Vector3 inputDir = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
            {
                _dragPanMoveActive = true;
                _lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragPanMoveActive = false;
            }

            if (_dragPanMoveActive)
            {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - _lastMousePosition;
                if (mouseMovementDelta.magnitude > _dragThreshold)
                {
                    // mouseMovementDelta.Normalize(); //only get direction
                    inputDir.x = -mouseMovementDelta.x * _dragSpeed;
                    inputDir.y = -mouseMovementDelta.y * _dragSpeed;
                    Vector3 moveDir = transform.up * inputDir.y + transform.right * inputDir.x;
                    transform.position += moveDir * Time.deltaTime;
                }
                _lastMousePosition = Input.mousePosition;
            }
        }

        private void HandleCameraRotation()
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

            transform.eulerAngles += new Vector3(0, rotateDir * _rotateSpeed * Time.deltaTime, 0);
        }

        private void HandleCameraZoom_FieldOfView()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                _targetFieldOfView += _fieldOfViewStep;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                _targetFieldOfView -= _fieldOfViewStep;
            }

            _targetFieldOfView = Mathf.Clamp(_targetFieldOfView, _fieldOfViewMin, _fieldOfViewMax);

            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
                _virtualCamera.m_Lens.FieldOfView,
                _targetFieldOfView,
                Time.deltaTime * _zoomSpeed
            );
        }

        private void HandleCameraZoom_MoveForward()
        {
            Vector3 zoomDir = _followOffset.normalized;
            if (Input.mouseScrollDelta.y > 0)
            {
                _followOffset += zoomDir * _zoomSpeed;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                _followOffset -= zoomDir * _zoomSpeed;
            }

            if (_followOffset.magnitude < _followOffsetMin)
            {
                _followOffset = zoomDir * _followOffsetMin;
            }

            if (_followOffset.magnitude > _followOffsetMax)
            {
                _followOffset = zoomDir * _followOffsetMax;
            }

            Vector3.Lerp(
                _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset,
                _followOffset,
                Time.deltaTime * _zoomSpeed
            );

            _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                _followOffset;
        }

        private void limitCameraMove()
        {
            Vector3 target = transform.position;
            target.x = Mathf.Clamp(target.x, _cameraMoveLimitX.x, _cameraMoveLimitX.y);
            target.y = Mathf.Clamp(target.y, _cameraMoveLimitY.x, _cameraMoveLimitY.y);
            target.z = Mathf.Clamp(target.z, _cameraMoveLimitZ.x, _cameraMoveLimitZ.y);
            transform.position = target;
        }
    }
}
