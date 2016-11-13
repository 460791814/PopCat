using UnityEngine;
using System.Collections;

public class Num : MonoBehaviour {
    private TextMesh txt;
    void Awake()
    {
     txt=GetComponent<TextMesh>();
    }
    public void MoveTo(Vector3 target, int num)
    {

        txt.text = num.ToString();
        Hashtable hash = new Hashtable();
        hash.Add("position", target);
        hash.Add("oncomplete", "MoveToTarget");
        hash.Add("oncompleteparams", num);

        hash.Add("easeType", iTween.EaseType.easeInBack);//iTween.EaseType.easeInExpo
        hash.Add("time", Random.Range(1f,1.5f));
        iTween.MoveTo(this.gameObject, hash);
    }
    void MoveToTarget(int  num)
    {
        GameUI.Instance.AddScore(num);
         Destroy(this.gameObject);
    }
}
