using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VrButton : MonoBehaviour {

    public Image buttonImage;
    private  string stext;
    private Text hardTextRef;
    private Sprite someSprite;
    
    private SpriteRenderer someSpriteRend;
    //private
    int methodtoCall;

    

    public VrButton( Image _buttonImage, string _stext , int _methodtoCall)
         {
        buttonImage = _buttonImage;
        stext = _stext;
        methodtoCall = _methodtoCall;
        //constructor
        }

    public string Stext
    {
        get{ return stext; }

        set{ stext = value; }
    }
    public int MethodtoCall
    {
        get { return methodtoCall; }

        set { methodtoCall = value; }
    }


    // Use this for initialization
    void Start () {
        hardTextRef = GetComponentInChildren<Text>();
        someSpriteRend = GetComponent<SpriteRenderer>();
    }

	public void ButtonClicked()
    {
        print("Hello");
    }

    public void CallMethodOnButton()
    {
        if(methodtoCall==0)
        {
            print("Method000 called method to call");
            ButtonClicked();
            HighLighted();
        }
        if (methodtoCall == 1)
        {
            print("Method1called method to call");
            ResetColor();

        }
    }

    public void HighLighted()
    {
        buttonImage.color = Color.green;
    }

    public void ResetColor()
    {
      
        buttonImage.color = Color.white;

}
    public void MenuONOFF()
    {
        this.gameObject.transform.parent.gameObject.GetComponent<MenuController>().ShowHide(false);
        print("button called menu off");
    }

// Update is called once per frame
void Update () {
        hardTextRef.text = Stext;

    }
}
