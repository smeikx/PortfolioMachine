using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSearcher : MonoBehaviourWithGameManager
{
	Transform foundObject = null;
	Transform cam;
	const int layerMask = 1 << 9;

	void Start()
	{
		SetGameManager();
		cam = Camera.main.transform;
	}

	void Update()
	{
		Transform newFoundObject = null;

		RaycastHit hit;
		if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
			newFoundObject = hit.transform;
		else
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);

		if (newFoundObject != foundObject)
		{
			if (newFoundObject != null)
				GM.ReportPersonFound(newFoundObject);
			else
				GM.ReportPersonLost(foundObject);
			foundObject = newFoundObject;
		}
	}
}
