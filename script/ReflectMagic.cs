using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMagic : MonoBehaviour {

	private float holdingTime;
	// Use this for initialization
	void Start () {
		holdingTime = 2.0f;
		StartCoroutine(HoldingTime());
	}
	IEnumerator HoldingTime()
	{
		yield return new WaitForSeconds(holdingTime);
		Destroy(gameObject);
	}
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			Vector3 goalPos = collision.gameObject.GetComponent<Bullet>().GetFirstPos();
			float speed = collision.gameObject.GetComponent<Bullet>().GetSpeed();
			GameObject copyBullet = Instantiate(collision.gameObject, collision.contacts[0].point, Quaternion.identity);
			copyBullet.GetComponent<Bullet>().SetDirect(goalPos, copyBullet.transform.position, speed);
			copyBullet.gameObject.tag = "magic";
			Destroy(collision.gameObject);
		}
	}
}
