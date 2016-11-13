using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get {
            if (_instance == null) {
                _instance = new GameController();
            }
            return GameController._instance;
        }
        set { GameController._instance = value; }
    }
    GameController() {
        _instance = this;
    }
    private Dictionary<int, Sprite> catList = new Dictionary<int, Sprite>();
    private Dictionary<int, Sprite> catOnList = new Dictionary<int, Sprite>();
    private Transform Cats;

    public GameObject catPrefab;//猫预设
    public GameObject catParticle;//特效

    //Dictionary 比list多损耗空间 因为要多存储一个key方便查找使用
    private Dictionary<int, Dictionary<int, Cat>> catDic = new Dictionary<int, Dictionary<int, Cat>>();

    private List<Cat> selectedCatList = new List<Cat>();
    private List<Cat> isHaveFriendList = new List<Cat>();//零时变量
    void Awake()
    {
       // _instance = this;
        Cats = this.transform.Find("Cats");
    }

    void Start()
    {

       LoadCat();

    StartCoroutine(  CreateCat());
    }
    /// <summary>
    /// 消耗魔法鱼 继续游戏
    /// </summary>
    public void ContinueCreateCat()
    {
        StartCoroutine(CreateCat());
    }
    private float catDropTime = 0;//开始生成时毛下落的时间
    /// <summary>
    /// 初始化所有的猫
    /// </summary>
    IEnumerator CreateCat()
    {
        yield return new WaitForSeconds(1.5f);
        catDic.Clear();
        for (int rowIndex = 0; rowIndex < 10; rowIndex++)
        {
            catDropTime = 0.3f;
            Dictionary<int, Cat> tempDic = new Dictionary<int, Cat>();
            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {
                Cat cat = AddCat(rowIndex, columnIndex);

                tempDic.Add(columnIndex, cat);

            }
            catDic.Add(rowIndex, tempDic);
        }
    }
    private Cat GetCat(int rowIndex, int columnIndex)
    {

        Dictionary<int, Cat> tempDic;
        catDic.TryGetValue(rowIndex, out tempDic);
        if (tempDic != null)
        {
            Cat cat;
            tempDic.TryGetValue(columnIndex, out cat);
            if (cat != null)
            {
                return cat;

            }
        }
        return null;
    }
    private void SetCat(int rowIndex, int columnIndex, Cat cat)
    {
        catDic[rowIndex][columnIndex] = cat;
    }
    /// <summary>
    /// 移除摸个元素
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    private void RemoveCat(int rowIndex, int columnIndex)
    {
        catDic[rowIndex][columnIndex] = null;
    }
    /// <summary>
    /// 添加猫
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    private Cat AddCat(int rowIndex, int columnIndex)
    {
        // var type = (CatType)Random.Range((int)CatType.Red, (int)CatType.Purple + 1);

        int r = Random.Range(0, 5);
        CatType tempType = (CatType)r;

        //GameObject go = new GameObject();
        //go.AddComponent<SpriteRenderer>().sprite = catList[r];
        //go.AddComponent<BoxCollider>();
        //go.AddComponent<Cat>();
        GameObject go = GameObject.Instantiate(catPrefab) as GameObject;

        go.name = "Cat_" + tempType;
        go.transform.parent = Cats;

        go.GetComponent<SpriteRenderer>().sprite = catList[r];
        Cat cat = go.GetComponent<Cat>();
        cat.type = tempType;
        cat.UpdatePostion(rowIndex, columnIndex, catDropTime);
        catDropTime += Random.Range(0.03f, 0.1f);
        return cat;
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    void LoadCat()
    {
        for (int i = 0; i < 5; i++)
        {
            Sprite cat = Resources.Load<Sprite>("7201280/Game/Cat" + (i + 1));
            catList.Add(i, cat);
            Sprite catOn = Resources.Load<Sprite>("7201280/Game/CatOn" + (i + 1));
            catOnList.Add(i, catOn);
        }
    }
    /// <summary>
    /// 当猫被点击的时候触发
    /// </summary>
    /// <param name="cat"></param>
    public void OnCatClick(Cat cat)
    {

        if (selectedCatList.Contains(cat))
        {
            int tempSelectedCount = selectedCatList.Count;
            if (tempSelectedCount >= 12)
            {
                GameUI.Instance.ShowFantasic();
            }
            else if (tempSelectedCount >= 10)
            {
                GameUI.Instance.ShowExcellent();
            }
            else if (tempSelectedCount >= 8)
            {
                GameUI.Instance.ShowCool();
            }
            else if (tempSelectedCount >= 6)
            {
                GameUI.Instance.ShowGood();
            }
            //如果已经变脸的猫再次点击了 就应该进行消除
            for (int i = 0; i < selectedCatList.Count; i++)
            {
             
                GameUI.Instance.PlayNum(selectedCatList[i].transform.position, ScoreController.Instance.ScoreWithStarNumber(tempSelectedCount));
                ClearCat(selectedCatList[i]);
              
            }
            selectedCatList.Clear();
            GameUI.Instance.SetTip(selectedCatList.Count);
            //进行空格填充
            StartCoroutine(Rearrangeall());
        }
        else
        {
            ///第一次点击 显示背景  切换猫脸
            ClearSelectedCatList();

            //查找到类型相同的
            FindFriend(cat);
            //跟新提示
            GameUI.Instance.SetTip(selectedCatList.Count);
            for (int i = 0; i < selectedCatList.Count; i++)
            {
                selectedCatList[i].UpdateSelectedFrame(true);
                selectedCatList[i].GetComponent<SpriteRenderer>().sprite = catOnList[(int)selectedCatList[i].type];
            }
            if (selectedCatList.Count > 0)
            {
                SoundController.Instance.PlayEffectSelectStar();
            }
        }

    }
    /// <summary>
    /// 清楚所有并奖励
    /// </summary>
    IEnumerator ClearAllAndReward()
    {
        yield return new WaitForSeconds(1.5f);
        List<Cat> tempCatList = new List<Cat>();//获得屏幕中所剩的猫猫
        for (int rowIndex = 0; rowIndex < 10; rowIndex++)
        {

            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {
                Cat cat = GetCat(rowIndex, columnIndex);
                if (cat != null)
                {
                    tempCatList.Add(cat);
                }
            }

        }
        //计算奖励
        int catCount=tempCatList.Count;
     
        int tempCount=0;
        GameUI.Instance.SetRewardActive(true);
        for (int i = 0; i < tempCatList.Count; i++)
        {
            tempCount++;
            GameUI.Instance.SetRewardTip(catCount,tempCount);
            yield return new WaitForSeconds(0.1f);

            ClearCat(tempCatList[i]);
        }
        GameUI.Instance.SetRewardActive(false);
        int reward = ScoreController.Instance.BonusScoreWithRemain(catCount);
        GameUI.Instance.AddScore(reward);
        //  进入下一关
        if (GameUI.Instance.IsStage())
        {
            StartCoroutine(GoToNextStage());
            print("进入下一关");
        }
        else
        {
            SoundController.Instance.PlayEffectGameOver();
            GameUI.Instance.ActiveGameOver(true);
            print("挑战失败");
        }
    }
    /// <summary>
    /// 进入下一关
    /// </summary>
    IEnumerator GoToNextStage()
    {
       
        yield return new WaitForSeconds(1f);
      
        GameUI.Instance.AddStage();
     
       StartCoroutine( CreateCat());
    }
    void ClearCat(Cat cat)
    {
        SoundController.Instance.PlayEliminateEffect1();
        RemoveCat(cat.rowIndex, cat.columnIndex);
        //播放特效
        PlayCatParticle(cat.transform.position, cat.type);

        Destroy(cat.gameObject);

    }
    /// <summary>
    /// 看是否OVER
    /// </summary>
    bool IsOver()
    {
        for (int rowIndex = 0; rowIndex < 10; rowIndex++)
        {

            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {
                Cat cat = GetCat(rowIndex, columnIndex);
                if (cat != null)
                {
                   isHaveFriendList.Clear();
                   HaveFriend(cat);
                   if (isHaveFriendList.Count > 0)
                    {
                        //如果还能找到可以消除的
                        return false;

                    }
                }
            }

        }
        return true;

    }
    /// <summary>
    ///重置上一次的状态
    /// </summary>
    private void ClearSelectedCatList()
    {
        for (int i = 0; i < selectedCatList.Count; i++)
        {
            selectedCatList[i].UpdateSelectedFrame(false);

            selectedCatList[i].GetComponent<SpriteRenderer>().sprite = catList[(int)selectedCatList[i].type];
        }
        selectedCatList.Clear();
    }
    /// <summary>
    /// 重新排列
    /// </summary>
    IEnumerator Rearrangeall()
    {
        float dropTime = 0;//记录所有毛下落完所花的时间
        yield return new WaitForSeconds(0.3f);
        //先查看所有列看是否有空格 有就缩进
        for (int rowIndex = 0; rowIndex < 10; rowIndex++)
        {
            int num = 0;
            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {

                Cat cat = GetCat(rowIndex, columnIndex);
                if (cat == null)
                {
                    num++;//记录空格的个数
                }
                else
                {
                    if (num > 0)
                    {
                   
                        //根据上面的空格进行缩进
                        int newColumnIndex = cat.columnIndex - num;
                        float tempTime = 0.2f * num;
                        dropTime = Mathf.Max(dropTime, tempTime);
                        cat.UpdatePostionBack(cat.rowIndex, newColumnIndex, tempTime);
                        //维护字典表中对应的值
                        SetCat(cat.rowIndex, newColumnIndex, cat);
                        //清空原有位置的值
                        RemoveCat(rowIndex, columnIndex);
                    }
                }
            }

        }
        //在扫描所有行 有空格就缩进
        int rowNum = 0;
        for (int rowIndex = 0; rowIndex < 10; rowIndex++)
        {
            int num = 0;
            for (int columnIndex = 0; columnIndex < 10; columnIndex++)
            {

                Cat cat = GetCat(rowIndex, columnIndex);
                if (cat == null)
                {
                    num++;//记录空格的个数
                }

            }
            if (num >= 10)
            {
                //如果该列都没有了 记录下
                rowNum++;
            }
            else
            {
                //后面的一列往前挪
                if (rowNum > 0)
                {
                    for (int columnIndex = 0; columnIndex < 10; columnIndex++)
                    {
                        SoundController.Instance.PlayEffectFall();
                        Cat cat = GetCat(rowIndex, columnIndex);
                        if (cat != null)
                        {
                            int newRowIndex = cat.rowIndex - rowNum;
                            cat.UpdatePostionSine(newRowIndex, cat.columnIndex, 0.3f, dropTime);
                            //维护字典表中对应的值
                            SetCat(newRowIndex, cat.columnIndex, cat);
                            //清空原有位置的值
                            RemoveCat(rowIndex, columnIndex);
                        }

                    }

                }

            }
        }

        //  已经没有可以消除的猫了IsOver()
        if (IsOver())
        {
            //  计算奖励
            StartCoroutine(ClearAllAndReward());
        }
    }


    /// <summary>
    /// 找到周围四个点
    /// </summary>
void  FindFriend(Cat c)
    {
        List<Cat> temp = new List<Cat>();
        int rowIndex = c.rowIndex;
        int columnIndex = c.columnIndex;
        CatType type = c.type;

        Cat upCat = GetCat(rowIndex, columnIndex + 1);
        if (upCat != null)
        {
            temp.Add(upCat);
        }
        //下

        Cat downCat = GetCat(rowIndex, columnIndex - 1);
        if (downCat != null)
        {
            temp.Add(downCat);
        }
        //左

        Cat leftCat = GetCat(rowIndex - 1, columnIndex);
        if (leftCat != null)
        {
            temp.Add(leftCat);
        }
        //右

        Cat rightCat = GetCat(rowIndex + 1, columnIndex);
        if (rightCat != null)
        {
            temp.Add(rightCat);
        }
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].type == type)
            {
                //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                if (!selectedCatList.Contains(temp[i]))
                {

                    selectedCatList.Add(temp[i]);
                    FindFriend(temp[i]);
                }
            }
        }
        
    }
    /// <summary>
    /// 找到周围四个点
    /// </summary>
    void  HaveFriend(Cat c)
    {
        List<Cat> temp = new List<Cat>();
        int rowIndex = c.rowIndex;
        int columnIndex = c.columnIndex;
        CatType type = c.type;

        Cat upCat = GetCat(rowIndex, columnIndex + 1);
        if (upCat != null)
        {
            temp.Add(upCat);
        }
        //下

        Cat downCat = GetCat(rowIndex, columnIndex - 1);
        if (downCat != null)
        {
            temp.Add(downCat);
        }
        //左

        Cat leftCat = GetCat(rowIndex - 1, columnIndex);
        if (leftCat != null)
        {
            temp.Add(leftCat);
        }
        //右

        Cat rightCat = GetCat(rowIndex + 1, columnIndex);
        if (rightCat != null)
        {
            temp.Add(rightCat);
        }
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].type == type)
            {
                //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                if (!isHaveFriendList.Contains(temp[i]))
                {

                    isHaveFriendList.Add(temp[i]);
                    HaveFriend(temp[i]);
                }
            }
        }
      
    }

    void FindFriends(Cat c)
    {
        int rowIndex = c.rowIndex;
        int columnIndex = c.columnIndex;
        CatType type = c.type;


        //上
        Dictionary<int, Cat> up;
        catDic.TryGetValue(rowIndex, out up);
        if (up != null)
        {
            Cat cat;
            up.TryGetValue(columnIndex + 1, out cat);
            if (cat != null)
            {
                if (cat.type == type)
                {
                    //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                    if (!selectedCatList.Contains(cat))
                    {

                        selectedCatList.Add(cat);
                        FindFriend(cat);
                    }
                }

            }
        }
        //下
        Dictionary<int, Cat> down;
        catDic.TryGetValue(rowIndex, out down);
        if (up != null)
        {
            Cat cat;
            down.TryGetValue(columnIndex - 1, out cat);
            if (cat != null)
            {
                if (cat.type == type)
                {
                    //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                    if (!selectedCatList.Contains(cat))
                    {

                        selectedCatList.Add(cat);
                        FindFriend(cat);
                    }
                }
            }
        }
        //左
        Dictionary<int, Cat> left;
        catDic.TryGetValue(rowIndex - 1, out left);
        if (left != null)
        {
            Cat cat;
            left.TryGetValue(columnIndex, out cat);
            if (cat != null)
            {
                if (cat.type == type)
                {
                    //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                    if (!selectedCatList.Contains(cat))
                    {

                        selectedCatList.Add(cat);
                        FindFriend(cat);
                    }
                }
            }
        }
        //右
        Dictionary<int, Cat> right;
        catDic.TryGetValue(rowIndex + 1, out right);
        if (right != null)
        {
            Cat cat;
            right.TryGetValue(columnIndex, out cat);
            if (cat != null)
            {
                if (cat.type == type)
                {
                    //如果不包含就添加  这个地方得判断 不然会堆栈溢出
                    if (!selectedCatList.Contains(cat))
                    {

                        selectedCatList.Add(cat);
                        FindFriend(cat);
                    }
                }

            }
        }


    }
    /// <summary>
    /// 播放猫的粒子特效
    /// </summary>
    void PlayCatParticle(Vector3 pos, CatType type)
    {
        GameObject go = GameObject.Instantiate(catParticle) as GameObject;
        go.transform.parent = Cats;
        go.transform.position = pos;
        go.particleSystem.renderer.material.mainTexture = catList[(int)type].texture;
        go.GetComponent<ParticleSystem>().Play();
        Destroy(go, 1f);
    }

}

/// <summary>
/// 猫的类型
/// </summary>
public enum CatType
{
    Red = 0, //红
    Yellow, //黄色
    Blue,//蓝色
    Green,//绿色
    Purple  //紫色

}