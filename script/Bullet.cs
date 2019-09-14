using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Vector3 firstPos;
	private Vector3 playPos;
    private float bulletSpeed;
	// Use this for initialization
	void Start () {
    }
	public void SetDirect(Vector3 dir, Vector3 pos,float speed)
    {
        transform.LookAt(dir);
		firstPos = pos;
		bulletSpeed = speed;
		GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime);
	}
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "magic")
			Destroy(gameObject);
		if(collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<PlayerController>().DamagedEffect();
		}
		if(collision.gameObject.tag == "AttackEnemy" && gameObject.tag == "magic")
		{
			collision.gameObject.GetComponent<EnemyCharacter>().GetDamge(magicEnum.MagicMissile, 1);
			Destroy(gameObject);
		}
	}
	public Vector3 GetFirstPos() { return firstPos; }
    public float GetSpeed() { return bulletSpeed; }
}
