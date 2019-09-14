using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObject : MonoBehaviour {

    [SerializeField] private int damage;
    [Tooltip("speed는 400정도가 적당한듯")]
    [SerializeField] private float speed;
    [SerializeField] private magicEnum magicName;
    private bool isShoot = false;
    private Vector3 targetDirection;

	// Use this for initialization
	void Start () {
        
    }
	
    public float Speed {
        get { return speed; }
    }

	// Update is called once per frame
	void Update () {
	}
	void OnCollisionEnter(Collision hit)
	{
		if (hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "AttackEnemy")
		{
			if (hit.gameObject.GetComponent<EnemyCharacter>())
			{
				hit.gameObject.GetComponent<EnemyCharacter>().GetDamge(magicName, damage);
			}
			
			//Destroy(this.gameObject);
		}
	}

	public magicEnum GetMagic() { return magicName; }
    public float Damage
    {
        get { return damage; }
    }
}
