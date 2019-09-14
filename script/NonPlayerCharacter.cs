using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour {
    [SerializeField] private Animator anim;
    [SerializeField] private bool isMove;
    [SerializeField] private int selectState;
    private float lerpValueRateOfIncrease;
	private EnemyCharacter enemyCharacter;
    // Use this for initialization
    void Start () {
        if (isMove) SetRunAndWalk();
        else SelectAnimation();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FollowPoint(GameObject mapPoint)
    {
        for (int i = 1; i < mapPoint.transform.childCount; i++)
        {
            float lerpValue = 0;
            while(true)
            {
                if (lerpValue >= 1.0f) break;
                lerpValue += lerpValueRateOfIncrease * 1 / Vector3.Distance(mapPoint.transform.GetChild(i - 1).position , mapPoint.transform.GetChild(i).position);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mapPoint.transform.GetChild(i).position - transform.position), lerpValue);
                transform.position = Vector3.Lerp(mapPoint.transform.GetChild(i - 1).position, mapPoint.transform.GetChild(i).position , lerpValue);
                yield return new WaitForSeconds(0.01f);
            }
        }
        Destroy(gameObject);
    }
	private void OnDestroy()
	{
		if (enemyCharacter != null) enemyCharacter.LossNonPlayer();
	}

	public IEnumerator ChangeState()
	{
		if(anim != null) anim.SetBool("isLookPlayer", true);
		lerpValueRateOfIncrease = 0;
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		if (anim != null) anim.SetBool("isLookPlayer", false);
        lerpValueRateOfIncrease = 0.1f;
    }

    private void SetRunAndWalk()
    {
        int randomRunState = Random.Range(0, 2);
        int randomPosSelect = Random.Range(0, 6);
        if (randomRunState == 0)
        {
            anim.SetBool("isRun", false);
            anim.SetInteger("selectAnim", randomPosSelect);
            lerpValueRateOfIncrease = 0.03f;
        }
        else
        {
            anim.SetBool("isRun", true);
            anim.SetInteger("selectAnim", randomPosSelect);
            lerpValueRateOfIncrease = 0.05f;
        }
    }
    private void SelectAnimation()
    {
        anim.SetInteger("selectState", selectState);
        if (selectState == 0)
        {
            StartCoroutine(SelectIdlePose());
        }
        else if(selectState == 1)
        {
            StartCoroutine(SelectTalkingPose());
        }
        else if(selectState == 4)
        {
            SelectSleepPose();
        }
    }
    private IEnumerator SelectIdlePose()
    {
        int randomIdleState = Random.Range(0, 6);
        anim.SetInteger("selectIdle", randomIdleState);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        StartCoroutine(SelectIdlePose());
    }
    private IEnumerator SelectTalkingPose()
    {
        int randomTalkingState = Random.Range(0, 4);
        anim.SetInteger("selectTalking", randomTalkingState);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        StartCoroutine(SelectTalkingPose());
    }
    private void SelectNodPose()
    {
        int randomNodState = Random.Range(0, 2);
        if (randomNodState == 0) anim.SetBool("selectNod", false);
        else anim.SetBool("selectNod", true);
    }
    private void SelectSleepPose()
    {
        int randomSleepPose = Random.Range(0, 3);
        if (randomSleepPose != 0) transform.Rotate(0, 90, 0);
        anim.SetInteger("selectSleep", randomSleepPose);
    }
	public void SetEnemyCharacter(EnemyCharacter enemy)	{ enemyCharacter = enemy; }
}
