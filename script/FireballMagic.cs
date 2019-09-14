using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMagic : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "AttackEnemy")
        {
            GameObject particle = gameObject.GetComponent<PixelArsenalProjectileScript>().projectileParticle;
            particle.transform.parent = collision.gameObject.transform;
        }
    }
}
