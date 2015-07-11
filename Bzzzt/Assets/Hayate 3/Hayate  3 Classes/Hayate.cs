using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

#pragma warning disable 0618

[ExecuteInEditMode]
public class Hayate : MonoBehaviour
{
	//Debug options
	public bool drawTurbulenceField;
	public Vector3 fieldSize = new Vector3(5, 0, 5);
	public Vector3 stepSize = new Vector3(0.1f, 0.15f, 0.15f);
	public Color debugColor = new Color(0f, 1f, 0f, 1f);
	public float rayLength = 1.0f;
	
	public bool UseTurbulence = true;
	
	//Texture turbulence
	public Texture2D Turbulence;
	public bool removeCurrentParticle;
	public bool useAlphaMask;
	public float threshold = 0.5f;
	public AnimationCurve turbulenceCurveX;
	public AnimationCurve turbulenceCurveY;
	public AnimationCurve turbulenceCurveZ;
	
	//Audio turbulence
	public AudioClip audioClip;
	public int amountOfSamples = 512;
	public int atSample = 10000;
	public float[] hayateSamples;
	
	public Color32[] turbulence;
	
	//Global settings
	public HayateEnums.AssignTo AssignTurbulenceTo;
	public HayateEnums.TurbulenceType UseRelativeOrAbsoluteValues;
	public HayateEnums.CalculationMethod UseCalculationMethodX;
	public HayateEnums.CalculationMethod UseCalculationMethodY;
	public HayateEnums.CalculationMethod UseCalculationMethodZ;
	
	public Vector3 Amplitude = Vector3.one;
	public AnimationCurve AmplitudeCurveX = new AnimationCurve();
	public AnimationCurve AmplitudeCurveY = new AnimationCurve();
	public AnimationCurve AmplitudeCurveZ = new AnimationCurve();
	public bool useAmplitudeCurve;

	public Vector3 Frequency = Vector3.one;
	public AnimationCurve FrequencyCurveX = new AnimationCurve();
	public AnimationCurve FrequencyCurveY = new AnimationCurve();
	public AnimationCurve FrequencyCurveZ = new AnimationCurve();
	public bool useFrequencyCurve;

	public Vector3 GlobalForce = Vector3.zero;
	public AnimationCurve  GlobalForceCurveX = new AnimationCurve();
	public AnimationCurve  GlobalForceCurveY = new AnimationCurve();
	public AnimationCurve  GlobalForceCurveZ = new AnimationCurve();
	public bool useGlobalForceCurve;
	
	public Vector3 Offset;
	public AnimationCurve OffsetCurveX = new AnimationCurve();
	public AnimationCurve OffsetCurveY = new AnimationCurve();
	public AnimationCurve OffsetCurveZ = new AnimationCurve();
	public bool useOffsetCurve;

	public Vector3 OffsetSpeed;
	public AnimationCurve OffsetSpeedCurveX = new AnimationCurve();
	public AnimationCurve OffsetSpeedCurveY = new AnimationCurve();
	public AnimationCurve OffsetSpeedCurveZ = new AnimationCurve();
	public bool useOffsetSpeedCurve;

	public bool lockOffsetToEmitterPosition;
	public bool randomizeOffsetAtStart;
	public Vector2 randomOffsetRange = new Vector2(-1000, 1000);
	
	//public List<Vector3> accelerations = new List<Vector3>(); Prototype
	
	//Collision
	public bool burstOnCollision;
	public int burstNum;

	//Frame rate settings
	public bool isDeltaIndependent;
	public float targetFps = 60f;
	
	//Transform particles
	public bool useTransformParticle;
	public bool detachTransformParticleAfterParticleDeath;
	public float detachedObjectDestructionTimeAfter = 1f;
	public bool transformParticleLookTowardsFlightDirection;
	public GameObject transformParticle;
	List<ParticleSystem.Particle> particleList;
		
	private List<GameObject> transformParticles = new List<GameObject>();
	
