using Cysharp.Threading.Tasks;
using SFramework;
using SFramework.Game;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameMainView : SSCENEView
    {
        private GameObject _gameRoot;

        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
        }


    }
}