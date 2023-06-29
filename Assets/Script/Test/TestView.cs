using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class TestView : SUIView
    {
        protected override UILayer GetViewLayer()
        {
            return UILayer.Popup;
        }

        protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
            prefabPath = "ss/Test";
            prefabName = "Test";
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        protected override void opening()
        {
            Image img = getAssetFromGoDict<Image>("image");
            var imgTexture = assetManager.LoadResource<Texture2D>("arrow");
            if(imgTexture == null)
            {
                return;
            }
            Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("test view enter:" + sprite);
            img.sprite = sprite;
        }

        protected override async void openingAsync()
        {
            Debug.Log("test view enterasync");
            // var tt = assetManager.LoadResource<TextAsset>("test11", "test1");
            // Debug.Log(tt);
            await UniTask.Yield();
        }
    }
}