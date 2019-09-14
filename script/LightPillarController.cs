using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillarController : MonoBehaviour {

    [SerializeField] private float shrinkSpeed = 1f;

	// Use this for initialization
	void Start () {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(ShrinkParentObject());
        }
    }

    private IEnumerator ShrinkParentObject()
    {
        Vector3 _parentScale = transform.parent.localScale;

        while (_parentScale.x > 2)
        {
            _parentScale.x -= shrinkSpeed * Time.deltaTime;
            _parentScale.z -= shrinkSpeed * Time.deltaTime;
            transform.parent.localScale = _parentScale;

            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
