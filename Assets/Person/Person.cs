using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviourWithGameManager
{
	public bool shouldZoomIn = false;
	Vector3 localOrigin;
	Quaternion originalRotation;

	Quaternion finalRotation;
	SphereCollider collider;

	void Start()
	{
		SetGameManager();
		localOrigin = transform.localPosition;
		originalRotation = transform.rotation;
		collider = GetComponent<SphereCollider>();
	}

	void Update()
	{
		if (shouldZoomIn)
		{
			ZoomTo(transform.parent.InverseTransformPoint(GM.personDestination));
			// Make rotation level relative to camera.
			Quaternion finalRotation = Quaternion.LookRotation(
				transform.position - Camera.main.transform.position,
				Camera.main.transform.up);
			RotateTo(finalRotation);
		}
		else
		{
			ZoomTo(localOrigin);
			// Adjust rotation to counter camera’s z-rotation.
			Vector3 rotationAxis = (transform.position - Camera.main.transform.position).normalized;
			finalRotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.z, rotationAxis) * originalRotation;
			RotateTo(finalRotation);
		}
	}


	void ZoomTo(Vector3 destination)
	{
		// interpoliere vor die Kamera
		transform.localPosition = Vector3.Lerp(transform.localPosition, destination, GM.personZoomSpeed * Time.deltaTime);

		// halte Collider an Ursprungsposition
		collider.center = transform.InverseTransformPoint(transform.parent.TransformPoint(localOrigin));
	}


	void RotateTo(Quaternion targetRotation)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GM.personRotationSpeed * Time.deltaTime);
	}


	void AdjustRotationToCamera()
	{
		Vector3 rotationAxis = (transform.position - Camera.main.transform.position).normalized;
		finalRotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.z, rotationAxis) * originalRotation;
	}
}
