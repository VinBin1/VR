using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class matchrotation : MonoBehaviour {
    private GameObject HMDcam;
    private Text notifytext;
    private GameObject camobj;
    private PhotonView thisPlayerPV;
    // Use this for initialization
    void Start () {

        thisPlayerPV = transform.root.gameObject.GetComponent<PhotonView>();
        HMDcam = transform.root.transform.Find("OVRCameraRig/TrackingSpace").gameObject;
        camobj= FindInChildren(HMDcam,"CenterEyeAnchor");
        print("camobj " + camobj.name);
        notifytext = camobj.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (thisPlayerPV.isMine)
        {

            Quaternion newrot = camobj.transform.localRotation;
            transform.localRotation = newrot;
        }
    }


    private static GameObject FindInChildren(GameObject gameObject, string name)
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
               if (t.name == name)
                return t.gameObject;
        }

        return null;
    }
}
