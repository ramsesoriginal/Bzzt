using UnityEngine;
using System.Collections;

public static class HayateTurbulence {
	
	#pragma warning disable 0414
	private static float turbX = 0f;
	private static float turbY = 0f;
	private static float turbZ = 0f;
	#pragma warning restore 0414
	
	
	private static Vector3 calculatedTurbulence = new Vector3();
	private static int positionRemapedX = new int();
	private static int positionRemapedY = new int();
	private static int positionRemapedZ = new int();
	private static int indexTurb = new int();
	
	private static float turbulenceValue = new float();
	
	public static Vector3 GetTurbulence(Vector3 _position, Hayate hayate)
	{
		float turbX = 0f;
		float turbY = 0f;
		float turbZ = 0f;
		
		if (hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.sine)
		{
			turbX = (Mathf.Sin(_position.z / hayate.Frequency.x - hayate.Offset.x) * hayate.Amplitude.x + hayate.GlobalForce.x) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.cosine)
		{
			turbX = (Mathf.Cos(_position.z / hayate.Frequency.x - hayate.Offset.x) * hayate.Amplitude.x + hayate.GlobalForce.x) * hayate.deltaTime;
			
		}
		else if(hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.animationCurve)
		{
			turbX = (hayate.turbulenceCurveX.Evaluate( _position.y / hayate.Frequency.x - hayate.Offset.x ) * hayate.Amplitude.x + hayate.GlobalForce.x) * hayate.deltaTime;
		}
		else if (hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.perlin)
		{
			turbX = ((Mathf.PerlinNoise(_position.z / hayate.Frequency.z - hayate.Offset.z, _position.y / hayate.Frequency.y - hayate.Offset.y) * 2 - 1) * hayate.Amplitude.x + hayate.GlobalForce.x) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.precalculatedTexture)
		{
			if (hayate.turbulence != null && hayate.turbulence.Length != 0)
				turbX = (CalculateTextureNoiseOnXAxis(_position.z / hayate.Frequency.z * 100f - hayate.Offset.z, _position.y / hayate.Frequency.y * 100f - hayate.Offset.y, hayate) + hayate.GlobalForce.x) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodX == HayateEnums.CalculationMethod.Audio)
		{
			if (hayate.hayateSamples != null && hayate.hayateSamples.Length != 0)
				turbX = (CalculateAudioNoiseX(_position.z / hayate.Frequency.z * 100f - hayate.Offset.z, _position.y / hayate.Frequency.y * 100f - hayate.Offset.y, hayate) + hayate.GlobalForce.x) * hayate.deltaTime;
		}
		
		
		if (hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.sine)
		{
			turbY = (Mathf.Sin(_position.z / hayate.Frequency.y - hayate.Offset.y) * hayate.Amplitude.y + hayate.GlobalForce.y) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.cosine)
		{
			turbY = (Mathf.Cos(_position.z / hayate.Frequency.y - hayate.Offset.y) * hayate.Amplitude.y + hayate.GlobalForce.y) * hayate.deltaTime;
			
		}
		else if(hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.animationCurve)
		{
			turbY = (hayate.turbulenceCurveY.Evaluate( _position.x / hayate.Frequency.y - hayate.Offset.y ) * hayate.Amplitude.y + hayate.GlobalForce.y) * hayate.deltaTime;
		}
		else if (hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.perlin)
		{
			turbY = ((Mathf.PerlinNoise(_position.x / hayate.Frequency.x - hayate.Offset.x, _position.z / hayate.Frequency.z - hayate.Offset.z) * 2 - 1) * hayate.Amplitude.y + hayate.GlobalForce.y) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.precalculatedTexture)
		{
			if (hayate.turbulence != null && hayate.turbulence.Length != 0)
				turbY = (CalculateTextureNoiseOnYAxis(_position.x / hayate.Frequency.x * 100f - hayate.Offset.x, _position.z / hayate.Frequency.z * 100f - hayate.Offset.z, hayate) + hayate.GlobalForce.y) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodY == HayateEnums.CalculationMethod.Audio)
		{
			if (hayate.hayateSamples != null && hayate.hayateSamples.Length != 0)
				turbY = (CalculateAudioNoiseY(_position.x / hayate.Frequency.x * 100f - hayate.Offset.x, _position.z / hayate.Frequency.z * 100f - hayate.Offset.z, hayate) + hayate.GlobalForce.y) * hayate.deltaTime;
		}
		
		
		if (hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.sine)
		{
			turbZ = (Mathf.Sin(_position.x / hayate.Frequency.z - hayate.Offset.z) * hayate.Amplitude.z + hayate.GlobalForce.z) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.cosine)
		{
			turbZ = (Mathf.Cos(_position.x / hayate.Frequency.z - hayate.Offset.z) * hayate.Amplitude.z + hayate.GlobalForce.z) * hayate.deltaTime;
			
		}
		else if(hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.animationCurve)
		{
			turbZ = (hayate.turbulenceCurveZ.Evaluate( _position.y / hayate.Frequency.z - hayate.Offset.z ) * hayate.Amplitude.z + hayate.GlobalForce.z) * hayate.deltaTime;
		}
		else if (hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.perlin)
		{
			turbZ = ((Mathf.PerlinNoise(_position.y / hayate.Frequency.y - hayate.Offset.y, _position.x / hayate.Frequency.x - hayate.Offset.x) * 2 - 1) * hayate.Amplitude.z + hayate.GlobalForce.z) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.precalculatedTexture)
		{
			if (hayate.turbulence != null && hayate.turbulence.Length != 0)
				turbZ = (CalculateTextureNoiseOnZAxis(_position.y / hayate.Frequency.y * 100f - hayate.Offset.y, _position.x / hayate.Frequency.x * 100f - hayate.Offset.x, hayate) + hayate.GlobalForce.z) * hayate.deltaTime;
			
		}
		else if (hayate.UseCalculationMethodZ == HayateEnums.CalculationMethod.Audio)
		{
			if (hayate.hayateSamples != null && hayate.hayateSamples.Length != 0)
				turbZ = (CalculateAudioNoiseZ(_position.y / hayate.Frequency.y * 100f - hayate.Offset.y, _position.x / hayate.Frequency.x * 100f - hayate.Offset.x, hayate) + hayate.GlobalForce.z) * hayate.deltaTime;
		}
		
		calculatedTurbulence = new Vector3(turbX, turbY, turbZ);
		return calculatedTurbulence;
	}
	
