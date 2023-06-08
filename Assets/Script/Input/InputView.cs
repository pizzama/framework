using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class InputView : SUIView
    {
        protected override UILayer GetViewLayer()
        {
            return UILayer.Hud;
        }

        protected override void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation)
        {
            trans =  assetManager.LoadResource<Transform>("center", "Center");
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
            var tt = assetManager.LoadResource<TextAsset>("test11", "test1");
            Debug.Log(tt);
            Debug.LogError(trans.localScale);
        }

        protected override void enter()
        {
            Image img = getAssetFromGoDict<Image>("image");
            var imgTexture = assetManager.LoadResource<Texture2D>("arrow");
            Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("test view enter:" + sprite);
            img.sprite = sprite;
        }

        protected override async void enterAsync()
        {
            // Debug.Log("test view enterasync");
            // var tt = await assetManager.LoadResourceAsync<TextAsset>("test11", "test1");
            // Debug.Log(tt);
            await UniTask.Yield();
        }
    }
}