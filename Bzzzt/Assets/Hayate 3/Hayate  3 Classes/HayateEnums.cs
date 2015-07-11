using UnityEngine;
using System.Collections;

public static class HayateEnums {

	public enum CalculationMethod
	{
		none, sine, cosine, animationCurve, perlin, precalculatedTexture, Audio
	};
	
	public enum TurbulenceType
	{
		relative, absolute
	};
	
	public enum DivisionType
	{
		center, edge
	};
	/* Prototype
	public enum AssignTo
	{
		velocity, position, acceleration
	};
	*/
	
	public enum AssignTo
	{
		velocity, position
	};
	
	public enum MeshFollow
	{
		byDistance, byTime, physical
	};
	
	public enum BuildOrder
	{
		TopBottom, TopLeftBottomRight, LeftRight, BottomLeftTopRight, BottomTop, BottomRightTopLeft, RightLeft, TopRightBottomLeft
	};
}
