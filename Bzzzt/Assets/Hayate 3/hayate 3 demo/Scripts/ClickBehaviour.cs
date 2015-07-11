using UnityEngine;
using System.Collections;

public class ClickBehaviour : MonoBehaviour {
	
	public Camera cam;
	Ray ray = new Ray();
	
	void Update () 
	{
		if(Input.GetMouseButtonDown( 0 ))
		{
			if(cam)
			{
				ray = cam.ScreenPointToRay( Input.mousePosition );
			}else{
				ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			}
			
			RaycastHit rayHit = new RaycastHit();

			if( Physics.Raycast(ray, out rayHit, 100f) )
			{
				if(rayHit.transform.gameObject.layer == 11)
				{
					rayHit.transform.gameObject.GetComponent<LoadSceneButton>().StartLoading();
					
				}else if( rayHit.transform.gameObject.layer == 12)
				{
					rayHit.transform.gameObject.GetComponent<OtherButton>().SetClicked();
					rayHit.transform.gameObject.GetComponent<OtherButton>().Execute();
				}
			}
		}
	}
}
