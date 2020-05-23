using System;
using UnityEngine;
using Photon;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.XR;



public class BASICMOVEANDCON : Photon.MonoBehaviour
{
    #region variables
    private Animator THEANIMATOR;
    

    //network
    Vector3 m_Network_Position;//defined here??
     PhotonView pv;
    float m_LastNetworkDataRecievedTime;

    private Text notifytext;
    float x,z;//forward//rotation

    //mobile
    float screenX,ScreenY;


    //vr
    private Vector2 stick;
    GameObject centreye;//getcentreeye
    GameObject player;//get player
    float Speed = 2.0f;
    bool isVR;

    
    enums.Platform thisPlatform;

    string playerplatform;
    #endregion
    public delegate void PlatformCon();
    public PlatformCon thisPlatformCon;



    private void Awake()
    {
        
          
    }

   


    void Start()
    {
        thisPlatform = transform.root.gameObject.GetComponent<PlatformDetect>().thisPlatformDetected;
        notifytext = FindInChildren(this.gameObject, "notifyplayertext").GetComponent<Text>();
        centreye = FindInChildren(this.gameObject, "CenterEyeAnchor");
        player = gameObject.transform.root.gameObject;

        THEANIMATOR = GetComponent<Animator>();
      
        pv = GetComponent<PhotonView>();
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;

        notifytext.text = thisPlatform.ToString();
               
        thisPlatformCon = SetPlatformController();

    }

    private PlatformCon SetPlatformController()
    {
        
        if (thisPlatform == enums.Platform.Editor || thisPlatform == enums.Platform.other)
        {

            return doconventionalcontrols;
        }
        else
        {
            if (thisPlatform == enums.Platform.Go)
            {
                return VRcontrols;
            }
            else
            {
                if (thisPlatform == enums.Platform.Android)
                { 
                    return doMobileControls;
                }
            }

        }
        return doconventionalcontrols;//default
    }

    void Update()
    {
       
        if (!pv.isMine)
        {
            //while networked lerp
        }
        else
        {
            DoLocalMove();

            //  DoNetworkUpdate();
        }
    }

   

    private void DoNetworkUpdate()
    {
        float PinginSec=(float)PhotonNetwork.GetPing()*0.001f;
        float TimeSinceLastUpdate = (float)PhotonNetwork.time- m_LastNetworkDataRecievedTime;
        float GapTime = PinginSec + TimeSinceLastUpdate;

        Vector3 PredictedPos = m_Network_Position + transform.forward *GapTime*5;

        Vector3 newpos = Vector3.MoveTowards(transform.position, PredictedPos,  Time.deltaTime);

        transform.position = newpos;
        m_Network_Position = newpos;

    }

    void DoLocalMove()
    {
        thisPlatformCon();
    }

    private void doMobileControls()
    {

        Vector2 touchpos;
        Input.simulateMouseWithTouches = true;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {

                touchpos = touch.position;

                if (touchpos != null)//invalid null check?
                {       //in middle zone of screen
                    if ((touchpos.x > (screenX / 3)) && (touchpos.x < (screenX - screenX / 3)))
                    {
                        if (touchpos.y > ScreenY - (ScreenY / 3))//top
                        {
                            z = Time.deltaTime * 5.0f;
                        }
                        if (touchpos.y < (ScreenY / 3))//bottom
                        {
                            z = -Time.deltaTime * 5.0f;
                        }
                    }
                    else
                    {

                        if (touchpos.y < (ScreenY / 3))//in bottom third of screen
                        {
                            if (touchpos.x > screenX - (screenX / 3))//right
                            {
                                x = Time.deltaTime * 150.0f;
                            }
                            if (touchpos.x < (screenX / 3))//left
                            {
                                x = -Time.deltaTime * 150.0f;
                            }
                        }
                    }
                }
            }
            else { x = 0;z = 0; }


        }
    }

    private void doconventionalcontrols()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        z = Input.GetAxis("Vertical") * Time.deltaTime * 1.50f;

        //actual movement implimentaion
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        //Animate walk
        if (z != 0)
        {
            THEANIMATOR.SetBool("iswalking", true);
        }
        else
        {
            THEANIMATOR.SetBool("iswalking", false);
        }
        if (x < 0)
        {
            THEANIMATOR.SetBool("left", true);
        }
        else { THEANIMATOR.SetBool("left", false); }
    }

    private void VRcontrols()//oculusgo
    {
        notifytext.text = "VR CON.............";
        stick = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        //transform.eulerAngles = new Vector3(0, centreye.transform.localEulerAngles.y, 0);
        Vector3 forwardmove = Vector3.forward * Speed * stick.y * Time.deltaTime;
        transform.Translate(forwardmove);//moveplayer
        float yval = stick.y;
        notifytext.text = ""+yval.ToString();

        if (yval > 0)
        {
            THEANIMATOR.SetBool("iswalking", true);
        }
        else { THEANIMATOR.SetBool("iswalking", false); }

             x =  Speed * stick.x ;//wtf
        transform.Rotate(0, x, 0);

        player.transform.position = Vector3.Lerp(player.transform.position, transform.position, 10f * Time.deltaTime);
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
   