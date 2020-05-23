using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using System.Reflection;


public class controllerscript : Photon.MonoBehaviour {

    private Text notifytext;

    private Transform theHand;

    private bool handsFull = false;

    GameObject HitObject;//last hit object or current
   
    pickup hitpickup;

    Vector2 rotation = new Vector2(0, 0);

    private float speed = 3;

    bool isEditor;

    PhotonView playerPV;
     
    GameObject ObjectinHand=null;

    GameObject ObjtoDestroy;

    bool buttonsOn = true;//imp

    GameObject buttonON;

    GameObject panelObject;

    GameObject rootObject;

    bool menuOn=true;
   
    enums.Platform thisPlat;
    public delegate bool PlatformPri();
    public PlatformPri thisPlatformPri;

    public delegate bool PlatformSec();
    public PlatformSec thisPlatformSec;

    private Animator THEANIMATOR;


    void Start()
    {
        rootObject = transform.root.gameObject;
        THEANIMATOR = rootObject.GetComponent<Animator>();
       

        notifytext = (FindInChildren(gameObject.transform.root.gameObject, "notifyplayertext")).transform.gameObject.GetComponent<Text>();


     
        GameObject modelObj = rootObject.transform.Find("engineworker_2ANO").gameObject;
        theHand = modelObj.transform.Find("CMU compliant skeleton/Hips/LowerBack/Spine/Spine1/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LThumb/holdPoint");

        playerPV = transform.root.gameObject.GetComponent<PhotonView>();
      
        thisPlat  = rootObject.GetComponent<PlatformDetect>().thisPlatformDetected;

        panelObject = rootObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/Canvas/menupanel").gameObject;
        thisPlatformPri = SetPlatformPri();
        thisPlatformSec = SetPlatformSec();
     
    }

    private PlatformPri SetPlatformPri()
    {
       if(thisPlat==enums.Platform.Go)
        {
            return PrimaryGo;
        }
        else
        {
            if (thisPlat == enums.Platform.Editor)
            {
                return PrimaryPC;
            }

        }
        return PrimaryPC;

    }

    private PlatformSec SetPlatformSec()
    {
        if (thisPlat == enums.Platform.Go)
        {
            return SecondaryGo;
        }
        else
        {
            if (thisPlat == enums.Platform.Editor)
            {
                return SecondaryPC;
            }

        }
        return SecondaryPC;

    }


    // Update is called once per frame
    void Update()
    {
        //pointer rotation
        if (thisPlat==enums.Platform.Editor|| thisPlat == enums.Platform.other)
        {
            PcMouseRot();
        }//togo
        else//go input
        {
            if (thisPlat == enums.Platform.Go)
            {
                var rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                transform.rotation = rot;
            }
        }
        
        doraycast();//main raycast!

        //check controller for drop/cancel menu
        DoButtonInputs(thisPlat);
    }

