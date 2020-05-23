using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerscript : MonoBehaviour {

    // Use this for initialization
    public Vector2 stick;
    public GameObject centreye;
    public GameObject player;
    float Speed = 5.0f;
    bool isVR;



    void Start()

    {
        
    }
    void Update()
    {
       
            VRcontrols();
       
    }

    private void MobileControls()
    {
        //
    }

    private void VRcontrols()
    {
        stick = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        transform.eulerAngles = new Vector3(0, centreye.transform.localEulerAngles.y, 0);
        transform.Translate(Vector3.forward * Speed * stick.y * Time.deltaTime);//moveplayer
        transform.Translate(Vector3.right * Speed * stick.x * Time.deltaTime);//straff

        player.transform.position = Vector3.Lerp(player.transform.position, transform.position, 10f * Time.deltaTime);
    }
}
