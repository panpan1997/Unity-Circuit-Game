
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{

    public Text timer;
    private float startTime;
    public string seconds;
    public bool keepTiming = true;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (keepTiming == true)
        {
            float t = Time.time - startTime;
            string mins = ((int)t / 60).ToString();
            seconds = (t % 60).ToString("f2");
            timer.text = mins + ":" + seconds;
        }
        
    }

    public void Resettimr()
    {
        startTime = Time.time;
    }

    public IEnumerator PutRequest(string url, string bodyJsonString)
    {
        url += ".json";
        var request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();
        Debug.Log(request.responseCode);
    }
}
