using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviourWithGameManager
{
	const float MEASURE_THRESHOLD = 0.2f;
	State state = State.GO_BACK;

	enum State {
		GO_BACK, ZOOM, SCROLL
	}

	Vector3 localOrigin;
	Quaternion originalRotation;
	float originalLocalZRotation;

	Vector3 targetPosition;
	Quaternion targetRotation;

	SphereCollider collider;
	Transform mainCam;
	
	Vector3 velocity = Vector3.zero; // for smooth damping
	Vector3 zoomedPosition;

	[HideInInspector]
	public Vector3 hindmostMediumPosition; // local
	public GameObject showcase;


	void Start()
	{
		SetGameManager();
		localOrigin = transform.localPosition;
		originalRotation = transform.rotation;
		originalLocalZRotation = transform.localEulerAngles.z;
		collider = GetComponent<SphereCollider>();
		mainCam = Camera.main.transform;
	}

	void Update()
	{
		Vector3 personToCameraAxis = transform.position - mainCam.position;

		switch (state)
		{
			case State.GO_BACK:
			{
				targetPosition = localOrigin;
				// Passe Z-Drehung so an, dass sie relativ zur Kamera gleich bleibt.
				targetRotation = Quaternion.LookRotation(
					originalRotation * Vector3.forward,
					Quaternion.AngleAxis(originalLocalZRotation, personToCameraAxis) * mainCam.up);
				break;
			}

			case State.SCROLL:
			{
				// positiv == nach unten scrollen
				// negativ == nach oben scrollen
				float scrollDelta = GM.GetRelativeRotationInput().x;

				GameManager.ScrollDirection scrollDir = GM.GetPossibleScrollDirection();
				if (scrollDir == GameManager.ScrollDirection.DOWN)
					scrollDelta = Mathf.Max(scrollDelta, 0f);
				else if (scrollDir == GameManager.ScrollDirection.UP)
					scrollDelta = Mathf.Min(scrollDelta, 0f);

				targetPosition += transform.parent.InverseTransformDirection(mainCam.up) * scrollDelta;
				break;
			}

			case State.ZOOM:
			{
				targetPosition = transform.parent.InverseTransformPoint(
					personToCameraAxis.normalized * GM.personZoomDistance);

				// Drehe Person so, dass sie parallel zur Kamera steht.
				targetRotation = Quaternion.LookRotation(
					personToCameraAxis,
					mainCam.up);

				if (Vector3.Distance(targetPosition, transform.position) <= MEASURE_THRESHOLD)
					GM.ReportPersonSelected(this);

				break;
			}
		}


		// TODO: möglicherweise MoveTo() und RotateTo() überspringingen, wenn nicht notwendig.
		MoveTo(targetPosition);
		RotateTo(targetRotation);
	}


	void MoveTo(Vector3 localDestination)
	{
		// interpoliere vor die Kamera
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, localDestination, ref velocity, GM.personMoveDuration);
		// Für Entscheidung zwischen Lerp und SmoothDamp siehe hier: http://i.imgur.com/FeKRE1c.gif

		// halte Collider an Ursprungsposition
		collider.center = transform.InverseTransformPoint(transform.parent.TransformPoint(localOrigin));
	}


	void RotateTo(Quaternion targetRotation)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GM.personRotationSpeed * Time.deltaTime);
	}


	public void ResetPosition()
	{
		targetPosition = localOrigin;
		transform.localPosition = localOrigin;
	}


	public void ShouldScroll()
	{
		state = State.SCROLL;
		zoomedPosition = GetGlobalTargetPosition();
	}


	public void ShouldZoom()
	{
		state = State.ZOOM;
	}


	public void ShouldGoBack()
	{
		state = State.GO_BACK;
	}


	public bool IsAtTop()
	{
		return (mainCam.InverseTransformPoint(zoomedPosition).y
			- mainCam.InverseTransformPoint(GetGlobalTargetPosition()).y) >= 0f;
	}


	public bool IsAtBottom()
	{
		return (mainCam.InverseTransformPoint(GetGlobalTargetPosition()).y
			+ mainCam.InverseTransformPoint(zoomedPosition).y + hindmostMediumPosition.y) >= 0f;
	}


	Vector3 GetGlobalTargetPosition()
	{
		return transform.parent.InverseTransformPoint(targetPosition);
	}
}
