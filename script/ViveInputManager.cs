using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveInputManager : MonoBehaviour {

	[SerializeField] private SteamVR_TrackedObject leftTrackedObject;
	[SerializeField] private SteamVR_TrackedObject rightTrackedObject;

    [SerializeField] private Transform leftMagicCreatePoint;
    [SerializeField] private Transform rightMagicCreatePoint;
	[SerializeField] private Transform visionPoint;

	[SerializeField] private MagicManager magicManager;
	[SerializeField] private UIManager uIManager;
	//[SerializeField] private CharacterController characterController;

	[SerializeField] private GameObject laserPrefab;
	// telport
	[SerializeField] private GameObject teleportReticlePrefab;
	private GameObject reticle;
	private Transform teleportReticleTransform;
	[SerializeField] private Transform headTransform;
	[SerializeField] private Vector3 teleportReticleOffset;
	[SerializeField] private LayerMask teleportMask;

    [SerializeField] private ParticleSystem paintParticle;
	[SerializeField] private HandController handController;
    private bool shouldTeleport = false;

	private GameObject laser;
	private Transform laserTransform;
	private Vector3 hitPoint;

	private SteamVR_Controller.Device mDevice;
    private ViveInputController leftController;
    
    private Transform mainCameraTransform;
	//private Transform cameraRigTransform;

	void Start () {
        leftController = leftTrackedObject.GetComponent<ViveInputController>();
        mainCameraTransform = Camera.main.GetComponent<Transform>();
		//cameraRigTransform = mainCameraTransform.parent;

		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;
		reticle = Instantiate(teleportReticlePrefab);
		teleportReticleTransform = reticle.transform;
    }
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("IsLockMagic: " + GameManager.Instance.IsLockMagic);
		if((int)rightTrackedObject.index != -1)
		{
			mDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);
			
			if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
			{
                if(!GameManager.Instance.IsLockMagic)
				    magicManager.ShootMagic();
			}
			if(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
			{
				uIManager.guideWindow();
                GameManager.Instance.GetPlayerController().StopMove();
                GameManager.Instance.IsLockMovement = !GameManager.Instance.IsLockMovement;
            }
            if(mDevice.GetAxis() != Vector2.zero)
            {
                if (mDevice.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    if (!GameManager.Instance.IsLockMovement)
                    {
                        float _speed = 3;
                        Vector3 _forwardDirection = mainCameraTransform.TransformDirection(Vector3.forward * _speed);
                        Vector3 _rightDirection = mainCameraTransform.TransformDirection(Vector3.right * _speed);
                        Vector3 _direction = _forwardDirection * mDevice.GetAxis().y + _rightDirection * mDevice.GetAxis().x;

                        GameManager.Instance.GetPlayerController().SimpleMove(_direction);
                    }
				}
            }
			if (mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
			{
				GameManager.Instance.GetPlayerController().StopMove();
			}

			if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				if(!GameManager.Instance.IsLockMagic)
					paintParticle.Play();
			}
			else if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				paintParticle.Stop();
				paintParticle.Clear();
				Invoke("CheckMagicFailSound", 0.05f);
			}
		}

		if ((int)leftTrackedObject.index != -1)
		{
			mDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);

			if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
                if (!uIManager.IsOpenGuideWindow)
                {
					if (!GameManager.Instance.IsLockMagic)
					{
						if(magicManager.GetMagicMissile() == null) handController.SetGrabAnimation(true);
						leftController.GrabObject(magicManager.CreateMagicMissile(leftMagicCreatePoint));
					}
                }
            }

            if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (leftController.ObjectInHand)
                {
					if (!GameManager.Instance.IsLockMagic)
					{
						leftController.ReleaseObject(mDevice);
						handController.SetAnimatorSpeed(1);
					}
                }
            }
            if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                uIManager.guideWindow();
                GameManager.Instance.GetPlayerController().StopMove();
                GameManager.Instance.IsLockMovement = !GameManager.Instance.IsLockMovement;
            }

            if (mDevice.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
			{
				 if (!uIManager.IsOpenGuideWindow)
				 {
                    RaycastHit _hit;
                    if (Physics.Raycast(leftTrackedObject.transform.position, leftTrackedObject.transform.forward, out _hit, 100, teleportMask))
                    {
						if (!Physics.Raycast(leftTrackedObject.transform.position, leftTrackedObject.transform.forward, out _hit, 100, ~teleportMask))
						{
						hitPoint = _hit.point;
                        ShowLaser(_hit, leftTrackedObject.transform);
                        }
                    }
                 }
			}
			else
			{
				laser.SetActive(false);
				reticle.SetActive(false);
			}

			if(mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
			{
                if (!uIManager.IsOpenGuideWindow)
                {
                    if(!GameManager.Instance.IsLockMovement)
                        Teleport();
                }
			}
            
        }
    }

    public Transform LeftMagicCreatePoint
    {
        get { return leftMagicCreatePoint; }
    }

    public Transform RightMagicCreatePoint
    {
        get { return rightMagicCreatePoint; }
    }

	public Transform VisionPoint
	{
		get { return visionPoint; }
	}

	private void ShowLaser(RaycastHit hit, Transform target)
	{
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(target.position, hitPoint, 0.5f);
		laserTransform.LookAt(hitPoint);
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);

		reticle.SetActive(true);
		teleportReticleTransform.position = hitPoint + teleportReticleOffset;
		shouldTeleport = true;
	}

	private void Teleport()
	{
        Transform _playerPoint = GameManager.Instance.GetPlayerController().transform;
		shouldTeleport = false;
		reticle.SetActive(false);
		Vector3 difference = _playerPoint.position - headTransform.position;
		difference.y = 0;
        _playerPoint.position = hitPoint + difference;
	}

	private void CheckMagicFailSound()
	{
		if (GameManager.Instance.GetPlayerController().IsMagicFail)
		{
			GameManager.Instance.GetPlayerController().MagicFailSound();
			GameManager.Instance.GetPlayerController().IsMagicFail = false;
		}
	}
}
