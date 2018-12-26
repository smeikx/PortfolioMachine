using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToCam : MonoBehaviour
{
	void Start ()
	{
	}
	
	void Update ()
	{
		transform.LookAt(Vector3.zero, Vector3.up);
	}
}
