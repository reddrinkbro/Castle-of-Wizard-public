using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPattern : MonoBehaviour {
    [SerializeField] private float speed;
    private List<Vector3> movePos;
    
	// Use this for initialization
	void Start () {
        
        StartCoroutine( DrawIceSpear());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator DrawIceSpear()
    {
        transform.position = new Vector3(0, 0.2f, 0);
        GetComponent<TrailRenderer>().enabled = true;
        movePos = new List<Vector3>();
        movePos.Clear();
        movePos.Add(new Vector3(-0.2f, 0, 0));      //왼쪽
        movePos.Add(new Vector3(0, -0.2f, 0));      //아래
        movePos.Add(new Vector3(0.2f, 0, 0));       //오른쪽
        movePos.Add(new Vector3(0, 0.2f, 0));       //위쪽
        movePos.Add(new Vector3(0, -0.4f, 0));      //막대기 아래

        int listIndex = 0;
        float lerpValue = 0;
        while (true)
        {
            if (listIndex == 5) break;
            if (lerpValue <= 1)
            {
                lerpValue += 0.01f;
                transform.position = Vector3.Lerp(transform.position, movePos[listIndex], lerpValue);
            }
            else
            {
                Debug.Log(transform.position);
                lerpValue = 0;
                listIndex++;
            }
			yield return null;
        }
        
    }
}
