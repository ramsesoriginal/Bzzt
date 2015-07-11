using UnityEngine;
using System.Collections;

public class ThrowLightning : MonoBehaviour {
	public Vector3 mousePos;
	public Vector3 mousePointerPos;
	public Transform mousePointer;
	public GameObject boltPrototype;
	public GameObject bolt;

	public float ThrowingTime;

	public UnityEngine.UI.Text timer;
	public GameObject timerPanel;

	float lastThrown;

	// Use this for initialization
	void Start () {
		lastThrown = Time.time;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {

		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			mousePos = hit.point;
			//Debug.Log(hit);
		}

		mousePointerPos = Input.mousePosition;
		mousePointerPos.z = 30.0f;
		mousePointerPos = Camera.main.ScreenToWorldPoint(mousePointerPos);
		mousePointer.position = mousePointerPos;
		var countdown = Time.time - (lastThrown + ThrowingTime);
		timer.text = countdown.ToString ("F2");

		if (countdown > 0) {
			timerPanel.SetActive (false);
			if (Input.GetButtonDown ("Fire1")) {
				bolt = (GameObject)Instantiate (boltPrototype, mousePos + Vector3.up * 100, Quaternion.identity);
				bolt.GetComponent<MoveToTarget> ().target = mousePos;
				lastThrown = Time.time;
			}
		} else {
			timerPanel.SetActive (true);
		}


	}
}
