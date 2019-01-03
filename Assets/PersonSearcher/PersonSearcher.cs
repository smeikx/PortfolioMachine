using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSearcher : MonoBehaviourWithGameManager
{
	Transform foundPerson = null;
	Transform cam;
	int layerMask = 1 << 9;
	const int personLayer = 9;
	const int mediumLayer = 10;

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
			newFoundPerson = hit.transform;
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
