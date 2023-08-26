using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game.Actor
{
    public interface ISFEntity
    {
        string Id{get;}
        string EntityAssetName{get;}
        GameObject Instance{get;}
        void Init(int entityId, string entityAssetName);
        void Recycle();
        void Show();
        void Hide(bool isDestory);
        void Attached(ISFEntity childEntity);
        void Detached(ISFEntity childEntity);
        void Update(float tickTime);
    }

    public class SFEntity : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
