using UnityEngine;
using System.Collections;

public class PlayEffects : MonoBehaviour {
	
	public ParticleSystem[] effects;
	public AnimateLight magicLight;
	public AnimateLight fireLight;
	
	public OtherButton fire;
	public OtherButton magic;
	public OtherButton ray;
	
	void Start()
	{
		if(GetComponent<Animation>().isPlaying)
			{
				magic.SetUnclicked();
				return;
			}
		
			GetComponent<Animation>().Play("Magic");
			magicLight.StartCoroutine( "StartAnimating" );
			effects[5].Play();
			effects[6].GetComponent<MagicBall>().startAnimation();
			magic.SetUnclicked();
	}
	
	void Update()
	{
		if(fire.clicked)
		{
			if(GetComponent<Animation>().isPlaying)
			{
				magic.SetUnclicked();
				return;
			}
				
		
			GetComponent<Animation>().Play("Fire");
			effects[0].Play();
			effects[1].Play();
			fire.SetUnclicked();
		}
		
		if(magic.clicked)
		{
			if(GetComponent<Animation>().isPlaying)
			{
				magic.SetUnclicked();
				return;
			}
		
			GetComponent<Animation>().Play("Magic");
			magicLight.StartCoroutine( "StartAnimating" );
			effects[5].Play();
			effects[6].GetComponent<MagicBall>().startAnimation();
			magic.SetUnclicked();
		}
		
		if(ray.clicked)
		{
			if(GetComponent<Animation>().isPlaying)
			{
				ray.SetUnclicked();
				return;
			}
			
			GetComponent<Animation>().Play("Ray");
			effects[2].Play();
			effects[3].Play();
			effects[4].Play();
			fireLight.StartCoroutine( "StartAnimating" );
			ray.SetUnclicked();
		}
	}
}
