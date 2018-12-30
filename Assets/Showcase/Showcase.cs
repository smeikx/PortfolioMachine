using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviourWithGameManager
{
	Transform[] works;

	void Start()
	{
		SetGameManager();

		works = new Transform[transform.childCount];
		int i = 0;
		foreach (Transform child in transform)
			works[i++] = child;
	}

	void Update()
	{
		transform.rotation *= Quaternion.Euler(GM.GetRelativeRotationInput().x, 0f, 0f);
	}

	public void Activate()
	{
	}
}
