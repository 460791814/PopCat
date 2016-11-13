using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
 

public class ImageLabel : MonoBehaviour {


    private string _text;
    public  string Text;
    public List<Sprite> list;
    private Vector3 old;
 
	// Use this for initialization
	void Start () {
        _text = Text;
        old = transform.localPosition;
	}
    void ShowScore()
    {

        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        if (_text != null && _text != "")
        {
            char[] charArray = _text.ToCharArray();
            float l = 0;
            float ttt = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                int num = int.Parse(charArray[i].ToString()) ;
                GameObject go = new GameObject();
                go.AddComponent<SpriteRenderer>().sprite = list[num];
             
                go.name = num.ToString();
               go.transform.parent = this.transform;
               go.transform.localPosition = new Vector3(l + list[num].rect.width / 200, 0, 0);
               go.transform.localScale = new Vector3(1,1,1);
               l += list[num].rect.width / 100;
               ttt += list[num].rect.width / 200;
            }
            transform.localPosition = new Vector3(old.x - ttt+ttt/10, old.y, old.z);
        }
     }
	// Update is called once per frame
	void Update () {
        if (_text != Text)
        {
            _text = Text;
            ShowScore();
        }
	}
}
