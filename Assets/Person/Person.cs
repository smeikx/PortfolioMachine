using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviourWithGameManager
{
	public bool shouldZoomIn = false;
	const float SELECTION_THRESHOLD = 0.2f;

	Vector3 localOrigin;
	Quaternion originalRotation;

	Vector3 targetPosition;
	Quaternion targetRotation;

	SphereCollider collider;

	Vector3 velocity = Vector3.zero; // for smooth damping


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
			targetPosition = transform.parent.InverseTransformPoint(
					(transform.position - Camera.main.transform.position).normalized * GM.personZoomDistance);
			// Drehe Person so, dass sie parallel zur Kamera steht.
			targetRotation = Quaternion.LookRotation(
				transform.position - Camera.main.transform.position,
				Camera.main.transform.up);
		}
		else
		{
			targetPosition = localOrigin;
			// Passe Z-Drehung so an, dass sie relativ zur Kamera gleich bleibt.
			Vector3 rotationAxis = (transform.position - Camera.main.transform.position).normalized;
			targetRotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.z, rotationAxis) * originalRotation;
		}

		if (shouldZoomIn && Vector3.Distance(targetPosition, transform.position) <= SELECTION_THRESHOLD)
			GM.ReportPersonSelected(transform);

		// TODO: möglicherweise MoveTo() und RotateTo() überspringingen, wenn nicht notwendig.
		MoveTo(targetPosition);
		RotateTo(targetRotation);
	}


	void MoveTo(Vector3 localDestination)
	{
		// interpoliere vor die Kamera
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, localDestination, ref velocity, GM.personZoomDuration);
		// Für Entscheidung zwischen Lerp und SmoothDamp siehe hier: http://i.imgur.com/FeKRE1c.gif

		// halte Collider an Ursprungsposition
		collider.center = transform.InverseTransformPoint(transform.parent.TransformPoint(localOrigin));
	}


	void RotateTo(Quaternion targetRotation)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GM.personRotationSpeed * Time.deltaTime);
	}
}
