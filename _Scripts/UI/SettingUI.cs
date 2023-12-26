using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SettingUI : MonoBehaviour
{
    public Slider Music, Sound;
    public SaveSetting saveSetting;

    private void Start()
    {
        LoadSaveData();
    }

    public void SetMusic()
    {
        SoundManager.SetMusic(Music.value);
    }
    public void SetSound()
    {
       
        SoundManager.SetSound(Sound.value);
    }

    public void SaveGameData()//本地化save数据
    {
        if (!Directory.Exists(Application.persistentDataPath + "/gameSave"))
            Directory.CreateDirectory(Application.persistentDataPath + "/gameSave");
        BinaryFormatter formatter = new BinaryFormatter();
        saveSetting.musicValue = Music.value;
        saveSetting.soundValue = Sound.value;
        var json = JsonUtility.ToJson(saveSetting);
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave/seting.txt");
        Debug.Log("seting");
        
        formatter.Serialize(file, json);
    }

    public void LoadSaveData()//读取save数据
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/gameSave/seting.txt"))
        {
            Debug.Log("LoadSaveData()");
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave/seting.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)binaryFormatter.Deserialize(file), saveSetting);
            file.Close();
            SoundManager.instance.soundValue = saveSetting.soundValue;
            Music.value = saveSetting.musicValue;
            Sound.value = saveSetting.soundValue;
            SetMusic();
            SetSound();
        }
    }
}
