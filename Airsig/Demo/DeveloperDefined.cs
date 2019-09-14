using System.Collections.Generic;
using UnityEngine;

using AirSig;

public class DeveloperDefined : BasedGestureHandle {

	[SerializeField] private MagicManager magicManager;
    [SerializeField] private ViveInputManager viveInputManager;

	private SteamVR_Controller.Device vrController;

	// Callback for receiving signature/gesture progression or identification results
	AirSigManager.OnDeveloperDefinedMatch developerDefined;

    // Handling developer defined gesture match callback - This is invoked when the Mode is set to Mode.DeveloperDefined and a gesture is recorded.
    // gestureId - a serial number
    // gesture - gesture matched or null if no match. Only guesture in SetDeveloperDefinedTarget range will be verified against
    // score - the confidence level of this identification. Above 1 is generally considered a match
    void HandleOnDeveloperDefinedMatch(long gestureId, string gesture, float score) { // 이 함수를 핸들러로 등록해서 패턴이 매치 되었을 때 원하는 코드를 실행할 수 있다.
        if(!GameManager.Instance.IsLockMagic)
            magicManager.ActMagic(gesture.Trim(), score, viveInputManager.VisionPoint);
	}

    // Use this for initialization
    void Awake() {
        //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

        // Update the display text
      /*  textMode.text = string.Format("Mode: {0}", AirSigManager.Mode.DeveloperDefined.ToString());
        textResult.text = defaultResultText = "Pressing trigger and write symbol in the air\nReleasing trigger when finish";
        textResult.alignment = TextAnchor.UpperCenter;
        instruction.SetActive(false);
        ToggleGestureImage("All");*/
		
        // Configure AirSig by specifying target 
        developerDefined = new AirSigManager.OnDeveloperDefinedMatch(HandleOnDeveloperDefinedMatch);
        airsigManager.onDeveloperDefinedMatch += developerDefined; // 위의 함수를 등록
        airsigManager.SetMode(AirSigManager.Mode.DeveloperDefined);
		airsigManager.SetDeveloperDefinedTarget(new List<string> {"HEART", "IceSpear", "FireBall", "thunder_ver2", "Shield2"});
		//airsigManager.SetDeveloperDefinedTarget(new List<string>(magicManager.MagicNames));
		airsigManager.SetClassifier("thunder_ver2", "");

        checkDbExist();

        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.RIGHT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            AirSigManager.PressOrTouch.PRESS);

		/*airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.LEFT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            AirSigManager.PressOrTouch.PRESS);*/

		//vrController = ViveController
	}


    void OnDestroy() {
        // Unregistering callback
        airsigManager.onDeveloperDefinedMatch -= developerDefined;
    }

    void Update() {
        UpdateUIandHandleControl();
	}
}