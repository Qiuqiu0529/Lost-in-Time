using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    public Save save;
    
    public int Loadchapter = 0;
    public int nowChapter = 0;
    public Vector3[] PlayerStartPos;
    public int TotalScheneCount = 4;

    public void SaveGameData()//本地化save数据
    {
        if (!Directory.Exists(Application.persistentDataPath + "/gameSave"))
            Directory.CreateDirectory(Application.persistentDataPath + "/gameSave");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave/chapter.txt");
        Debug.Log("SaveData");
        var json = JsonUtility.ToJson(save);
        formatter.Serialize(file,json);
    }

    public void LoadSaveData()//读取save数据
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/gameSave/chapter.txt"))
        {
            Debug.Log("LoadSaveData()");
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave/chapter.txt",FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)binaryFormatter.Deserialize(file), save);
            file.Close();
            Loadchapter = save.chapterdata;
            Debug.Log(Loadchapter);
        }
    }

    public void ReloadScene()
    {
        StartCoroutine(Reload());
    }

    public void NextChapter()
    {
        Player.instance.StartCoroutine(Player.instance.StartcoolDown(2f));
        StartCoroutine(NextScene());
    }

    public void ContinueGmae()//继续游戏回到上一个到达的场景。
    {
        LoadSaveData();
       
        if (Loadchapter < TotalScheneCount)
        {
            nowChapter = Loadchapter;
            SceneManager.LoadScene(Loadchapter);
            Debug.Log("continuegame");
        }
    }

    public IEnumerator Reload(float time = 1.5f)
    {
        GameObject item = GameObject.FindGameObjectWithTag("ExitScene");
        item.GetComponent<ActiveItem>().Active();
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public IEnumerator NextScene(float time = 1.5f)
    {
        GameObject item = GameObject.FindGameObjectWithTag("ExitScene");
        item.GetComponent<ActiveItem>().Active();
        
        yield return new WaitForSeconds(time);
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index < TotalScheneCount)
        {
            nowChapter++;
            Loadchapter = index;
            save.onlyStart = true;
            SaveGameData();
            SceneManager.LoadScene(index);
            Debug.Log("下一关" +
                 "");
        }
        else
        {
            Debug.Log("结束游戏");
        }
    }

    public void StartGame()
    {
        nowChapter = 1;
        Loadchapter = 1;
        save.onlyStart = true;//不读取上一次的数据
        SaveGameData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
