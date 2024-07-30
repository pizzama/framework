using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NativeHelper;
using SFramework;
using SFramework.Game;
using SFramework.Game.Map;
using SFramework.GameCamera;
using SFramework.Statics;
using SFramework.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace App.Farm
{
    public class FarmView : SSCENEView
    {
        private SInputEvent _cameraTarget;
        private SMGrid _grid;
        private Transform _flowerParent;
        private Ray _ray;

        private float _minDelay = 0.1f;
        private float _maxDelay = 1f;

        private INativeHelper _nativeHelper = NativeHelperFactory.Create();

        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
            // Code Here
            // _grid = new SMGrid(10, 10, 3, new Vector3(0, 0, 0));
            // create farm
            _flowerParent = getExportObject<Transform>("Flowers");
            Camera cam = UIRoot.MainCamera;
            _cameraTarget = cam.GetComponent<SInputEvent>();
            // _cameraTarget.MouseEventHandle = sceneMove;

            createFlower();
            createForest();

            _nativeHelper.Alert("hahah");
        }

        protected override void closing()
        {
            // Code Here
        }

        protected override void viewUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // #if UNITY_ANDROID || UNITY_PHONE
                // 				if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                // #else
                // 				if (EventSystem.current.IsPointerOverGameObject())
                // #endif
                {
                    //发送射线做碰撞检测
                    _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    int mask = LayerMask.GetMask("ColliderLayer");
                    if (Physics.Raycast(_ray, out hit, Mathf.Infinity, mask))
                    {
                        FlowerEntity entity = hit.collider.GetComponent<FlowerEntity>();
                        Debug.Log("ddddd" + entity);
                        if (entity != null)
                        {
                            // GetControl<FarmControl>().OpenFarmView(entity);
                            entity.Grow();
                        }
                        Debug.Log("hit collider:" + hit.collider.tag + ";" + hit.collider.name);
                    }
                    Debug.DrawLine(_ray.origin, hit.point, Color.red, 2);
                }

                // Vector3 vec = MapTools.GetMouseWorldPosition();
                // Debug.Log(vec);
                // _grid.SetValue(vec, 100);
            }
        }

        private void createFlower()
        {
            List<Vector3> flowerPos = new List<Vector3>();
            flowerPos.Add(
                new Vector3()
                {
                    x = 0,
                    y = 0,
                    z = 0
                }
            );
            flowerPos.Add(
                new Vector3()
                {
                    x = 3f,
                    y = 0,
                    z = 0
                }
            );
            flowerPos.Add(
                new Vector3()
                {
                    x = 6,
                    y = 0,
                    z = 0
                }
            );
            flowerPos.Add(
                new Vector3()
                {
                    x = 9f,
                    y = 0,
                    z = 0
                }
            );

            for (var i = 0; i < flowerPos.Count; i++)
            {
                Vector3 pos = flowerPos[i];
                if (pos != null)
                {
                    CreateEntity<FlowerEntity>(
                        SFResAssets.App_farm_sfp_FlowerTemplate_prefab,
                        _flowerParent,
                        pos
                    );
                }
            }
        }

        private void createForest()
        {
            SpriteAtlas atlas = LoadFromBundle<SpriteAtlas>(
                SFResAssets.App_farm_ground_sfp_Forest_spriteatlasv2
            );
            Sprite sprite = atlas.GetSprite("5103524");
            Debug.Log(sprite);
        }

        private void sceneMove(
            bool isTouchUI,
            SInputEventType enumInputEventType,
            Vector2 mousePosition,
            int clickCount,
            int keyCode
        )
        {
            if (enumInputEventType == SInputEventType.Move)
            {
                Vector3 mv = Camera.main.ScreenToWorldPoint(mousePosition);
                _cameraTarget.transform.position = mv;
            }
        }

        private async UniTask spawnFlower()
        {
            while (true)
            {
                float delay = Random.Range(_minDelay, _maxDelay);
				await UniTask.Delay(System.TimeSpan.FromSeconds(delay));
				
                // int spawnIndex = Random.Range(0, spawnpoints.Length);
                // Transform spawnpoint = spawnpoints[spawnIndex];
                // GameObject spawnedFruit = Instantiate(
                //     fruit,
                //     spawnpoint.position,
                //     spawnpoint.rotation
                // );
                // Destroy(spawnedFruit, 5f);
            }
        }
    }
}
