using SFramework;
using SFramework.Game;
using UnityEngine;

namespace Game
{
    public class InputView : SUIView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override UILayer GetViewLayer()
        {
            return UILayer.Hud;
        }

        protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
            prefabPath = "";
            prefabName = "";
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        protected override void opening()
        {
            // Image img = getAssetFromGoDict<Image>("image");
            // var imgTexture = assetManager.LoadResource<Texture2D>("arrow");
            // Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
            // Debug.Log("test view enter:" + sprite);
            // img.sprite = sprite;
        }

        protected override void viewUpdate()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Control.OpenControl("game", "TestControl");
            }
        }
    }
}