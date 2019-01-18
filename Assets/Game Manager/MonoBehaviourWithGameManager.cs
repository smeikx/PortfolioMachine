using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourWithGameManager : MonoBehaviour
{
	protected GameManager GM;
	protected void SetGameManager()
	{
		GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}
}
