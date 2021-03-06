﻿//#define OSC_AVAILABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

#if OSC_AVAILABLE
[RequireComponent(typeof(OSCMice))]
#endif
public class GameManager : MonoBehaviour
{
	[Header("Objekt-Referencen")]
	[Tooltip("Die Hauptkamera, der Betrachter.")]
	[SerializeField] Transform mainCamera;
	[SerializeField] RectTransform cursor;

	[Header("Selektions-Parameter")]
	[Tooltip("Wie nah vor der Kamera schwebt die Person?")]
	public float personZoomDistance = 1.0f;
	[Tooltip("Wie lange dauert es, bis die Person ausgewählt wurde?")]
	public float personZoomDuration = 1.0f;
	[Tooltip("Wie schnell dreht sich die Person so, dass sie gerade und parallel zum Betrachter steht, und optimal gelesen werden kann?")]
	public float personRotationSpeed = 1.0f;
	[Tooltip("Wie stark wird der Blick in den Mittelpunkt der Person gezogen, bevor sie ausgewählt wurde?")]
	public float weakPersonPullForce = 1.0f;
	[Tooltip("Wie stark wird der Blick im Mittelpunkt der Person gehalten, sobald sie ausgewählt ist?")]
	public float strongPersonPullForce = 1.0f;

	[Header("Showcase-Parameter")]
	[Tooltip("Wie träge ist das Scrollen?")]
	[SerializeField] float scrollInertia = 1.0f;
	[Tooltip("Wie stark beeinflusst die Kugeldrehung das Scrollen durch die Arbeiten?")]
	public float scrollFactor = 1.0f;

	[Header("Drehungs-Parameter")]
	[Tooltip("Wie groß ist die Verzögerung, mit der die Drehung der Kugel den Blickwinkel verändert?")]
	public float viewRotationSpeed = 2f;
	[Tooltip("Wie stark beeinflusst die Drehung der Kugel den Blickwinkel auf der X-Achse?")]
	public Vector3 rotationFactor = Vector3.one;

	OSCMice oscMice;
	Viewer viewer;
	[HideInInspector] public float personPullForce = 1.0f;
	[HideInInspector] public float personMoveDuration = 1.0f;

	Person selectedPerson;


	void Start ()
	{
		if (mainCamera == null) mainCamera = Camera.main.transform;
		viewer = mainCamera.GetComponent<Viewer>();

		oscMice = GetComponent<OSCMice>();
		#if !OSC_AVAILABLE
		Destroy(oscMice);
		//oscMice = null;
		Debug.Log("Keine OSC-Mäuse verfügbar");
		#endif
	}


	void Update ()
	{
	}


	public Vector3 GetRelativeRotationInput ()
	{
		#if OSC_AVAILABLE
		Vector3 relativeRotation = new Vector3(
			oscMice.mouse_1.y,
			oscMice.mouse_2.y,
			oscMice.mouse_1.x
		);
		#else
		Vector3 relativeRotation = new Vector3(
			Input.GetAxis("Mouse Y"),
			- Input.GetAxis("Mouse X"),
			0f
		);
		#endif

		return relativeRotation;
	}


	public void ReportPersonFound(Transform person)
	{
		viewer.StartTracking(person.position);

		person.GetComponent<Person>().ShouldZoom();
	}


	public void ReportPersonLost(Transform person)
	{
		viewer.StopTracking();
		viewer.restrictRotation = false;

		personMoveDuration = personZoomDuration;
		personPullForce = weakPersonPullForce;

		Person p = person.GetComponent<Person>();
		p.ShouldGoBack();
		p.showcase.GetComponent<Showcase>().StartOutro();

		cursor.localScale = Vector3.one;
	}


	public void ReportPersonSelected(Person person)
	{
		selectedPerson = person;

		viewer.restrictRotation = true;

		personMoveDuration = scrollInertia;
		personPullForce = strongPersonPullForce;

		person.ShouldScroll();
		person.showcase.SetActive(true);

		cursor.localScale = Vector3.zero;
	}


	public enum ScrollDirection { UP, BOTH, DOWN }

	public ScrollDirection GetPossibleScrollDirection()
	{
		if (selectedPerson.IsAtTop())
			return ScrollDirection.DOWN;
		else if (selectedPerson.IsAtBottom())
			return ScrollDirection.UP;
		return ScrollDirection.BOTH;
	}


	public void ReportMediumFound(Transform medium)
	{
		// Startet Video oder Sound, falls möglich
		VideoPlayer vp = medium.GetComponent<VideoPlayer>();
		if (vp) vp.Play();
		else
		{
			AudioSource ap = medium.GetComponent<AudioSource>();
			if (ap) ap.Play();
		}
	}


	public void ReportMediumLost(Transform medium)
	{
		// Stoppt Video oder Sound, falls möglich
		VideoPlayer vp = medium.GetComponent<VideoPlayer>();
		if (vp) vp.Stop();
		else
		{
			AudioSource ap = medium.GetComponent<AudioSource>();
			if (ap) ap.Stop();
		}
	}
}
