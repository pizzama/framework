using SFramework;
using SFramework.Game;
using SFramework.StateMachine;
using SFramework.Statics;
using UnityEngine;

namespace App.Farm
{
    public class FlowerEntity : SEntity
    {
        private FlowerFSM _fSM;
        [SerializeField] private SpriteRenderer _flower;
        [SerializeField] private Transform _land;
        protected override void initEntity()
        {
            base.initEntity();
            _fSM = new FlowerFSM();
            //init data
            _fSM.ChangeState<FlowerEmpty>();
        }

        public void Empty()
        {
            _flower.gameObject.SetActive(false);
        }

        public void Seed()
        {
            _flower.gameObject.SetActive(true);
            Sprite sp = ((RootView)ParentView).LoadFromBundle<Sprite>(SFResAssets.App_farm_ground_sfp_a40011411_1_png);
            if (sp != null)
            {
                _flower.sprite = sp;
            }
        }

        public void Grow()
        {
            _flower.gameObject.SetActive(true);
            Sprite sp = ((RootView)ParentView).LoadFromBundle<Sprite>(SFResAssets.App_farm_ground_sfp_a40011411_2_png);
            if (sp != null)
            {
                _flower.sprite = sp;
            }
        }

        public void Harvest()
        {
            _flower.gameObject.SetActive(true);
            Sprite sp = ((RootView)ParentView).LoadFromBundle<Sprite>(SFResAssets.App_farm_ground_sfp_a40011411_3_png);
            if (sp != null)
            {
                _flower.sprite = sp;
            }
        }

    }
}
