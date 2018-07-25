using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitButton : MonoBehaviour {

    [SerializeField]
    private GameObject circuitPrefab;

    [SerializeField]
    private Sprite sprite;

    public GameObject CircuitPrefab
    {
        get
        {
            return circuitPrefab;
        }

    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public void ShowInfo(string type)
    {
        GameManager.Instance.SetTooltipText(type);
        GameManager.Instance.Showstats();
    }
}
