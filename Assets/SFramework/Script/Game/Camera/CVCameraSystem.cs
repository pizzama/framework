using Cinemachine;
using UnityEngine;

namespace SFramework.GameCamera
{
    public class CVCameraSystem : MonoBehaviour
    {
        [SerializeField]
        protected CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        protected bool validOnUI = false;

        [SerializeField]
        protected bool useEdgeScrolling = false;

        [SerializeField]
        protected bool useDragPan = false;

        [SerializeField]
        protected float moveSpeed = 50f;

        [SerializeField]
        protected int edgeScrollSize = 20;

        [SerializeField]
        protected float rotateSpeed = 100f;

        [SerializeField]
        protected float dragThreshold = 0f; //drag thread

        [Header("Camera Zoom Params")]
        [SerializeField]
        protected float fieldOfViewMax = 50f;

        [SerializeField]
        protected float fieldOfViewMin = 10f;

        [SerializeField]
        protected float fieldOfViewStep = 5f;

        [SerializeField]
        protected float followOffsetMax = 50f;

        [SerializeField]
        protected float followOffsetMin = 10f;

        [SerializeField]
        protected float zoomSpeed = 10f;

        [Header("Camera direction Limit Params")]
        [SerializeField]
        protected Vector2 cameraMoveLimitX = new Vector2(-10, 10);

        [SerializeField]
        protected Vector2 cameraMoveLimitY = new Vector2(-10, 10);

        [SerializeField]
        protected Vector2 cameraMoveLimitZ = new Vector2(-10, 10);

        protected bool dragPanMoveActive = false;
        protected float targetFieldOfView = -1;
        protected Vector2 lastMousePosition;
        protected Vector3 followOffset;

        protected bool zoomMoveActive = false;
        protected Touch oldTouch1;
        protected Touch oldTouch2;

        private void Start()
        {
            if (virtualCamera == null)
            {
                virtualCamera = GetComponent<CinemachineVirtualCamera>();
            }

            if (virtualCamera == null)
            {
                virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            }

            if (virtualCamera == null)
            {
                throw new NotFoundException(
                    "Not Found CinemachineVirtualCamera on this transform:" + this.name
                );
            }
        }

        protected void Update()
        {
            if (validOnUI == false && SInputDefine.IsTouchUI())
            {
                return;
            }
            handleCameraMovement();
            if (useEdgeScrolling)
                handleCameraMovementEdgeScrolling();
            if (useDragPan)
                handleCameraMovementDragPan();
            handleCameraRotation();
            handleCameraZoomOnByTouch();
            handleCameraZoom_FieldOfView();

            limitCameraMove();
        }

        protected virtual void handleCameraMovement() { }

        protected virtual void handleCameraMovementEdgeScrolling() { }

        protected virtual void handleCameraMovementDragPan() { }

        protected virtual void handleCameraRotation() { }

        protected virtual void handleCameraZoomOnByTouch() { }

        protected virtual void handleCameraZoom_FieldOfView() { }

        protected virtual void handleCameraZoom_MoveForward() { }

        protected virtual void limitCameraMove()
        {
            Vector3 target = transform.position;
            target.x = Mathf.Clamp(target.x, cameraMoveLimitX.x, cameraMoveLimitX.y);
            target.y = Mathf.Clamp(target.y, cameraMoveLimitY.x, cameraMoveLimitY.y);
            target.z = Mathf.Clamp(target.z, cameraMoveLimitZ.x, cameraMoveLimitZ.y);
            transform.position = target;
        }
    }
}