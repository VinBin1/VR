using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour {
    private Color OrginalColor;
    private Color NewColor;
    private Renderer thisRend;
    public Vector3 objHandRotation;

    // Use this for initialization
    void Start () {
        thisRend = GetComponentInChildren<Renderer>();
        OrginalColor = thisRend.material.color;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void highlight(bool ison)
    {
        if(ison==true)
        {

            thisRend.material.SetColor("_Color", NewColor);
        }
        else
        {
            thisRend.material.SetColor("_Color", OrginalColor);
        }
    }
}
