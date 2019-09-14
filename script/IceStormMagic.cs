using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStormMagic : MonoBehaviour {

    private bool isEndFirstParticle = false;
    private bool isHit = false;

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Enemy") // Projectile will destroy objects tagged as Destructible
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GameObject effectObj = transform.GetChild(0).gameObject;
            effectObj.transform.parent = null;
            effectObj.transform.parent = hit.gameObject.transform;
            Destroy(effectObj, 4f);
            Destroy(gameObject);
        }
    }
}
