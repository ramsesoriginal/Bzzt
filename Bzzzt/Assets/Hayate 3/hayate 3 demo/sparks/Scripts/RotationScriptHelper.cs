using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RotationScriptHelper : MonoBehaviour {
	
	public float xSpeed;
	public float ySpeed;
	public float zSpeed;
	
	void Update () {
		transform.Rotate( new Vector3( xSpeed, ySpeed, zSpeed) );
	}
}
