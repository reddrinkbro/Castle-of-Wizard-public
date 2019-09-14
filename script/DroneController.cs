using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour {

    [SerializeField] private Transform patrolPath;
    [SerializeField] private Transform[] warningPath;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 runDirectionVector = Vector3.zero;
	
    private Transform[] points;
    private Transform player;
    private bool isDetectPlayer = false;
    private int positionIndex = 1;

    private Coroutine moveRoutine = null;

	private Text waringText;

    private AudioSource warningAudio;

	// Use this for initialization
	void Start () {
        PathToMove(patrolPath.GetComponentsInChildren<Transform>());
		waringText = textObject.transform.GetChild(0).GetComponent<Text>();
        warningAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!isDetectPlayer)
            {
                isDetectPlayer = true;
                player = other.transform;
                GetComponent<Collider>().enabled = false;
                StopAllCoroutines();
                StartCoroutine(Warning());
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.IsLockMovement = false;
        GameManager.Instance.IsLockMagic = false;
    }

    private void PathToMove(Transform[] points)
    {
        SetPath(points);
        moveRoutine = StartCoroutine(MoveSequence());
    }

    private void SetPath(Transform[] points)
    {
        this.points = points;
        InitPosition();
    }

    private void InitPosition()
    {
        transform.position = points[1].position;
        if (points.Length > 2)
        {
            transform.LookAt(points[2].position);
        }
    }

    private IEnumerator MoveSequence()
    {
        bool _isRevers = false;

        while (true)
        {
            if (!_isRevers)
            {
                positionIndex++;
            }
            else
            {
                positionIndex--;
            }

            if (positionIndex == points.Length - 1)
            {
                _isRevers = true;
            }
            else if (positionIndex == 1)
            {
                _isRevers = false;
            }

            transform.LookAt(points[positionIndex].position);

            yield return StartCoroutine(Move(points[positionIndex].position));
        }
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        Vector3 _startPoint = transform.position;
        float _journeyLength = Vector3.Distance(_startPoint, targetPosition);
        float _startTime = Time.time;
        float _distCovered = 0;
        float _fracJourney = 0;

        while (_fracJourney < 1)
        {
            _distCovered = (Time.time - _startTime) * speed;
            _fracJourney = _distCovered / _journeyLength;
            transform.position = Vector3.Lerp(_startPoint, targetPosition, _fracJourney);
            yield return null;
        }
    }

    private IEnumerator Warning()
    {
        transform.LookAt(player);
        WarningSound();
        textObject.SetActive(true);
		yield return StartCoroutine(ChangeText("경고!"));
        yield return new WaitForSeconds(1f);
		yield return StartCoroutine(ChangeText("위험지역\n입니다!"));
		yield return new WaitForSeconds(2f);
        textObject.SetActive(false);
        yield return StartCoroutine(Run());

        StopWarningSound();

        StartCoroutine(SelfDestroy());
    }

    private IEnumerator Run()
    {
        yield return StartCoroutine(Move(transform.position + runDirectionVector));
    }

    private IEnumerator SelfDestroy()
    {
        yield return null;
        GameObject _effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(patrolPath.gameObject);
        Destroy(this.gameObject, 0.5f);
        Destroy(_effect, 1.5f);
    }

	private IEnumerator ChangeText(string text)
	{
		char[] _token = text.ToCharArray();
		waringText.text = "";

		for(int i = 0; i < _token.Length; i++)
		{
			waringText.text += _token[i];
			yield return new WaitForSeconds(0.1f);
		}
	}

    private void WarningSound()
    {
        warningAudio.Play();
    }

    private void StopWarningSound()
    {
        warningAudio.Stop();
    }
}
