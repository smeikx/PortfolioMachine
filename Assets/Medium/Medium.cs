using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Medium : MonoBehaviour
{
	Plane[] camPlanes;
	new Collider collider;
	new Renderer renderer;


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
