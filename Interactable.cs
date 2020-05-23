using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    private Color OrginalColor;
    private Color NewColor;
    private Renderer thisRend;

    // Use this for initialization
    void Start () {
        thisRend = GetComponentInChildren<Renderer>();
        OrginalColor = thisRend.material.color;
        NewColor = Color.green;
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallObjMethod()
    {

        highlight(true);
        print("called object method");
    }



    public void highlight(bool ison)
    {

        if (ison == true)
        {

            thisRend.material.SetColor("_Color", NewColor);
        }
        else
        {
            thisRend.material.SetColor("_Color", OrginalColor);
        }
    }

    public void CallObjMethod2()
    {
        nudge();
    }

    private void nudge()
    {
        transform.Translate(0, 0, 0.1f);
    }
}
