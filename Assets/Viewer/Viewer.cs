using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviourWithGameManager
{
	public Transform rotationHelper;
	[HideInInspector] public bool restrictRotation = false;

	Vector3 focusPoint;
	bool shouldTrack = false;

	void Start()
	{
		SetGameManager();
	}


	void Update()
	{
		Vector3 rotationData = GM.GetRelativeRotationInput();

		if (restrictRotation)
		{
			var scrollDir = GM.GetPossibleScrollDirection();
			switch (scrollDir)
			{
				case GameManager.ScrollDirection.BOTH:
					rotationData.x = 0f;
					break;
				case GameManager.ScrollDirection.UP:
					rotationData.x = Mathf.Max(rotationData.x, 0f);
					break;
				case GameManager.ScrollDirection.DOWN:
					rotationData.x = Mathf.Min(rotationData.x, 0f);
					break;
			}
		}

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
