using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
	
	public float flickerSpeed;
	public float flickerInterval;
	public Vector2 flickerRange;
	
	private float currentIntensity;
	private float lastChange;
	
	void Start()
	{
		lastChange = Time.time;
	}
	
	void Update()
	{
		if(Time.time > lastChange + flickerInterval)
		{
			currentIntensity = Random.Range( flickerRange.x, flickerRange.y);
			lastChange = Time.time;
		}
		
		GetComponent<Light>().intensity = Mathf.Lerp( GetComponent<Light>().intensity, currentIntensity, flickerSpeed * Time.deltaTime);
	}
}
