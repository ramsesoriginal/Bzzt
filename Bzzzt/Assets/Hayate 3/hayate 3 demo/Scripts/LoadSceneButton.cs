using UnityEngine;
using System.Collections;

public class LoadSceneButton : MonoBehaviour {

	public int SceneID;
	
	private LoadingScreen LS;
	
	void Start()
	{
		LS = Camera.main.gameObject.GetComponent<LoadingScreen>();
	}
	
	public void StartLoading()
	{
		LS.StopAllCoroutines();
		StartCoroutine( LS.FadeToBlack () );
		StartCoroutine( Load() );
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F))
			Screen.fullScreen = !Screen.fullScreen;
	}
	
	private IEnumerator Load()
	{
		while(true)
		{
			if(LS.fadeCol.a != 1f)
			{
				yield return new WaitForSeconds(Time.deltaTime);
			}else{
				Application.LoadLevel( SceneID );
				yield break;
			}
		}
	}
}
