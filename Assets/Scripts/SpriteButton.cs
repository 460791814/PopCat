using UnityEngine;
using System.Collections;

public class SpriteButton : MonoBehaviour {
    private Vector3 oldPos;
    public  Sprite openSprite;
    public Sprite closeSprite;
    void Awake()
    {
        oldPos = transform.localScale;
        if (openSprite == null)
        {
            openSprite = this.GetComponent<SpriteRenderer>().sprite;
        }
    }
    void Start()
    {
        if (SoundController.Instance.isOpen)
        {
            this.GetComponent<SpriteRenderer>().sprite = openSprite;
         
        }else{
         this.GetComponent<SpriteRenderer>().sprite = closeSprite;
        }
    }
    //鼠标按下  
    void OnMouseDown()
    {
        float f = 0.8f;
        transform.localScale=new Vector3(transform.localScale.x*f,transform.localScale.y*f,transform.localScale.z*f);
        SoundController.Instance.PlayEffectButton();
    }
    void OnMouseUp()
    { 
        //切换声音
        transform.localScale = oldPos;
        if (closeSprite != null)
        {
           
            if (this.GetComponent<SpriteRenderer>().sprite == openSprite)
            {
                this.GetComponent<SpriteRenderer>().sprite = closeSprite;
                GameMnager.Instance.OpenOrCloseSound(false);
            }
            else {
                this.GetComponent<SpriteRenderer>().sprite = openSprite;
                GameMnager.Instance.OpenOrCloseSound(true);
            }
        }
    }

    public void OnMouseExit()
    {

    }
    
}
