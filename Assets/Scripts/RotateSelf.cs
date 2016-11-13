using UnityEngine;
using System.Collections;

public class RotateSelf : MonoBehaviour {

	 
        
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0,0,-10) * Time.deltaTime);
	}
}
