using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    private static MenuController _instance;

    public static MenuController Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new MenuController();
            }
            return MenuController._instance;
        }
        set { MenuController._instance = value; }
    }
    MenuController()
    {
        _instance = this;
     
    }
    private TextMesh highScore;
   // private TweenPosition moonTweenPosition;//月亮
   // private TweenPosition houseTweenPosition;//房子
    private GameObject  light;//
	// Use this for initialization
	void Awake () {
      //  moonTweenPosition = transform.Find("Moon").GetComponent<TweenPosition>();
      //  houseTweenPosition = transform.Find("House").GetComponent<TweenPosition>();
        highScore = transform.Find("MainHighScore_Zh/Num").GetComponent<TextMesh>();
        light = transform.Find("Light").gameObject;
	}

    public void Init()
    { 
     
       //  moonTweenPosition.PlayForward(); 
       //  houseTweenPosition.PlayForward();
       //  gameTweenScale.PlayForward(); 
    }

    public void ShowLight()
    {
        light.SetActive(true);
    }
    public void UpdateHighScore(int i)
    {
        highScore.text = i.ToString();
    }
}
