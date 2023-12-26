using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MyVideo : MonoBehaviour
{
    public VideoPlayer vPlayer;
    public GameObject rawImage;
    private double totaltime;         //��Ƶ��ʱ��
    private float skiptime = 10;   //���or���˵�ʱ��

    public GameObject BG;
    bool first;


    private void Awake()
    {
        vPlayer = GetComponent<VideoPlayer>();
        rawImage = this.gameObject;
        vPlayer.loopPointReached += EndReached;
        totaltime = vPlayer.length;
        vPlayer.Play();
        Debug.Log("PlayVideo");
        StartCoroutine(Wait());
       
    }

    void EndReached(VideoPlayer vPlayer)
    {
        rawImage.SetActive(false);
    }

    //�ر���Ƶ
    public void close()
    {
        Debug.Log("PlayVideoEND");
        vPlayer.Stop();
        rawImage.SetActive(false);
    }

    //����or��ͣ
    public void startandpause()
    {
        if (vPlayer.isPaused == true)
        {
            vPlayer.Play();
        }
        else if (vPlayer.isPlaying == true)
        {
            vPlayer.Pause();
        }
    }

    //���ٲ���
    public void playspeed(int value)
    {
        float speed = 1;
        switch (value)
        {
            case 0:
                speed = 1;
                break;
            case 1:
                speed = 0.5f;
                break;
            case 2:
                speed = 1.5f;
                break;
            case 3:
                speed = 2.0f;
                break;
        }
        vPlayer.playbackSpeed = speed;
    }

    //���
    public void next()
    {
        vPlayer.time += skiptime;
    }

    //����
    public void last()
    {
        vPlayer.time -= skiptime;
    }

    public IEnumerator Wait()
    {

        yield return new WaitForSeconds(1f);
        Debug.Log("Wait BG.");
        BG.SetActive(false);
    }
}
