using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour {

    [SerializeField] GameObject[] cars;
    [SerializeField] Transform[] points;

    private List<GameObject> inactiveCarQueue;


    private bool isCreateRunnig = false;
    private int carIndex = -1;

	// Use this for initialization
	void Start () {
        inactiveCarQueue = new List<GameObject>();

        StartCoroutine(RandomLoopCreate(points[0], 8));
        StartCoroutine(RandomLoopCreate(points[1], 10));
        StartCoroutine(RandomLoopCreate(points[2], 12));
    }

    private IEnumerator RandomLoopCreate(Transform point, float createDeltatime)
    {
        if(point.childCount >= 2)
        {
            while (true)
            {
                Transform[] _childs = point.GetComponentsInChildren<Transform>();
                // 부모 포함해서 반환하기 때매 1부터 써야함

                if (isCreateRunnig) yield return new WaitUntil( ()=> !isCreateRunnig);

                isCreateRunnig = true;
                GameObject _car = OptimizeCreateCar(cars, point.GetChild(1).position, Quaternion.identity);
                isCreateRunnig = false;

                _car.GetComponent<CarController>().InitCar();
                _car.GetComponent<CarController>().PathToMove(_childs);

                yield return new WaitForSeconds(createDeltatime);
            }
        }
        else
        {
            Debug.LogError("car path point setting error");
        }
    }

    private GameObject OptimizeCreateCar(GameObject[] originals, Vector3 position, Quaternion rotaion)
    {
        GameObject _result = DequeueCar();
        if (_result != null)
        {
            _result.transform.position = position;
            _result.SetActive(true);
        }
        else
        {
            int _randNum = CalculateCarIndex();
            _result = Instantiate(originals[_randNum], position, rotaion, this.transform);
            _result.GetComponent<CarController>().SetCarManager(this);
        }
        return _result;
    }

    public void EnqueueCar(GameObject car)
    {
        car.SetActive(false);
        inactiveCarQueue.Add(car);
    }

    private GameObject DequeueCar()
    {
        GameObject _result = null;
        if (inactiveCarQueue.Count > 0)
        {
            _result = inactiveCarQueue[0];
            inactiveCarQueue.RemoveAt(0);
        }
        return _result;
    }

    private int CalculateCarIndex()
    {
        if (carIndex == -1) carIndex = Random.Range(0, cars.Length);

        if (carIndex > cars.Length - 1)
            carIndex = 0;

        return carIndex++;
    }
}
