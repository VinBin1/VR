using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.XR;
using UnityEngine.UI;


public class NetworkPlayer :Photon.MonoBehaviour {
    private GameObject mycam;
    private Animator thisanimator;
    private PhotonView pvnet;
    GameObject theHand;
    GameObject overvcam;
    bool isEditor;
    controllerscript thisHandController;
    matchrotation thisMatrortation;
    GameObject cameratrack;
    CharacterController thisCon;
    PlatformDetect PlayerPlatform;
          

        // Use this for initialization
        void Start () {

        isEditor = Application.isEditor;
        PlayerPlatform = GetComponent<PlatformDetect>();
        mycam = this.transform.Find("playercam").gameObject;
        thisanimator = this.GetComponent<Animator>();
        pvnet = this.GetComponent<PhotonView>();
       // theHand = GameObject.FindWithTag("pickuped");
        overvcam = FindInChildren(this.gameObject,"OVRCameraRig");
        cameratrack = FindInChildren(overvcam, "TrackingSpace");
        thisHandController = GetComponentInChildren<controllerscript>();
        thisMatrortation = GetComponentInChildren<matchrotation>();
        thisCon = GetComponent<CharacterController>();

        if (pvnet.isMine)
            {
            cameratrack.SetActive(true);
            thisHandController.enabled = true;
            GetComponent<BASICMOVEANDCON>().enabled = true;
            thisMatrortation.enabled = true;

           
            if (thisCon) { thisCon.enabled = true; }
            PlayerPlatform.enabled = true;
        }
        else {
            PlayerPlatform.enabled = false;
           GetComponent<BASICMOVEANDCON>().enabled = false;
           // thisanimator.enabled = false;
            cameratrack.SetActive(false);
            //overvcam.SetActive(false);
            thisHandController.enabled = false;
            thisMatrortation.enabled = false;
            //TODO disable VR
            // disableVR();
            if (thisCon) { thisCon.enabled = false; }
        }
        
	}
    void disableVR()
    {
        theHand.SetActive(false);
        overvcam.SetActive(false);
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
