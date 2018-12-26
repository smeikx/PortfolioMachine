using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviourWithGameManager
{
	const float speed = 1.0f;
	public bool shouldZoomIn = false;
	Vector3 localOrigin;
	Quaternion originalRotation;

	Vector3 finalDestination;
	Quaternion finalRotation;
	SphereCollider collider;

	void Start()
	{
		localOrigin = transform.localPosition;
		originalRotation = transform.rotation;
		finalDestination = Camera.main.transform.position + Vector3.forward * 7;
		collider = GetComponent<SphereCollider>();
	}

	void Update()
	{
		if (shouldZoomIn)
		{
			ZoomTo(transform.parent.InverseTransformPoint(finalDestination));
			Quaternion finalRotation = Quaternion.LookRotation(
				transform.position - Camera.main.transform.position,
				Vector3.up);
			RotateTo(finalRotation);
		}
		else
		{
			ZoomTo(localOrigin);
			RotateTo(transform.parent.rotation * originalRotation);
		}
		AdjustRotationToCamera();
	}


	void ZoomTo(Vector3 destination)
	{
		// interpoliere vor die Kamera
		transform.localPosition = Vector3.Lerp(transform.localPosition, destination, speed * Time.deltaTime);

		// halte Collider an Ursprungsposition
		collider.center = transform.InverseTransformPoint(transform.parent.TransformPoint(localOrigin));

		/*
			Debug.Log(collider.center);
			Debug.Log(transform.localPosition);
			Debug.Log(localOrigin);
			Debug.Log("-----------------");
		*/
	}


	void RotateTo(Quaternion targetRotation)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
	}


	void AdjustRotationToCamera()
	{
	}
}
