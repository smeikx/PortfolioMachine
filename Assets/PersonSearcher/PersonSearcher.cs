using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSearcher : MonoBehaviourWithGameManager
{
	Transform foundPerson = null;
	Transform cam;
	int layerMask = 1 << 0;

	void Start()
	{
		SetGameManager();
		cam = Camera.main.transform;
	}

	void Update()
	{
		Transform newFoundPerson = null;

		RaycastHit hit;
		if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
		{
			Debug.DrawRay(cam.position, cam.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
			newFoundPerson = hit.transform;
		}
		else
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);

		if (newFoundPerson != foundPerson)
		{
			if (newFoundPerson != null)
				GM.ReportPersonFound(newFoundPerson);
			else
				GM.ReportPersonLost(foundPerson);
			foundPerson = newFoundPerson;
		}
	}
}
