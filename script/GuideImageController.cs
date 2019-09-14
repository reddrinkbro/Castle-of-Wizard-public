using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideImageController : MonoBehaviour {

    [SerializeField] MagicManager magicManager;
    [SerializeField] GameObject guideImageObject;
    [SerializeField] Sprite[] magicImages;

    //[SerializeField] Transform test;

    private float deltaColor = 0.05f;
    private float overScale = 1.2f;
    private float deltaSacle = 0.05f;
    private Coroutine guideCorutine;

	private Image guideImageComponent;

	private bool isActGuideImage = false;
	private string magicName;
	private Transform imagePoint;

	// Use this for initialization
	void Start () {
		guideImageComponent = guideImageObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ActGuideImage("Thunder2", test);
        //}

		if (isActGuideImage)
		{
			isActGuideImage = false;
			ActGuideImage();
		}

	}

	public void ActGuideImage(string magicName, Transform point)
	{
		this.magicName = magicName;
		imagePoint = point;
        isActGuideImage = true;
	}

	private void ActGuideImage()
    {
        //Quaternion _rotation = new Quaternion(guideImageObject.transform.rotation.x, imagePoint.rotation.y, imagePoint.rotation.z, guideImageObject.transform.rotation.w);
		//Vector3 _point = new Vector3(imagePoint.position.x, imagePoint.position.y, imagePoint.position.z + 1);
		Debug.Log("magicName: " + magicName);
        guideImageObject.SetActive(false);
		//ToggleInvisibleImage(guideImageComponent);

		Quaternion _imagePointRotaionValue = imagePoint.rotation;
		_imagePointRotaionValue.z = 0;

		guideImageObject.transform.position = imagePoint.position;
        guideImageObject.transform.rotation = _imagePointRotaionValue;

		SwitchMagicImage(magicName);

		if (guideCorutine == null)
        {
            guideCorutine = StartCoroutine(GuideSequence());
        }
        else
        {
            StopCoroutine(guideCorutine);
            guideCorutine = null;
            ActGuideImage(magicName, imagePoint);
        }
    }

    private IEnumerator GuideSequence()
    {
        float _alpha = 1;
        float _scale = 1;

        guideImageObject.SetActive(true);
		//ToggleVisibleImage(guideImageComponent);
		guideImageObject.GetComponent<Image>().color = new Color(1, 1, 1, _alpha);
        guideImageObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        while (_scale < overScale)
        {
            _scale += deltaSacle;
            guideImageObject.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, _scale);
            yield return new WaitForSeconds(0.05f);
        }

        while (_alpha > 0.1f)
        {
            _alpha -= deltaColor;
            guideImageObject.GetComponent<Image>().color = new Color(1, 1, 1, _alpha);
            yield return new WaitForSeconds(0.01f);
        }

		guideImageObject.SetActive(false);
		//ToggleInvisibleImage(guideImageComponent);
	}

    private void SwitchMagicImage(string magicName)
    {
        guideImageObject.GetComponent<Image>().sprite = FindMagicImage(magicName);
    }

    private Sprite FindMagicImage(string magicName)
    {
        Sprite _result = null;

        for(int i = 0; i < magicManager.MagicNames.Length; i++)
        {
            if(magicManager.MagicNames[i] == magicName)
            {
				_result = magicImages[i];
            }
        }

        return _result;
    }

	private void ToggleVisibleImage(Image image)
	{
		image.color = new Color(1, 1, 1, 1);
	}

	private void ToggleInvisibleImage(Image image)
	{
		image.color = new Color(1, 1, 1, 0);
	}
}
