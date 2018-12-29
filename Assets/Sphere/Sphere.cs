// Ursprünglich drehten sich die Arbeiten, nicht die Kamera;
// deshalb wird dieses Script nicht mehr gebraucht.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviourWithGameManager
{
/*
	Vector3 desiredSphereRotationEuler;
	public float rotationSpeed = 1.0f;

	public Transform rotationHelper;

	void Start()
	{
		SetGameManager();
		desiredSphereRotationEuler = transform.rotation.eulerAngles;
	}

	void Update()
	{
		Vector3 rotationData = GM.GetRelativeRotationInput();

		rotationHelper.Rotate(rotationData, Space.World); // TODO: um Kamera-Achsen drehen

		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			rotationHelper.rotation,
			rotationSpeed * Time.deltaTime);

		//Debug.Log(transform.rotation.eulerAngles);
	}
*/
}
