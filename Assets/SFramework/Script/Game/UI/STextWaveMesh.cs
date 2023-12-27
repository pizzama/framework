using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SFramework.UI
{
    public class STextWaveMesh : MonoBehaviour
    {
        [SerializeField] TMP_Text _tmpText;
        [SerializeField] private float _offsetX = 3.5f;
        [SerializeField] private float _offsetY = 2.5f;
        private Mesh _mesh;
        private Vector3[] myVecs;


        void Update()
        {
            _tmpText.ForceMeshUpdate();
            _mesh = _tmpText.mesh;
            myVecs = _mesh.vertices;
            for (int i = 0; i < _tmpText.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo c = _tmpText.textInfo.characterInfo[i];

                int index = c.vertexIndex;

                Vector3 offset = Wobble(Time.time + i);
                myVecs[index] += offset;
                myVecs[index + 1] += offset;
                myVecs[index + 2] += offset;
                myVecs[index + 3] += offset;
            }

            _mesh.vertices = myVecs;
            _tmpText.canvasRenderer.SetMesh(_mesh);

        }
        Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time * _offsetX), 5 * Mathf.Cos(time * _offsetY));
            // return new Vector2(0, 5 * Mathf.Cos(time * 3.5f));
        }
    }
}
