using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviour
{
	Animator animator;


	void Start()
	{
		animator = GetComponent<Animator>();
		gameObject.SetActive(false);
	}


	public void StartOutro()
	{
		Debug.Log("Start Oturo");
		animator.SetTrigger("startOutro");
	}


	public void Disable()
	{
		animator.ResetTrigger("startOutro");
		gameObject.SetActive(false);
	}
}
