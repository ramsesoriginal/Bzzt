using UnityEngine;
using UnityEditor;
using System.Collections;
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
using UnityEditor.AnimatedValues;
#endif

[CustomEditor( typeof ( Hayate ) ) ]
public class HayateEditor : Editor {
	
	public enum CalculationMethod{
		none, sine, cosine, animationCurve, perlin, precalculatedTexture, Audio
	}
	
	private bool useDebugFeatures;
	private float width;
	private Gradient gradient;
	
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
	private AnimBool MainTab = new AnimBool();
	private AnimBool TextureTab = new AnimBool();
	private AnimBool TransformParticleTab = new AnimBool();
	private AnimBool AttractorTab = new AnimBool();
	private AnimBool MeshTab = new AnimBool();
	private AnimBool CollisionTab = new AnimBool();
	private AnimBool SFXTab = new AnimBool();
	private AnimBool AudioTab = new AnimBool();
	private AnimBool DebugTab = new AnimBool();
#endif
	
	public override void OnInspectorGUI()
	{
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		
		//Start of 4.5 Editor script
		
		
		var hayate = target as Hayate;
		//DrawDefaultInspector ();

		GUILayout.Space (15f);

		EditorGUILayout.BeginHorizontal();

		width = EditorGUIUtility.currentViewWidth;
		DebugTab.speed = 2f;

		EditorGUIUtility.labelWidth = width / 6f;
		Repaint();

		MainTab.target = hayate.UseTurbulence;

		if(hayate.UseTurbulence)
		{
			MainTab.target = true;
			GUI.color = Color.green;

			if(GUILayout.Button("Turbulence", EditorStyles.toolbarButton))
				hayate.UseTurbulence = !hayate.UseTurbulence;

			GUI.color = Color.white;
		}else{
			MainTab.target = false;
			GUI.color = Color.grey;

			if(GUILayout.Button("Turbulence", EditorStyles.toolbarButton))

				hayate.UseTurbulence = !hayate.UseTurbulence;

			GUI.color = Color.white;
		}

		if(hayate.drawTurbulenceField)
		{
			DebugTab.target = true;

			GUI.color = Color.green;

			if(GUILayout.Button("Debugging", EditorStyles.toolbarButton))
				hayate.drawTurbulenceField = !hayate.drawTurbulenceField;

			GUI.color = Color.white;
		}else{
			DebugTab.target = false;
			GUI.color = Color.grey;

			if(GUILayout.Button("Debugging", EditorStyles.toolbarButton))
				hayate.drawTurbulenceField = !hayate.drawTurbulenceField;

			GUI.color = Color.white;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		if(hayate.burstOnCollision)
		{
			CollisionTab.target = true;
			GUI.color = Color.green;
			
			if(GUILayout.Button("Collision", EditorStyles.toolbarButton))
			{
				hayate.burstOnCollision = !hayate.burstOnCollision;
				CollisionTab.target = !CollisionTab.target;
			}
			
			
			GUI.color = Color.white;
		}else{
			CollisionTab.target = false;
			GUI.color = Color.grey;
			
			if(GUILayout.Button("Collision", EditorStyles.toolbarButton))
				hayate.burstOnCollision = !hayate.burstOnCollision;
			
			GUI.color = Color.white;
		}

		if(hayate.useTransformParticle)
		{
			TransformParticleTab.target = true;
			GUI.color = Color.green;
			
			if(GUILayout.Button("Transform particles", EditorStyles.toolbarButton))
				hayate.useTransformParticle = !hayate.useTransformParticle;

			GUI.color = Color.white;
		}else{
			TransformParticleTab.target = false;
			GUI.color = Color.grey;
			
			if(GUILayout.Button("Transform particles", EditorStyles.toolbarButton))
				hayate.useTransformParticle = !hayate.useTransformParticle;

			GUI.color = Color.white;
		}

		if(hayate.useAttractor)
		{
			AttractorTab.target = true;
			GUI.color = Color.green;
			
			if(GUILayout.Button("Attractors", EditorStyles.toolbarButton))
				hayate.useAttractor = !hayate.useAttractor;
			
			GUI.color = Color.white;
		}else{
			AttractorTab.target = false;
			GUI.color = Color.grey;
			
			if(GUILayout.Button("Attractors", EditorStyles.toolbarButton))
				hayate.useAttractor = !hayate.useAttractor;
			
			GUI.color = Color.white;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		if(hayate.useSfx)
		{
			SFXTab.target = true;
			GUI.color = Color.green;
			
			if(GUILayout.Button("SFX", EditorStyles.toolbarButton))
			{
				hayate.useSfx = !hayate.useSfx;
				SFXTab.target = !SFXTab.target;
			}

			GUI.color = Color.white;
		}else{
			SFXTab.target = false;
			GUI.color = Color.grey;
			
			if(GUILayout.Button("SFX", EditorStyles.toolbarButton))
				hayate.useSfx = !hayate.useSfx;
			
			GUI.color = Color.white;
		}

		if(hayate.moveToMesh)
		{
			MeshTab.target = true;
			GUI.color = Color.green;
			
			if(GUILayout.Button("Mesh", EditorStyles.toolbarButton))
				hayate.moveToMesh = !hayate.moveToMesh;
			
			GUI.color = Color.white;
		}else{
			MeshTab.target = false;
			GUI.color = Color.grey;
			
			if(GUILayout.Button("Mesh", EditorStyles.toolbarButton))
				hayate.moveToMesh = !hayate.moveToMesh;
			
			GUI.color = Color.white;
		}

		EditorGUILayout.EndHorizontal();

		if (DebugTab.faded > 0) {

			EditorGUILayout.BeginFadeGroup ( DebugTab.faded );
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Debug options", EditorStyles.toolbarButton);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Field Color", EditorStyles.miniLabel, GUILayout.Width( width / 4f ));
			hayate.debugColor = EditorGUILayout.ColorField("", hayate.debugColor);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Ray length", EditorStyles.miniLabel, GUILayout.Width( width / 4f ));
				hayate.rayLength = EditorGUILayout.FloatField("Units:", hayate.rayLength);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Field size", EditorStyles.miniLabel, GUILayout.Width( width / 4f ));
			hayate.fieldSize = EditorGUILayout.Vector3Field("", hayate.fieldSize);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Step size", EditorStyles.miniLabel, GUILayout.Width( width / 4f ));
			hayate.stepSize = EditorGUILayout.Vector3Field("", hayate.stepSize);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space (15f);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndFadeGroup ();

		}

		if (MainTab.faded > 0) {

			EditorGUILayout.BeginFadeGroup ( MainTab.faded );
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical("box");
			
			EditorGUILayout.LabelField("Turbulence options", EditorStyles.toolbarButton);

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Lock FPS", EditorStyles.miniLabel, GUILayout.Width( width / 6f ));
				hayate.isDeltaIndependent = EditorGUILayout.Toggle("", hayate.isDeltaIndependent, GUILayout.MaxWidth(40));

				EditorGUILayout.LabelField("Target FPS", EditorStyles.miniLabel, GUILayout.Width( width / 5f ));
				hayate.targetFps = EditorGUILayout.FloatField("", hayate.targetFps, GUILayout.MaxWidth(190f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Type", EditorStyles.miniLabel, GUILayout.Width( width / 5f ));
				hayate.UseCalculationMethodX = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodX, EditorStyles.toolbarPopup);
				hayate.UseCalculationMethodY = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodY, EditorStyles.toolbarPopup);
				hayate.UseCalculationMethodZ = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodZ, EditorStyles.toolbarPopup);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space (3f);

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Simulation", EditorStyles.miniLabel, GUILayout.Width( width / 5f ));
				hayate.AssignTurbulenceTo = (HayateEnums.AssignTo) EditorGUILayout.EnumPopup("", hayate.AssignTurbulenceTo, EditorStyles.toolbarPopup);
				hayate.UseRelativeOrAbsoluteValues = (HayateEnums.TurbulenceType) EditorGUILayout.EnumPopup("", hayate.UseRelativeOrAbsoluteValues, EditorStyles.toolbarPopup);
			EditorGUILayout.EndHorizontal();
			
				EditorGUI.BeginDisabledGroup(
					hayate.UseCalculationMethodX != HayateEnums.CalculationMethod.animationCurve &&
		  			hayate.UseCalculationMethodY != HayateEnums.CalculationMethod.animationCurve &&
		   			hayate.UseCalculationMethodZ != HayateEnums.CalculationMethod.animationCurve
					);
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Turbulence :", EditorStyles.miniLabel, GUILayout.Width( width / 5f ));
				
			if(hayate.turbulenceCurveX == null)
				hayate.turbulenceCurveX = new AnimationCurve();

			if(hayate.turbulenceCurveY == null)
				hayate.turbulenceCurveY = new AnimationCurve();

			if(hayate.turbulenceCurveZ == null)
				hayate.turbulenceCurveZ = new AnimationCurve();

				EditorGUI.BeginDisabledGroup(hayate.UseCalculationMethodX != HayateEnums.CalculationMethod.animationCurve);
					GUILayout.Label("X", EditorStyles.miniLabel);
					hayate.turbulenceCurveX = EditorGUILayout.CurveField( hayate.turbulenceCurveX );
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup(hayate.UseCalculationMethodY != HayateEnums.CalculationMethod.animationCurve);
					GUILayout.Label("Y", EditorStyles.miniLabel);
					hayate.turbulenceCurveY = EditorGUILayout.CurveField( hayate.turbulenceCurveY );
				EditorGUI.EndDisabledGroup();
			
				EditorGUI.BeginDisabledGroup(hayate.UseCalculationMethodZ != HayateEnums.CalculationMethod.animationCurve);
					GUILayout.Label("Z", EditorStyles.miniLabel);
					hayate.turbulenceCurveZ = EditorGUILayout.CurveField( hayate.turbulenceCurveZ );
				EditorGUI.EndDisabledGroup();
				
				EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			
			GUILayout.Space(5f);
			
			GUI.color = new Color(.8f,.8f,.8f,1f);

			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useAmplitudeCurve)
			{
				if(GUILayout.Button("V", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useAmplitudeCurve = !hayate.useAmplitudeCurve;
				}
			}else{
				if(GUILayout.Button("C", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useAmplitudeCurve = !hayate.useAmplitudeCurve;
				}
			}
			
			if(hayate.useAmplitudeCurve)
			{
				EditorGUILayout.LabelField("Amplitude", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				GUILayout.Label("X");
				hayate.AmplitudeCurveX = EditorGUILayout.CurveField( hayate.AmplitudeCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y");
				hayate.AmplitudeCurveY = EditorGUILayout.CurveField( hayate.AmplitudeCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z");
				hayate.AmplitudeCurveZ = EditorGUILayout.CurveField( hayate.AmplitudeCurveZ, GUILayout.MaxWidth(126f) );
			}else{
				EditorGUILayout.LabelField("Amplitude", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				hayate.Amplitude = EditorGUILayout.Vector3Field("", hayate.Amplitude, GUILayout.MaxWidth(380f));
			}
			EditorGUILayout.EndHorizontal();
			
			GUI.color = Color.white;
			
			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useFrequencyCurve)
			{
				if(GUILayout.Button("V", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useFrequencyCurve = !hayate.useFrequencyCurve;
				}
			}else{
				if(GUILayout.Button("C", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useFrequencyCurve = !hayate.useFrequencyCurve;
				}
			}
			
			if(hayate.useFrequencyCurve)
			{
				EditorGUILayout.LabelField("Frequency", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				GUILayout.Label("X");
				hayate.FrequencyCurveX = EditorGUILayout.CurveField( hayate.FrequencyCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y");
				hayate.FrequencyCurveY = EditorGUILayout.CurveField( hayate.FrequencyCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z");
				hayate.FrequencyCurveZ = EditorGUILayout.CurveField( hayate.FrequencyCurveZ, GUILayout.MaxWidth(126f) );
			}else{
				EditorGUILayout.LabelField("Frequency", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				hayate.Frequency = EditorGUILayout.Vector3Field("", hayate.Frequency, GUILayout.MaxWidth(380f));
			}
			EditorGUILayout.EndHorizontal();
			
			GUI.color = new Color(.8f,.8f,.8f,1f);

			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useOffsetCurve)
			{
				if(GUILayout.Button("V", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useOffsetCurve = !hayate.useOffsetCurve;
				}
			}else{
				if(GUILayout.Button("C", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useOffsetCurve = !hayate.useOffsetCurve;
				}
			}
			
			if(hayate.useOffsetCurve)
			{
				EditorGUILayout.LabelField("Offset", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				GUILayout.Label("X");
				hayate.OffsetCurveX = EditorGUILayout.CurveField( hayate.OffsetCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y");
				hayate.OffsetCurveY = EditorGUILayout.CurveField( hayate.OffsetCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z");
				hayate.OffsetCurveZ = EditorGUILayout.CurveField( hayate.OffsetCurveZ, GUILayout.MaxWidth(126f) );
			}else{
				EditorGUILayout.LabelField("Offset", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				hayate.Offset = EditorGUILayout.Vector3Field("", hayate.Offset, GUILayout.MaxWidth(380f));
			}
			EditorGUILayout.EndHorizontal();
			
			GUI.color = Color.white;

			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useOffsetSpeedCurve)
			{
				if(GUILayout.Button("V", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useOffsetSpeedCurve = !hayate.useOffsetSpeedCurve;
				}
			}else{
				if(GUILayout.Button("C", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useOffsetSpeedCurve = !hayate.useOffsetSpeedCurve;
				}
			}
			
			if(hayate.useOffsetSpeedCurve)
			{
				EditorGUILayout.LabelField("Offset speed", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				GUILayout.Label("X");
				hayate.OffsetSpeedCurveX = EditorGUILayout.CurveField( hayate.OffsetSpeedCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y");
				hayate.OffsetSpeedCurveY = EditorGUILayout.CurveField( hayate.OffsetSpeedCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z");
				hayate.OffsetSpeedCurveZ = EditorGUILayout.CurveField( hayate.OffsetSpeedCurveZ, GUILayout.MaxWidth(126f) );
			}else{
				EditorGUILayout.LabelField("Offset speed", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				hayate.OffsetSpeed = EditorGUILayout.Vector3Field("", hayate.OffsetSpeed, GUILayout.MaxWidth(380f));
			}
			EditorGUILayout.EndHorizontal();
			
			GUI.color = new Color(.8f,.8f,.8f,1f);
			
			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useGlobalForceCurve)
			{
				if(GUILayout.Button("V", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useGlobalForceCurve = !hayate.useGlobalForceCurve;
				}
			}else{
				if(GUILayout.Button("C", EditorStyles.toolbarButton, GUILayout.Width( width / 30f )))
				{
					hayate.useGlobalForceCurve = !hayate.useGlobalForceCurve;
				}
			}
			
			if(hayate.useGlobalForceCurve)
			{
				EditorGUILayout.LabelField("Global force", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				GUILayout.Label("X");
				hayate.GlobalForceCurveX = EditorGUILayout.CurveField( hayate.GlobalForceCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y");
				hayate.GlobalForceCurveY = EditorGUILayout.CurveField( hayate.GlobalForceCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z");
				hayate.GlobalForceCurveZ = EditorGUILayout.CurveField( hayate.GlobalForceCurveZ, GUILayout.MaxWidth(126f) );
			}else{
				EditorGUILayout.LabelField("Global force", EditorStyles.miniLabel, GUILayout.Width( width / 6.2f ));
				hayate.OffsetSpeed = EditorGUILayout.Vector3Field("", hayate.OffsetSpeed, GUILayout.MaxWidth(380f));
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(5f);
			
			GUI.color = Color.white;

			EditorGUILayout.BeginHorizontal();

			if( hayate.lockOffsetToEmitterPosition )
			{
				GUI.color = Color.green;

				if(GUILayout.Button("Lock Offset", EditorStyles.toolbarButton, GUILayout.Width( width / 2f )))
					hayate.lockOffsetToEmitterPosition = !hayate.lockOffsetToEmitterPosition;

				GUI.color = Color.white;

			}else{
				GUI.color = Color.grey;

				if(GUILayout.Button("Lock Offset", EditorStyles.toolbarButton, GUILayout.Width( width / 2f )))
					hayate.lockOffsetToEmitterPosition = !hayate.lockOffsetToEmitterPosition;

				GUI.color = Color.white;
			}

			if( hayate.randomizeOffsetAtStart )
			{
				GUI.color = Color.green;
				
				if(GUILayout.Button("Randomized Offset", EditorStyles.toolbarButton))
					hayate.randomizeOffsetAtStart = !hayate.randomizeOffsetAtStart;
				
				GUI.color = Color.white;
				
			}else{
				GUI.color = Color.grey;
				
				if(GUILayout.Button("Randomized Offset", EditorStyles.toolbarButton))
					hayate.randomizeOffsetAtStart = !hayate.randomizeOffsetAtStart;
				
				GUI.color = Color.white;
			}

			EditorGUILayout.EndHorizontal();


			EditorGUI.BeginDisabledGroup(!hayate.randomizeOffsetAtStart);
				EditorGUILayout.BeginHorizontal();
					hayate.randomOffsetRange = EditorGUILayout.Vector2Field("", hayate.randomOffsetRange, GUILayout.Width( width / 4f ));
					EditorGUILayout.MinMaxSlider( ref hayate.randomOffsetRange.x, ref hayate.randomOffsetRange.y, -2000f, 2000f);
				EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();

			GUILayout.Space (15f);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndFadeGroup ();
			
		}

		if ( hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.precalculatedTexture ||
			 hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.precalculatedTexture ||
			 hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.precalculatedTexture) 
		{
			if (!TextureTab.target)
				TextureTab.target = true;

		} else {
			if (TextureTab.target)
				TextureTab.target = false;
		}

		if ( TextureTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (TextureTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Texture turbulence options", EditorStyles.toolbarButton);
			
			EditorGUILayout.BeginHorizontal ();
				hayate.Turbulence = (Texture2D)EditorGUILayout.ObjectField ("", hayate.Turbulence, typeof(Texture2D), true);
				EditorGUILayout.HelpBox ("This texture uses different color channels per axis. Enable 'UseAlphaMask' to use the alpha channel as well. This will remove particles, that spawned where 'alpha = 0'.", MessageType.Info);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal();

			if(hayate.useAlphaMask)
			{
				GUI.color = Color.green;

				if(GUILayout.Button("Use alpha mask", EditorStyles.toolbarButton))
					hayate.useAlphaMask = !hayate.useAlphaMask;

				GUI.color = Color.white;

			}else{
				GUI.color = Color.grey;

				if(GUILayout.Button("Use alpha mask", EditorStyles.toolbarButton))
					hayate.useAlphaMask = !hayate.useAlphaMask;

				GUI.color = Color.white;
			}
			
			EditorGUI.BeginDisabledGroup(!hayate.useAlphaMask);
			hayate.threshold = EditorGUILayout.FloatField("Threshold:", hayate.threshold, GUILayout.Width( width / 3f ));
			EditorGUI.EndDisabledGroup();

			GUI.color = Color.green;

			if(GUILayout.Button("Update", EditorStyles.toolbarButton, GUILayout.Width( width / 4f )))
				hayate.UpdateTexture();

			GUI.color = Color.white;

			EditorGUILayout.EndHorizontal ();

			GUILayout.Space (15f);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.Audio ||
		     hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.Audio ||
		     hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.Audio) 
		{
			if (!AudioTab.target)
				AudioTab.target = true;
			
		} else {
			if (AudioTab.target)
				AudioTab.target = false;
		}
	
		if ( AudioTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (AudioTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Audio turbulence options", EditorStyles.toolbarButton);
		
			hayate.audioClip = (AudioClip)EditorGUILayout.ObjectField("",hayate.audioClip, typeof(AudioClip), true );
			EditorGUILayout.HelpBox("Make sure to select a sample far enough into the Track to not contain silence ('At sample'). Also the amount of samples should be inbetween 256 - 4096 and be a multiple of 2.", MessageType.Info );

			EditorGUILayout.BeginHorizontal();

			hayate.amountOfSamples = EditorGUILayout.IntField("Samples: ", hayate.amountOfSamples );
			hayate.atSample = EditorGUILayout.IntField("At sample: ", hayate.atSample , GUILayout.Width( width / 4f ));

			GUI.color = Color.green;

			if(GUILayout.Button("Update turbulence!", EditorStyles.toolbarButton ) && hayate.audioClip)
				HayateHelper.CalculateAudioTurbulence( hayate );
			
			GUI.color = Color.white;
			
			EditorGUILayout.EndHorizontal ();
			GUILayout.Space (15f);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( CollisionTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (CollisionTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Collision options", EditorStyles.toolbarButton);

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Emit Particles", GUILayout.Width( width / 4f ));
			hayate.burstNum = EditorGUILayout.IntField("     ", hayate.burstNum );

			EditorGUILayout.EndHorizontal();
			GUILayout.Space (15f);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( SFXTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (SFXTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("SFX", EditorStyles.toolbarButton);

			if(hayate.CreateOnStart)
			{
				GUI.color = Color.green;
				
				if(GUILayout.Button("Create On Start", EditorStyles.toolbarButton))
				{
					hayate.CreateOnStart = !hayate.CreateOnStart;
				}
				
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				
				if(GUILayout.Button("Create On Start", EditorStyles.toolbarButton))
					hayate.CreateOnStart = !hayate.CreateOnStart;
				
				GUI.color = Color.white;
			}

			if(hayate.CreateOnStart)
			{
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Dimensions: ", EditorStyles.miniLabel);	
					hayate.Width = EditorGUILayout.FloatField(hayate.Width);
					hayate.Height = EditorGUILayout.FloatField(hayate.Height);
					hayate.Depth = EditorGUILayout.FloatField(hayate.Depth);
				EditorGUILayout.EndHorizontal();	



				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Particle Size: ", EditorStyles.miniLabel);	
					hayate.TargetParticleSize = EditorGUILayout.FloatField(" ",hayate.TargetParticleSize);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Ui Scale: ", EditorStyles.miniLabel);	
					hayate.UiScale = EditorGUILayout.FloatField(" ",hayate.UiScale);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Particle order: ", EditorStyles.miniLabel);	
					hayate.buildOrder = (HayateEnums.BuildOrder)EditorGUILayout.EnumPopup(hayate.buildOrder);
				EditorGUILayout.EndHorizontal();
			}
			
			GUILayout.Space (15f);

			if(GUILayout.Button("Emit", EditorStyles.toolbarButton ))
			{
				if(hayate.particleSystem.simulationSpace == ParticleSystemSimulationSpace.Local)
				{
					hayate.CreateParticles( hayate.transform.InverseTransformPoint( hayate.transform.position ), hayate.Width, hayate.Height, hayate.Depth, hayate.TargetParticleSize, hayate.UiScale, hayate.buildOrder);
				}else{
					hayate.CreateParticles( hayate.transform.position, hayate.Width, hayate.Height, hayate.Depth, hayate.TargetParticleSize, hayate.UiScale, hayate.buildOrder);
				}

			}
			
			GUILayout.Space (5f);
			EditorGUILayout.LabelField ("Other options", EditorStyles.toolbarButton);
			
			EditorGUILayout.BeginHorizontal();
			
			if(hayate.isAlwaysOnTop)
			{
				GUI.color = Color.green;
				if(GUILayout.Button("Always on top", EditorStyles.toolbarButton))
					hayate.isAlwaysOnTop = !hayate.isAlwaysOnTop;
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				if(GUILayout.Button("Always on top", EditorStyles.toolbarButton))
					hayate.isAlwaysOnTop = !hayate.isAlwaysOnTop;
				GUI.color = Color.white;
			}
			
			if(hayate.isTimeScaleIndependent)
			{
				GUI.color = Color.green;
				if(GUILayout.Button("Timescale independent", EditorStyles.toolbarButton))
					hayate.isTimeScaleIndependent = !hayate.isTimeScaleIndependent;
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				if(GUILayout.Button("Timescale independent", EditorStyles.toolbarButton))
					hayate.isTimeScaleIndependent = !hayate.isTimeScaleIndependent;
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space (15f);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( TransformParticleTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (TransformParticleTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Transform particle options", EditorStyles.toolbarButton);

				hayate.transformParticle = (GameObject)EditorGUILayout.ObjectField("",hayate.transformParticle, typeof(GameObject), true, GUILayout.MaxWidth(380f));

				EditorGUILayout.BeginHorizontal();
				
				if(hayate.detachTransformParticleAfterParticleDeath)
				{
					if(GUILayout.Button("Detach", EditorStyles.toolbarButton ))
						hayate.detachTransformParticleAfterParticleDeath = !hayate.detachTransformParticleAfterParticleDeath;
					
				}else{
					if(GUILayout.Button("Delete", EditorStyles.toolbarButton ))
						hayate.detachTransformParticleAfterParticleDeath = !hayate.detachTransformParticleAfterParticleDeath;
				}
				
				EditorGUI.BeginDisabledGroup(!hayate.detachTransformParticleAfterParticleDeath);
				hayate.detachedObjectDestructionTimeAfter = EditorGUILayout.FloatField("after", hayate.detachedObjectDestructionTimeAfter, GUILayout.MaxWidth(190f));
				EditorGUI.EndDisabledGroup();
				
				EditorGUILayout.EndHorizontal();
				
				if(hayate.transformParticleLookTowardsFlightDirection)
				{
					if(GUILayout.Button("Transform particle looking at flight direction.", EditorStyles.toolbarButton ))
						hayate.transformParticleLookTowardsFlightDirection = !hayate.transformParticleLookTowardsFlightDirection;
					
				}else{
					if(GUILayout.Button("Transform particle rotation is not being altered.", EditorStyles.toolbarButton ))
						hayate.transformParticleLookTowardsFlightDirection = !hayate.transformParticleLookTowardsFlightDirection;
				}
			
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( AttractorTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (AttractorTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Attractors", EditorStyles.toolbarButton);

			EditorGUILayout.BeginHorizontal();
			
			if(GUILayout.Button("Add", EditorStyles.miniButtonLeft))
			{
				GameObject newAttractor = new GameObject();
				newAttractor.name = "Attractor " + hayate.attractors.Count.ToString();
				hayate.attractors.Add( newAttractor );
				hayate.attractorPositions.Add ( newAttractor.transform.position );
				hayate.attractorStrength.Add( 1 );
				hayate.attractorAttenuation.Add ( 1 );	
			}
			
			if(GUILayout.Button("Remove", EditorStyles.miniButtonRight))
			{
				if(hayate.attractors.Count > 0)
				{
					DestroyImmediate(hayate.attractors[ hayate.attractors.Count-1 ]);
					hayate.attractors.RemoveAt( hayate.attractors.Count-1);
					hayate.attractorStrength.RemoveAt( hayate.attractorStrength.Count-1 );
					hayate.attractorAttenuation.RemoveAt( hayate.attractorAttenuation.Count-1 );
				}
			}
			
			EditorGUILayout.EndHorizontal();

			if(hayate.attractors.Count > 0)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Reference", EditorStyles.miniLabel, GUILayout.MaxWidth( width/3.6f ));
				EditorGUILayout.LabelField("Strength", EditorStyles.miniLabel, GUILayout.MaxWidth( width/3.6f ));
				EditorGUILayout.LabelField("Attenuation", EditorStyles.miniLabel, GUILayout.MaxWidth( width/3.6f ));
				EditorGUILayout.EndHorizontal();
			}

			for(int i = 0; i < hayate.attractors.Count; i++)
			{
				if(i%2 == 0)
					GUI.color = new Color(.8f,.8f,.8f,1f);
					

				EditorGUILayout.BeginHorizontal();
				hayate.attractors[i] = (GameObject)EditorGUILayout.ObjectField("",hayate.attractors[i], typeof(GameObject), true, GUILayout.MaxWidth( width/3.6f ));
				hayate.attractorStrength[i] = EditorGUILayout.FloatField("  +/-", hayate.attractorStrength[i], GUILayout.MaxWidth( width/3.6f ));
				hayate.attractorAttenuation[i] = EditorGUILayout.FloatField("  +/-", hayate.attractorAttenuation[i], GUILayout.MaxWidth( width/3.6f ));
				EditorGUILayout.EndHorizontal();

				if(i%2 == 0)
					GUI.color = Color.white;
			}
			
			GUILayout.Space (15f);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		if ( MeshTab.faded > 0 )
		{
			EditorGUILayout.BeginFadeGroup (MeshTab.faded);
			GUILayout.Space (15f);
			EditorGUILayout.BeginVertical ("box");
			
			EditorGUILayout.LabelField ("Mesh options", EditorStyles.toolbarButton);
			
			if(hayate.useSkinnedMesh)
			{
				GUI.color = Color.green;
				if(GUILayout.Button("Use skinned mesh", EditorStyles.toolbarButton))
					hayate.useSkinnedMesh = false;
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				if(GUILayout.Button("Use skinned mesh",EditorStyles.toolbarButton))
					hayate.useSkinnedMesh = true;
				GUI.color = Color.white;
			}

			if(!hayate.useSkinnedMesh)
			{
				hayate.meshTarget = (GameObject)EditorGUILayout.ObjectField("",hayate.meshTarget, typeof(GameObject), true);

				EditorGUILayout.BeginHorizontal();

				if(hayate.useParticleSpeedCurve)
				{
					GUI.color = Color.green;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
					{
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					}
					
					GUI.color = Color.white;
				}else{
					GUI.color = Color.grey;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					
					GUI.color = Color.white;
				}

				hayate.meshFollow = (HayateEnums.MeshFollow) EditorGUILayout.EnumPopup( hayate.meshFollow, EditorStyles.toolbarPopup);
				EditorGUILayout.LabelField("Speed");

				if(hayate.useParticleSpeedCurve)
				{
					hayate.particleSpeedToMeshAnimation = EditorGUILayout.CurveField( hayate.particleSpeedToMeshAnimation );
				}else{
					hayate.particleSpeedToMesh = EditorGUILayout.FloatField("+/-", hayate.particleSpeedToMesh);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.Space(12f);

				EditorGUILayout.LabelField("Subdivision options", GUILayout.MaxWidth(188f));
			
				EditorGUILayout.BeginHorizontal();
					hayate.divisionType = (HayateEnums.DivisionType) EditorGUILayout.EnumPopup( hayate.divisionType, EditorStyles.toolbarPopup );
					EditorGUILayout.LabelField("Threshold");
					hayate.smallestTriangle = EditorGUILayout.FloatField("+/-", hayate.smallestTriangle );
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				
				if(GUILayout.Button("Get mesh info", EditorStyles.miniButtonLeft))
					hayate.retrieveMeshInfo();
				
				if(GUILayout.Button("Subdivide", EditorStyles.miniButtonMid))
					hayate.UpdateMeshCoordinates();
				
				if(GUILayout.Button("Reset", EditorStyles.miniButtonRight))
					hayate.RstMesh();
				
				EditorGUILayout.EndHorizontal();

			}else{

				hayate.skinnedMeshTarget = (GameObject)EditorGUILayout.ObjectField("",hayate.skinnedMeshTarget, typeof(GameObject), true);

				EditorGUILayout.BeginHorizontal();
					hayate.meshFollow = (HayateEnums.MeshFollow) EditorGUILayout.EnumPopup( hayate.meshFollow, EditorStyles.toolbarPopup );
					EditorGUILayout.LabelField("Speed");
					hayate.particleSpeedToMesh = EditorGUILayout.FloatField("+/-", hayate.particleSpeedToMesh);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Emit from target");
					hayate.emitFromMeshTarget = EditorGUILayout.Toggle("",hayate.emitFromMeshTarget);
				EditorGUILayout.EndHorizontal();
			
			}
			
			GUILayout.Space (15f);
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndFadeGroup ();
		}

		GUILayout.Space (15f);
		
		//End of 4.5+ Editor script
#else
		
		//Start of generally working Editor script
		
		var hayate = target as Hayate;
		
		EditorGUILayout.HelpBox("If the simulation is behaving strangely, make sure the particleSystem is set to 'Worldspace simulation'! If the simulation does not work in edit mode, push the 'pause' button twice.", MessageType.Info );
		
		if(hayate.UseTurbulence)
		{
			GUI.color = Color.green;

			if(GUILayout.Button("Turbulence : ENABLED", EditorStyles.toolbarButton ))
				hayate.UseTurbulence = !hayate.UseTurbulence;
			GUI.color = Color.white;
		}else{
			if(GUILayout.Button("Turbulence : DISABLED", EditorStyles.toolbarButton ))
				hayate.UseTurbulence = !hayate.UseTurbulence;
		}
		
		EditorGUI.BeginDisabledGroup(!hayate.UseTurbulence);
		
		if(hayate.drawTurbulenceField)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Debugging features : ENABLED", EditorStyles.toolbarButton ))
				hayate.drawTurbulenceField = !hayate.drawTurbulenceField;
			GUI.color = Color.white;
		}else{
			if(GUILayout.Button("Debugging features : DISABLED", EditorStyles.toolbarButton ))
				hayate.drawTurbulenceField = !hayate.drawTurbulenceField;
		}
		
		if(hayate.drawTurbulenceField)
		{
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Field Color", GUILayout.MaxWidth(190f));
				hayate.debugColor = EditorGUILayout.ColorField("", hayate.debugColor, GUILayout.MaxWidth(190f));
                
			EditorGUILayout.EndHorizontal();

            hayate.rayLength = EditorGUILayout.FloatField("Ray length", hayate.rayLength);
			
			hayate.fieldSize = EditorGUILayout.Vector3Field("Field size:", hayate.fieldSize);
			hayate.stepSize = EditorGUILayout.Vector3Field("Step size:", hayate.stepSize);

			GUILayout.Space(5f);

		}
		
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Turbulence settings per axis (X/Y/Z):", EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "Max particles" );
			hayate.maxParticles = EditorGUILayout.IntField( hayate.maxParticles );
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();

		hayate.isDeltaIndependent = EditorGUILayout.Toggle("Is delta independent: ", hayate.isDeltaIndependent, EditorStyles.radioButton );
		hayate.targetFps = EditorGUILayout.FloatField("Target FPS: ", hayate.targetFps);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		hayate.UseCalculationMethodX = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodX );
		hayate.UseCalculationMethodY = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodY );
		hayate.UseCalculationMethodZ = (HayateEnums.CalculationMethod) EditorGUILayout.EnumPopup( hayate.UseCalculationMethodZ );
		EditorGUILayout.EndHorizontal();
		
		hayate.AssignTurbulenceTo = (HayateEnums.AssignTo) EditorGUILayout.EnumPopup("Assign to: ", hayate.AssignTurbulenceTo );
		hayate.UseRelativeOrAbsoluteValues = (HayateEnums.TurbulenceType) EditorGUILayout.EnumPopup("Simulation: ", hayate.UseRelativeOrAbsoluteValues );


		if(	hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.animationCurve ||
		   hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.animationCurve ||
		   hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.animationCurve )
		{
			GUILayout.Label("Turbulence curves:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			if(	hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.animationCurve )
			{
				GUILayout.Label("X: ");
				hayate.turbulenceCurveX = EditorGUILayout.CurveField( hayate.turbulenceCurveX);
			}

			if(	hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.animationCurve )
			{
				GUILayout.Label("Y: ");
				hayate.turbulenceCurveY = EditorGUILayout.CurveField( hayate.turbulenceCurveY);
			}

			if(	hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.animationCurve )
			{
				GUILayout.Label("Z: ");
				hayate.turbulenceCurveZ = EditorGUILayout.CurveField( hayate.turbulenceCurveZ);
			}
			EditorGUILayout.EndHorizontal();
		}

		GUILayout.Space(15f);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Amplitude", EditorStyles.toolbarButton);
		
		if(hayate.useAmplitudeCurve)
		{
			if(GUILayout.Button("Toggle Value", EditorStyles.toolbarButton))
			{
				hayate.useAmplitudeCurve = !hayate.useAmplitudeCurve;
			}
		}else{
			if(GUILayout.Button("Toggle Curve", EditorStyles.toolbarButton))
			{
				hayate.useAmplitudeCurve = !hayate.useAmplitudeCurve;
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if(hayate.useAmplitudeCurve)
		{
			GUILayout.Space(19f);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ");
				hayate.AmplitudeCurveX = EditorGUILayout.CurveField( hayate.AmplitudeCurveX, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Y: ");
				hayate.AmplitudeCurveY = EditorGUILayout.CurveField( hayate.AmplitudeCurveY, GUILayout.MaxWidth(126f) );
				GUILayout.Label("Z: ");
				hayate.AmplitudeCurveZ = EditorGUILayout.CurveField( hayate.AmplitudeCurveZ, GUILayout.MaxWidth(126f) );
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.BeginHorizontal();
				hayate.Amplitude = EditorGUILayout.Vector3Field("", hayate.Amplitude, GUILayout.MaxWidth(380f));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Frequency", EditorStyles.toolbarButton);
		
		if(hayate.useFrequencyCurve)
		{
			if(GUILayout.Button("Toggle Value", EditorStyles.toolbarButton))
			{
				hayate.useFrequencyCurve = !hayate.useFrequencyCurve;
			}
		}else{
			if(GUILayout.Button("Toggle Curve", EditorStyles.toolbarButton))
			{
				hayate.useFrequencyCurve = !hayate.useFrequencyCurve;
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if(hayate.useFrequencyCurve)
		{
			GUILayout.Space(19f);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ");
				hayate.FrequencyCurveX = EditorGUILayout.CurveField( hayate.FrequencyCurveX );
				GUILayout.Label("Y: ");
				hayate.FrequencyCurveY = EditorGUILayout.CurveField( hayate.FrequencyCurveY );
				GUILayout.Label("Z: ");
				hayate.FrequencyCurveZ = EditorGUILayout.CurveField( hayate.FrequencyCurveZ );
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.BeginHorizontal();
				hayate.Frequency = EditorGUILayout.Vector3Field("", hayate.Frequency, GUILayout.MaxWidth(380f));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Offset", EditorStyles.toolbarButton);
		
		if(hayate.useOffsetCurve)
		{
			if(GUILayout.Button("Toggle Value", EditorStyles.toolbarButton))
			{
				hayate.useOffsetCurve = !hayate.useOffsetCurve;
			}
		}else{
			if(GUILayout.Button("Toggle Curve", EditorStyles.toolbarButton))
			{
				hayate.useOffsetCurve = !hayate.useOffsetCurve;
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if(hayate.useOffsetCurve)
		{
			GUILayout.Space(19f);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ");
				hayate.OffsetCurveX = EditorGUILayout.CurveField( hayate.OffsetCurveX );
				GUILayout.Label("Y: ");
				hayate.OffsetCurveY = EditorGUILayout.CurveField( hayate.OffsetCurveY );
				GUILayout.Label("Z: ");
				hayate.OffsetCurveZ = EditorGUILayout.CurveField( hayate.OffsetCurveZ );
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.BeginHorizontal();
				hayate.Offset = EditorGUILayout.Vector3Field("", hayate.Offset, GUILayout.MaxWidth(380f));
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Offset speed", EditorStyles.toolbarButton);
		
		if(hayate.useOffsetSpeedCurve)
		{
			if(GUILayout.Button("Toggle Value", EditorStyles.toolbarButton))
			{
				hayate.useOffsetSpeedCurve = !hayate.useOffsetSpeedCurve;
			}
		}else{
			if(GUILayout.Button("Toggle Curve", EditorStyles.toolbarButton))
			{
				hayate.useOffsetSpeedCurve = !hayate.useOffsetSpeedCurve;
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if(hayate.useOffsetSpeedCurve)
		{
			GUILayout.Space(19f);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ");
				hayate.OffsetCurveX = EditorGUILayout.CurveField( hayate.OffsetCurveX );
				GUILayout.Label("Y: ");
				hayate.OffsetCurveY = EditorGUILayout.CurveField( hayate.OffsetCurveY );
				GUILayout.Label("Z: ");
				hayate.OffsetCurveZ = EditorGUILayout.CurveField( hayate.OffsetCurveZ );
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.BeginHorizontal();
				hayate.OffsetSpeed = EditorGUILayout.Vector3Field("", hayate.OffsetSpeed, GUILayout.MaxWidth(380f));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Global force", EditorStyles.toolbarButton);
		
		if(hayate.useGlobalForceCurve)
		{
			if(GUILayout.Button("Toggle Value", EditorStyles.toolbarButton))
			{
				hayate.useGlobalForceCurve = !hayate.useGlobalForceCurve;
			}
		}else{
			if(GUILayout.Button("Toggle Curve", EditorStyles.toolbarButton))
			{
				hayate.useGlobalForceCurve = !hayate.useGlobalForceCurve;
			}
		}
		
		EditorGUILayout.EndHorizontal();
		
		if(hayate.useGlobalForceCurve)
		{
			GUILayout.Space(19f);
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ");
				hayate.GlobalForceCurveX = EditorGUILayout.CurveField( hayate.GlobalForceCurveX );
				GUILayout.Label("Y: ");
				hayate.GlobalForceCurveY = EditorGUILayout.CurveField( hayate.GlobalForceCurveY );
				GUILayout.Label("Z: ");
				hayate.GlobalForceCurveZ = EditorGUILayout.CurveField( hayate.GlobalForceCurveZ );
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.BeginHorizontal();
				hayate.GlobalForce = EditorGUILayout.Vector3Field("", hayate.GlobalForce, GUILayout.MaxWidth(380f));
			EditorGUILayout.EndHorizontal();
		}
		
		if(hayate.lockOffsetToEmitterPosition)
		{
			if(GUILayout.Button("Offset LOCKED", EditorStyles.toolbarButton))
				hayate.lockOffsetToEmitterPosition = !hayate.lockOffsetToEmitterPosition;
			
		}else{
			
			if(GUILayout.Button("Offset NOT LOCKED", EditorStyles.toolbarButton))
				hayate.lockOffsetToEmitterPosition = !hayate.lockOffsetToEmitterPosition;
		}
		
		EditorGUI.BeginDisabledGroup(hayate.lockOffsetToEmitterPosition);
		
		if(hayate.randomizeOffsetAtStart)
		{
			if(GUILayout.Button("Offset random", EditorStyles.toolbarButton))
			{
				hayate.randomizeOffsetAtStart = !hayate.randomizeOffsetAtStart;
			}
		}else{
			if(GUILayout.Button("Offset not random", EditorStyles.toolbarButton))
				hayate.randomizeOffsetAtStart = !hayate.randomizeOffsetAtStart;
		}
		
		EditorGUI.BeginDisabledGroup(!hayate.randomizeOffsetAtStart);
			EditorGUILayout.BeginHorizontal();	
				GUILayout.Label("Random offset range:");
				hayate.randomOffsetRange = EditorGUILayout.Vector2Field("", hayate.randomOffsetRange);
			EditorGUILayout.EndHorizontal();
		EditorGUI.EndDisabledGroup();
		
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.Space();
		
		
		if(	hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.precalculatedTexture ||
			hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.precalculatedTexture ||
			hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.precalculatedTexture )
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Texture turbulence settings:");
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			hayate.Turbulence = (Texture2D)EditorGUILayout.ObjectField("",hayate.Turbulence, typeof(Texture2D), true);
			EditorGUILayout.HelpBox("This texture uses different color channels per axis. Enable 'UseAlphaMask' to use the alpha channel as well. This will remove particles, that spawned where 'alpha = 0'.", MessageType.Info );
			EditorGUILayout.EndHorizontal();
			
			if(!hayate.Turbulence)
				EditorGUILayout.HelpBox("No Texture assigned! Precalculated turbulance only works with a Texture!", MessageType.Warning );
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			
			if(hayate.useAlphaMask)
			{
				if(GUILayout.Button("Alpha mask: ENABLED", EditorStyles.toolbarButton))
					hayate.useAlphaMask = !hayate.useAlphaMask;
				
			}else{
				if(GUILayout.Button("Alpha mask: DISABLED", EditorStyles.toolbarButton))
					hayate.useAlphaMask = !hayate.useAlphaMask;
			}
			
			EditorGUI.BeginDisabledGroup(!hayate.useAlphaMask);
			hayate.threshold = EditorGUILayout.FloatField("Threshold:", hayate.threshold);
			EditorGUI.EndDisabledGroup();
			
			EditorGUILayout.EndHorizontal();
			
			if(GUILayout.Button("Update turbulence!", EditorStyles.toolbarButton) && hayate.Turbulence)
				hayate.UpdateTexture();
		}
		
		EditorGUILayout.Space();
		
		if(	hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.Audio ||
			hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.Audio ||
			hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.Audio )
		{
			if(!hayate.audioClip)
				EditorGUILayout.HelpBox("No AudioClip assigned! Audio turbulance only works with an audioClip assigned!", MessageType.Warning );
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Audio turbulence settings:", GUILayout.MaxWidth(380f));
			EditorGUILayout.Space();
			
			hayate.audioClip = (AudioClip)EditorGUILayout.ObjectField("",hayate.audioClip, typeof(AudioClip), true);
			EditorGUILayout.HelpBox("Make sure to select a sample far enough into the Track to not contain silence ('At sample'). Also the amount of samples should be inbetween 256 - 4096 and be a multiple of 2.", MessageType.Info );
			hayate.amountOfSamples = EditorGUILayout.IntField("Amount of samples: ", hayate.amountOfSamples);
			hayate.atSample = EditorGUILayout.IntField("At sample: ", hayate.atSample);
			
			if(GUILayout.Button("Update turbulence!", EditorStyles.toolbarButton) && hayate.audioClip)
				HayateHelper.CalculateAudioTurbulence( hayate );
			
			GUILayout.Space(5f);
		}
		
		EditorGUILayout.Space();
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("SFX settings:");
				
		if(hayate.useSfx)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Disable SFX", EditorStyles.toolbarButton))
				hayate.useSfx = !hayate.useSfx;
			GUI.color = Color.white;
			
		}else{
			if(GUILayout.Button("Enable SFX", EditorStyles.toolbarButton))
				hayate.useSfx = !hayate.useSfx;
		}
		
		if(hayate.useSfx)
		{
			if(hayate.CreateOnStart)
			{
				GUI.color = Color.green;
				
				if(GUILayout.Button("Create On Start", EditorStyles.toolbarButton))
				{
					hayate.CreateOnStart = !hayate.CreateOnStart;
				}
				
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				
				if(GUILayout.Button("Create On Start", EditorStyles.toolbarButton))
					hayate.CreateOnStart = !hayate.CreateOnStart;
				
				GUI.color = Color.white;
			}

			if(hayate.CreateOnStart)
			{
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Dimensions: ",GUILayout.MaxWidth(80.0f));	
					hayate.Width = EditorGUILayout.FloatField(hayate.Width);
					hayate.Height = EditorGUILayout.FloatField(hayate.Height);
					hayate.Depth = EditorGUILayout.FloatField(hayate.Depth);
				EditorGUILayout.EndHorizontal();	



				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Particle Size: ",GUILayout.MaxWidth(80.0f));	
					hayate.TargetParticleSize = EditorGUILayout.FloatField(" ",hayate.TargetParticleSize);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Ui Scale: ",GUILayout.MaxWidth(80.0f));	
					hayate.UiScale = EditorGUILayout.FloatField(" ",hayate.UiScale);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Particle order: ",GUILayout.MaxWidth(80.0f));	
					hayate.buildOrder = (HayateEnums.BuildOrder)EditorGUILayout.EnumPopup(hayate.buildOrder);
				EditorGUILayout.EndHorizontal();
				
			}

			GUILayout.Space (15f);

			if(GUILayout.Button("Emit", EditorStyles.toolbarButton ))
			{
				/* 
				 * 
				 * Unity 4.5 or later specific feature
				 * 
				 * 
				if(hayate.particleSystem.simulationSpace == ParticleSystemSimulationSpace.Local)
				{
					hayate.CreateParticles( hayate.transform.InverseTransformPoint( hayate.transform.position ), hayate.Width, hayate.Height, hayate.Depth, hayate.TargetParticleSize, hayate.UiScale, hayate.buildOrder);
				}else{
					hayate.CreateParticles( hayate.transform.position, hayate.Width, hayate.Height, hayate.Depth, hayate.TargetParticleSize, hayate.UiScale, hayate.buildOrder);
				}
				 */
				hayate.CreateParticles( hayate.transform.InverseTransformPoint( hayate.transform.position ), hayate.Width, hayate.Height, hayate.Depth, hayate.TargetParticleSize, hayate.UiScale, hayate.buildOrder);
			}
			
			if(hayate.isAlwaysOnTop)
			{
				GUI.color = Color.green;
				
				if(GUILayout.Button("Always on top", EditorStyles.toolbarButton))
				{
					hayate.isAlwaysOnTop = !hayate.isAlwaysOnTop;
				}
				
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				
				if(GUILayout.Button("Always on top", EditorStyles.toolbarButton))
					hayate.isAlwaysOnTop = !hayate.isAlwaysOnTop;
				
				GUI.color = Color.white;
			}
			
			if(hayate.isTimeScaleIndependent)
			{
				GUI.color = Color.green;
				
				if(GUILayout.Button("Timescale independent", EditorStyles.toolbarButton))
				{
					hayate.isTimeScaleIndependent = !hayate.isTimeScaleIndependent;
				}
				
				GUI.color = Color.white;
			}else{
				GUI.color = Color.grey;
				
				if(GUILayout.Button("Timescale independent", EditorStyles.toolbarButton))
					hayate.isTimeScaleIndependent = !hayate.isTimeScaleIndependent;
				
				GUI.color = Color.white;
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Collision settings:");
				
		if(hayate.burstOnCollision)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Emit particles on collision: ENABLED", EditorStyles.toolbarButton))
				hayate.burstOnCollision = !hayate.burstOnCollision;
			GUI.color = Color.white;
			
		}else{
			if(GUILayout.Button("Emit particles on collision: DISABLED", EditorStyles.toolbarButton))
				hayate.burstOnCollision = !hayate.burstOnCollision;
		}
		
		if(hayate.burstOnCollision)
		{
			hayate.burstNum = EditorGUILayout.IntField("Emit particles: ", hayate.burstNum);
		
			GUILayout.Space(5f);
		}
			
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Transform particle options:");
		
		if(hayate.useTransformParticle)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Transform particles: ENABLED", EditorStyles.toolbarButton))
				hayate.useTransformParticle = !hayate.useTransformParticle;
			GUI.color = Color.white;
			
		}else{
			if(GUILayout.Button("Transform particles: DISABLED", EditorStyles.toolbarButton))
				hayate.useTransformParticle = !hayate.useTransformParticle;
		}
		
		if(hayate.useTransformParticle)
		{
			hayate.transformParticle = (GameObject)EditorGUILayout.ObjectField("",hayate.transformParticle, typeof(GameObject), true);
			
			EditorGUILayout.LabelField("What to do with Transform after particle death:");
			
			EditorGUILayout.BeginHorizontal();
			
				if(hayate.detachTransformParticleAfterParticleDeath)
				{
					if(GUILayout.Button("Detach", EditorStyles.toolbarButton))
						hayate.detachTransformParticleAfterParticleDeath = !hayate.detachTransformParticleAfterParticleDeath;
				
				}else{
					if(GUILayout.Button("Delete", EditorStyles.toolbarButton))
						hayate.detachTransformParticleAfterParticleDeath = !hayate.detachTransformParticleAfterParticleDeath;
				}
				
				EditorGUI.BeginDisabledGroup(!hayate.detachTransformParticleAfterParticleDeath);
			hayate.detachedObjectDestructionTimeAfter = EditorGUILayout.FloatField("Time until death:", hayate.detachedObjectDestructionTimeAfter);
				EditorGUI.EndDisabledGroup();
				
			EditorGUILayout.EndHorizontal();
			
			if(hayate.transformParticleLookTowardsFlightDirection)
			{
				if(GUILayout.Button("Transform particle looking at flight direction.", EditorStyles.toolbarButton))
					hayate.transformParticleLookTowardsFlightDirection = !hayate.transformParticleLookTowardsFlightDirection;
			
			}else{
				if(GUILayout.Button("Transform particle rotation is not being altered.", EditorStyles.toolbarButton))
					hayate.transformParticleLookTowardsFlightDirection = !hayate.transformParticleLookTowardsFlightDirection;
			}

			GUILayout.Space(5f);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Attractors:");
		
		if(hayate.useAttractor)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Using attractors: ", EditorStyles.toolbarButton))
				hayate.useAttractor = !hayate.useAttractor;
			GUI.color = Color.white;
			
		}else{
			if(GUILayout.Button("Attractor(s) disabled", EditorStyles.toolbarButton))
				hayate.useAttractor = !hayate.useAttractor;
		}
		
		if(hayate.useAttractor)
		{
			EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("Add", EditorStyles.toolbarButton))
				{
					GameObject newAttractor = new GameObject();
					newAttractor.name = "Attractor "+hayate.attractors.Count.ToString();
					hayate.attractors.Add(newAttractor);
					hayate.attractorPositions.Add (newAttractor.transform.position);
					hayate.attractorStrength.Add(1);
					hayate.attractorAttenuation.Add (1);
				}

				if(GUILayout.Button("Remove", EditorStyles.toolbarButton))
				{
					if(hayate.attractors.Count > 0)
					{
						DestroyImmediate(hayate.attractors[ hayate.attractors.Count-1] );
						hayate.attractors.RemoveAt( hayate.attractors.Count-1 );
						hayate.attractorPositions.RemoveAt( hayate.attractorPositions.Count-1 );
						hayate.attractorStrength.RemoveAt( hayate.attractorStrength.Count-1 );
						hayate.attractorAttenuation.RemoveAt( hayate.attractorAttenuation.Count-1 );
					}
				}

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Reference", GUILayout.MaxWidth(120.0f));
			EditorGUILayout.LabelField("Strength", GUILayout.MaxWidth(120.0f));
			EditorGUILayout.LabelField("Attenuation", GUILayout.MaxWidth(120.0f));
			EditorGUILayout.EndHorizontal();

			for(int i = 0; i < hayate.attractors.Count; i++)
			{
				
				EditorGUILayout.BeginHorizontal();
				hayate.attractors[i] = (GameObject)EditorGUILayout.ObjectField("",hayate.attractors[i], typeof(GameObject), true, GUILayout.MaxWidth(120.0f));
				hayate.attractorStrength[i] = EditorGUILayout.FloatField("", hayate.attractorStrength[i], GUILayout.MaxWidth(120.0f));
				hayate.attractorAttenuation[i] = EditorGUILayout.FloatField("", hayate.attractorAttenuation[i], GUILayout.MaxWidth(120.0f));
				EditorGUILayout.EndHorizontal();
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Mesh target options:");
		
		if(hayate.moveToMesh)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Moving to mesh", EditorStyles.toolbarButton))
				hayate.moveToMesh = !hayate.moveToMesh;
			GUI.color = Color.white;

			if(hayate.useSkinnedMesh)
			{
				GUI.color = Color.green;
				if(GUILayout.Button("Using skinned mesh", EditorStyles.toolbarButton))
					hayate.useSkinnedMesh = false;
				GUI.color = Color.white;
			}else{

				if(GUILayout.Button("SkinnedMesh target DISABLED", EditorStyles.toolbarButton))
					hayate.useSkinnedMesh = true;

			}
			
		}else{
			if(GUILayout.Button("Mesh target DISABLED", EditorStyles.toolbarButton))
				hayate.moveToMesh = !hayate.moveToMesh;
		}


		
		if(hayate.moveToMesh)
		{
			if(!hayate.useSkinnedMesh)
			{
				EditorGUILayout.BeginHorizontal();
				hayate.meshFollow = (HayateEnums.MeshFollow) EditorGUILayout.EnumPopup( hayate.meshFollow);
				hayate.meshTarget = (GameObject)EditorGUILayout.ObjectField("",hayate.meshTarget, typeof(GameObject), true);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				
				if(hayate.useParticleSpeedCurve)
				{
					GUI.color = Color.green;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
					{
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					}
					
					GUI.color = Color.white;
				}else{
					GUI.color = Color.grey;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					
					GUI.color = Color.white;
				}
				
				if(hayate.useParticleSpeedCurve)
				{
					hayate.particleSpeedToMeshAnimation = EditorGUILayout.CurveField( "Speed: ", hayate.particleSpeedToMeshAnimation );
				}else{
					hayate.particleSpeedToMesh = EditorGUILayout.FloatField("Speed: ", hayate.particleSpeedToMesh);
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Subdivision options:");
				hayate.divisionType = (HayateEnums.DivisionType) EditorGUILayout.EnumPopup( hayate.divisionType);
				hayate.smallestTriangle = EditorGUILayout.FloatField("Smallest triangle: ", hayate.smallestTriangle);
				
				EditorGUILayout.BeginHorizontal();
				
				if(GUILayout.Button("Get mesh info", EditorStyles.toolbarButton))
					hayate.retrieveMeshInfo();
				
				if(GUILayout.Button("Subdivide", EditorStyles.toolbarButton))
					hayate.UpdateMeshCoordinates();
				
				if(GUILayout.Button("Reset", EditorStyles.toolbarButton))
					hayate.RstMesh();
				
				EditorGUILayout.EndHorizontal();
				
				for(int i = 0; i < 5; i++)
					EditorGUILayout.Space();
			}else{

				EditorGUILayout.BeginHorizontal();
				hayate.meshFollow = (HayateEnums.MeshFollow) EditorGUILayout.EnumPopup( hayate.meshFollow );
				hayate.skinnedMeshTarget = (GameObject)EditorGUILayout.ObjectField("",hayate.skinnedMeshTarget, typeof(GameObject), true);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				//hayate.emitFromMeshTarget = EditorGUILayout.Toggle("Emit from target: ",hayate.emitFromMeshTarget);
				
				if(hayate.emitFromMeshTarget)
				{
					GUI.color = Color.green;
					
					if(GUILayout.Button("Emit from Target", EditorStyles.toolbarButton))
					{
						hayate.emitFromMeshTarget = !hayate.emitFromMeshTarget;
					}
					
					GUI.color = Color.white;
				}else{
					GUI.color = Color.grey;
					
					if(GUILayout.Button("Emit from Target", EditorStyles.toolbarButton))
						hayate.emitFromMeshTarget = !hayate.emitFromMeshTarget;
					
					GUI.color = Color.white;
				}
				
				if(hayate.useParticleSpeedCurve)
				{
					GUI.color = Color.green;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
					{
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					}
					
					GUI.color = Color.white;
				}else{
					GUI.color = Color.grey;
					
					if(GUILayout.Button("Animate", EditorStyles.toolbarButton))
						hayate.useParticleSpeedCurve = !hayate.useParticleSpeedCurve;
					
					GUI.color = Color.white;
				}
				
				if(hayate.useParticleSpeedCurve)
				{
					hayate.particleSpeedToMeshAnimation = EditorGUILayout.CurveField( "Speed: ", hayate.particleSpeedToMeshAnimation );
				}else{
					hayate.particleSpeedToMesh = EditorGUILayout.FloatField("Speed: ", hayate.particleSpeedToMesh);
				}
				EditorGUILayout.EndHorizontal();

				for(int i = 0; i < 5; i++)
					EditorGUILayout.Space();
			}
		}
		
		EditorGUI.EndDisabledGroup();
		#endif
	}
}
