using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour {

    [SerializeField] private GameObject fireBallObject;
    [SerializeField] private GameObject iceSpearObject;
    [SerializeField] private GameObject magicMissileObject;
    [SerializeField] private GameObject thunderObject;
    [SerializeField] private GameObject reflectObject;
    [SerializeField] private GameObject shieldObject;

    [SerializeField] private ViveInputManager viveInputManager;
	[SerializeField] private GuideImageController guideImageController;

    [SerializeField] private Vector3 thunderCreateEnemyVector;

    [SerializeField] private ThunderArea thunderArea;
    [SerializeField] private ThunderArea nonVRThunderArea;

    private string[] magicNames = { "FireBall", "IceSpear", "thunder_ver2", "HEART", "Shield2" };

    private float matchScoreStandard = 0.9f;
    private int maxThunderTarget = 3;
	private GameObject magicMissile;
	private GameObject nextCreateMagicObject;
    private Transform nextCreatePostion;
    private Transform enemyObject = null;

    private bool isNextCreateMagic = false;

    private List<GameObject> tempMagicGameObjectList;

    // Use this for initialization
    void Start () {
        tempMagicGameObjectList = new List<GameObject>();
    }

    private void ShootMagic(GameObject magicObject, Transform createPoint)
    {
        GameObject _magicObject = null;

        if (magicObject == thunderObject)
        {
            tempMagicGameObjectList = ShootThunder(magicObject);
        }
        else if (magicObject == shieldObject)
        {
			Vector3 tmpPos = createPoint.position;
			_magicObject = Instantiate(magicObject, tmpPos, Quaternion.identity);
			//tempMagicGameObjectList.Add(_magicObject);
        }
		else if (magicObject == reflectObject)
		{
			_magicObject = Instantiate(magicObject, createPoint.parent.transform.position, Quaternion.identity);
			//tempMagicGameObjectList.Add(_magicObject);
		}
		else
        {
            if (magicObject != null)
            {
                _magicObject = Instantiate(magicObject, createPoint.position, createPoint.rotation);
                tempMagicGameObjectList.Add(_magicObject);
            }
            else
            {
                Debug.Log("Did not act Magic");
            }
            
        }

        if(tempMagicGameObjectList.Count > 0)
        {
            for(int i = 0; i < tempMagicGameObjectList.Count; i++)
            {
				if (tempMagicGameObjectList[i])
				{
					MagicObject _script = tempMagicGameObjectList[i].GetComponent<MagicObject>();
					tempMagicGameObjectList[i].GetComponent<Rigidbody>().AddForce(tempMagicGameObjectList[i].transform.forward * _script.Speed);
				}
                
            }
        }
    }

    /// <summary>
    /// 마법을 실행하는 함수
    /// </summary>
    /// <param name="magicName">마법의 이름</param>
    /// <param name="matchScore">해당 마법과 일치하는 수치</param>
    /// <returns></returns>
    public bool ActMagic(string magicName, float matchScore, Transform createPoint)
    {
		if (magicName == "FireBall") matchScore += 0.1f;

		if (matchScore > matchScoreStandard)
        {
            nextCreateMagicObject = GetMagicObject(magicName);
			guideImageController.ActGuideImage(magicName, createPoint);

			if (nextCreateMagicObject != null)
            {
                isNextCreateMagic = true;
                return true;
            }
        }
        else
        {
			GameManager.Instance.GetPlayerController().IsMagicFail = true;
        }

        return false;
    }

    public void ShootMagic()
    {
		if (isNextCreateMagic)
        {
            isNextCreateMagic = false;
            ShootMagic(nextCreateMagicObject, viveInputManager.RightMagicCreatePoint);
        }
    }

    public void TestShootMagic(Transform createPoint)
    {
        ShootMagic(nextCreateMagicObject, createPoint);
    }

    public GameObject CreateMagicMissile(Transform createPoint)
    {
		if(magicMissile == null)
		{
			magicMissile = Instantiate(magicMissileObject, createPoint.position, createPoint.rotation);
			return magicMissile;
		}
		else
		{
			return null;
		}
	}

    private GameObject GetMagicObject(string magicName)
    {
        GameObject _result = null;

        if(magicNames[0] == magicName)
        {
            _result = fireBallObject;
        }
        else if(magicNames[1] == magicName)
        {
            _result = iceSpearObject;
        }
        else if (magicNames[2] == magicName)
        {
            _result = thunderObject;
        }
        else if (magicNames[3] == magicName)
        {
            _result = reflectObject;
        }
        else if (magicNames[4] == magicName)
        {
            _result = shieldObject;
        }

        return _result;
    }

    private List<GameObject> ShootThunder(GameObject magicObject)
    {
        List<GameObject> _magicObjectList = new List<GameObject>();
        List<Collider> _list = null;
        GameObject _magicObject;
        
        if (thunderArea.gameObject.activeInHierarchy)
        {
            if (thunderArea != null)
            {
                _list = thunderArea.GetColiderList();
            }
            else
            {
                Debug.LogError("thunderArea is null!");
            }
        }
        else
        {
            if(nonVRThunderArea != null)
            {
                _list = nonVRThunderArea.GetColiderList();
            }
            else
            {
                Debug.LogError("nonVRThunderArea is null!");
            }
        }

        if(_list != null)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (maxThunderTarget <= i) break;
                _magicObject = Instantiate(magicObject, _list[i].transform.position + thunderCreateEnemyVector, Quaternion.identity);
                _magicObject.transform.LookAt(_list[i].transform.position);
                _magicObjectList.Add(_magicObject);
            }
        }
        else
        {
            Debug.Log("thunderArea colider list is null");
        }

        return _magicObjectList;
    }

    public string[] MagicNames {
        get { return magicNames; }
    }

	public GameObject GetMagicMissile()
	{
		return magicMissile; 
	}
}
