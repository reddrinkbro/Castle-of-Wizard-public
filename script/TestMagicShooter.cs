using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMagicShooter : MonoBehaviour {

    [SerializeField] private MagicManager magicManager;
    [SerializeField] private Transform createPoint;

    [SerializeField] private float speed = 3.0F;
    [SerializeField] private float rotateSpeed = 3.0F;
    private CharacterController controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float _curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * _curSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            magicManager.TestShootMagic(createPoint);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            magicManager.ActMagic("FireBall", 1.5f, createPoint);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            magicManager.ActMagic("IceSpear", 1.5f, createPoint);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            magicManager.ActMagic("thunder_ver2", 1.5f, createPoint);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            magicManager.ActMagic("HEART", 1.5f, createPoint);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            magicManager.ActMagic("Shield2", 1.5f, createPoint);
        }
    }

}
