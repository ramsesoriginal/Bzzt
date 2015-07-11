using UnityEngine;
using System.Collections;

public class OtherButton : MonoBehaviour {
	
	public bool FSButton;
	public bool LinkButton;
	public string URL;
	
	public bool clicked;
	
	public void SetClicked()
	{
		clicked = true;
	}
	
	public void SetUnclicked()
	{
		clicked = false;
	}
	
	public void Execute()
	{
		if(FSButton)
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		
		if(LinkButton)
		{
			Application.OpenURL( URL );
		}
	}
}
