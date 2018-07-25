using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{ 
    public CircuitButton ClickButton { get; set; }

    private Circuit selectedCircuit;

    private Power selectedPower;

    private float realRotation;

    public double winValue = 1;
    public int win = 0;

    public int step = 0;
    public int reset = 0;

    [SerializeField]
    private Image winMenu;

    [SerializeField]
    private Image winButton;

    [SerializeField]
    private Image loseMenu;

    [SerializeField]
    private Image loseButton;

    [SerializeField]
    private GameObject textbutton;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private Text statstext;



    private readonly WaitForSeconds m_skipFrame = new WaitForSeconds(0.01f);

    TileScript tileScript;

    private GameObject myCircuit;
    private GameObject myCircuittop;
    private GameObject myCircuitright;

    // Use this for initialization
    void Start () {
        winMenu.enabled = false;
        winButton.enabled = false;
        loseMenu.enabled = false;
        loseButton.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        HandleEscape();
	}

    public void PickCircuit(CircuitButton circuitButton)
    {
        this.ClickButton = circuitButton;
        Hover.Instance.Activate(circuitButton.Sprite);
    }

    public void clearbutton()
    {
        Hover.Instance.Deactivate();
    }


    public void SelectCircuit(Circuit circuit)
    {
        selectedCircuit = circuit;
        selectedCircuit.Select();
    }

    public void SelectPower(Power power)
    {
        selectedPower = power;
        selectedPower.Select();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void winmenu()
    {

        winMenu.enabled = true;
        winButton.enabled = true;
        StartCoroutine(Fade(winMenu,winButton));
    }

     IEnumerator Fade (Image image1, Image image2){

        Color c = image1.color;
        while (c.a <= 1)
        {
            c.a += Time.deltaTime/1;
            image1.color = c;
            image2.color = c;
            yield return m_skipFrame;
        }
        
        }

    public void debutton()
    {
        textbutton.SetActive(false);
    }

    public void losemenu()
    {
        loseMenu.enabled = true;
        loseButton.enabled = true;
        StartCoroutine(Fade(loseMenu, loseButton));
    }

    public void Showstats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetTooltipText(string text)
    {
        statstext.text = text;
    }

    public void re()
    {
        loseMenu.enabled = false;
        loseButton.enabled = false;
        Color c = loseMenu.color;
        c.a = 0;
        loseMenu.color = c;
        loseButton.color = c;
        TileScript.Instance.Reset();
        Timer.Instance.Resettimr();
    Timer.Instance.keepTiming = true;
    }

}
