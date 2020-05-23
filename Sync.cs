using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Sync : MonoBehaviour, IPunObservable
{
    Vector3 trueLoc;
    Quaternion trueRot;
    PhotonView pv;
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    private Animator charAnimator;
    private bool walking=false;//

    // Use this for initialization
    void Start()
    {
        pv = GetComponent<PhotonView>();
        charAnimator = this.GetComponent<Animator>();
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;
        GetComponent<PhotonView>().RPC("ChangePlayers", PhotonTargets.All, new object[] { false });
    }


    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
       

    }


    public void OnPhotonSerializeView(PhotonStream Stream, PhotonMessageInfo info)
    {
        if (Stream.isWriting == true)
        {
            Stream.SendNext(transform.position);
            Stream.SendNext(transform.rotation);

              if (charAnimator != null)
              {
                   Stream.SendNext(this.charAnimator.GetBool("iswalking"));
                Stream.SendNext(this.charAnimator.GetBool("holdingleft"));
            }


        }
        else
        {
            this.correctPlayerPos = (Vector3)Stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)Stream.ReceiveNext();

            if (charAnimator != null)
             {
                this.charAnimator.SetBool("iswalking", (bool)Stream.ReceiveNext());
                this.charAnimator.SetBool("holdingleft", (bool)Stream.ReceiveNext());
            }
           
        }
    }

    [PunRPC]
    void ChangePlayers(bool somebool)
    {
        var playermesh = gameObject.GetComponentInChildren<Renderer>();
        var playerRenderer = GetComponent<Renderer>();
        playermesh.material.SetColor("_Color", Color.red);

    }

    [PunRPC]
    void pickupObjRPC(Vector3 handPos, int ID, int toolID, Vector3 objHandRot)
    {


        
        //pass object.name to string;
        GameObject objectToPickUp;
        objectToPickUp = PhotonView.Find(toolID).transform.gameObject;

        
        GameObject playerGameObject = PhotonView.Find(ID).transform.gameObject;
         GameObject handgameObject = playerGameObject.transform.Find("engineworker_2ANO/CMU compliant skeleton/Hips/LowerBack/Spine/Spine1/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LThumb/holdPoint").gameObject; //TODO un HardCode
        objectToPickUp.transform.parent = handgameObject.transform;
        objectToPickUp.transform.localPosition = Vector3.zero;
        objectToPickUp.transform.localRotation =  Quaternion.Euler(objHandRot);


    }

    [PunRPC]
    void dropObjRPC(int ID, int toolID)
    {

        PhotonView.Find(toolID).gameObject.transform.parent = null;
     }

    [PunRPC]
    void destroyOBJ(int ID, int toolID)
    {
        Destroy(PhotonView.Find(toolID).gameObject);
    }

    [PunRPC]
    void ChangeAnimation()
    {
        charAnimator.SetBool("iswalking", true);
        print("called changeaniation");
    }


}
