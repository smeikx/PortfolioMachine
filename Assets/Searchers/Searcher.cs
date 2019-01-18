using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviourWithGameManager
{
	Transform foundObject = null;
	Transform cam;
	int layerMask = 1 << 0;

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
				ReportObjectFound(newFoundObject);
				//GM.ReportPersonFound(newFoundObject);
			else
				ReportObjectLost(foundObject);
				//GM.ReportPersonLost(foundObject);
			foundObject = newFoundObject;
		}
	}


	void ReportObjectFound(Transform foundObject) {}


	void ReportObjectLost(Transform lostObject) {}
}
