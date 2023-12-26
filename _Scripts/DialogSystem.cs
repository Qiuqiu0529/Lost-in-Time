using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{
    [Header("UI���")]
    public Text textLabel;
    public Image faceImage;
    public Text textLabelInDialogBox;
    public GameObject panelDialogBox;
    public bool dialogBox;
    public string nextSceneName;


    [Header("�ı��ļ�")]
    public TextAsset textFile;
    public int indexOfLine;
    public float textSpeed = 0.1f;
    

    [Header("ͷ��")]
    public Sprite faceA, faceB, faceC;

    bool textFinished;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        GetTextFormFile(textFile);
    }

    private void OnEnable()
    {
        textFinished = true;

        if(dialogBox == true)
        {
            panelDialogBox.SetActive(false);
        }
        
        StartCoroutine(SetTextUI());
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.anyKeyDown && textFinished)
        {
            
            if(indexOfLine == textList.Count)
            // ���������ı����ţ��л�����
            {
                gameObject.SetActive(false);
                indexOfLine = 0;
                SceneManager.LoadScene(nextSceneName);
            }

            else
            {
               StartCoroutine(SetTextUI());
            }
            
        }
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        indexOfLine = 0;

        var lineData = file.text.Split('\n');

        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";
        

        // ������ڶԻ�
        if (dialogBox == true)
        {
            textLabelInDialogBox.text = "";

            switch (textList[indexOfLine])
            {
                case "A\r":

                    panelDialogBox.SetActive(true);

                    faceImage.sprite = faceA;
                    indexOfLine++;

                    for (int i = 0; i < textList[indexOfLine].Length; i++)
                    {
                        textLabelInDialogBox.text += textList[indexOfLine][i];

                        yield return new WaitForSeconds(textSpeed);
                    }
                    indexOfLine++;

                    break;

                case "B\r":

                    panelDialogBox.SetActive(true);

                    faceImage.sprite = faceB;
                    indexOfLine++;

                    for (int i = 0; i < textList[indexOfLine].Length; i++)
                    {
                        textLabelInDialogBox.text += textList[indexOfLine][i];

                        yield return new WaitForSeconds(textSpeed);
                    }
                    indexOfLine++;

                    break;

                case "C\r":

                    panelDialogBox.SetActive(false);

                    faceImage.sprite = faceC;
                    indexOfLine++;

                    for (int i = 0; i < textList[indexOfLine].Length; i++)
                    {
                        textLabel.text += textList[indexOfLine][i];

                        yield return new WaitForSeconds(textSpeed);
                    }
                    indexOfLine++;

                    break;
            }
        }

        // ��������ڶԻ���ֻ�к�������
        else
        {
            
            for (int i = 0; i < textList[indexOfLine].Length; i++)
            {
                textLabel.text += textList[indexOfLine][i];

                yield return new WaitForSeconds(textSpeed);
            }
            indexOfLine++;
        }

       
        textFinished = true;       
    }

}
