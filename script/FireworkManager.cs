using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkManager : MonoBehaviour {

    [SerializeField] private GameObject fireworks;
    [SerializeField] private float fireworkRapidTime = 0.05f;
    [SerializeField] private int fireworkListMaxSize = 9;

    private List<Transform> fireworkEffects;
    private List<int> fireworkIndexList;
	private AudioSource fireworkSound;

	// Use this for initialization
	void Start () {
		fireworkSound = GetComponent<AudioSource>();
	}

    public void StartFirework()
    {
        fireworkEffects = new List<Transform>();
        fireworkIndexList = new List<int>();

        for (int i = 0; i < fireworks.transform.childCount; i++)
        {
            fireworkEffects.Add(fireworks.transform.GetChild(i));
        }

        StartCoroutine(FireWork());
		fireworkSound.Play();
	}

    private IEnumerator FireWork()
    {
        while (true)
        {
            int _index = RandomListNum(fireworkIndexList, 0, fireworkEffects.Count - 1);
            AddFirewokIndexList(_index);
            StartFireWork(fireworkEffects[_index]);
            yield return new WaitForSeconds(fireworkRapidTime);
        }
    }

    private int RandomListNum(List<int> list, int min, int max)
    {
        int _result = -1;
        do
        {
            _result = Random.Range(min, max);
        }
        while (CheckIsFireWork(list, _result));

        return _result;
    }

    private bool CheckIsFireWork(List<int> list, int index)
    {
        return list.Contains(index);
    }

    private void StartFireWork(Transform target)
    {
        if (!target.gameObject.activeInHierarchy) target.gameObject.SetActive(true);

        target.GetComponent<ParticleSystem>().Play();
        for(int i = 0; i < target.childCount; i++)
        {
            target.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
    }

    private void AddFirewokIndexList(int index)
    {
        fireworkIndexList.Add(index);
        if(fireworkIndexList.Count > fireworkListMaxSize)
        {
            fireworkIndexList.RemoveAt(0);
        }
    }
}
