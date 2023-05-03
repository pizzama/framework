using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void OpenUI(UILayer layer, Transform trans, Vector3 postion = default, Quaternion rotation = default)
    {
        Transform result = Instantiate(trans, postion, rotation);
        switch (layer)
        {
            case UILayer.Popup:
                result.SetParent(_popUp.transform);
                break;
        }
    }

    public void CloseUI(Transform trans)
    {
        if (trans != null)
            Destroy(trans.gameObject);
    }
}
