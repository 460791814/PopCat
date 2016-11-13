using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    private static SoundController _instance;
  
    public static SoundController Instance
    {
        get { return SoundController._instance; }
        set { SoundController._instance = value; }
    }

    public float volume;//声音大小
    public  bool isOpen = true;

    public AudioClip effectButton;
    public AudioClip effectFall;
    public AudioClip effectGameOver;
    public AudioClip effectGood;
    public AudioClip effectLevelCleared;
    public AudioClip effectReplace;
    public AudioClip effectSelectStar;
    public AudioClip eliminateEffect1;
    public AudioClip eliminateEffect2;
    public AudioClip eliminateEffect3;
    public AudioClip eliminateEffect4;
    public AudioClip eliminateEffect5;
    public AudioClip eliminateEffect6;
    public AudioClip eliminateEffect7;
    public AudioClip eliminateEffect8;
    public AudioClip eliminateEffect9;
    public AudioClip eliminateEffect10;
    public AudioClip eliminateEffect11;
    public AudioClip eliminateEffect12;
    public AudioClip eliminateEffect13;
    public AudioClip eliminateEffect14;

	void Awake () {
        _instance = this;
	}
    private float time = 0;
    private bool isPlayFall = false;
    void Update()
    {
        if (isPlayFall == false)
        {
            time += Time.deltaTime;
            if (time > 0.6f)
            {
                isPlayFall = true;
            }
        }
    }
    public void PlayEffectButton()
    {
        PlaySound(effectButton);
    }
    public void PlayEffectFall()
    {
        if (isPlayFall)
        {
            PlaySound(effectFall);
            isPlayFall = false;
        }
    }
    public void PlayEffectGameOver()
    {
        PlaySound(effectGameOver);
    }
    public void PlayEffectGood()
    {
        PlaySound(effectGood);
    }
    public void PlayEffectLevelCleared()
    {
        PlaySound(effectLevelCleared);

    }
    public void PlayEffectReplace()
    {
        PlaySound(effectReplace);

    }
    public void PlayEffectSelectStar()
    {
        PlaySound(effectSelectStar);

    }
    public void PlayEliminateEffect1()
    {
        PlaySound(eliminateEffect1);
    }
    public void PlayEliminateEffect2()
    {
        PlaySound(eliminateEffect2);
    }
    public void PlayEliminateEffect3()
    {
        PlaySound(eliminateEffect3);
    }
    public void PlayEliminateEffect4()
    {
        PlaySound(eliminateEffect4);
    }
    public void PlayEliminateEffect5()
    {
        PlaySound(eliminateEffect5);
    }
    public void PlayEliminateEffect6()
    {
        PlaySound(eliminateEffect6);
    }
    public void PlayEliminateEffect7()
    {
        PlaySound(eliminateEffect7);
    }
    public void PlayEliminateEffect8()
    {
        PlaySound(eliminateEffect8);
    }
    public void PlayEliminateEffect9()
    {
        PlaySound(eliminateEffect9);
    }
    public void PlayEliminateEffect10()
    {
        PlaySound(eliminateEffect10);
    }
    public void PlayEliminateEffect11()
    {
        PlaySound(eliminateEffect11);
    }
    public void PlayEliminateEffect12()
    {
        PlaySound(eliminateEffect12);
    }
    public void PlayEliminateEffect13()
    {
        PlaySound(eliminateEffect13);
    }
    public void PlayEliminateEffect14()
    {
        PlaySound(eliminateEffect14);
    }

    void PlaySound(AudioClip clip)
    {
        if (isOpen)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
        //audio.PlayOneShot(clip);//不能同时播放多个声音  这里同一段声音可能会有交叉 这里不能使用
    }
   
}
