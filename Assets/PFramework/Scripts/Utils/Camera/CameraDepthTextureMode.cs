using UnityEngine;

//设置摄像机的纹理深度
[RequireComponent(typeof(Camera))]
public class CameraDepthTextureMode : MonoBehaviour
{
    [SerializeField]
    DepthTextureMode depthTextureMode;

    private void OnValidate()
    {
        SetCameraDepthTextureMode();
    }

    private void Awake()
    {
        SetCameraDepthTextureMode();
    }

    private void SetCameraDepthTextureMode()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }
}
