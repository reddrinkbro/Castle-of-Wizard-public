using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 1f;

	// Use this for initialization
	void Start () {
        StartCoroutine(PatrolPath());
	}

    private IEnumerator PatrolPath()
    {
        while (true)
        {
            for (int i = 1; i < points.Length; i++)
            {
                yield return StartCoroutine(Move(points[i]));
            }
            for (int i = points.Length - 2; i >= 0; i--)
            {
                yield return StartCoroutine(Move(points[i]));
            }
        }
    }

    private IEnumerator Move(Transform target)
    {
        Vector3 _startPoint = transform.position;
        float _journeyLength = Vector3.Distance(_startPoint, target.position);
        float _startTime = Time.time;
        float _distCovered = 0;
        float _fracJourney = 0;
        
        while (_fracJourney < 1)
        {
            _distCovered = (Time.time - _startTime) * speed;
            _fracJourney = _distCovered / _journeyLength;
            transform.position = Vector3.Lerp(_startPoint, target.position, _fracJourney);
            yield return null;
        }
    }
}
