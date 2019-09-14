using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacterManager : MonoBehaviour {
    [SerializeField] private GameObject[] nonPlayerCharacter;
    [SerializeField] private GameObject npcCreatePosition;
	// Use this for initialization
	void Start () {
		InvokeRepeating("RandomNonPlayerCharacterMake", 0, 5);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void RandomNonPlayerCharacterMake()
    {
        int randomNpcValue = Random.Range(0, nonPlayerCharacter.Length);
        int randomPositionValue = Random.Range(0, npcCreatePosition.transform.childCount);
        GameObject npcCreatePoint = npcCreatePosition.transform.GetChild(randomPositionValue).gameObject;
        GameObject tmpCivil = Instantiate(nonPlayerCharacter[randomNpcValue], npcCreatePoint.transform.GetChild(0).position, npcCreatePoint.transform.GetChild(0).rotation);
        NonPlayerCharacter npc = tmpCivil.GetComponent<NonPlayerCharacter>();
        if(gameObject.activeInHierarchy) StartCoroutine(npc.FollowPoint(npcCreatePoint));
    }
}
