using UnityEngine;
using System.Collections;

public class HitMeMover : MonoBehaviour {

	public float detailRotationSpeed;
	public float mainrotationSpeed;
	public float mainrotationFrequency;
	public float movementSpeed;
	public float distanceAjust;
	public float distanceAdjustPower;
	public float distanceAjustRange;


	float detailRotation;
	float mainRotation;
	float lastMainRotation;

	public GameObject WinPanel;
	public GameObject BoltPanel;
	public ThrowLightning lightning;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var curRotation = transform.localEulerAngles;
		detailRotation = Random.Range (-detailRotationSpeed,detailRotationSpeed);
		if ((lastMainRotation + mainrotationFrequency) < Time.time) {
			mainRotation = Random.Range (-mainrotationSpeed,mainrotationSpeed);
			lastMainRotation = Time.time;
		}
		/*var curDistance = Mathf.Abs ((transform.position - new Vector3 (0, 2, 0)).magnitude);
		if (curDistance > distanceAjustRange) {
			var targetAngle = Mathf.Rad2Deg * Mathf.Asin (transform.position.x / curDistance);
			distanceAjust = Mathf.Lerp(curRotation.y,targetAngle,((curDistance )+distanceAjustRange)/(distanceAjustRange*2))/distanceAdjustPower;

		} else {
			distanceAjust = 0;
		}*/
		curRotation.y += detailRotation + mainRotation + distanceAjust;
		transform.localEulerAngles = curRotation;
		GetComponent<Rigidbody> ().AddForce (transform.forward*movementSpeed);
	}

	void OnCollisionEnter() {
		transform.LookAt (new Vector3 (0, 2, 0));
		mainRotation = 0;
		lastMainRotation = Time.time;
		//Debug.Log ("Collision");
	}

	void OnTriggerEnter() {
		Debug.Log ("HIT");
		WinPanel.SetActive (true);
		BoltPanel.SetActive (false);
		lightning.ThrowingTime = 0;
	}
}
