using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWin : Singleton<CheckWin> {

    public void checkwin()
    {

        GameObject[] objcircuit = GameObject.FindGameObjectsWithTag("circuit");
        GameObject[] objbattery = GameObject.FindGameObjectsWithTag("battery");
        GameObject[] objpower = GameObject.FindGameObjectsWithTag("power");

        Circuit[] circuitarray = GetCircuitArray(objcircuit);

        Circuit[] batteryarray = GetBatteryArray(objbattery);

        Power[] powerarray = GetPowerArray(objpower);

        //for (int i = 0; i < circuitarray.Length; i++)
        //{
        //    Debug.Log(circuitarray[i].x + "," + circuitarray[i].y);
        //}



        for (int i = 0; i < batteryarray.Length; i++)
        {
            for (int w = 0; w < circuitarray.Length; w++)
            {
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
            }

            for (int w = 0; w < circuitarray.Length; w++)
            {
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
        }

        for (int i = 0; i < 5; i++)
        {
            checkwin2(circuitarray);
        }



        for (int i = 0; i < powerarray.Length; i++)

        {
            powerarray[i].win = 0;
            for (int w = 0; w < circuitarray.Length; w++)
            {
                if ((powerarray[i].y == circuitarray[w].y) && (powerarray[i].x) + 1 == circuitarray[w].x)
                {

                    if (powerarray[i].values[1] == 1 && circuitarray[w].values[3] == 1)
                    {
                        powerarray[i].win++;

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
            if (powerarray[i].win == 2 && powerarray[i].rightvalue == true)
            {
                ww = true;
            }
            else
            {
                ww = false;
            }
        }

        if (ww == true)
        {
            GameManager.Instance.win = 1;
            Debug.Log("win");
            for (int i = 0; i < powerarray.Length; i++)
            {
                powerarray[i].Trigger();
                StartCoroutine(myFunc());

                StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "win/",
"\"" + GameManager.Instance.win.ToString() + "\""));

                StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" + Timer.Instance.seconds.ToString() + "\""));

                Timer.Instance.keepTiming = false;
            }
        }

        StartCoroutine(PutRequest(LevelManager.url + SceneManager.GetActiveScene().name.ToString() + "/" + LevelManager.Instance.GuidString + "/" + "time/",
"\"" + Timer.Instance.seconds.ToString() + "\""));

        return;
    }

    private IEnumerator PutRequest(string v1, string v2)
    {
        throw new NotImplementedException();
    }

    IEnumerator myFunc()
    {

        yield return new WaitForSeconds(2);
        GameManager.Instance.winmenu();

    }

    public void checkwin2(Circuit[] circuitarray)
    {
        for (int i = 0; i < circuitarray.Length; i++)
        {
            for (int w = 0; w < circuitarray.Length; w++)
            {
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

                if ((circuitarray[i].y) - 1 == circuitarray[w].y && (circuitarray[i].x) == circuitarray[w].x)
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
}
