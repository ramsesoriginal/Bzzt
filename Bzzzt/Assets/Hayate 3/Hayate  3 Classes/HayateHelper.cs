using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HayateHelper {

	public static Mesh SubDivide( GameObject meshTarget, HayateEnums.DivisionType divisionType, float smallestTriangle)
	{
		List<Vector3> Vertices = new List<Vector3>();
		List<int> Triangles = new List<int>();
		List<Vector2> Uvs = new List<Vector2>();
		List<Vector3> Normals = new List<Vector3>();
		
		if (meshTarget)
		{
			for (int i = 0; i < meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices.Length; i++)
			{
				Vertices.Add(meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices[i]);
			}
			
			for (int i = 0; i < meshTarget.GetComponent<MeshFilter>().sharedMesh.triangles.Length; i++)
			{
				Triangles.Add(meshTarget.GetComponent<MeshFilter>().sharedMesh.triangles[i]);
			}
			
			for (int i = 0; i < meshTarget.GetComponent<MeshFilter>().sharedMesh.uv.Length; i++)
			{
				Uvs.Add(meshTarget.GetComponent<MeshFilter>().sharedMesh.uv[i]);
			}
			
			for (int i = 0; i < meshTarget.GetComponent<MeshFilter>().sharedMesh.normals.Length; i++)
			{
				Normals.Add(meshTarget.GetComponent<MeshFilter>().sharedMesh.normals[i]);
			}
			
			List<Vector3> newVertices = new List<Vector3>(Vertices);
			List<int> newTriangles = new List<int>(Triangles);
			List<Vector2> newUvs = new List<Vector2>(Uvs);
			List<Vector3> newNormals = new List<Vector3>(Normals);
			
			if (divisionType == HayateEnums.DivisionType.center)
			{
				for (int i = 0; i < newTriangles.Count; i += 3)
				{
					if (SignedVolumeOfTriangle(newVertices[newTriangles[i]], newVertices[newTriangles[i + 1]], newVertices[newTriangles[i + 2]]) > smallestTriangle)
					{
						if (newVertices.Count > 60000)
						{
							Debug.Log("Stoped! Too many vertices");
							break;
						}
						
						int p0 = newTriangles[i];
						int p1 = newTriangles[i + 1];
						int p2 = newTriangles[i + 2];
						
						int pI0 = i;
						int pI1 = i + 1;
						int pI2 = i + 2;
						
						Vector3 v0 = newVertices[p0];
						Vector3 v1 = newVertices[p1];
						Vector3 v2 = newVertices[p2];
						
						Vector3 n0 = newNormals[p0];
						Vector3 n1 = newNormals[p1];
						Vector3 n2 = newNormals[p2];
						
						Vector2 uv0 = newUvs[p0];
						Vector2 uv1 = newUvs[p1];
						Vector2 uv2 = newUvs[p2];
						
						Vector3 nV = (v0 + v1 + v2) / 3;
						Vector3 nN = ((n0 + n1 + n2) / 3).normalized;
						Vector2 nU = (uv0 + uv1 + uv2) / 3;
						
						int l = newVertices.Count;
						
						newVertices.Add(nV);
						newNormals.Add(nN);
						newUvs.Add(nU);
						
						newTriangles[pI0] = p0;
						newTriangles[pI1] = p1;
						newTriangles[pI2] = l;
						
						newTriangles.Add(l);
						newTriangles.Add(p1);
						newTriangles.Add(p2);
						
						newTriangles.Add(p0);
						newTriangles.Add(l);
						newTriangles.Add(p2);
						
						if (SignedVolumeOfTriangle(newVertices[p0], newVertices[p1], newVertices[l]) > smallestTriangle)
						{
							i -= 3;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < newTriangles.Count; i += 3)
				{
					if (SignedVolumeOfTriangle(newVertices[newTriangles[i]], newVertices[newTriangles[i + 1]], newVertices[newTriangles[i + 2]]) > smallestTriangle)
					{
						if (newVertices.Count > 40000)
						{
							Debug.Log("Stoped! Too many vertices");
							break;
						}
						
						int p0 = newTriangles[i];
						int p1 = newTriangles[i + 1];
						int p2 = newTriangles[i + 2];
						
						int pI0 = i;
						int pI1 = i + 1;
						int pI2 = i + 2;
						
						Vector3 v0 = newVertices[p0];
						Vector3 v1 = newVertices[p1];
						Vector3 v2 = newVertices[p2];
						
						Vector3 n0 = newNormals[p0];
						Vector3 n1 = newNormals[p1];
						Vector3 n2 = newNormals[p2];
						
						Vector2 uv0 = newUvs[p0];
						Vector2 uv1 = newUvs[p1];
						Vector2 uv2 = newUvs[p2];
						
						Vector3 v0_m = (v0 + v1) / 2;
						Vector3 v1_m = (v1 + v2) / 2;
						Vector3 v2_m = (v2 + v0) / 2;
						
						Vector3 n0_m = ((n0 + n1) / 2).normalized;
						Vector3 n1_m = ((n1 + n2) / 2).normalized;
						Vector3 n2_m = ((n2 + n0) / 2).normalized;
						
						Vector2 uv0_m = (uv0 + uv1) / 2;
						Vector2 uv1_m = (uv1 + uv2) / 2;
						Vector2 uv2_m = (uv2 + uv0) / 2;
						
						int l0 = newVertices.Count;
						int l1 = newVertices.Count + 1;
						int l2 = newVertices.Count + 2;
						
						newVertices.Add(v0_m);
						newVertices.Add(v1_m);
						newVertices.Add(v2_m);
						
						newNormals.Add(n0_m);
						newNormals.Add(n1_m);
						newNormals.Add(n2_m);
						
						newUvs.Add(uv0_m);
						newUvs.Add(uv1_m);
						newUvs.Add(uv2_m);
						
						newTriangles[pI0] = p0;
						newTriangles[pI1] = l0;
						newTriangles[pI2] = l2;
						
						newTriangles.Add(l0);
						newTriangles.Add(l1);
						newTriangles.Add(l2);
						
						newTriangles.Add(l0);
						newTriangles.Add(p1);
						newTriangles.Add(l1);
						
						newTriangles.Add(l2);
						newTriangles.Add(l1);
						newTriangles.Add(p2);
						
						if (SignedVolumeOfTriangle(newVertices[p0], newVertices[l0], newVertices[l2]) > smallestTriangle)
						{
							i -= 3;
						}
					}
				}
			}
			
			Vector3[] ver = new Vector3[newVertices.Count];
			Vector3[] norms = new Vector3[newNormals.Count];
			Vector2[] uvs = new Vector2[newVertices.Count];
			int[] tri = new int[newTriangles.Count];
			
			for (int w = 0; w < newUvs.Count; w++)
			{
				uvs[w] = newUvs[w];
			}
			
			for (int w = 0; w < newNormals.Count; w++)
			{
				norms[w] = newNormals[w];
			}
			
			for (int x = 0; x < newVertices.Count; x++)
			{
				ver[x] = newVertices[x];
			}
			
			for (int y = 0; y < newTriangles.Count; y++)
			{
				tri[y] = newTriangles[y];
			}
			
			/*-------------ALLOCTION-------------*/
			Mesh subdivided = new Mesh();
			subdivided.vertices = ver;
			subdivided.triangles = tri;
			subdivided.uv = uvs;
			subdivided.normals = norms;
			subdivided.RecalculateBounds();
			
			Debug.Log((newVertices.Count - Vertices.Count) + " New Vertices created.");
			Debug.Log((newTriangles.Count - Triangles.Count) + " New Triangles created.");
			Debug.Log("Mesh target ready to work with " + newVertices.Count + " particles.");
			
			return subdivided;
		}
		else
		{
			Debug.LogWarning("No mesh target assigned!");
		}
		
		return null;
	}
	
	public static void CalculateAudioTurbulence( Hayate hayate )
	{
		if (!hayate.audioClip)
		{
			return;
		}
		
		hayate.hayateSamples = new float[hayate.amountOfSamples * hayate.amountOfSamples];
		
		float[] audioTurbX = new float[hayate.amountOfSamples * 2];
		float[] audioTurbY = new float[hayate.amountOfSamples * 2];
		hayate.audioClip.GetData(audioTurbX, hayate.atSample);
		hayate.audioClip.GetData(audioTurbY, hayate.atSample + hayate.amountOfSamples * 2);
		
		for (int x = 0; x < hayate.amountOfSamples; x++)
		{
			for (int y = 0; y < hayate.amountOfSamples; y++)
			{
				hayate.hayateSamples[y * hayate.amountOfSamples + x] = audioTurbY[x * 2] + audioTurbX[y * 2] / 2f;
			}
		}
	}
	
	public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float distance_1 = Vector3.Distance( p1, p2 );
		float distance_2 = Vector3.Distance( p2, p3 );
		float distance_3 = ( distance_1 * distance_2 ) / 2f;
		
		return distance_3;
	}
	
	public static ParticleSystem.Particle[] CreateParticlesDynamically( Hayate hayate, Vector3 _Position, float _Width, float _Height, float _Depth, float _TargetParticleSize, float _UiScale, HayateEnums.BuildOrder _buildOrder)
	{
		int NumX = Mathf.CeilToInt (_Width / _TargetParticleSize );
		int NumY = Mathf.CeilToInt (_Height / _TargetParticleSize );
		int NumZ = Mathf.CeilToInt (_Depth / _TargetParticleSize );
		int MaxParticles = NumX * NumY * NumZ;
		hayate.GetComponent<ParticleSystem>().startSize = _TargetParticleSize * _UiScale;
		hayate.GetComponent<ParticleSystem>().emissionRate = 0f;
#if UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		hayate.particleSystem.maxParticles = MaxParticles;
#endif
		hayate.GetComponent<ParticleSystem>().Emit( MaxParticles );
		
		ParticleSystem.Particle[] Particles = new ParticleSystem.Particle[MaxParticles];
		hayate.GetComponent<ParticleSystem>().GetParticles(Particles);
		
		int index = 0;
		int zIndex = 1;

		switch (_buildOrder)
		{
			case HayateEnums.BuildOrder.TopBottom:
				for(int z = 0; z < NumZ; z++)
				{
					int yIndex = 1;
					for(int y = NumY; y > 0; y--)
					{
						int xIndex = 1;
						for(int x = 0; x < NumX; x++)
						{
							
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * ((NumX/2f) * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;

							index++;
							xIndex++;
						}	
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.TopRightBottomLeft:
				for(int z = 0; z < NumZ; z++)
				{
					int yIndex = 1;
					for(int y = NumY; y > 0; y--)
					{
						int xIndex = 1;
						for(int x = NumX; x > 0 ; x--)
						{
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
							
							index++;
							xIndex++;
						}	
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.RightLeft:
				for(int z = 0; z < NumZ; z++)
				{
					int xIndex = 1;
					for(int x = NumX; x > 0; x--)
					{
						int yIndex = 1;
						for(int y = 0; y < NumY; y++)
						{
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
								);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
							
							index++;
							xIndex++;
						}
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.BottomRightTopLeft:
				for(int z = 0; z < NumZ; z++)
				{
					int yIndex = 1;
					for(int y = 0; y < NumY; y++)
					{
						int xIndex = 1;
						for(int x = NumX; x > 0 ; x--)
						{
							
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
							
							index++;
							xIndex++;
						}	
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.BottomTop:
			for(int z = 0; z < NumZ; z++)
			{
				int yIndex = 1;
				for(int y = 0; y < NumY; y++)
				{
					int xIndex = 1;
					for(int x = 0; x < NumX; x++)
					{
						
						Particles[index].position = new Vector3(
							((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (x * _TargetParticleSize * _UiScale )), 
							((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (y * _TargetParticleSize * _UiScale )),
							((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
						
						Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * ((NumX/2f) * yIndex * zIndex);
						Particles[index].startLifetime = Particles[index].lifetime;
						
						index++;
						xIndex++;
					}	
					yIndex++;
				}
				zIndex++;
			}
			break;

			case HayateEnums.BuildOrder.BottomLeftTopRight:
				for(int z = 0; z < NumZ; z++)
				{
					int yIndex = 1;
					for(int y = 0; y < NumY; y++)
					{
						int xIndex = 1;
						for(int x = 0; x < NumX; x++)
						{
							
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale )));
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
							
							index++;
							xIndex++;
						}	
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.LeftRight:
				for(int z = 0; z < NumZ; z++)
				{
					
					int xIndex = 1;
					for(int x = 0; x < NumX; x++)
					{
						int yIndex = 1;
						for(int y = 0; y < NumY; y++)
						{
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (y * _TargetParticleSize * _UiScale )),
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
							
							index++;
							xIndex++;
						}
						yIndex++;
					}
					zIndex++;
				}
			break;

			case HayateEnums.BuildOrder.TopLeftBottomRight:
				for(int z = 0; z < NumZ; z++)
				{
					int yIndex = 1;
					for(int y = NumY; y > 0; y--)
					{
						int xIndex = 1;
						for(int x = 0; x < NumX; x++)
						{
							Particles[index].position = new Vector3(
								((_Position.x - ( _Width * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (x * _TargetParticleSize * _UiScale )), 
								((_Position.y - ( _Height * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) - (_TargetParticleSize * _UiScale) + (y * _TargetParticleSize * _UiScale )), 
								((_Position.z - ( _Depth * _UiScale / 2f) + _TargetParticleSize * _UiScale / 2f) + (z * _TargetParticleSize * _UiScale ))
							);
							
							Particles[index].lifetime += (Particles[index].startLifetime / MaxParticles) * (xIndex * yIndex * zIndex);
							Particles[index].startLifetime = Particles[index].lifetime;
						
							index++;
							xIndex++;
						}	
						yIndex++;
					}
					zIndex++;
				}
			break;
		}
		
		return Particles;
	}
}
