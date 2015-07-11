using UnityEngine;
using System.Collections;

public class SineCosineMovement : MonoBehaviour {

	public Vector3 startOffset;
	public Vector3 strength;
	public float speed;
	
	private Vector3 startPos;
	
	void Start()
	{
		startPos = transform.localPosition;
	}
	
	void Update () {
		
		transform.localPosition = new Vector3(startPos.x + Mathf.Sin(Time.time * speed + startOffset.x) * strength.x, startPos.y + Mathf.Sin(Time.time * speed * 2f + startOffset.y) * strength.y, startPos.z + Mathf.Sin(Time.time * speed * 2f + startOffset.z) * strength.z);

	}
}
