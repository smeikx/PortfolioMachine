using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviourWithGameManager
{
	Transform[] works;
	Vector3 localDestination;
	Vector3 velocity = Vector3.zero; // für SmoothDamp
	float scrollDuration = 0.3f;
	const float SCROLL_FACTOR = 0.1f;
	Animation animation;

	void Start()
	{
		SetGameManager();

		localDestination = transform.localPosition;

		animation = GetComponent<Animation>();

		works = new Transform[transform.childCount];
		int i = 0;
		foreach (Transform child in transform)
			works[i++] = child;

		gameObject.SetActive(false);
	}


	void Update()
	{
		localDestination += GM.GetRelativeRotationInput().x * SCROLL_FACTOR * Vector3.up;
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, localDestination, ref velocity, scrollDuration);
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
}
