﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel3 : MonoBehaviour {

    public void change(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
