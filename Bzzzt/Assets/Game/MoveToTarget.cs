using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour {

	public Vector3 target;
	public float speed;
	bool destroyMe = false;
	bool timestopped;

	// Use this for initialization
	void Start () {
		transform.LookAt (target);
		destroyMe = false;;
		timestopped = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + transform.forward * (speed / 100f) + new Vector3 (Random.Range(-0.2f,0.2f),Random.Range(-0.1f,0.1f),Random.Range(-0.2f,0.2f));

		if (transform.position.y < -200) {
			destroyMe = true;
		}

		if (destroyMe) {
			GameObject.Destroy(gameObject);
		}
	}

	void OnCollisionEnter() {
		//destroyMe = true;
	}
}
