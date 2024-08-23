using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Test
{
    public class SceneTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 100;
            Debug.Log("Application target frame set rate 100");
        }

        // Update is called once per frame
        void Update() { }
    }
}
