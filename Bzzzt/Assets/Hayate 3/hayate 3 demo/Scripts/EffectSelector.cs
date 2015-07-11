using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectSelector : MonoBehaviour {
	
	public GameObject offscreenLeft;
	public GameObject offscreenRight;
	public float animationSpeed;
	
	public OtherButton previous;
	public OtherButton next;
	
	public List<GameObject> effectList = new List<GameObject>();
	public int currentIndex = 0;
	private bool isAnimating;
	private GameObject currentEffect;
	private GameObject nextEffect;
	
	void Start () {
		nextEffect = Instantiate( effectList[currentIndex], offscreenRight.transform.position, Quaternion.identity) as GameObject;
		StartCoroutine(MoveToLeft());
		
	}
	
	void Update () {
		if(next.clicked && !isAnimating)
		{
			next.SetUnclicked();
			
			if(currentIndex < effectList.Count - 1)
			{
				currentIndex++;
			}else{
				currentIndex = 0;
			}
			
			nextEffect = Instantiate( effectList[currentIndex], offscreenRight.transform.position, Quaternion.identity) as GameObject;
			StartCoroutine(MoveToLeft());
			
		}
		
		if(previous.clicked && !isAnimating)
		{
			previous.SetUnclicked();
			
			if(currentIndex > 0)
			{
				currentIndex--;
			}else{
				currentIndex = effectList.Count - 1;
			}
			
			nextEffect = Instantiate( effectList[currentIndex], offscreenLeft.transform.position, Quaternion.identity) as GameObject;
			StartCoroutine(MoveToRight());
		}
	}
	
	private IEnumerator MoveToLeft()
	{
		isAnimating = true;
		while(true)
		{
			if( Vector3.Distance( nextEffect.transform.position, transform.position) <= 0.05f)
			{
				nextEffect.transform.position = transform.position;
				Destroy ( currentEffect );
				currentEffect = nextEffect;
				nextEffect = null;
				isAnimating = false;
				yield break;
			}else{
				if(currentEffect != null)
					currentEffect.transform.position = Vector3.Lerp(currentEffect.transform.position, offscreenLeft.transform.position, animationSpeed * Time.deltaTime);
				
				if(nextEffect != null)
					nextEffect.transform.position = Vector3.Lerp(nextEffect.transform.position, transform.position, animationSpeed * Time.deltaTime);
				
				yield return new WaitForSeconds( Time.deltaTime );
			}
		}
	}
	
	private IEnumerator MoveToRight()
	{
		isAnimating = true;
		while(true)
		{
			if( Vector3.Distance( nextEffect.transform.position, transform.position) <= 0.05f)
			{
				nextEffect.transform.position = transform.position;
				Destroy ( currentEffect );
				currentEffect = nextEffect;
				nextEffect = null;
				isAnimating = false;
				yield break;
			}else{
				if(currentEffect != null)
					currentEffect.transform.position = Vector3.Lerp(currentEffect.transform.position, offscreenRight.transform.position, animationSpeed * Time.deltaTime);
				
				if(nextEffect != null)
					nextEffect.transform.position = Vector3.Lerp(nextEffect.transform.position, transform.position, animationSpeed * Time.deltaTime);
				
				yield return new WaitForSeconds( Time.deltaTime );
			}
		}
	}
	
}
