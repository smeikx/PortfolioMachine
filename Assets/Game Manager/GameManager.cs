﻿//#define OSC_AVAILABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if OSC_AVAILABLE
[RequireComponent(typeof(OSCMice))]
#endif
public class GameManager : MonoBehaviour
{
	[Header("Objekt-Referencen")]
	[SerializeField] Transform mainCamera;
	[SerializeField] Sphere sphere;

	[Header("Selektions-Parameter")]
	[SerializeField] float zoomDistance = 1.0f;
	public float personZoomSpeed = 1.0f;
	public float personRotationSpeed = 1.0f;
	
	[Header("Drehungs-Parameter")]
	public float viewRotationSpeed = 2f;
	public float rotationFactorX = 1f;
	public float rotationFactorY = 1f;
	public float rotationFactorZ = 1f;

	OSCMice oscMice;

	enum State {
		Searching, // in Übersicht, schaut sich um
		Focusing, // hat Person fokussiert
		Selected // Person ist herangezogen, Arbeiten werden angezeigt
	};
	State state = State.Searching;

	// globale Position, zu der Personen zoomen sollen
	[HideInInspector] public Vector3 personDestination;


	void Start ()
	{
		if (mainCamera == null) mainCamera = Camera.main.transform;

		oscMice = GetComponent<OSCMice>();
		#if !OSC_AVAILABLE
		Destroy(oscMice);
		//oscMice = null;
		Debug.Log("Keine OSC-Mäuse verfügbar");
		#endif
	}
	

	void Update ()
	{
		personDestination = mainCamera.forward * zoomDistance;
	}


	public Vector3 GetRelativeRotationInput ()
	{
		#if OSC_AVAILABLE
		Vector3 relativeRotation = new Vector3(
			oscMice.mouse_1.y * rotationFactorX,
			oscMice.mouse_2.y * rotationFactorY,
			oscMice.mouse_1.x * rotationFactorZ
		);
		#else
		Vector3 relativeRotation = new Vector3(
			Input.GetAxis("Mouse Y") * rotationFactorX,
			Input.GetAxis("Mouse X") * -rotationFactorY,
			0f
		);
		#endif

		return relativeRotation;
	}


	public void ReportPersonFound(Transform person)
	{
		state = State.Focusing;
		Debug.Log("Person Found");
		person.GetComponent<Person>().shouldZoomIn = true;
	}

	public void ReportPersonLost(Transform person)
	{
		Debug.Log("Person Lost");
		person.GetComponent<Person>().shouldZoomIn = false;
	}
}
