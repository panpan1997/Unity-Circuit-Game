using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    private SpriteRenderer mySpriteRender;
    public int[] values;
    public float speed;
    public float value;
    public int win;
    public int x;
    public int y;
    public bool rightvalue = false;
    Animator anim;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Trigger()
    {
        anim.SetTrigger("Light");
    }

    public void Select()
    {
        
    }
}
