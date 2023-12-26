using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseChapter : MonoBehaviour
{
    public void ChangeToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
