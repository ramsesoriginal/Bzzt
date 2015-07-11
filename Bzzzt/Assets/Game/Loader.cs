using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Cancel")) {
			if (Application.loadedLevelName == "Game") {
				LoadMenu();
			} else {
				Quit ();
			}
		}
	}

	public void LoadGame() {
		Application.LoadLevel ("Game");
	}
	
	public void LoadMenu() {
		Application.LoadLevel ("Menu");
	}
	
	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}
}
