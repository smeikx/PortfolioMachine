using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviourWithGameManager
{
	void Start()
	{
		SetGameManager();
	}

	void Update()
	{
		transform.rotation *= Quaternion.Euler(GM.GetRelativeRotationInput().x, 0f, 0f);
	}

	public void Activate()
	{
	}
}
