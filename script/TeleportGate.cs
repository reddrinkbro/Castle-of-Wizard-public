using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGate : MonoBehaviour {

    //[SerializeField] private SceneChangeManager sceneChangeManager;
    [SerializeField] private TeleportType teleportType = TeleportType.City;

    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!GameManager.Instance.IsNight)
            {
                GameManager.Instance.SettingWarp(teleportType);
            }
        }
    }
}