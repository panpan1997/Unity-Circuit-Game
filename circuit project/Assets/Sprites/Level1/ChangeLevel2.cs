using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel2 : MonoBehaviour {

    public void change(string scenename)
    {
        SceneManager.LoadSceneAsync(scenename);
    }
}
