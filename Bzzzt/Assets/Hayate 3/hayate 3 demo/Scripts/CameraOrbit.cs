using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = .5f;
    public float distanceMax = 15f;
 	public float lerpSpeed = 2f;
	
    private float x = 0.0f;
    private float y = 0.0f;
	
	void Start () {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
	}
 
    void LateUpdate () {
	    if (target) {
	        if(Input.GetMouseButton(0))
			{
				x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
	       		y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			}
	        y = ClampAngle(y, yMinLimit, yMaxLimit);
	 
	        Quaternion rotation = Quaternion.Euler(y, x, 0);
	 
	        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
	 
	        RaycastHit hit;
	        if (Physics.Linecast (target.position, transform.position, out hit)) {
	                distance -=  hit.distance;
	        }
	        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
	        Vector3 position = rotation * negDistance + target.position;
	 		
	        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lerpSpeed * 2f * Time.deltaTime);
	        transform.position = Vector3.Lerp( transform.position, position, lerpSpeed * Time.deltaTime);
	    }
	}
	
	public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
