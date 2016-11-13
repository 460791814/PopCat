using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

    public int rowIndex = 0;
    public int columnIndex = 0;
    public CatType type;
    private GameObject selectedFrame;
	// Use this for initialization
	void Awake () {
        selectedFrame = transform.Find("SelectedFrame").gameObject;
	}
	


    public void UpdatePostion(int rowIndex, int columnIndex)
    {
      
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        Vector3 targetPos = new Vector3(rowIndex * 72 + 72 / 2, columnIndex * 72 + 72 / 2, 0) / 100;

        this.transform.localPosition = targetPos;
   

    }
    public void UpdatePostion(int rowIndex, int columnIndex,float time)
    {
      
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        Vector3 targetPos = new Vector3(rowIndex * 72 + 72 / 2, columnIndex * 72 + 72 / 2, 0) / 100;

        this.transform.localPosition = new Vector3(targetPos.x,targetPos.y+12.8f,targetPos.z);
        Hashtable hash = new Hashtable();
        hash.Add("position", targetPos + transform.parent.position);
        hash.Add("oncomplete", "SetToTarget");
       hash.Add("oncompleteparams", targetPos);

       hash.Add("easeType", iTween.EaseType.easeInSine);
        hash.Add("time", time);
       iTween.MoveTo(this.gameObject,hash);
      
     
    }
    public void UpdatePostionBack(int rowIndex, int columnIndex,float t)
    {

        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        Vector3 targetPos = new Vector3(rowIndex * 72 + 72 / 2, columnIndex * 72 + 72 / 2, 0) / 100;

     //   iTween.MoveTo(this.gameObject,iTween.Hash("time",0.05f,"position",transform.localPosition+transform.parent.position+new Vector3(0,1,0)));
         
        Hashtable hash = new Hashtable();
        hash.Add("position", targetPos + transform.parent.position);
        hash.Add("oncomplete", "SetToTarget");
        hash.Add("oncompleteparams", targetPos);
       // hash.Add("delay", 0.05f);
        hash.Add("easeType", iTween.EaseType.easeInBack);
         hash.Add("time", t);
        iTween.MoveTo(this.gameObject, hash);


    }
    public void UpdatePostionSine(int rowIndex, int columnIndex, float time,float delayTime)
    {

        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        Vector3 targetPos = new Vector3(rowIndex * 72 + 72 / 2, columnIndex * 72 + 72 / 2, 0) / 100;


        Hashtable hash = new Hashtable();
        hash.Add("position", targetPos + transform.parent.position);
        hash.Add("oncomplete", "SetToTarget");
        hash.Add("oncompleteparams", targetPos);
        hash.Add("delay", delayTime);
        hash.Add("easeType", iTween.EaseType.easeInSine);
        hash.Add("time", time);
        iTween.MoveTo(this.gameObject, hash);


    }
    void SetToTarget(Vector3 targetPos)
    {
        SoundController.Instance.PlayEffectFall();
        this.transform.localPosition = targetPos;
    }
 
    /// <summary>
    /// 显示隐藏白色背景
    /// </summary>
    /// <param name="isSelected"></param>
    public void UpdateSelectedFrame(bool isSelected)
    {
        selectedFrame.SetActive(isSelected);
        if (isSelected)
        {
            Hashtable hash = new Hashtable();
            hash.Add("position", transform.position + new Vector3(0, 0.1f, 0));
            hash.Add("oncomplete", "PingPong");

            hash.Add("easeType", iTween.EaseType.easeInSine);
            hash.Add("time", 0.1f);
            iTween.MoveTo(this.gameObject, hash);
        }
 
    }
    void PingPong()
    {
        Hashtable hash = new Hashtable();
        hash.Add("position", transform.position - new Vector3(0, 0.1f, 0));
      
        hash.Add("easeType", iTween.EaseType.easeInSine);
        hash.Add("time", 0.1f);
        iTween.MoveTo(this.gameObject, hash);
    }
    //鼠标按下  
    void OnMouseDown()
    {
     
        GameController.Instance.OnCatClick(this);
    }
}

