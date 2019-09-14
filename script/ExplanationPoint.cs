using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationPoint : MonoBehaviour {
	[SerializeField] UIManager uIManager;
	[SerializeField] GameObject detectCollision;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			uIManager.SetWarningExplanation();
			detectCollision.SetActive(true);
			Destroy(gameObject);
		}
		
	}
}
