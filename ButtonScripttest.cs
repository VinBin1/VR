using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonScripttest : MonoBehaviour {

    // Use this for initialization
    ColorBlock thisCB;
    Button thisbutton;
    Color newColor = Color.green;
    Color oldColor=Color.white;

    void Start() {
        
        thisbutton = this.gameObject.GetComponent<Button>();
        
    }
    

    //once you have that reference(its not null) you can call

     public void DoChange()
    {
      
        ColorBlock cb = thisbutton.colors;
        cb.normalColor = newColor;
        thisbutton.colors = cb;

      
    }
    public void ResetColor()
    {
        
        ColorBlock cb = thisbutton.colors;
        cb.normalColor = oldColor;
        thisbutton.colors = cb;
    }
    public void MenuOff()
    {

        print("CHANGE! different......");
        ColorBlock cb = thisbutton.colors;
        cb.normalColor = new Color32(50, 50, 255, 0);
        thisbutton.colors = cb;
        this.gameObject.transform.parent.gameObject.GetComponent<MenuController>().ShowHide(false);
    }
    void Update () {
       
        
    }
}
