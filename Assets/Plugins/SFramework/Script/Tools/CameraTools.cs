using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CameraTools
{
    public async UniTask ShakeCameraAsync(float duration = 0.3f, float magnitude = 0.1f)
    {
        await ShakeCamera(duration, magnitude).ToUniTask();
    }
    
    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 originalPos = mainCamera.transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude + originalPos.x;
                float y = UnityEngine.Random.Range(-1f, 1f) * magnitude + originalPos.y;
            
                mainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            mainCamera.transform.localPosition = originalPos;
        }
    }
    
    public static float AdaptCameraSize(float originSize, float aspectWidth, float aspectHeight)
    { 
        if (Screen.width < Screen.height) //判断横竖屏
        {
            float defaultAspect = aspectWidth / aspectHeight;
            float currentAspect = (float)Screen.height/ Screen.width;
            return originSize * (currentAspect / defaultAspect);
        }
        else
        {
            float defaultAspect = aspectWidth / aspectHeight;
            float currentAspect = (float)Screen.width / Screen.height;
            return originSize * (defaultAspect / currentAspect);
        }
    }
}
