using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    [SerializeField] private float speed = 1f;
    [SerializeField] private ParticleSystem[] smokeEffects;

    private Transform[] points;

    private CarManager carManager;
    private Coroutine moveSequence;

    private int initPositionIndex = 1;
    private int positionIndex = 1;

    void Start () {

    }

    public void SetCarManager(CarManager carManager)
    {
        this.carManager = carManager;
    }

    public void InitCar()
    {
        positionIndex = initPositionIndex;
        points = null;
    }

    public void PathToMove(Transform[] points)
    {
        SetPath(points);
        moveSequence = StartCoroutine(MoveSequence());
        StartSmokeEffect();
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
        while (positionIndex < points.Length - 1)
        {
            positionIndex++;
            transform.LookAt(points[positionIndex].position);
            yield return StartCoroutine(Move(points[positionIndex]));
        }
        //Destroy(this.gameObject);
        carManager.EnqueueCar(this.gameObject);
        StopCoroutine(moveSequence);
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

    private void StartSmokeEffect()
    {
        for(int i = 0; i < smokeEffects.Length; i++)
        {
            smokeEffects[i].Play();
        }
    }
}
