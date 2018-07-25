using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Firebase2 : Singleton<Firebase2>
{



    public IEnumerator GetRequest(string uri)
    {
        uri += ".json";
        Debug.Log(uri);
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.Send();
        
        if (request.isError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);
        }
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

    IEnumerator PostRequest(string url, string bodyJsonString)
    {

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();
    }
}
