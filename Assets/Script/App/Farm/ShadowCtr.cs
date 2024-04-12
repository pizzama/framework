using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCtr : MonoBehaviour {

    private LineRenderer line;
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();

        line.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0.5f, 0));
	}
	

	void Update () {
		
	}
}
