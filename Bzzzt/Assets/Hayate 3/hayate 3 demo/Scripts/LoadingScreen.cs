using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
	
	public Texture image;
	public float fadeSpeed;
	private bool showTex;
	public Color fadeCol;
	private bool isStart = true;
	
	void Start()
	{
		StopAllCoroutines();
		StartCoroutine( FadeToWhite() );
	}
	
	public IEnumerator FadeToBlack()
	{
		fadeCol = Color.clear;
		showTex = true;
		
		while(true)
		{
			fadeCol = Color.Lerp (fadeCol, Color.white, fadeSpeed * Time.deltaTime);
			
			if(fadeCol.a >= 0.96f)
			{
				fadeCol = Color.white;
				yield break;
			}
			
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	
	public IEnumerator FadeToWhite()
	{
		fadeCol = Color.white;
		showTex = true;
		
		if(isStart)
		{
			yield return new WaitForSeconds(1.5f);
			isStart = false;
		}
		
		while(true)
		{
			fadeCol = Color.Lerp (fadeCol, Color.clear, fadeSpeed * Time.deltaTime);
			
			if(fadeCol.a <= 0.04f)
			{
				fadeCol = Color.clear;
				showTex = false;
				yield break;
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	
	void OnGUI()
	{
		GUI.color = fadeCol;
			
		if(showTex)
			GUI.DrawTexture( new Rect(  0, 0, Screen.width, Screen.height ), image);
	}
	
}
