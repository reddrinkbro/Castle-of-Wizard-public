using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMagic : MonoBehaviour {
	[SerializeField] private float holdingTime = 2.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine(HoldingTime());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator HoldingTime()
	{
		yield return new WaitForSeconds(holdingTime);
		Destroy(gameObject);
	}
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.gameObject);
	}
}
