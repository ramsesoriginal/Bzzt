using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextureOverlay : MonoBehaviour {

	public Texture2D tex;
	public Vector2 Offset;
	
	public bool fromLeft;

	void OnGUI () {
		if(!fromLeft)
		{
			GUI.DrawTexture( new Rect(  ( Screen.width - Offset.x),  ( Screen.height - Offset.y ), tex.width, tex.height ), tex);
		}else{
			GUI.DrawTexture( new Rect(  ( Offset.x),  ( Screen.height - Offset.y ), tex.width, tex.height ), tex);
		}
	}
	
}
