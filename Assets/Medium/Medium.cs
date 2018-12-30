using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medium : MonoBehaviour
{
	Plane[] camPlanes;
	Collider collider;
	Renderer renderer;

	void Start()
	{
		camPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		collider = GetComponent<BoxCollider>();
		renderer = GetComponent<Renderer>();
	}

	void Update()
	{
		renderer.enabled = GeometryUtility.TestPlanesAABB(camPlanes, collider.bounds);
	}
}
