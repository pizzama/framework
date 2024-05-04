using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Game.Map;
using SFramework.Tools;
using SFramework.Statics;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace App.Farm
{
	public class FarmView : SSCENEView
	{
		private SMGrid _grid;
		private Transform _flowerParent;
		private Ray _ray;
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

			createFlower();
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
					int layerMask = 1 << 7;
					layerMask += 1 << 6;
					int mask = LayerMask.GetMask("ColliderLayer");
					if(Physics.Raycast(_ray, out hit, Mathf.Infinity, mask))
					{
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
			flowerPos.Add(new Vector3() { x = 1, y = 0, z = 0 });
			flowerPos.Add(new Vector3() { x = 2.5f, y = 0, z = 0 });
			flowerPos.Add(new Vector3() { x = 4, y = 0, z = 0 });
			flowerPos.Add(new Vector3() { x = 5.5f, y = 0, z = 0 });

			for (var i = 0; i < flowerPos.Count; i++)
			{
				Vector3 pos = flowerPos[i];
				if (pos != null)
				{
					CreateComponent<FlowerEntity>(SFResAssets.App_farm_sfp_FlowerTemplate_prefab, _flowerParent, pos);
				}

			}
		}
	}
}
