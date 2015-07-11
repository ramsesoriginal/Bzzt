using UnityEngine;
using System.Collections;

public class HingeForce : MonoBehaviour {
	
	public AnimationCurve curve;

	void Update()
	{
		Vector3 force = new Vector3( curve.Evaluate(Time.time), 0, 0);
		if(force.x > 0)
			GetComponent<Rigidbody>().AddForce( force );
	}
}