    private void PcMouseRot()
    {
        //windows/editorinput
        if (Input.GetAxis("Mouse X") != 0)
        {
            rotation.y += Input.GetAxis("Mouse X");
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            rotation.x += Input.GetAxis("Mouse Y");
        }

        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, -78.923f);//hardcode
    }

    private void DoButtonInputs(enums.Platform SomePlatform)
    {
        if(thisPlatformSec())
        {
            notifytext.text = "backbutton pressed";
            if (ObjectinHand != null)
            {
                dropOBJ(HitObject);
            }
            else
            {
                
                    menuclicked();
                
            }
        }
                              
        
    }

    private void menuclicked()
    {
        if (menuOn)
        {
            panelObject.GetComponent<MenuController>().ShowHide(false);
            menuOn = false;
        }
        else
        {
            if (!menuOn)
            {
                panelObject.GetComponent<MenuController>().ShowHide(true);
                menuOn = true;
            }
        }
    }

    private void doraycast()
    {
        
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, 1.0f))
        {
            //range check??
            //ui?
            string hitTag = hit.transform.gameObject.tag;

            switch(hitTag)
            {

                case "Button":
                    hit.collider.gameObject.GetComponent<VrButton>().HighLighted();//change colour 
                    buttonON = hit.collider.gameObject;// assign hit obj to var
                    checkUITrigger(buttonON);
                    break;

                case "interactable":
                    HitObject = hit.collider.gameObject;
                    checkTriggerInputs(HitObject);
                    break;

                case "pickupable":
                    HitObject = hit.collider.gameObject;


                    if (HitObject.GetComponent<pickup>() != null)
                    {
                        hitpickup = HitObject.GetComponent<pickup>();
                        hitpickup.highlight(true);

                        //check for trigger press
                        if (!handsFull)
                        {
                            //prompt to pick up
                            notifytext.text = "PICK UP?";
                            checkTriggerInputs(HitObject);
                        }
                        else { notifytext.text = "Carrying an object?"; }
                    }
                        break;

                default:
                    resetHitobject(HitObject);
                    if (buttonON != null)
                    {
                        buttonON.GetComponent<VrButton>().ResetColor();
                    }
                   
                    resetHitobject(HitObject);

                    break;
            }


        }
        else//hit nothing
        {
            resetHitobject(HitObject);
            if (buttonON != null)
            {
                buttonON.GetComponent<VrButton>().ResetColor();
            }
        }
    }

    private void checkUITrigger(GameObject buttonON) //check for click while over button
    {
              

        if (thisPlatformPri() && (handsFull == false))
        {
            //could call main function with or without handsfull 
            buttonON.GetComponent<VrButton>().MenuONOFF();
        }

        if (thisPlatformPri())//editor
        {
            
            buttonON.GetComponent<VrButton>().CallMethodOnButton();
        }
        
    }

    private static void printm(MethodBase someMethod)
    {
        print(someMethod.Name);
    }

    private void resetHitobject(GameObject thisOBJ)
    {
        //reset last hit object
        if (thisOBJ != null)
        {
            if (thisOBJ.GetComponent<pickup>() != null)
            {
                hitpickup = thisOBJ.GetComponent<pickup>();//extraline
                hitpickup.highlight(false);
                notifytext.text = "";
            }

        }
    }

    private void checkTriggerInputs ( GameObject trigObj)
    {
        if (trigObj.tag == "pickupable")
        {
            if ((thisPlatformPri()) && (handsFull == false))
            {
                pickupTheObj(trigObj);
            }
                       
        }

        if (trigObj.tag == "interactable")
        {
            trigObj.GetComponent<Interactable>().CallObjMethod();

            if (thisPlatformPri())
            {
                if (ObjectinHand.name == "spanners2")//TODO null check obj innhand 
                {
                    trigObj.GetComponent<Interactable>().CallObjMethod2();
                }
            }
        }


    }

    private bool PrimaryGo()
    {
       return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
    }

    private bool PrimaryPC()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool SecondaryGo()
    {
        return OVRInput.GetDown(OVRInput.Button.Two);
    }

    private bool SecondaryPC()
    {
        return Input.GetMouseButtonDown(1);
    }



    private void pickupTheObj( GameObject HitObject)
    {
        if (playerPV.isMine)
        {
            ///parent,move,setlocalpos
            ///
            ObjtoDestroy = HitObject;
            THEANIMATOR.SetBool("holdingleft", true);
            StartCoroutine(WaitFor(0.4f));
         

            //ui         
            hitpickup.highlight(false);
            notifytext.text = "";
            handsFull = true;
            ObjectinHand= HitObject;
          
                       
        }
        
    }
    IEnumerator WaitFor(float secs)
    {
        yield return new WaitForSeconds(secs);
        attachObj(HitObject, theHand.gameObject);
       

        
    }

    private void attachObj(GameObject hitObject,GameObject targetObj)
    {
        Vector3 objHandRot;
        objHandRot = hitObject.GetComponent<pickup>().objHandRotation;
       
            playerPV.RPC("pickupObjRPC", PhotonTargets.All, new object[] { targetObj.transform.position, playerPV.viewID, hitObject.GetComponent<PhotonView>().viewID, objHandRot });
            
     
        print("doing..ATTACH");
        #region old
        //Destroy(HitObject);
        //GameObject oldObj = PhotonView.Find(toolID).transform.gameObject;
        // PhotonNetwork.Destroy(oldObj);


        //hitObject.GetComponent<PhotonView>().TransferOwnership(playerPV.owner as PhotonPlayer);
        // hitObject.GetComponent<PhotonTransformView>().enabled = false;

        //transfer ownership,stop syncing,move object
        // hitObject.GetComponent<PhotonView>().synchronization = ViewSynchronization.Off;

        // hitObject.GetComponent<PhotonView>().enabled = false;
        //hitObject.GetComponent<PhotonView>().ObservedComponents = null;
        //  hitObject.transform.position = targetObj.transform.position;
        // targetObj.GetComponent<ConfigurableJoint>().connectedBody = hitObject.GetComponent<Rigidbody>();
        // hitObject.transform.parent = targetObj.transform;

        //  hitObject.GetComponent<PhotonView>().enabled = false;
        // hitObject.transform.localPosition = new Vector3(0, 0, 0);
        // hitObject.GetComponent<Rigidbody>().isKinematic = true;
        #endregion
    }

    void changeColor( Renderer objectrend,Color anewcolor)
    {
             
        objectrend.material.SetColor("_Color", anewcolor);
    }

    private void dropOBJ(GameObject itemtodrop)
    {
        print("dropping...........");
        THEANIMATOR.SetBool("holdingleft", false);
        playerPV.RPC("dropObjRPC",PhotonTargets.All, new object[] { 0, ObjectinHand.GetComponent<PhotonView>().viewID });
       
        handsFull = false;
        ObjectinHand = null;
       
       
    }

    [PunRPC]
    void pickupObjRPC(Vector3 handPos, int ID, int toolID)
    {


        print("pickypObjPC called");
        //pass object.name to string;
        GameObject objectToPickUp;
        objectToPickUp = PhotonView.Find(toolID).transform.gameObject;
        ObjectinHand = objectToPickUp;

        GameObject playerGameObject = PhotonView.Find(ID).transform.gameObject;

        GameObject handgameObject = playerGameObject.transform.Find("OVRCameraRig/hand").gameObject;
        objectToPickUp.transform.parent = handgameObject.transform;
        objectToPickUp.transform.localPosition = Vector3.zero;

       
    }


    
    [PunRPC]
    void dropObjRPC( int ID, int toolID)
    {

        print("dropObjPC called");
       PhotonView.Find(toolID).gameObject.transform.parent=null;
                
    }

    [PunRPC]
    void destroyOBJ(int ID, int toolID)
    {

        print("dropObjPC called");
        Destroy(PhotonView.Find(toolID).gameObject);

        
    }


    //utility

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

      