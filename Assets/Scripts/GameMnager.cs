using UnityEngine;
using System.Collections;

public class GameMnager : MonoBehaviour {
    private static GameMnager _instance;

    public static GameMnager Instance
    {
        get { return GameMnager._instance; }
        set { GameMnager._instance = value; }
    }
    private AudioSource bgMusic;//背景音乐
    public void SaveHighScore(int nextHighScore)
    {
        int highscore = GetHighScore();
        if (nextHighScore > highscore)
        {
            PlayerPrefs.SetInt("highscore", nextHighScore);
        }
    }
    #region 分数存取
    /// <summary>
    /// 获得最高分
    /// </summary>
    /// <returns></returns>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt("score");
    }
    /// <summary>
    /// 保存关卡
    /// </summary>
    public void SaveStage(int stage)
    {
        PlayerPrefs.SetInt("stage", stage);
    }
    /// <summary>
    /// 获得关卡
    /// </summary>
    /// <returns></returns>
    public int GetStage()
    {
        return PlayerPrefs.GetInt("stage");
    }
    #endregion
 
    void Awake()
    {
        _instance = this;
        bgMusic = GetComponent<AudioSource>();
    }
    void Start()
    {
        MenuController.Instance.UpdateHighScore(GetHighScore());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
          //后退
            if (MenuController.Instance.gameObject.activeInHierarchy)
            {
                Application.Quit();
            }
            else {
                BackToMain();
            }
        }
    }
        
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void NewGame()
    {
       GameController.Instance.gameObject.SetActive(true);
        MenuController.Instance.gameObject.SetActive(false);  

       

        GameUI.Instance.SetStage(1);
        GameUI.Instance.SetScore(0);
        GameUI.Instance.SetHighScore(GetHighScore());
        //清空存储的状态
        GameMnager.Instance.SaveStage(1);
        GameMnager.Instance.SaveScore(0); 
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        GameController.Instance.gameObject.SetActive(true);
        MenuController.Instance.gameObject.SetActive(false);

        int highscore = PlayerPrefs.GetInt("highscore");

        GameUI.Instance.SetStage(GetStage());
        GameUI.Instance.SetScore(GetScore());
        GameUI.Instance.SetHighScore(GetHighScore());
     
    }
    /// <summary>
    /// 回到主界面
    /// </summary>
    public void BackToMain()
    {
        GameController.Instance.gameObject.SetActive(false);
        MenuController.Instance.gameObject.SetActive(true);
        //刷新最高分数
        MenuController.Instance.UpdateHighScore(GetHighScore());
    }
    /// <summary>
    /// 开关声音
    /// </summary>
    public void OpenOrCloseSound(bool b)
    {
        SoundController.Instance.isOpen=b;
        if (b)
        {
            bgMusic.Play();
        }
        else {
            bgMusic.Stop();
        }
    }

  
}