	//Attractors
	public bool useAttractor;
	public Transform followTransform;
	public Vector3 followPosition;
	public float followStrength;
	public List<GameObject> attractors = new List<GameObject>();
	public List<Vector3> attractorPositions = new List<Vector3>();
	public List<float> attractorStrength = new List<float>();
	public List<float> attractorAttenuation = new List<float>();
	
	//Mesh target
	public bool moveToMesh;
	public bool useSkinnedMesh;
	public GameObject meshTarget;
	public GameObject skinnedMeshTarget;
	public bool useParticleSpeedCurve;
	public float particleSpeedToMesh = 0.5f;
	public AnimationCurve particleSpeedToMeshAnimation;
	public HayateEnums.MeshFollow meshFollow;
	public bool emitFromMeshTarget;
	private Vector3[] targets;
	private Mesh resetMesh;
	public float smallestTriangle = 0.1f;
	public HayateEnums.DivisionType divisionType;
	
	//Misc
	private ParticleSystem.Particle[] particles;
	private int particleCount;
	
	public float deltaTime;
	private List<int> index = new List<int>();
	private List<int> length = new List<int>();
	
	private int turbulenceWidth;
	private Vector3 currentPosition;
	private float lastUpdateTime;
	
	//Special FX
	public bool useSfx;
	public bool CreateOnStart = true;

	public HayateEnums.BuildOrder buildOrder = HayateEnums.BuildOrder.TopLeftBottomRight;

	public float Width = 10f;
	public float Height = 10f;
	public float Depth = 10f;
	
	public float TargetParticleSize = 1.0f;

	public float UiScale = 1f;

	public bool isAlwaysOnTop;
	public bool isTimeScaleIndependent;
	private float previousUpdate;
	
	//Buffer
	private Vector3 debugPos;
	private Vector3 debugTurb;
	private Vector3 debugTarget;
	
	private int normalLength;
	private int endLength;
	
	private int _index;
	private int _length;
	
	private Vector3 desiredVelocity;
	private Vector3 _desiredVelocity;
	
	private GameObject temp;
	private int safeIndex;
	
	private Mesh skinnedMeshBuffer;
	private Vector3[] skinnedMeshVertices;
	
	public int maxParticles = 1000; // In Unity 3.5, Shuriken does not support particleSystem.maxParticles
	
	void Start()
	{
		if (isAlwaysOnTop)
		{
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			particleSystem.renderer.sortingLayerName = "Foreground";
			particleSystem.renderer.sortingOrder = 2;
#endif
		}
		
		if (detachTransformParticleAfterParticleDeath && !Application.isPlaying)
			Debug.Log("Detaching transform particles only work when playing the scene.");
		
		if(UseCalculationMethodX == HayateEnums.CalculationMethod.precalculatedTexture ||
			UseCalculationMethodY == HayateEnums.CalculationMethod.precalculatedTexture ||
			UseCalculationMethodZ == HayateEnums.CalculationMethod.precalculatedTexture
			)
			UpdateTexture();

		if(UseCalculationMethodX == HayateEnums.CalculationMethod.Audio || UseCalculationMethodY == HayateEnums.CalculationMethod.Audio || UseCalculationMethodZ == HayateEnums.CalculationMethod.Audio )
			HayateHelper.CalculateAudioTurbulence( this );
		
		if (randomizeOffsetAtStart)
		{
			Offset = new Vector3(UnityEngine.Random.Range(randomOffsetRange.x, randomOffsetRange.y), UnityEngine.Random.Range(randomOffsetRange.x, randomOffsetRange.y), UnityEngine.Random.Range(randomOffsetRange.x, randomOffsetRange.y));
		}
		
		if(CreateOnStart && useSfx)
		{
			CreateParticles( transform.InverseTransformPoint( transform.position ), Width, Height, Depth, TargetParticleSize, UiScale, buildOrder);
			GetComponent<ParticleSystem>().Play();
		}
	
		previousUpdate = Time.realtimeSinceStartup;
	}
	
	void OnEnable()
	{
		if(CreateOnStart && useSfx)
		{
			CreateParticles( transform.InverseTransformPoint( transform.position ), Width, Height, Depth, TargetParticleSize, UiScale, buildOrder);
			GetComponent<ParticleSystem>().Play();
		}
	}
	
