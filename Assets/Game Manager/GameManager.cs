//#define OSC_AVAILABLE

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

	[Header("Drehungs-Parameter")]
	[Tooltip("Wie groß ist die Verzögerung, mit der die Drehung der Kugel den Blickwinkel verändert?")]
	public float viewRotationSpeed = 2f;
	[Tooltip("Wie stark beeinflusst die Drehung der Kugel den Blickwinkel auf der X-Achse?")]
	[SerializeField] float rotationFactorX = 1f;
	[Tooltip("Wie stark beeinflusst die Drehung der Kugel den Blickwinkel auf der Y-Achse?")]
	[SerializeField] float rotationFactorY = 1f;
	[Tooltip("Wie stark beeinflusst die Drehung der Kugel den Blickwinkel auf der Z-Achse?")]
	[SerializeField] float rotationFactorZ = 1f;

	[Header("Medien-Parameter")]
	[Tooltip("Wie lange dauert es, bis das Schlusslicht eines Showcase sich schließt?")]
	public float exitMediumTime = 5f;

	OSCMice oscMice;
	Viewer viewer;
	[HideInInspector] public float personPullForce = 1.0f;

	enum State {
		Searching, // in Übersicht, schaut sich um
		Focusing, // hat Person fokussiert
		Selected // Person ist herangezogen, Arbeiten werden angezeigt
	};
	State state = State.Searching;


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

		person.GetComponent<Person>().shouldZoomIn = true;

		viewer.StartTracking(person.position);
	}

	Transform lastPerson;


	public void ReportPersonLost(Transform person)
	{
		lastPerson = null;

		Person p = person.GetComponent<Person>();
		p.shouldZoomIn = false;
		p.showcase.GetComponent<Showcase>().DeactivateAfterReset();

		viewer.StopTracking();
		viewer.blockXRotation = false;

		personPullForce = weakPersonPullForce;
	}


	public void ReportPersonSelected(Transform person)
	{
		lastPerson = person;

		viewer.blockXRotation = true;

		personPullForce = strongPersonPullForce;

		person.GetComponent<Person>().showcase.SetActive(true);
	}


	public void ReportMediumFound(Transform medium)
	{
		Debug.Log("Medium Found: "+medium.name);

		if (medium.tag == "ExitMedium" && lastPerson != null)
			lastPerson.GetComponent<Person>().exitMediumActivated = true;

		// Startet Video oder Sound falls möglich
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
		Debug.Log("Medium Lost: "+medium.name);

		if (medium.tag == "ExitMedium" && lastPerson != null)
			lastPerson.GetComponent<Person>().exitMediumActivated = false;


		// Stoppt Video oder Sound falls möglich
		VideoPlayer vp = medium.GetComponent<VideoPlayer>();
		if (vp) vp.Stop();
		else
		{
			AudioSource ap = medium.GetComponent<AudioSource>();
			if (ap) ap.Stop();
		}
	}
}
