using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderArea : MonoBehaviour {

    private List<Collider> coliderList;

	// Use this for initialization
	void Start () {
        coliderList = new List<Collider>();
    }

    private void LateUpdate()
    {
        coliderList.Clear();
    }

    void OnTriggerStay(Collider other)
    {
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "AttackEnemy")
		{
			if (!coliderList.Contains(other))
				coliderList.Add(other);
		}
	}

    public List<Collider> GetColiderList() {
        return coliderList;
    }

    public bool DeleteColider(Collider target)
    {
        if (coliderList.Contains(target))
        {
            coliderList.Remove(target);
            return true;
        }
        else
        {
            return false;
        }
    }
}
