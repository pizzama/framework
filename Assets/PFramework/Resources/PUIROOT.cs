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

    public static PUIROOT Instance;
    void Start()
    {
        PUIROOT.Instance = this;
    }
    
    void Update()
    {

    }
}
