using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tileprefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    public string GuidString;

    [SerializeField]
    private Point Powerpoint;

    public const string url = "https://gamedatabase-201411.firebaseio.com/";
   

    private string bodyJsonString ;

    [SerializeField]
    private GameObject powerprefeb;

    public GameObject PowerPrefeb
    {
        get
        {
            return powerprefeb;
        }

    }

    [SerializeField]
    public GameObject Power;

    public Dictionary<Point, TileScript> Tiles { get; set; }


    public float TileSize {
       get { return tileprefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;}

    }

	// Use this for initialization
	void Start () {

        System.Guid g = System.Guid.NewGuid();
        GuidString = System.Convert.ToBase64String(g.ToByteArray());
        GuidString = GuidString.Replace("=", "");
        GuidString = GuidString.Replace("+", "");
        GuidString = GuidString.Replace("/", "");
        GuidString = GuidString.Replace("-", "");



        //bodyJsonString = "{ \"Level1\":{" + GuidString + ":{ \"x\":" + reset + ", \"y\":-94}}}";
        bodyJsonString = "{\"step\":\"" + GameManager.Instance.step + "\", \"reset\":\"" + GameManager.Instance.reset+"\", \"time\":\"0\", \"win\":\"0\"}";
        StartCoroutine(PutRequest(url + SceneManager.GetActiveScene().name.ToString()+ "/" + GuidString+ "/", bodyJsonString));

        

        CreateLevel();



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

    // Update is called once per frame
    void Update () {
		
	}

    public void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    private void CreateLevel()
    {

        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = new string[]
            {
                "217454",
                "214866",
                "124897",
                "542133",
                "359096"
            };
;

        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapXSize; x++)
            {
               PlaceTile(newTiles[x].ToString(),x,y,worldStart);
            }

        }

        maxTile = Tiles[new Point(mapXSize -1, mapYSize -1)].transform.position;

        //cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));


        TileScript.Instance.Powers();
    }

    private void PlaceTile(string tiletype, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tiletype);

        TileScript newTile = Instantiate(tileprefabs[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

    }





}
