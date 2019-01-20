using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviourWithGameManager
{
	public Transform rotationHelper;
	[HideInInspector] public bool blockXRotation = false;

	Vector3 focusPoint;
	bool shouldTrack = false;

	void Start()
	{
		SetGameManager();
	}


	void Update()
	{
		Vector3 rotationData = GM.GetRelativeRotationInput();
		if (blockXRotation)
			rotationData.x = 0.0f;

		rotationHelper.Rotate(rotationData);
		if (shouldTrack)
		{
			Quaternion targetRotation = Quaternion.LookRotation(focusPoint, transform.up);
			rotationHelper.rotation = Quaternion.Lerp(rotationHelper.rotation, targetRotation, Time.deltaTime * GM.personPullForce);
		}

		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			rotationHelper.rotation,
			GM.viewRotationSpeed * Time.deltaTime);
	}


	public void StartTracking(Vector3 globalPoint)
	{
		shouldTrack = true;
		focusPoint = globalPoint;
	}


	public void StopTracking()
	{
		shouldTrack = false;
	}
}
