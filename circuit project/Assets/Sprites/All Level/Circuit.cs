using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour {

    int rotation;
    private SpriteRenderer mySpriteRender;
    public int[] values;
    public float speed;
    public float value;
    public int x;
    public int y;
    public int resistor;
    public bool connect = false;

	// Use this for initialization
	void Start () {
        mySpriteRender = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(mySpriteRender.transform.root.eulerAngles.z != rotation)
        {
            mySpriteRender.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (0, 0, rotation), speed);
        }

	}

    public void Select()
    {
        rotation += 90;

        if(rotation == 90)
        {
            mySpriteRender.transform.rotation = Quaternion.Euler(0, 0, rotation);
            mySpriteRender.transform.position = new Vector3(mySpriteRender.transform.position.x,mySpriteRender.transform.position.y -2 , mySpriteRender.transform.position.z);
            changeindex();
        }

        if (rotation == 180)
        {
            mySpriteRender.transform.rotation = Quaternion.Euler(0, 0, rotation);
            mySpriteRender.transform.position = new Vector3(mySpriteRender.transform.position.x + 2, mySpriteRender.transform.position.y, mySpriteRender.transform.position.z);
            changeindex();
        }

        if (rotation == 270)
        {
            mySpriteRender.transform.rotation = Quaternion.Euler(0, 0, rotation);
            mySpriteRender.transform.position = new Vector3(mySpriteRender.transform.position.x, mySpriteRender.transform.position.y + 2, mySpriteRender.transform.position.z);
            changeindex();
        }

        if (rotation == 360)
        {
            mySpriteRender.transform.rotation = Quaternion.Euler(0, 0, rotation);
            mySpriteRender.transform.position = new Vector3(mySpriteRender.transform.position.x - 2, mySpriteRender.transform.position.y , mySpriteRender.transform.position.z);
            rotation = 0;
            changeindex();
        }

    }

    public void changeindex()
    {
        int aux = values[0];

        for (int i = 0; i < values.Length - 1; i++)
        {
            values[i] = values[i + 1];
        }
        values[3] = aux;
    }
}
