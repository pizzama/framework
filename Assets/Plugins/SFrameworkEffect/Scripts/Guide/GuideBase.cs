using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 对应使用的shader是 Guide
// 中心点移动的类型
public enum TranslateType {
    Direct,
    Slow
}

[RequireComponent(typeof(Image))]
public class GuideBase : MonoBehaviour, ICanvasRaycastFilter
{
    [SerializeField] protected Material material; // 材质

    [SerializeField] protected Vector3 center;    // 镂空区域的中心

    [SerializeField] protected RectTransform target; // 要显示的目标

    [SerializeField] protected Vector3[] targetCorners = new Vector3[4]; // 要引导的目标的边界

    #region Scale变化相关

    protected float scaleTimer;
    protected float scaleTime;
    protected bool isScaling;
    #endregion

    #region 中心点移动相关

    private Vector3 _startCenter;
    private float _centerTimer;
    private float _centerTime;
    private bool _isMoving;

    #endregion

    public Vector3 Center {
        get {
            if (material == null) { return Vector3.zero; }
            return material.GetVector("_Center");
        }
    }

    protected virtual void Start()
    {
        if (material == null)
        {
            material = transform.GetComponent<Image>()?.material;
        }
    }

    protected virtual void Update()
    {
        if (isScaling)
        {
            scaleTimer += Time.deltaTime * 1 / scaleTime;
            if (scaleTimer >= 1)
            {
                scaleTimer = 0;
                isScaling = false;
            }
        }

        if (_isMoving)
        {
            _centerTimer += Time.deltaTime * 1 / _centerTime;

            // 设置中心点
            material.SetVector("_Center", Vector3.Lerp(_startCenter, center, _centerTimer));

            if (_centerTimer >=1 )
            {
                _centerTimer = 0;
                _isMoving = false;
            }
        }
    }

    public void MoveTarget(Vector3 move)
    {
        center = move;
        target.localPosition = move;
        _isMoving = true;
    }

    // 引导
    public virtual void Guide(Canvas canvas, RectTransform target,TranslateType translateType = TranslateType.Direct, float time = 1)
    {
        // 初始化材质
        material = transform.GetComponent<Image>().material;
        this.target = target;
        if (target != null)
        {
            // 获取中心点 
            target.GetWorldCorners(targetCorners);

            // 把世界坐标 转成屏幕坐标
            for (int i = 0; i < targetCorners.Length; i++)
            {
                targetCorners[i] = WorldToScreenPoint(canvas, targetCorners[i]);
            }
            // 计算中心点
            center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
            center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;

            //Debug.Log(" 移动类型: " + translateType);

            switch (translateType)
            {
                case TranslateType.Direct:
                    // 设置中心点
                    material.SetVector("_Center", center);
                    break;
                case TranslateType.Slow:

                    _startCenter = material.GetVector("_Center");

                    _isMoving = true;
                    _centerTimer = 0;
                    _centerTime = time;
                    break;
            }
        }
        else {
            center = Vector3.zero;
            targetCorners[0] = new Vector3(-2000,-2000,0);
            targetCorners[1] = new Vector3(-2000, 2000, 0);
            targetCorners[2] = new Vector3(2000, 2000, 0);
            targetCorners[3] = new Vector3(2000, -2000, 0);
        }
         
    }

    public virtual void Guide(Canvas canvas, RectTransform target,float scale, float time, TranslateType translateType = TranslateType.Direct, float moveTime = 1) {

    }

    public Vector2 WorldToScreenPoint(Canvas canvas, Vector3 world)
    {
        // 把世界坐标转成 屏幕坐标
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, world);
        Vector2 localPoint;
        // 把屏幕坐标 转成 局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out localPoint);
        return localPoint;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (target == null) { return true; } // 事件不会渗透 
        //如果ui摄像机是用的是screen space - camera 则需要添加摄像机如果用的是 screen spcae-overaly 则可以省略
        return !RectTransformUtility.RectangleContainsScreenPoint(target,sp, eventCamera);
    }

    protected virtual void OnValidate()
    {
        // 当 myValue 在 Inspector 中更改时，调用此方法
        _isMoving = true;
    }
}
