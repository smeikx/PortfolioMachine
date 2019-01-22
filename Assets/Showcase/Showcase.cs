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
		AudioListener.pause = true;
		animator.SetTrigger("startOutro");
	}


	public void Disable()
	{
		AudioListener.pause = false;
		animator.ResetTrigger("startOutro");
		gameObject.SetActive(false);
	}
}
