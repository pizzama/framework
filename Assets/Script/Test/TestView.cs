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

        protected override void SetViewTransform(out Transform trans, out Vector3 position, out Quaternion rotation)
        {
            trans = assetManager.LoadResource<Transform>("Test");
            //abManager.LoadResource<RectTransform>("ss/test", "Test");
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 180);
            var tt = assetManager.LoadResource<TextAsset>("test11", "test1");
            Debug.Log(tt);
        }

        protected override void enter()
        {
            Image img = getGameObjectFromGoDict("Image").GetComponent<Image>();
            var imgTexture = assetManager.LoadResource<Texture2D>("arrow");
            Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("test view enter:" + sprite);
            img.sprite = sprite;
        }

        protected override async void enterAsync()
        {
            Debug.Log("test view enterasync");
            var tt = await assetManager.LoadResourceAsync<TextAsset>("test11", "test1");
            Debug.Log(tt);
            await UniTask.Yield();
        }
    }
}