	void Update()
	{
		if (!UseTurbulence )
			return;
		
		CheckValues();
		EvaluateCurves();
		
#if UNITY_EDITOR
		if(useSfx)
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				CreateParticles( transform.InverseTransformPoint( transform.position ), Width, Height, Depth, TargetParticleSize, UiScale, buildOrder);
			}
				
		}
#endif
		if (drawTurbulenceField)
			DebugField();

		if (!GetComponent<ParticleSystem>())
		{
			gameObject.AddComponent<ParticleSystem>();
		}
		
		if(isTimeScaleIndependent)
		{
			float DeltaTime = Time.realtimeSinceStartup - previousUpdate;
        	GetComponent<ParticleSystem>().Simulate(DeltaTime, true, false); 
        	previousUpdate = Time.realtimeSinceStartup;
		}
		
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		if (particles == null || maxParticles != particles.Length)
			particles = new ParticleSystem.Particle[particleSystem.maxParticles];
#else
		if (particles == null || maxParticles != particles.Length)
			particles = new ParticleSystem.Particle[maxParticles];
#endif

		if (isDeltaIndependent)
		{
			deltaTime = 1f / targetFps;
		} else {
			deltaTime = Time.deltaTime;
			
			if (!Application.isPlaying)
			{
				deltaTime = Time.realtimeSinceStartup - lastUpdateTime;
				lastUpdateTime = Time.realtimeSinceStartup;
			}
		}
		
		particleCount = GetComponent<ParticleSystem>().GetParticles(particles);
		
		currentPosition = transform.position;
		
		for (int i = 0; i < attractors.Count; i++)
		{
			attractorPositions[i] = attractors[i].transform.position;
		}
		
		index.Add(0);					//Can be used to distribute calculations into multiple calls
		length.Add(particleCount);		// e.g: Calculations only every n frame or n particles get updated every frame
		HayateUpdate();
		
		GetComponent<ParticleSystem>().SetParticles(particles, particleCount);
	}
	
	private void DebugField()
	{
		Vector3 debugPos;
		Vector3 debugTurb;
		Vector3 debugTarget;
		
		for (float x = -fieldSize.x; x <= fieldSize.x; x += stepSize.x)
		{
			for (float y = -fieldSize.y; y <= fieldSize.y; y += stepSize.y)
			{
				for (float z = -fieldSize.z; z <= fieldSize.z; z += stepSize.z)
				{
					debugPos.x = transform.position.x + x;
					debugPos.y = transform.position.y + y;
					debugPos.z = transform.position.z + z;
					debugTurb = HayateTurbulence.GetTurbulence(debugPos, this);
					
					if (UseRelativeOrAbsoluteValues == HayateEnums.TurbulenceType.relative)
					{
						debugTarget = debugPos + Vector3.Scale(debugTurb, new Vector3(10f, 10f, 10f) * rayLength);
					}
					else
					{
						debugTarget = debugPos + Vector3.Scale(debugTurb, new Vector3(0.1f, 0.1f, 0.1f) * rayLength);
					}
					
					if (removeCurrentParticle)
					{
						removeCurrentParticle = false;
					}
					else
					{
						Debug.DrawLine(debugPos, debugTarget, debugColor);
					}
				}
			}
		}
	}
	
	private void HayateUpdate()
	{
		if (index.Count > 0)
		{
			_index = index[0];
			_length = length[0];
			index.RemoveAt(0);
			length.RemoveAt(0);
		}
		else
		{
			Debug.LogError("Index empty! Thread can't execute.");
			return;
		}
		/*
		 * Prototype
		if(accelerations.Count < particleCount)
		{
			for(int i = 0; i < particleCount - accelerations.Count; i++)
			{
				accelerations.Add( Vector3.zero );
			}
		}
		*/
		for (int i = _index; i < (_index + _length); i++)
		{
			if ( AssignTurbulenceTo == HayateEnums.AssignTo.position )
			{
				if (UseRelativeOrAbsoluteValues == HayateEnums.TurbulenceType.absolute)
				{
					if(particles[i].velocity == Vector3.zero)
						particles[i].velocity = particles[i].position;
					
					particles[i].position = transform.position + HayateTurbulence.GetTurbulence(particles[i].velocity, this);
				}
				else
				{
					particles[i].position += HayateTurbulence.GetTurbulence(particles[i].position, this);
				}
			}
			else if ( AssignTurbulenceTo == HayateEnums.AssignTo.velocity )
			{
				if (UseRelativeOrAbsoluteValues == HayateEnums.TurbulenceType.absolute)
				{
					particles[i].velocity = HayateTurbulence.GetTurbulence(particles[i].position, this);
				}
				else
				{
					particles[i].velocity += HayateTurbulence.GetTurbulence(particles[i].position, this);
				}
			}
			/* 
			 * Prototype
			else if ( AssignTurbulenceTo == HayateEnums.AssignTo.acceleration )
			{
				accelerations[i] = HayateTurbulence.GetTurbulence(particles[i].position, this);
				
				if (UseRelativeOrAbsoluteValues == HayateEnums.TurbulenceType.absolute)
				{
					particles[i].velocity = HayateTurbulence.GetTurbulence(particles[i].position, this);
				}
				else
				{
					particles[i].velocity += HayateTurbulence.GetTurbulence(particles[i].position, this);
				}
			}
			*/
			if (useAttractor)
			{
				for (int a = 0; a < attractors.Count; a++)
				{
					desiredVelocity = attractorPositions[a] - particles[i].position;
					particles[i].velocity += (desiredVelocity.normalized * attractorStrength[a] * deltaTime) * (1f - Mathf.Clamp01((Vector3.Distance(particles[i].position, attractorPositions[a]) / attractorAttenuation[a])));
				}
					
			}
			
			if (useAlphaMask && removeCurrentParticle && particles[i].lifetime >= particles[i].startLifetime - threshold)
			{
				particles[i].lifetime = 0;
				removeCurrentParticle = false;
			}
		}
		
		if (useTransformParticle && transformParticle && Application.isPlaying)
		{
			particleList = new List<ParticleSystem.Particle> ();

			for(int i = 0; i < particleCount; i++)
			{
				particleList.Add( particles[i] );
			}
	
			if (transformParticles.Count < particleList.Count) 
			{
				int diff = particleList.Count - transformParticles.Count;
	
				for(int i = 0; i < diff; i++)
				{
					GameObject g = Instantiate(transformParticle) as GameObject;
					g.transform.parent = transform;
					transformParticles.Add( g );
				}
			}
	
			for(int i = 0; i < particleList.Count; i++)
			{
				transformParticles[i].transform.position = particleList[i].position;
	
				if(transformParticleLookTowardsFlightDirection)
				{
					transformParticles[i].transform.LookAt( particles[i].position + particles[i].velocity );
				}
				
				if(particleList[i].lifetime < 1f)
				{
					particleList.RemoveAt(i);
					
					if(detachTransformParticleAfterParticleDeath)
					{
						Destroy (transformParticles[i].gameObject, detachedObjectDestructionTimeAfter);
					}else{
						Destroy (transformParticles[i].gameObject);
					}			
					
					transformParticles.RemoveAt(i);
					particleCount--;
					i--;
				}
			}

			particles = particleList.ToArray ();
		}
		else
		{
			for (int n = 0; n < transformParticles.Count; n++)
			{
				DestroyImmediate(transformParticles[n]);
				transformParticles.RemoveAt(n);
			}
		}
		
		if (lockOffsetToEmitterPosition)
		{
			float halfTextureWidth = turbulenceWidth / 2;
			
			if (
				UseCalculationMethodX == HayateEnums.CalculationMethod.precalculatedTexture ||
				UseCalculationMethodY == HayateEnums.CalculationMethod.precalculatedTexture ||
				UseCalculationMethodZ == HayateEnums.CalculationMethod.precalculatedTexture ||
				UseCalculationMethodX == HayateEnums.CalculationMethod.Audio ||
				UseCalculationMethodY == HayateEnums.CalculationMethod.Audio ||
				UseCalculationMethodZ == HayateEnums.CalculationMethod.Audio)
			{
				Offset = new Vector3(currentPosition.x / Frequency.x, currentPosition.y / Frequency.y, currentPosition.z / Frequency.z) * 100f + new Vector3(halfTextureWidth, halfTextureWidth, halfTextureWidth);
			}
			else
			{
				Offset = new Vector3(currentPosition.x / Frequency.x, currentPosition.y / Frequency.y, currentPosition.z / Frequency.z);
			}
			
		}
		else
		{
			if(GetComponent<ParticleSystem>().isPlaying)
				Offset += OffsetSpeed * deltaTime;
		}
		
		if (moveToMesh)
		{
			float meshSpeed = 0;
			
			if (!useSkinnedMesh && meshTarget)
			{
				if (meshTarget.GetComponent<MeshFilter>())
				{
					targets = meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices;
					
					for (int z = _index; z < (_index + _length); z++)
					{
						if(useParticleSpeedCurve)
						{
							meshSpeed = particleSpeedToMeshAnimation.Evaluate(particles[z].startLifetime - particles[z].lifetime);
						}else{
							meshSpeed = particleSpeedToMesh;
						}
						
						if(meshSpeed < 0)
							meshSpeed = 0;
						
						safeIndex = z % targets.Length;
						
						if (meshFollow == HayateEnums.MeshFollow.byDistance)
						{
							particles[z].position = Vector3.Lerp(particles[z].position, meshTarget.transform.TransformPoint(targets[safeIndex]), 1 / Mathf.Pow(Vector3.Distance(meshTarget.transform.TransformPoint(targets[safeIndex]), particles[z].position), 2) * meshSpeed);
						}
						else if (meshFollow == HayateEnums.MeshFollow.byTime)
						{
							particles[z].position = Vector3.Lerp(particles[z].position, meshTarget.transform.TransformPoint(targets[safeIndex]), meshSpeed * deltaTime);
						}
						else
						{
							_desiredVelocity = meshTarget.transform.TransformPoint(targets[safeIndex]) - particles[z].position;
							particles[z].velocity += _desiredVelocity.normalized * meshSpeed * deltaTime;
						}
					}
				}
				else
				{
					Debug.LogWarning("No MeshFilter attached to this GameObject!");
					meshTarget = null;
				}
			}
			
			if (useSkinnedMesh && skinnedMeshTarget)
			{
				if (skinnedMeshTarget.GetComponent<SkinnedMeshRenderer>())
				{
					if (skinnedMeshBuffer == null)
						skinnedMeshBuffer = new Mesh();
					
					skinnedMeshTarget.GetComponent<SkinnedMeshRenderer>().BakeMesh(skinnedMeshBuffer);
					skinnedMeshVertices = skinnedMeshBuffer.vertices;
					targets = skinnedMeshBuffer.vertices;
					
					for (int z = _index; z < (_index + _length); z++)
					{
						if(useParticleSpeedCurve)
						{
							meshSpeed = particleSpeedToMeshAnimation.Evaluate(particles[z].lifetime);
						}else{
							meshSpeed = particleSpeedToMesh;
						}
						
						if(meshSpeed < 0)
							meshSpeed = 0;
						
						safeIndex = z % targets.Length;
						
						if (particles[z].startLifetime - particles[z].lifetime <= 0.05f)
						{
							particles[z].position = skinnedMeshTarget.transform.TransformPoint(skinnedMeshVertices[UnityEngine.Random.Range(0, skinnedMeshVertices.Length)]);
						}
						else
						{
							if (meshFollow == HayateEnums.MeshFollow.byDistance)
							{
								particles[z].position = Vector3.Lerp(
									particles[z].position,
									skinnedMeshTarget.transform.TransformPoint( targets[safeIndex] ),
									1 / Mathf.Pow(Vector3.Distance( skinnedMeshTarget.transform.TransformPoint( targets[safeIndex] ), particles[z].position), 2) * meshSpeed );
							}
							else if ( meshFollow == HayateEnums.MeshFollow.byTime )
							{
								particles[z].position = Vector3.Lerp(particles[z].position, skinnedMeshTarget.transform.TransformPoint(targets[safeIndex]), meshSpeed * deltaTime);
							}
							else
							{
								_desiredVelocity = skinnedMeshTarget.transform.TransformPoint(targets[safeIndex]) - particles[z].position;
								particles[z].velocity += _desiredVelocity.normalized * meshSpeed * deltaTime;
							}
						}
					}
				}
				else
				{
					Debug.LogWarning("No SkinnedMesh attached to this GameObject!");
					skinnedMeshTarget = null;
				}
			}
		}
	}
	
	public void UpdateTexture()
	{
		if (Turbulence)
		{
			turbulenceWidth = Turbulence.width;
			turbulence = Turbulence.GetPixels32(0);
		}
		else
		{
			Debug.LogWarning("No texture assigned. Precaluclated texture turbulence won't work!");
		}
	}
	
	public void retrieveMeshInfo()
	{
		if (!meshTarget)
		{
			Debug.LogWarning("No mesh target assigned!");
			return;
		}
		
		List<float> TriLength = new List<float>();
		
		Debug.Log("Vertices: " + meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
		Debug.Log("Triangles: " + meshTarget.GetComponent<MeshFilter>().sharedMesh.triangles.Length);
		
		Vector3[] vertices = meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices;
		int[] tris = meshTarget.GetComponent<MeshFilter>().sharedMesh.triangles;
		
		for (int i = 0; i < tris.Length; i += 3)
		{
			float l = HayateHelper.SignedVolumeOfTriangle( vertices[tris[i]], vertices[tris[i + 1]], vertices[tris[i + 2]]);
			
			if (l != 0)
			{
				TriLength.Add(l);
			}
		}
		TriLength.Sort();
		
		Debug.Log("Smallest trangle: " + TriLength[1] + " Largest triangle: " + TriLength[TriLength.Count - 1]);
		Debug.Log("Mesh target ready to work with " + vertices.Length + " particles.");
	}
	
	public void RstMesh()
	{
		if (!resetMesh)
		{
			Debug.LogWarning("No mesh subdivided yet.");
			return;
		}
		
		meshTarget.GetComponent<MeshFilter>().sharedMesh = resetMesh;
	}
	
	public void UpdateMeshCoordinates()
	{
		if (!meshTarget)
		{
			Debug.LogWarning("No mesh target assigned!");
			return;
		}
		
		resetMesh = meshTarget.GetComponent<MeshFilter>().sharedMesh;
		
		meshTarget.GetComponent<MeshFilter>().mesh = HayateHelper.SubDivide( meshTarget, divisionType, smallestTriangle);
		
	}
	
	public AnimationCurve EmptyAnimationCurve()
	{
		AnimationCurve a = new AnimationCurve();
		Keyframe k1 = new Keyframe();
		Keyframe k2 = new Keyframe();
		k1.time = 0;
		k1.value = -1f;
		k2.time = GetComponent<ParticleSystem>().duration;
		k2.value = 1f;
		a.AddKey( k1 );
		a.AddKey( k2 );

		return a;
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (burstOnCollision)
		{
			GetComponent<ParticleSystem>().Emit(burstNum);
			GetComponent<ParticleSystem>().Play();
		}
	}
	
	public void CreateParticles( Vector3 _Position, float _Width, float _Height, float _Depth, float TargetParticleSize, float _UiScale, HayateEnums.BuildOrder _buildOrder)
	{
		GetComponent<ParticleSystem>().Stop ();
		ParticleSystem.Particle[] p = new ParticleSystem.Particle[maxParticles];
		
		p = HayateHelper.CreateParticlesDynamically( this, _Position, _Width, _Height, _Depth, TargetParticleSize, _UiScale, _buildOrder);
		GetComponent<ParticleSystem>().SetParticles( p, maxParticles );
		GetComponent<ParticleSystem>().Play();
	}

	public void EvaluateCurves()
	{
		if(useAmplitudeCurve)
		{
			Amplitude = new Vector3(AmplitudeCurveX.Evaluate(GetComponent<ParticleSystem>().time),
			                        AmplitudeCurveY.Evaluate(GetComponent<ParticleSystem>().time),
			                        AmplitudeCurveZ.Evaluate(GetComponent<ParticleSystem>().time)
			                        );
		}

		if(useFrequencyCurve)
		{
			Frequency = new Vector3(FrequencyCurveX.Evaluate(GetComponent<ParticleSystem>().time),
			                        FrequencyCurveY.Evaluate(GetComponent<ParticleSystem>().time),
			                        FrequencyCurveZ.Evaluate(GetComponent<ParticleSystem>().time)
			                        );
		}

		if(useOffsetCurve)
		{
			Offset = new Vector3(OffsetCurveX.Evaluate(GetComponent<ParticleSystem>().time),
		                        OffsetCurveY.Evaluate(GetComponent<ParticleSystem>().time),
		                        OffsetCurveZ.Evaluate(GetComponent<ParticleSystem>().time)
		                        );
		}
		
		if(useOffsetSpeedCurve)
		{
			OffsetSpeed = new Vector3(OffsetSpeedCurveX.Evaluate(GetComponent<ParticleSystem>().time),
		                        OffsetSpeedCurveY.Evaluate(GetComponent<ParticleSystem>().time),
		                        OffsetSpeedCurveZ.Evaluate(GetComponent<ParticleSystem>().time)
		                        );
		}

		if(useGlobalForceCurve)
		{
			GlobalForce = new Vector3(GlobalForceCurveX.Evaluate(GetComponent<ParticleSystem>().time),
			                          GlobalForceCurveY.Evaluate(GetComponent<ParticleSystem>().time),
			                          GlobalForceCurveZ.Evaluate(GetComponent<ParticleSystem>().time)
			                        );
		}
		
		if(useParticleSpeedCurve)
		{
			particleSpeedToMesh = particleSpeedToMeshAnimation.Evaluate( GetComponent<ParticleSystem>().time );
		}
	}
	

	#if UNITY_EDITOR
	void OnDrawGizmos ()
	{
		for(int i = 0; i < attractors.Count; i++)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(attractors[i].transform.position, attractorAttenuation[i]);
		}
	}
	#endif
	
	private void CheckValues()
	{
		if (audioClip)
		{
			atSample = (int)Mathf.Clamp(atSample, 0, audioClip.samples - (amountOfSamples * 2f));
			amountOfSamples = (int)Mathf.Clamp(amountOfSamples, 0, 2048f);
		}
		
		if (Frequency.x == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( x < 0 > x)");
			Frequency = new Vector3(0.001f, Frequency.y, Frequency.z);
		}
		
		if (Frequency.y == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( y < 0 > y)");
			Frequency = new Vector3(Frequency.x, 0.001f, Frequency.z);
		}
		
		if (Frequency.z == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( z < 0 > z)");
			Frequency = new Vector3(Frequency.x, Frequency.y, 0.001f);
		}
		
		if (Amplitude.x == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( x < 0 > x)");
			Amplitude = new Vector3(0.001f, Amplitude.y, Amplitude.z);
		}
		
		if (Amplitude.y == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( y < 0 > y)");
			Amplitude = new Vector3(Amplitude.x, 0.001f, Amplitude.z);
		}
		
		if (Amplitude.z == 0)
		{
			Debug.LogWarning("Frequency must not be 0, ( z < 0 > z)");
			Amplitude = new Vector3(Amplitude.x, Amplitude.y, 0.001f);
		}
		
		if (stepSize.x == 0)
		{
			Debug.LogWarning("StepSize must not be 0, ( x < 0 > x)");
			stepSize = new Vector3(1f, stepSize.y, stepSize.z);
		}
		
		if (stepSize.y == 0)
		{
			Debug.LogWarning("StepSize must not be 0, ( y < 0 > y)");
			stepSize = new Vector3(stepSize.x, 1f, stepSize.z);
		}
		
		if (stepSize.z == 0)
		{
			Debug.LogWarning("StepSize must not be 0, ( z < 0 > z)");
			stepSize = new Vector3(stepSize.x, stepSize.y, 1f);
		}
	}
}