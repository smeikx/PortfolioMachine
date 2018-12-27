using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviourWithGameManager
{
	public Transform rotationHelper;

	void Start()
	{
		SetGameManager();
	}

	void Update()
	{
		Vector3 rotationData = GM.GetRelativeRotationInput();

		rotationHelper.Rotate(rotationData); // TODO: um Kamera-Achsen drehen

		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			rotationHelper.rotation,
			GM.viewRotationSpeed * Time.deltaTime);
	}
}
