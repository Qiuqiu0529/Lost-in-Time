using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text myDia;
    public int index;
    List<string> textList = new List<string>();
    public GameObject diaImage;
    private CanvasGroup canvasGroup;
    public float textSpeed;

    public static int maxSize = 80;

    public Slider speedSet;

    public Text SpeekerName;
    public string speeker;


    public static DialogueSystem instance;

    void Awake()
    {
        //canvasGroup = diaImage.GetComponent<CanvasGroup>();
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetSpeed()
    {
        textSpeed = speedSet.value;
    }
    public void GetTextFromFile(TextAsset file)
    {
        Debug.Log("Startdia");
        SpeekerName.text = "";
        diaImage.SetActive(true);
        //canvasGroup.alpha = 1f;
        textList.Clear();
        index = 0;
        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
        Print();
    }

    /* public void Jump()
     {
         if (index >= textList.Count - 1)
         {
             StopAllCoroutines();
             StartCoroutine(Disappear());
         }
         else if (!diaFinish)
         {
             diaFinish = true;
             StopAllCoroutines();
             StartCoroutine(JumpText());
         }
     }*/
    public void DiaFinished()
    {
        myDia.text = "";
        SpeekerName.text = "";
        Debug.Log("DiaFinish");
        diaImage.SetActive(false);
        index = 0;
    }

    public void Print()
    {
        StartCoroutine(SetTextUi());
    }

    public IEnumerator SetTextUi()
    {
        while (index < textList.Count)
        {
            myDia.text = "";
            if (textList[index][0] == 'A')
            {
                for (int i = 1; i < textList[index].Length; ++i)
                {
                    speeker += textList[index][i];
                }
            }
            else
            {
                for (int i = 0; i < textList[index].Length; ++i)
                {
                    if (i % maxSize == 0)
                    {
                        myDia.text = "";
                    }
                    myDia.text += textList[index][i];
                    yield return new WaitForSeconds(textSpeed);
                }
                yield return new WaitForSeconds(textSpeed * 2 + 0.5f);
            }
            ++index;
        }

    }
    public IEnumerator Disappear()
    {
        /* while (canvasGroup.alpha > 0)
         {
             canvasGroup.alpha -= 0.2f;
             yield return new WaitForSeconds(0.05f);
         }*/
        DiaFinished();
        yield return 0;
    }
}