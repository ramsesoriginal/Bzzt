using UnityEngine;
using System.Collections;

public class AnimateLight : MonoBehaviour {
	
	public AnimationCurve intensity;
	public AnimationCurve range;
	
	private float animationLength;
	private float startTime;
	
	public IEnumerator StartAnimating () {
		animationLength = intensity.length;
		startTime = Time.time;
		
		if(GetComponent<Light>())
		{
			GetComponent<Light>().enabled = true;
			
			for(;;)
			{
				GetComponent<Light>().intensity = intensity.Evaluate( Time.time - startTime );
				GetComponent<Light>().range = range.Evaluate( Time.time - startTime );
				
				if(animationLength <= Time.time - startTime)
				{
					GetComponent<Light>().enabled = false;
					yield break;
				}else{
					yield return new WaitForSeconds(Time.deltaTime);
				}
			}
		}else{
			yield break;
		}
	}
}
