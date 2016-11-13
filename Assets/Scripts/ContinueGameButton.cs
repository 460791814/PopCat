using UnityEngine;
using System.Collections;

public class ContinueGameButton : MonoBehaviour {
    public float scaleOffer = 0.9f;
    private Vector3 initTransformScale;
    // Use this for initialization
    void Awake()
    {
        initTransformScale = transform.localScale;
    }
    //鼠标按下  
    void OnMouseDown()
    {

        transform.localScale = new Vector3(transform.localScale.x * scaleOffer, transform.localScale.y * scaleOffer, transform.localScale.z * scaleOffer);
        SoundController.Instance.PlayEffectButton();
    }
    void OnMouseUp()
    {
        transform.localScale = initTransformScale;
        GameMnager.Instance.ContinueGame();
    }


}
