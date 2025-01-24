
using UnityEngine;
using UnityEngine.UI;

namespace SFramework.Game
{
    public enum UILayer
    {
        None,
        Tags,
        Pend,
        Hud,
        Popup,
        Dialog,
        Toast,
        Blocker
    }
    public class SUIROOT : MonoBehaviour
    {
        [SerializeField] private Transform _tags;
        [SerializeField] private Transform _pend;
        [SerializeField] private Transform _hud;
        [SerializeField] private Transform _popUp;
        [SerializeField] private Transform _dialog;
        [SerializeField] private Transform _toast;
        [SerializeField] private Transform _blocker;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _mainCameraOriginSize;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Canvas _canvas;
        const string uiName = "SUIROOTMB";
        public static SUIROOT Instance;
        public Camera UICamera
        {
            get { return _uiCamera; }
        }

        public Camera MainCamera
        {
            get { return _mainCamera; }
        }

        public Canvas UICanvas
        {
            get { return _canvas; }
        }

        private void Awake()
        {
            this.name = uiName;
            SUIROOT.Instance = this;
            if (_mainCamera == null)
            {
                _mainCamera = this.transform.Find("RotateCenter/MainCamera")?.GetComponent<Camera>();
            }
            _mainCameraOriginSize = _mainCamera.orthographicSize;

            if (_uiCamera == null)
                _uiCamera = this.transform.Find("UICamera").GetComponent<Camera>();
            if (_canvasScaler == null)
                _canvasScaler = this.GetComponent<CanvasScaler>();
            if (_tags == null)
                _tags = this.transform.Find("Tags").GetComponent<Transform>();
            if (_pend == null)
                _pend = this.transform.Find("Pend").GetComponent<Transform>();
            if (_hud == null)
                _hud = this.transform.Find("Hud").GetComponent<Transform>();
            if (_popUp == null)
                _popUp = this.transform.Find("PopUp").GetComponent<Transform>();
            if (_dialog == null)
                _dialog = this.transform.Find("Dialog").GetComponent<Transform>();
            if (_toast == null)
                _toast = this.transform.Find("Toast").GetComponent<Transform>();
            if (_blocker == null)
                _blocker = this.transform.Find("Blocker").GetComponent<Transform>();
            if(_canvas == null)
                _canvas = this.GetComponent<Canvas>();
            // deal with camera
            if (_canvasScaler != null)
            {
                Vector2 design = _canvasScaler.referenceResolution;
                float aspect = CameraTools.AdaptCameraSize(_mainCameraOriginSize, design.x, design.y);
                MainCamera.orthographicSize = aspect;
                bool rt = CameraTools.JudgeScreenIsWide(design.x, design.y);
                if (rt)
                    _canvasScaler.matchWidthOrHeight = 1;
                else
                    _canvasScaler.matchWidthOrHeight = 0;
            }
        }

        public void OpenUI(UILayer layer, Transform result, Vector2 offsetMin = default, Vector2 offsetMax = default, Quaternion rotation = default)
        {
            Transform parent = null;
            switch (layer)
            {
                case UILayer.Tags:
                    parent = _tags.transform;
                    break;
                case UILayer.Pend:
                    parent = _pend.transform;
                    break;
                case UILayer.Hud:
                    parent = _hud.transform;
                    break;
                case UILayer.Popup:
                    parent = _popUp.transform;
                    break;
                case UILayer.Dialog:
                    parent = _dialog.transform;
                    break;
                case UILayer.Toast:
                    parent = _toast.transform;
                    break;
                case UILayer.Blocker:
                    parent = _blocker.transform;
                    break;
            }
            if (parent != null)
            {
                // Transform result = Instantiate(transPrefab, parent, false);
                result.SetParent(parent, false);
                result.rotation = rotation;

                var rectTransform = result.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.offsetMin = offsetMin;
                    rectTransform.offsetMax = offsetMax;
                }
            }
            else
            {
                Debug.LogWarning("not found ui parent");
            }
        }

        public void CloseUI(Transform trans)
        {
            if (trans != null)
                Destroy(trans.gameObject);
        }

        private void OnDestroy()
        {
            SUIROOT.Instance = null;
        }
    }
}
