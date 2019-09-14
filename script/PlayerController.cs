using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject bloodImage;
	[SerializeField] private float speed = 1.0f;
	[SerializeField] private AudioSource walkAudio;
    [SerializeField] private AudioSource magicFailAudio;

	//private bool isPauseWalkAudio = false;
	private bool isMagicFail = false;


    // Use this for initialization
    void Start () {

	}

    public void SimpleMove(Vector3 value)
    {
		transform.Translate(value * Time.deltaTime * speed);
        if (!GameManager.Instance.IsLockMovement)
        {
			if(Time.timeScale != 0)
				StartWalkSound();
        }
	}

	public void StopMove()
	{
		if (walkAudio.isPlaying)
			//walkAudio.Stop();
            walkAudio.Pause();
    }

	private void StartWalkSound()
	{
		if (!walkAudio.isPlaying)
			walkAudio.Play();
    }

	public void SettingWarp(Transform point)
    {
        transform.position = point.position;
        //transform.localRotation = point.localRotation;
    }

    public void DamagedEffect()
    {
        StartCoroutine(ShowBloodEffect());
    }

    private IEnumerator ShowBloodEffect()
    {
		bloodImage.gameObject.SetActive(true);
		bloodImage.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, Random.Range(0.4f, 0.5f));
		yield return new WaitForSeconds(0.2f);
		bloodImage.gameObject.SetActive(false);
    }

    public void MagicFailSound()
    {
		if(!magicFailAudio.isPlaying)
			magicFailAudio.Play();
    }

	public bool IsMagicFail
	{
		get { return isMagicFail; }
		set { isMagicFail = value; }
	}
}
