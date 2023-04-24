using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUIROOT : MonoBehaviour
{
    private Canvas _tags;
    private Canvas _pend;
    private Canvas _hud;
    private Canvas _popUp;
    private Canvas _toast;
    private Canvas _blocker;

    public static PUIROOT Instance;
    void Start()
    {
        PUIROOT.Instance = this;
    }
    
    void Update()
    {

    }
}
