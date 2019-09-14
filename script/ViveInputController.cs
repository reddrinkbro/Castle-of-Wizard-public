using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveInputController : MonoBehaviour {
    private GameObject objectInHand;

    // Use this for initialization
    void Start () {
		
	}

    public void GrabObject(GameObject grapObject)
    {
		objectInHand = grapObject;
		FixedJoint joint = null;
		if (!GetComponent<FixedJoint>() && grapObject != null)
		{
			joint = AddFixedJoint();
		}
		if(joint != null && objectInHand != null)
		{
			joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
		}

	}

    private FixedJoint AddFixedJoint()
    {
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void ReleaseObject(SteamVR_Controller.Device device)
    {
        if (GetComponent<FixedJoint>())
        {
			GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            Quaternion _rotation = GameManager.Instance.GetPlayerController().transform.rotation;

            objectInHand.GetComponent<Rigidbody>().velocity = device.velocity * 3.5f;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = device.angularVelocity;
            objectInHand.transform.Rotate(-_rotation.x, -_rotation.y, -_rotation.z);
        }
		objectInHand = null;
	}

    public GameObject ObjectInHand
    {
        get { return objectInHand; }
    }
}
