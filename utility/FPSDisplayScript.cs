using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplayScript : MonoBehaviour
{
    float timeA;
    public int fps;
    public int lastFPS;
    public GUIStyle textStyle;
    public Text fpstext;
    // Use this for initialization
    void Start()
    {
        // = GameObject.Find("notifyplayertext").GetComponent<Text>();
        fpstext = FindInChildren(this.gameObject, "notifyplayertext").GetComponent<Text>();
        timeA = Time.timeSinceLevelLoad; 
        DontDestroyOnLoad(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.timeSinceLevelLoad+" "+timeA);
        if (Time.timeSinceLevelLoad - timeA <= 1)
        {
            fps++;
        }
        else
        {
            lastFPS = fps + 1;
            timeA = Time.timeSinceLevelLoad;
            fps = 0;
        }
        fpstext.text = lastFPS.ToString();


    }
    void OnGUI()
    {
        GUI.Label(new Rect(450, 5, 30, 30), "" + lastFPS, textStyle);
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

