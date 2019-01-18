using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviourWithGameManager
{
	Transform[] works;
	Vector3 localDestination, originalNameLocalPosition, startPosition;
	Vector3 velocity = Vector3.zero; // für SmoothDamp
	float scrollDuration = 0.3f, nameResetDuration = 0.5f;
	const float SCROLL_FACTOR = 0.1f;
	Animation animation;
	bool resetNameStarted = false;
	public GameObject name;

	void Start()
	{
		SetGameManager();

		startPosition = localDestination = transform.localPosition;
		originalNameLocalPosition = name.transform.localPosition;

		animation = GetComponent<Animation>();

		works = new Transform[transform.childCount];
		int i = 0;
		foreach (Transform child in transform)
			works[i++] = child;

		gameObject.SetActive(false);
	}


	void Update()
	{
		if(resetNameStarted)
		{
			name.transform.localPosition = Vector3.SmoothDamp(name.transform.localPosition, originalNameLocalPosition, ref velocity, nameResetDuration);
			if(Vector3.Distance(name.transform.localPosition, originalNameLocalPosition) < 0.001f)
			{
				gameObject.SetActive(false);
				transform.localPosition = localDestination = startPosition;
				resetNameStarted = false;
			}
		}
		else
		{
			localDestination += GM.GetRelativeRotationInput().x * SCROLL_FACTOR * Vector3.up;
			if(localDestination.y < startPosition.y)
				localDestination = startPosition;
			
			Transform finalMedium = transform.GetChild(transform.childCount-1);
			//float height = finalMedium.position.y - finalMedium.GetComponent<Renderer>().bounds.size.y/2;
			float height = finalMedium.position.y - finalMedium.GetComponent<Renderer>().bounds.size.y;
			if(localDestination.y > -(startPosition.y + height))
				localDestination.y = -(startPosition.y + height);
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, localDestination, ref velocity, scrollDuration);
			if(!animation.isPlaying)
				name.transform.localPosition = Vector3.SmoothDamp(name.transform.localPosition, localDestination, ref velocity, scrollDuration);
		}
	}


	void OnEnable()
	{
		if(animation != null)
		{
			animation["Slide in"].speed = 1;
			animation.Play();
		}
	}


	void OnDisable()
	{
	}


	public void Activate()
	{
	}

	public void DeactivateAfterReset()
	{
		resetNameStarted = true;
		transform.localPosition = new Vector3(transform.localPosition.x, 1000f, transform.localPosition.z);
	}
}
