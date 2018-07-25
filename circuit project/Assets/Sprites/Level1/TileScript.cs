using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityScript.Lang;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TileScript : Singleton<TileScript>
{

    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; private set; }

    private Color32 fullColor = new Color32(225, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);
    
    private SpriteRenderer spriteRenderer;

    private Circuit myCircuit;

    public Power mypower;

    private int step;

    private int reset;

    public GameObject FloatingTextPrefabs;

    

    public Vector2 WorldPosition
    {
        get
        {
           return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);

        LevelManager.Instance.Tiles.Add(gridPos, this);
        
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickButton != null )
        {
            if (SceneManager.GetActiveScene().name.ToString() == "Level4")
            {
                if (GridPosition.X == 2 && GridPosition.Y == 3) { }
                else if(GridPosition.X == 2 && GridPosition.Y == 2) { }

                else
                {
                    if (myCircuit == null)
                    {
                        ColorTile(emptyColor);
                    }
                    if (!IsEmpty && myCircuit != null)
                    {
                        ColorTile(fullColor);
                        if (Input.GetMouseButtonDown(0))
                        {
                            IsEmpty = true;
                            Destroy(myCircuit.transform.gameObject);
                            PlaceTower();


                        }
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {

                        PlaceTower();

                    }
                }
            }
            else
            {

                if (GridPosition.X == 2 && GridPosition.Y == 2)
                {
                }

                else
                {
                    if (myCircuit == null)
                    {
                        ColorTile(emptyColor);
                    }
                    if (!IsEmpty && myCircuit != null)
                    {
                        ColorTile(fullColor);
                        if (Input.GetMouseButtonDown(0))
                        {
                            IsEmpty = true;
                            Destroy(myCircuit.transform.gameObject);
                            PlaceTower();


                        }
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {

                        PlaceTower();

                    }
                }
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickButton == null && Input.GetMouseButtonDown(0))
        {
            if (myCircuit != null)
            {
                GameManager.Instance.SelectCircuit(myCircuit);

            }
            else
            {

            }
        }
        

    }
 
    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {

            GameObject circuit = (GameObject)Instantiate(GameManager.Instance.ClickButton.CircuitPrefab, transform.position, Quaternion.identity);
            circuit.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

            circuit.transform.SetParent(transform);

        double one = 0;
          

            this.myCircuit = circuit.transform.GetComponent<Circuit>();




        for (int i = 0; i < this.myCircuit.values.Length; i++) {
            if (this.myCircuit.values[i] == 1)
            {
                one += 1;
            }
        }

        GameManager.Instance.winValue += one/2;

        myCircuit.x = GridPosition.X;
        myCircuit.y = GridPosition.Y;

        IsEmpty = false;

        ColorTile(Color.white);

        GameManager.Instance.step++;
        step = GameManager.Instance.step;



        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "step/",
             "\"" + step.ToString() + "\""  ));


        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" +  Timer.Instance.seconds.ToString() + "\""));

        GameManager.instance.clearbutton();
    }

    private Point powerp;

    public void Powers()
    {
        powerp = new Point(2, 2);

        GameObject tmp = (GameObject)Instantiate(LevelManager.Instance.PowerPrefeb, LevelManager.Instance.Tiles[powerp].GetComponent<TileScript>().transform.position, Quaternion.identity);


        tmp.transform.SetParent(transform);

        this.mypower = tmp.transform.GetComponent<Power>();

        mypower.x = powerp.X;
        mypower.y = powerp.Y;

        if(SceneManager.GetActiveScene().name.ToString() == "Level4")
        {
            powerp = new Point(2, 3);

            tmp = (GameObject)Instantiate(LevelManager.Instance.PowerPrefeb, LevelManager.Instance.Tiles[powerp].GetComponent<TileScript>().transform.position, Quaternion.identity);


            tmp.transform.SetParent(transform);

            this.mypower = tmp.transform.GetComponent<Power>();

            mypower.x = powerp.X;
            mypower.y = powerp.Y;
        }

    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public void checkwin()
    {
        
        //find out all game object 
        GameObject[] objcircuit = GameObject.FindGameObjectsWithTag("circuit");
        GameObject[] objbattery = GameObject.FindGameObjectsWithTag("battery");
        GameObject[] objpower = GameObject.FindGameObjectsWithTag("power");

        //Put gameobject into their class seperately
        Circuit[] circuitarray = GetCircuitArray(objcircuit);
        Circuit[] batteryarray = GetBatteryArray(objbattery);
        Power[] powerarray = GetPowerArray(objpower);

        //check for battery 
        for (int i = 0; i < batteryarray.Length; i++)
        {

            //check there are battery near together
            for (int w = 0; w < batteryarray.Length; w++)
            {
                //check right
                if (batteryarray[i].y == batteryarray[w].y && (batteryarray[i].x) + 1 == batteryarray[w].x)
                {
                    if (batteryarray[i].values[1] == 1 && batteryarray[w].values[3] == 1)
                    {
                        if (batteryarray[i].connect != true && batteryarray[w].connect != true)
                        {
                            batteryarray[i].value += batteryarray[w].value;
                            batteryarray[w].value = batteryarray[i].value;
                            batteryarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                    }
                }

                //check left
                if (batteryarray[i].y == batteryarray[w].y && (batteryarray[i].x) - 1 == batteryarray[w].x)
                {
                    if (batteryarray[i].values[3] == 1 && batteryarray[w].values[1] == 1)
                    {
                        if (batteryarray[i].connect != true && batteryarray[w].connect != true)
                        {
                            batteryarray[i].value += batteryarray[w].value;
                            batteryarray[w].value = batteryarray[i].value;
                            batteryarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                    }
                }
            }

            //check there are circuit near by the battery
            for (int w = 0; w < circuitarray.Length; w++)
            {
                //check right
                if ((batteryarray[i].y == circuitarray[w].y) && (batteryarray[i].x) + 1 == circuitarray[w].x)
                {
                    if (batteryarray[i].values[1] == 1 && circuitarray[w].values[3] == 1)
                    {
                        if (circuitarray[w].resistor != 0)
                        {
                            circuitarray[w].value += (batteryarray[i].value / circuitarray[w].resistor);
                            circuitarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                        else
                        {
                            circuitarray[w].value += batteryarray[i].value;
                            circuitarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                    }
                }

                //check left
                if ((batteryarray[i].y) == circuitarray[w].y && batteryarray[i].x - 1 == circuitarray[w].x)
                {
                    if (batteryarray[i].values[3] == 1 && circuitarray[w].values[1] == 1)
                    {
                        if (circuitarray[w].resistor != 0)
                        {
                            circuitarray[w].value += (batteryarray[i].value / circuitarray[w].resistor);
                            circuitarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                        else
                        {
                            circuitarray[w].value += batteryarray[i].value;
                            circuitarray[w].connect = true;
                            batteryarray[i].connect = true;
                        }
                    }
                }
            }

            //check the circuit connect
            for (int z = 0; z < 5; z++)
            {
                checkwin2(circuitarray, batteryarray);
            }

        }

        //check the power connection
        for (int i = 0; i < powerarray.Length; i++)
        {
            //reset the win condition
            powerarray[i].win = 0;
            for (int w = 0; w < circuitarray.Length; w++)
            {
                //check the right connection
                if ((powerarray[i].y == circuitarray[w].y) && (powerarray[i].x) + 1 == circuitarray[w].x)
                {

                    if (powerarray[i].values[1] == 1 && circuitarray[w].values[3] == 1)
                    {
                        powerarray[i].win++;

                        //check the value is correct or not
                        if (powerarray[i].value == circuitarray[w].value)
                        {
                            powerarray[i].rightvalue = true;
                            if (circuitarray[w].value == 0.09)
                            {
                                powerarray[i].rightvalue = true;
                            }
                        }
                    }
                }
            }

            //check the left condition
            for (int w = 0; w < circuitarray.Length; w++)
            {
                if ((powerarray[i].y) == circuitarray[w].y && powerarray[i].x - 1 == circuitarray[w].x)
                {
                    if (powerarray[i].values[3] == 1 && circuitarray[w].values[1] == 1)
                    {
                        powerarray[i].win++;

                        if (powerarray[i].value == circuitarray[w].value)
                        {
                            powerarray[i].rightvalue = true;
                        }
                    }
                }
            }
        }

        bool ww = false; ;
        for (int i = 0; i < powerarray.Length; i++)
        {
            if(powerarray[i].win == 2 && powerarray[i].rightvalue == true)
            {
                ww = true;
            }
            else
            {
                ww = false;
            }
        }

        //if win condition is true, win
        if (ww == true)
        {
            GameManager.Instance.win = 1;
            Debug.Log("win");
            for (int i = 0; i < powerarray.Length; i++)
            {
                powerarray[i].Trigger();
                StartCoroutine(winmyFunc());

                StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "win/",
"\"" + GameManager.Instance.win.ToString() + "\""));

                StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" + Timer.Instance.seconds.ToString() + "\""));

                Timer.Instance.keepTiming = false;
            }
        }
        else
        {
            //lose
            StartCoroutine(losemyFunc());
            Timer.Instance.keepTiming = false;
        }

        //update the data to firebase
        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" + Timer.Instance.seconds.ToString() + "\""));


    }

    IEnumerator winmyFunc()
    {

        yield return new WaitForSeconds(3);
        GameManager.Instance.winmenu();

    }

    IEnumerator losemyFunc()
    {

        yield return new WaitForSeconds(3);
        GameManager.Instance.losemenu();

    }

    public void checkwin2(Circuit[] circuitarray, Circuit[] batteryarray)
    {
        for (int i = 0; i < circuitarray.Length; i++)
        {
            //check battery and circuit
            for(int w = 0; w < batteryarray.Length;w++)
            {
                //check right
                if (batteryarray[w].y == circuitarray[i].y && (batteryarray[w].x) + 1 == circuitarray[i].x)
                {
                    if (batteryarray[w].values[1] == 1 && circuitarray[i].values[3] == 1)
                    {
                        if (batteryarray[w].connect == true && circuitarray[i].connect == true) { }
                        if (batteryarray[w].connect == true && circuitarray[i].connect == false)
                        {
                                circuitarray[i].value += batteryarray[w].value;
                                circuitarray[i].connect = true;
                        }
                        if (batteryarray[w].connect == false && circuitarray[i].connect == true)
                        {
                                batteryarray[w].value += circuitarray[i].value;
                                batteryarray[w].connect = true;
                        }
                        if (batteryarray[w].connect == false && circuitarray[i].connect == false) { }
                    }
                }

                //check left
                if (batteryarray[w].y == circuitarray[i].y && (batteryarray[w].x) - 1 == circuitarray[i].x)
                {
                    if (batteryarray[w].values[3] == 1 && circuitarray[i].values[1] == 1)
                    {
                        if (batteryarray[w].connect == true && circuitarray[i].connect == true) { }
                        if (batteryarray[w].connect == true && circuitarray[i].connect == false)
                        {
                            circuitarray[i].value += batteryarray[w].value;
                            circuitarray[i].connect = true;
                        }
                        if (batteryarray[w].connect == false && circuitarray[i].connect == true)
                        {
                            batteryarray[w].value += circuitarray[i].value;
                            batteryarray[w].connect = true;
                        }
                        if (batteryarray[w].connect == false && circuitarray[i].connect == false) { }
                    }
                }
            }

            for (int w = 0; w < circuitarray.Length; w++)
            {
                //check right
                if ((circuitarray[i].y == circuitarray[w].y) && (circuitarray[i].x) + 1 == circuitarray[w].x)
                {
                    if (circuitarray[i].values[1] == 1 && circuitarray[w].values[3] == 1)
                    {
                        if(circuitarray[i].connect == true && circuitarray[w].connect== true) { }
                        if (circuitarray[i].connect == true && circuitarray[w].connect == false)
                        {
                            if (circuitarray[w].resistor != 0)
                            {
                                circuitarray[w].value += circuitarray[i].value / circuitarray[w].resistor;
                                circuitarray[w].connect = true;
                            }
                            else
                            {
                                circuitarray[w].value += circuitarray[i].value;
                                circuitarray[w].connect = true;
                            }
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == true)
                        {
                            if (circuitarray[i].resistor != 0)
                            {
                                circuitarray[i].value += circuitarray[w].value / circuitarray[i].resistor;
                                circuitarray[i].connect = true;
                            }
                            else
                            {
                                circuitarray[i].value += circuitarray[w].value;
                                circuitarray[i].connect = true;
                            }
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == false) { }
                    }
                }

                //check top
                if ((circuitarray[i].y)-1 == circuitarray[w].y && (circuitarray[i].x) == circuitarray[w].x)
                {
                    if (circuitarray[i].values[0] == 1 && circuitarray[w].values[2] == 1)
                    {
                        if (circuitarray[i].connect == true && circuitarray[w].connect == true) { }
                        if (circuitarray[i].connect == true && circuitarray[w].connect == false)
                        {
                            if (circuitarray[w].resistor != 0)
                            {
                                circuitarray[w].value += circuitarray[i].value / circuitarray[w].resistor;
                                circuitarray[w].connect = true;
                            }
                            else
                            {
                                circuitarray[w].value += circuitarray[i].value;
                                circuitarray[w].connect = true;
                            }
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == true)
                        {
                            if (circuitarray[i].resistor != 0)
                            {
                                circuitarray[i].value += circuitarray[w].value / circuitarray[i].resistor;
                                circuitarray[i].connect = true;
                            }
                            else
                            {
                                circuitarray[i].value += circuitarray[w].value;
                                circuitarray[i].connect = true;
                            }
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == false) { }
                    }
                }

                //check right again
                if ((circuitarray[i].y == circuitarray[w].y) && (circuitarray[i].x) + 1 == circuitarray[w].x)
                {
                    if (circuitarray[i].values[1] == 1 && circuitarray[w].values[3] == 1)
                    {
                        if (circuitarray[i].connect == true && circuitarray[w].connect == true) { }
                        if (circuitarray[i].connect == true && circuitarray[w].connect == false)
                        {
                            if (circuitarray[w].resistor != 0)
                            {
                                circuitarray[w].value += circuitarray[i].value / circuitarray[w].resistor;
                                circuitarray[w].connect = true;
                            }
                            else
                            {
                                circuitarray[w].value += circuitarray[i].value;
                                circuitarray[w].connect = true;
                            }
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == true)
                        {
                            if (circuitarray[i].resistor != 0)
                            {
                                circuitarray[i].value += circuitarray[w].value / circuitarray[i].resistor;
                                circuitarray[i].connect = true;
                            }
                            else
                            {
                                circuitarray[i].value += circuitarray[w].value;
                                circuitarray[i].connect = true;
                            }   
                        }
                        if (circuitarray[i].connect == false && circuitarray[w].connect == false) { }
                    }
                }
            }
        }
    }

    public Circuit[] GetCircuitArray(GameObject[] objcircuit)
    {
        List<Circuit> circuitlist = new List<Circuit>();
        Circuit[] circuitarray;

        for (int i = 0; i < objcircuit.Length; i++)
        {
            circuitlist.Add(objcircuit[i].transform.GetComponent<Circuit>());
        }

        circuitarray = GetCircuit(circuitlist).ToArray();

        return circuitarray;
    }

    public IEnumerable<Circuit> GetCircuit(IList<Circuit> readings)
    {
        var sortedReadings = readings.OrderBy(x => x.y)
            .ThenBy(x => x.x);

        return sortedReadings;
    }

    public Circuit[] GetBatteryArray(GameObject[] objbattery)
    {
        List<Circuit> batterylist = new List<Circuit>();
        Circuit[] batteryarray;

        for (int i = 0; i < objbattery.Length; i++)
        {
            batterylist.Add(objbattery[i].transform.GetComponent<Circuit>());
        }

        batteryarray = GetBattery(batterylist).ToArray();
        return batteryarray;
    }

    public IEnumerable<Circuit> GetBattery(IList<Circuit> readings)
    {
        var sortedReadings = readings.OrderBy(x => x.y)
            .ThenBy(x => x.x);

        return sortedReadings;
    }

    public Power[] GetPowerArray(GameObject[] objpower)
    {
        List<Power> powerlist = new List<Power>();
        Power[] powerarray;

        for (int i = 0; i < objpower.Length; i++)
        {
            powerlist.Add(objpower[i].transform.GetComponent<Power>());
        }

        powerarray = GetPower(powerlist).ToArray();
        return powerarray;
    }

    public IEnumerable<Power> GetPower(IList<Power> readings)
    {
        var sortedReadings = readings.OrderBy(x => x.y)
            .ThenBy(x => x.x);

        return sortedReadings;
    }


    public void Reset()
    {
        GameManager.Instance.reset++;
        reset = GameManager.Instance.reset;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("circuit");
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            IsEmpty = true;
            Destroy(obj[i]);
        }

        obj = GameObject.FindGameObjectsWithTag("battery");
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            IsEmpty = true;
            Destroy(obj[i]);
        }

        obj = GameObject.FindGameObjectsWithTag("power");
        Power[] powerarray = GetPowerArray(obj);
        for (int i = 0; i < powerarray.Length; i++)
        {

            powerarray[i].win = 0;
            powerarray[i].rightvalue = false;


        }



        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "reset/",
     "\"" + reset.ToString() + "\""));

        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" + Timer.Instance.seconds.ToString() + "\""));

    }

    public IEnumerator PutRequest(string url, string bodyJsonString)
    {
        url += ".json";
        Debug.Log(url);
        var request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log(request.responseCode);
    }


}
