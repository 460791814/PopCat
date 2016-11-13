using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {
    public GameObject numPrefab;//数字
    
    private static GameUI _instance;

    public static GameUI Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new GameUI();
            }
            return GameUI._instance; 
        }
        set { GameUI._instance = value; }
    }
    private TextMesh highScoreNum;
    private TextMesh stageNum;
    private TextMesh targetNum;
    private TextMesh scoreNum;
    private GameObject sound;
    private GameObject pause;
    private TextMesh tip;
    private int score = 0;
    private int stage = 1;//关卡
    private int tatgetScore;//目标分数

    private TextMesh tipOne;
    private TextMesh tipTwo;
    private TextMesh stageTip;
    private GameObject stageClear;//过关提示
    private GameObject good;
    private GameObject cool;
    private GameObject excellent;
    private GameObject fantastic;
    private GameObject gameOver;

	// Use this for initialization
	void Awake () {
        _instance = this;
        highScoreNum = transform.Find("TopNumberBG/HighScore_Zh/Num").GetComponent<TextMesh>();
        stageNum = transform.Find("TopNumberBG/Stage_Zh/Num").GetComponent<TextMesh>();
        targetNum = transform.Find("TopNumberBG/Target_Zh/Num").GetComponent<TextMesh>();
        scoreNum = transform.Find("ScoreBG/Num").GetComponent<TextMesh>();
        tip = transform.Find("ScoreBG/Tip").GetComponent<TextMesh>();
        tipOne= transform.Find("TipOne").GetComponent<TextMesh>();
        tipTwo = transform.Find("TipTwo").GetComponent<TextMesh>();
        stageTip = transform.Find("StageTip").GetComponent<TextMesh>();
        stageClear = transform.Find("StageClear").gameObject;
        good = transform.Find("Good_Zh").gameObject;
        cool = transform.Find("Cool_Zh").gameObject;
        excellent = transform.Find("Excellent_Zh").gameObject;
        fantastic = transform.Find("Fantastic_Zh").gameObject;
        gameOver = transform.Find("GameOverBG").gameObject;
	}
    /// <summary>
    /// 设置gameover提示框的显示隐藏
    /// </summary>
    /// <param name="b"></param>
    public void ActiveGameOver(bool b)
    {
        gameOver.SetActive(b);
    }
    public void ShowGood()
    {
        SoundController.Instance.PlayEffectGood();
        good.GetComponent<TweenPosition>().ResetToBeginning();
        good.GetComponent<TweenPosition>().PlayForward();
    }
    public void ShowCool()
    {
        SoundController.Instance.PlayEffectGood();
        cool.GetComponent<TweenPosition>().ResetToBeginning();
        cool.GetComponent<TweenPosition>().PlayForward();
    }
    public void ShowExcellent()
    {
        SoundController.Instance.PlayEffectGood();
        excellent.GetComponent<TweenPosition>().ResetToBeginning();
        excellent.GetComponent<TweenPosition>().PlayForward();
    }
    public void ShowFantasic()
    {
        SoundController.Instance.PlayEffectGood();
        fantastic.GetComponent<TweenPosition>().ResetToBeginning();
        fantastic .GetComponent<TweenPosition>().PlayForward();
    }
    /// <summary>
    /// 根据猫猫的个数得到总积分
    /// </summary>
    /// <param name="num"></param>
    public void SetTip(int num)
    {
        if (num > 0)
        {
            tip.text = "消除" + num + "个猫猫可以得" + ScoreController.Instance.ScoreWithNumber(num)  + "分";
        }
        else {
            tip.text = "";
        }
    }
    /// <summary>
    /// 最后奖励提示
    /// </summary>
    public void SetRewardTip(int totalCat,int catNum)
    {
        tipOne.text = "还剩" + catNum + "只,奖励" + ScoreController.Instance.BonusScoreWithRemain(catNum);
        tipTwo.text = "总共剩余" + totalCat + "只猫猫";
    }
    private void ShowNextStageTip(int stage)
    {
        stageTip.text = "第"+stage+"关";
        stageTip.GetComponent<TweenPosition>().ResetToBeginning();
        stageTip.GetComponent<TweenPosition>().PlayForward();
    }
    public void SetRewardActive(bool b) { 
    
    tipOne.gameObject.SetActive(b);
    tipTwo.gameObject.SetActive(b);
    }
    /// <summary>
    /// 是否通过当前关卡
    /// </summary>
    public bool IsStage()
    {
        return score >= tatgetScore;
    }
    /// <summary>
    /// 获得分数
    /// </summary>
    /// <param name="s"></param>
   public  void AddScore(int s)
    {
        score += s;
        scoreNum.text = score.ToString();
        GameMnager.Instance.SaveHighScore(score);
        if (stageClear.activeInHierarchy==false&&IsStage())
        {
            SoundController.Instance.PlayEffectLevelCleared();
            ShowStageClear();
        }
      
    }
    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="i"></param>
   public void SetScore(int i)
   {
       score = i;
       scoreNum.text = score.ToString();
     
   }
    /// <summary>
    /// 过关提示
    /// </summary>
   public void ShowStageClear()
   {
       stageClear.gameObject.SetActive(true);
       stageClear.GetComponent<TweenPosition>().ResetToBeginning();
       stageClear.GetComponent<TweenScale>().ResetToBeginning();
       stageClear.GetComponent<TweenPosition>().PlayForward();
       stageClear.GetComponent<TweenScale>().PlayForward();
   }
    /// <summary>
    /// 还原过关提示
    /// </summary>
   public void HideStageClear()
   {
       stageClear.gameObject.SetActive(false);
       stageClear.transform.localPosition = Vector3.zero;
       stageClear.transform.localScale = new Vector3(1, 1, 1);
   }
    /// <summary>
    /// 下一关
    /// </summary>
    /// <param name="i"></param>
   public void AddStage(int i=1)
   {
       stage += i;
       SetStage(stage);
      //当切换关卡的时候得保存下当前关卡及分数 方面下次继续游戏
       GameMnager.Instance.SaveStage(stage);
       GameMnager.Instance.SaveScore(score);//保存当前状态
       //清除上一关的相关状态
       HideStageClear();
   }
   /// <summary>
   /// 设置关卡
   /// </summary>
   /// <param name="i"></param>
   public void SetStage(int i )
   {
     
       stage = i;
    

       stageNum.text = stage.ToString();
       //设置目标分
       tatgetScore = (int)ScoreController.Instance.GetTargetScore(stage);
       targetNum.text = ScoreController.Instance.GetTargetScore(stage).ToString();
       GameUI.Instance.ShowNextStageTip(stage);
   }
    /// <summary>
    /// 设置最高分数
    /// </summary>
   public void SetHighScore(int score)
   {
       highScoreNum.text = score.ToString();
   }
   /// <summary>
   /// 播放数字上浮特效特效
   /// </summary>
    public   void PlayNum(Vector3 pos, int num)
    {
        GameObject go = GameObject.Instantiate(numPrefab) as GameObject;
        go.transform.parent = this.transform;
        go.transform.position = pos;
        go.GetComponent<Num>().MoveTo(scoreNum.transform.position, num);
   
    }

}
