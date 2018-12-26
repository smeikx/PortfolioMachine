using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

	[SerializeField] float X_SPEED = 1.0f;
	[SerializeField] float Y_SPEED = 1.0f;
	[SerializeField] float Z_SPEED = 1.0f;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		float x = Input.GetAxis("Mouse Y") * X_SPEED;
		float z = Input.GetAxis("Mouse X") * Z_SPEED;
		float y = GetYRotation() * Y_SPEED;

		transform.Rotate(x, y, z);
	}

	private float GetYRotation()
	{
		//TODO: Daten per OSC (o. Ä.) sammeln
		return 0f;
	}
}
