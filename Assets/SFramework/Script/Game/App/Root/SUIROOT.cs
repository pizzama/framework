
using UnityEngine;
using UnityEngine.UI;

namespace SFramework.Game
{
    public enum UILayer
    {
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
        [SerializeField] private CanvasScaler _canvasScaler;
        public static SUIROOT Instance;
        public Camera UICamera
        {
            get { return _uiCamera; }
        }

        private void Awake()
        {
            SUIROOT.Instance = this;
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
        }

        public void OpenUI(UILayer layer, Transform result, Vector3 position = default, Quaternion rotation = default)
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
                result.position = position;
                result.rotation = rotation;
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
    }
}
