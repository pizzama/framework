using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUIROOT : MonoBehaviour
{
    [SerializeField] private Canvas _tags;
    [SerializeField] private Canvas _pend;
    [SerializeField] private Canvas _hud;
    [SerializeField] private Canvas _popUp;
    [SerializeField] private Canvas _dialog;
    [SerializeField] private Canvas _toast;
    [SerializeField] private Canvas _blocker;

    private Camera _uiCamera;

    public static PUIROOT Instance;
    private void Start()
    {
        PUIROOT.Instance = this;
    }

    private void Awake()
    {
        if (_uiCamera == null)
            _uiCamera = this.transform.Find("UICamera").GetComponent<Camera>();
        if (_tags == null)
            _tags = this.transform.Find("Tags").GetComponent<Canvas>();
        if (_pend == null)
            _pend = this.transform.Find("Pend").GetComponent<Canvas>();
        if (_hud == null)
            _hud = this.transform.Find("Hud").GetComponent<Canvas>();
        if (_popUp == null)
            _popUp = this.transform.Find("PopUp").GetComponent<Canvas>();
        if (_dialog == null)
            _dialog = this.transform.Find("Dialog").GetComponent<Canvas>();
        if (_toast == null)
            _toast = this.transform.Find("Toast").GetComponent<Canvas>();
        if (_blocker == null)
            _blocker = this.transform.Find("Blocker").GetComponent<Canvas>();
    }

    public void OpenUI(string uiName, string prefabPath)
    {

    }


}
