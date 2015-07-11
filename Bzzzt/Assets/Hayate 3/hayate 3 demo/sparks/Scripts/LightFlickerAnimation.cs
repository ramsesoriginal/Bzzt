using UnityEngine;
using System.Collections;

public class LightFlickerAnimation : MonoBehaviour {

	public AnimationCurve lightIntensity;
	public ParticleSystem p;
	
	public int lastParticleCount;
	
	void Start()
	{
		lastParticleCount = p.particleCount;
	}
	
	void Update () {
		int diff = p.particleCount - lastParticleCount;
		
		if( diff > 0 )
		{
			GetComponent<Light>().intensity += diff / (p.particleCount /2f);
		}else{
			GetComponent<Light>().intensity -= .1f * Time.time;
		}
		
		lastParticleCount = p.particleCount;
	}
}
