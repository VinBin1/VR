using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformDetect : MonoBehaviour {

    public delegate void PlatformCon();
    public PlatformCon thisPlatformCon;
 
    public enums.Platform thisPlatformDetected;

    string playerplatform;
    private Text notifytext;
    bool isEditor = false;
    float screenX, ScreenY;

    private void Awake()
    {
        thisPlatformDetected = platformDetectionStuff();
       
    }

    // Use this for initialization
    void Start () {
        notifytext = FindInChildren(this.gameObject, "notifyplayertext").GetComponent<Text>();
        notifytext.text = thisPlatformDetected.ToString();

        //pd
        if (thisPlatformDetected == enums.Platform.Android)
        {
            screenX = Screen.width; ScreenY = Screen.height;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

        }
        isEditor = Application.isEditor;


        //pd
        if (thisPlatformDetected == enums.Platform.Android)
        {
            screenX = Screen.width; ScreenY = Screen.height;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

        }
        isEditor = Application.isEditor;
        print("Platform is" + thisPlatformDetected);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public enums.Platform platformDetectionStuff()
    {
   
        playerplatform = OVRPlugin.productName;
       
     

        if (OVRPlugin.hmdPresent)//assume platform with hmd is GO.
        {
            if (playerplatform.ToLower().Contains("go"))
            {
                return enums.Platform.Go;

            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            return enums.Platform.Android;
        }

        isEditor = Application.isEditor;

        if (isEditor)
        {
            return enums.Platform.Editor;
        }

        else { return enums.Platform.other; }


    }
    public static GameObject FindInChildren(GameObject gameObject, string name)
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
                return t.gameObject;
        }

        return null;
    }

}
