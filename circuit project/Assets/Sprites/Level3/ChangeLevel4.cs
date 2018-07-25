using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel4 : MonoBehaviour {

    public void change(string level)
    {
        SceneManager.LoadScene(level);
    }
}