	private static float CalculateAudioNoiseX(float _positionZ, float _positionY, Hayate hayate)
	{
		
		positionRemapedZ = (int)(_positionZ % hayate.amountOfSamples);
		positionRemapedY = (int)(_positionY % hayate.amountOfSamples);
		
		indexTurb = ((positionRemapedZ) * hayate.amountOfSamples + (positionRemapedY));
		indexTurb = Mathf.Abs(indexTurb);
		
		turbulenceValue = hayate.hayateSamples[indexTurb] * hayate.Amplitude.x * hayate.deltaTime;
		return (turbulenceValue);
	}
	
	private static float CalculateAudioNoiseY(float _positionX, float _positionZ, Hayate hayate)
	{
		
		positionRemapedX = (int)(_positionX % hayate.amountOfSamples);
		positionRemapedZ = (int)(_positionZ % hayate.amountOfSamples);
		
		indexTurb = ((positionRemapedX) * hayate.amountOfSamples + (positionRemapedZ));
		indexTurb = Mathf.Abs(indexTurb);
		
		turbulenceValue = hayate.hayateSamples[indexTurb] * hayate.Amplitude.y * hayate.deltaTime;
		return (turbulenceValue);
	}
	
	private static float CalculateAudioNoiseZ(float _positionY, float _positionX, Hayate hayate)
	{
		
		positionRemapedY = (int)(_positionY % hayate.amountOfSamples);
		positionRemapedX = (int)(_positionX % hayate.amountOfSamples);
		
		indexTurb = ((positionRemapedX) * hayate.amountOfSamples + (positionRemapedY));
		indexTurb = Mathf.Abs(indexTurb);
		
		turbulenceValue = hayate.hayateSamples[indexTurb] * hayate.Amplitude.z * hayate.deltaTime;
		return (turbulenceValue);
	}
	
	private static float CalculateTextureNoiseOnXAxis(float _positionZ, float _positionY, Hayate hayate)
	{
		
		positionRemapedX = (int)(_positionZ % hayate.Turbulence.width);
		positionRemapedY = (int)(_positionY % hayate.Turbulence.height);
		
		indexTurb = ((positionRemapedY) * hayate.Turbulence.width + (positionRemapedX));
		indexTurb = Mathf.Abs(indexTurb);
		
		if (hayate.useAlphaMask && hayate.turbulence[indexTurb].a == 0)
			hayate.removeCurrentParticle = true;
		
		turbulenceValue = ((hayate.turbulence[indexTurb].r / 256f) * 2 - 1) * hayate.Amplitude.x * hayate.deltaTime;
		return (turbulenceValue);
	}
	
	private static float CalculateTextureNoiseOnYAxis(float _positionX, float _positionZ, Hayate hayate)
	{
		
		positionRemapedX = (int)(_positionX % hayate.Turbulence.width);
		positionRemapedZ = (int)(_positionZ % hayate.Turbulence.height);
		
		indexTurb = ((positionRemapedX) * hayate.Turbulence.width + (positionRemapedZ));
		indexTurb = Mathf.Abs(indexTurb);
		
		if (hayate.useAlphaMask && hayate.turbulence[indexTurb].a == 0)
			hayate.removeCurrentParticle = true;
		
		turbulenceValue = ((hayate.turbulence[indexTurb].g / 256f) * 2 - 1) * hayate.Amplitude.y * hayate.deltaTime;
		return (turbulenceValue);
	}
	
	private static float CalculateTextureNoiseOnZAxis(float _positionY, float _positionX, Hayate hayate)
	{
		
		positionRemapedY = (int)(_positionY % hayate.Turbulence.width);
		positionRemapedX = (int)(_positionX % hayate.Turbulence.height);
		
		indexTurb = ((positionRemapedX) * hayate.Turbulence.width + (positionRemapedY));
		indexTurb = Mathf.Abs(indexTurb);
		
		if (hayate.useAlphaMask && hayate.turbulence[indexTurb].a == 0)
			hayate.removeCurrentParticle = true;
		
		turbulenceValue = ((hayate.turbulence[indexTurb].b / 256f) * 2 - 1) * hayate.Amplitude.z * hayate.deltaTime;
		return (turbulenceValue);
	}

}
