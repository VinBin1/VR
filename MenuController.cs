using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    List<VrButton> CurrentButtons = new List<VrButton>();
    Image blank;
    public  VrButton[] testButtons;

    public delegate void MethodToCall();
    private MethodToCall callThis;
    int methodReq;

    // Use this for initialization
    void Start () {
        //callThis = ChangeButtons;
        //load default buttons
        CurrentButtons.Add (new VrButton(blank, "Click me!", 0));
        CurrentButtons.Add(new VrButton(blank, "Cancel", 1));
        CurrentButtons.Add(new VrButton(blank, "or Mee!", 2));
        CurrentButtons.Add(new VrButton(blank, "ehey!", 3));

        LoadText();

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            Swapbuttons(0,1);
        }
	}

    private void Swapbuttons(int f, int g)
    {
        CurrentButtons[f] = CurrentButtons[g];
        LoadText();
    }

    public void LoadText()
    {
        for (int i = 0; i < CurrentButtons.Count; i++)
        {
            testButtons[i].Stext = CurrentButtons[i].Stext;
            testButtons[i].MethodtoCall = CurrentButtons[i].MethodtoCall;
        }

            
       

    }
    void ChangeButtons(VrButton button1, VrButton button2, VrButton button3, VrButton button4)
    {
        CurrentButtons.Add(button1);
        CurrentButtons.Add(button2);
        CurrentButtons.Add(button3);
        CurrentButtons.Add(button4);
    }


    public void ShowHide(bool somebool)
    {
               
            this.gameObject.SetActive(somebool);
        
    }
}
