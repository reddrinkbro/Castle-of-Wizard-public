using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] bullet;
    [SerializeField] private Transform shootTransform;
	private EnemyCharacterManager enemyCharacterManager;
    private float lerpValueRateOfIncrease;
    private Coroutine followPoint;
    private int priority;
    private int currentPoint;
    private bool isFindPlayer = false;
    private GameObject root;
    private float lerpValue;
    private GameObject player;
    private float rotateAngleToPlayer = 0;
    private float reloadTime;
	[SerializeField] private int hp;
    [SerializeField] private AudioSource audio;
    // Use this for initialization
    void Start ()
    {
        hp = 20;
        lerpValue = 0;
        reloadTime = 0;
        lerpValueRateOfIncrease = 0.04f;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(isFindPlayer)
        {
            LookAtPlayer();
            ShootPlayer();
        }
		if(hp <= 0)
		{
			if (!anim.GetBool("isDie"))
			{
				StopAllCoroutines();
				StartCoroutine(EnemyDie());
			}
		}
	}
    public void Init(GameObject mapPoint, int createPoint)
    {
        followPoint = StartCoroutine(FollowPoint(mapPoint, createPoint));
        root = mapPoint;
    }
    public void RestartFollowPoint()
    {
        anim.SetBool("isShoot", false);
        isFindPlayer = false;
        rotateAngleToPlayer = 0;
        followPoint = StartCoroutine(FollowPoint(root, currentPoint));
    }
    public IEnumerator FollowPoint(GameObject mapPoint, int createPoint)
    {
        int index = 0;
        int createIndex = createPoint;
        priority = createPoint;
        
        while (index < 100)
        {
            
            for (int i = createIndex; i < mapPoint.transform.childCount; i++)
            {
                while (true)
                {
                    if (lerpValue >= 1.0f) break;
                    lerpValue += lerpValueRateOfIncrease * 1 / Vector3.Distance(mapPoint.transform.GetChild(i - 1).position, mapPoint.transform.GetChild(i).position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mapPoint.transform.GetChild(i).position - transform.position), lerpValue);
                    transform.position = Vector3.Lerp(mapPoint.transform.GetChild(i - 1).position, mapPoint.transform.GetChild(i).position, lerpValue);
                    currentPoint = i;
                    yield return new WaitForSeconds(0.01f);
                }
                lerpValue = 0;
            }
            while (true)
            {
                if (lerpValue >= 1.0f) break;
                lerpValue += lerpValueRateOfIncrease * 1 / Vector3.Distance(mapPoint.transform.GetChild(mapPoint.transform.childCount - 1).position, mapPoint.transform.GetChild(0).position);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mapPoint.transform.GetChild(0).position - transform.position), lerpValue);
                transform.position = Vector3.Lerp(mapPoint.transform.GetChild(mapPoint.transform.childCount - 1).position, mapPoint.transform.GetChild(0).position, lerpValue);
                yield return new WaitForSeconds(0.01f);
            }
            currentPoint = 0;
            createIndex = 0;
            index++;
        }
    }
    public void ChangeState(GameObject playerObject)
    {
        anim.SetBool("isLookPlayer", true);
        lerpValueRateOfIncrease = 0;
        StopCoroutine(followPoint);
        player = playerObject;
        lerpValueRateOfIncrease = 0.04f;
		transform.tag = "AttackEnemy";
		anim.SetBool("isShoot", true);
        isFindPlayer = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RunningNPC" || other.tag == "EscapeNPC")
        {
			if (!anim.GetBool("isStandBy")) anim.SetBool("isStandBy", true);
            lerpValueRateOfIncrease = 0;
        }
		else if(other.tag == "Enemy" && transform.tag == "Enemy")
		{
			StopAllCoroutines();
			EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
			enemy.StopAllCoroutines();
			transform.tag = "AttackEnemy";
			enemy.tag = "AttackEnemy";
			transform.LookAt(enemy.transform);
			enemy.transform.LookAt(transform);
			if (priority >= enemy.GetPriority())
			{
				enemy.GetAnimator().SetBool("isStandBy", true);
				anim.SetBool("isShoot", true);
				StartCoroutine(SeeAnotherEnemy(enemy));
			}
			else
			{
				anim.SetBool("isStandBy", true);
				enemy.GetAnimator().SetBool("isShoot", true);
				enemy.StartCoroutine(enemy.SeeAnotherEnemy(this));
			}
		}
	}

	private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RunningNPC" || other.tag == "EscapeNPC")
        {
            if (anim.GetBool("isStandBy")) anim.SetBool("isStandBy", false);
            lerpValueRateOfIncrease = 0.04f;
			NonPlayerCharacter nonPlayer = other.gameObject.GetComponent<NonPlayerCharacter>();
			nonPlayer.SetEnemyCharacter(this);
        }
    }
    private void LookAtPlayer()
    { 
        Vector3 dirToTarget = player.transform.position - transform.position;
        Vector3 look = Vector3.Slerp(transform.forward, dirToTarget.normalized, Time.deltaTime * 4);
		if(look.y > 0.2f)
		{
			look.y = 0.2f;
		}
        transform.rotation = Quaternion.LookRotation(look, Vector3.up);
    }
    public void ShootPlayer()
    {
        reloadTime += Time.deltaTime;
        if (reloadTime > anim.GetCurrentAnimatorStateInfo(0).length)
        {
            audio.Play();
            GameObject bulletObj = Instantiate(bullet[0], shootTransform.position, shootTransform.rotation);
            Bullet tmpBullet = bulletObj.GetComponent<Bullet>();
            Vector3 dirToTarget = player.transform.position - shootTransform.localPosition;
			Vector3 thisPos = gameObject.transform.position;
			thisPos.y += 1f;
			dirToTarget.y -= 0.4f;
			tmpBullet.SetDirect(dirToTarget, thisPos, 30000);
            reloadTime = 0;
        }
    }
	public void LossNonPlayer()
	{
		anim.SetBool("isStandBy", false);
		lerpValueRateOfIncrease = 0.05f;
	}
	public IEnumerator SeeAnotherEnemy(EnemyCharacter enemy)
	{
		yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
		Destroy(Instantiate(bullet[1], enemy.transform.position + Vector3.up * 0.8f, shootTransform.rotation), 1);
		enemy.GetAnimator().SetBool("isDie", true);
		anim.SetBool("isShoot", false);
		anim.SetBool("isStandBy", true);
		yield return new WaitForSeconds(enemy.GetAnimator().GetCurrentAnimatorClipInfo(0).Length);
		Destroy(enemy.gameObject);
		anim.SetBool("isStandBy", false);
		transform.tag = "Enemy";
		RestartFollowPoint();
	}
	private IEnumerator EnemyDie()
	{
		anim.SetBool("isDie", true);
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		Destroy(this.gameObject);
	}
    public void GetDamge(magicEnum magic, int damage)
    {
        //색깔보고 확인해야함
        if (magic == magicEnum.FireBall)
        {
            StartCoroutine(FireballDotDamage(damage));
        }
		hp -= damage;
	}
    private IEnumerator FireballDotDamage(int damage)
    {
        for (int i = 0; i < 5; i++)
        {
            hp -= damage;
            yield return new WaitForSeconds(2.5f);
        }
    }
	public void OnDestroy()
	{
		enemyCharacterManager.DecreaseEnemyCount(this);
	}

	public void SetEnemyCharacterManager(EnemyCharacterManager enemy) { enemyCharacterManager = enemy; } 
	public int GetPriority() { return priority; }
	public Animator GetAnimator() { return anim; }
}
