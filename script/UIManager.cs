using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	[SerializeField] private Canvas[] curvedUICanvas;
	[SerializeField] private Canvas explanationCanvas;
	[SerializeField] private GameObject[] explanationPanel;
	[SerializeField] private Text goalText;
	[SerializeField] private GameObject laserBeam;
	private bool isExplanationEnd;
	private bool isOpenGuideWindow;
	// Use this for initialization
	void Start () {
		//Time.timeScale = 0;
		isOpenGuideWindow = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void guideWindow()
	{
		if (isExplanationEnd)
		{
			Debug.Log(isOpenGuideWindow);
			if (isOpenGuideWindow)
			{
				for (int i = 0; i < curvedUICanvas.Length; i++)
				{
					curvedUICanvas[i].gameObject.SetActive(false);
				}
				Time.timeScale = 1;
				isOpenGuideWindow = false;
			}
			else
			{
				for (int i = 0; i < curvedUICanvas.Length; i++)
				{
					curvedUICanvas[i].gameObject.SetActive(true);
				}
				Time.timeScale = 0;
				isOpenGuideWindow = true;
			}
		}
	}

    public bool IsOpenGuideWindow
    {
        get
        {
            return isOpenGuideWindow;
        }
    }

	public void ClickNextButton()
	{
		explanationPanel[0].SetActive(false);
		explanationPanel[1].SetActive(true);
	}

	public void ClickEndButton()
	{
		explanationPanel[1].SetActive(false);
		explanationPanel[2].SetActive(false);
		explanationCanvas.gameObject.SetActive(false);
		isExplanationEnd = true;
		Time.timeScale = 1;
		laserBeam.SetActive(false);
		GameManager.Instance.IsLockMagic = false;
		GameManager.Instance.IsLockMovement = false;
	}

	public void ClickBackButton()
	{
		explanationPanel[0].SetActive(true);
		explanationPanel[1].SetActive(false);
	}

	public void SetWarningExplanation()
	{
		explanationCanvas.gameObject.SetActive(true);
		explanationCanvas.transform.localPosition = new Vector3(675.5f, 1.58f, 25.49f);
		explanationPanel[2].SetActive(true);
		isExplanationEnd = false;
		Time.timeScale = 0;
		laserBeam.SetActive(true);
        GameManager.Instance.IsLockMagic = true;
        GameManager.Instance.IsLockMovement = true;
    }
}
