using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wikitude;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class ImageIdentifier : MonoBehaviour
{

    TargetData loadedData; //load all data
    private Targets[] targets; //load only target set
    private string gameDataFileName;
    public GameObject QuizButton;
    public GameObject VideoMonitor;
    private GameObject Model3DObject;
    private float posi_x, posi_y, posi_z;
    private int rotate_x, rotate_y, rotate_z;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Method");
        BetterStreamingAssets.Initialize();
        setData();
        LoadTargetData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnObjectRecognized(ImageTarget recognizedTarget)
    {
        string label_name = recognizedTarget.Name;
        for (int i = 0; i < targets.Length; i++)
        {

            if(targets[i].targetname == label_name)
            {
                posi_x = targets[i].position_x;
                posi_x = 0 - posi_x;
                posi_y = targets[i].position_y;
                posi_z = targets[i].position_z;

                rotate_x = targets[i].rotation_x;
                rotate_y = targets[i].rotation_y;
                rotate_z = targets[i].rotation_z;


                if (targets[i].type == "Quiz")
                {
                    print("Quiz found");

                    QuizButton.transform.parent = recognizedTarget.Drawable.transform;
                    QuizButton.transform.localPosition = Vector3.zero;
                    QuizButton.SetActive(true);
                    GameStage.stage = targets[i].scriptname;
                    var transform = QuizButton.gameObject.transform;
                    transform.position = new Vector3(posi_x, posi_y, posi_z);
                    transform.Rotate(rotate_x, rotate_y, rotate_z);
                    break;
                }
                if (targets[i].type == "Video")
                {
                    
                    print("Video found");
                    VideoMonitor.transform.parent = recognizedTarget.Drawable.transform;
                    VideoMonitor.transform.localPosition = Vector3.zero;
                    VideoMonitor.SetActive(true);
                    GameStage.url = targets[i].scriptname;
                    var transform = VideoMonitor.gameObject.transform;
                    transform.position = new Vector3(posi_x, posi_y, posi_z);
                    transform.Rotate(rotate_x, rotate_y, rotate_z);
                    break;
                }
                if (targets[i].type == "3DObject")
                {

                    print("3DModel found");

                    print(targets[i].scriptname);
                    Model3DObject =   FindInActiveObjectByName(targets[i].scriptname);

                    Model3DObject.transform.parent = recognizedTarget.Drawable.transform;
                    Model3DObject.transform.localPosition = Vector3.zero;
                    Model3DObject.SetActive(true);
                    GameStage.url = targets[i].scriptname;
                    var transform = Model3DObject.gameObject.transform;
                    transform.position = new Vector3(posi_x, posi_y, posi_z);
                    transform.Rotate(rotate_x, rotate_y, rotate_z);
                    break;
                }
                print("target:  "+targets[i].targetname+"  " +targets[i].type + "  " + targets[i].scriptname);
            }
            
        }
    }
    public void OnObjectLost(ImageTarget recognizedTarget)
    {


        string targetname;
        string label_name = recognizedTarget.Name;


        for (int i = 0; i < targets.Length; i++)
        {
            targetname = targets[i].targetname;
            if (targetname == label_name)
            {
                if (targets[i].type == "Quiz")
                {
                    QuizButton.transform.parent = null;
                    QuizButton.SetActive(false);
                    print("Quiz lost");
                }
                if (targets[i].type == "Video")
                {
                    VideoMonitor.transform.parent = null;
                    VideoMonitor.SetActive(false);
                    print("Video lost");
                }
                if (targets[i].type == "3DObject")
                {
                    Model3DObject.transform.parent = null;
                    Model3DObject.SetActive(false);
                    print("Video lost");
                }
                print(targets[i].targetname + targets[i].type + targets[i].scriptname);
            }
        }
        // Create the custom augmentation.
        // You can use recognizedTarget.Name and recognizedTarget.ID 
        //GameObject newAugmentation = GameObject.Find("mySnowman");
    }



    public class GameStage
    {
        static public string stage = "";    // this is reachable from everywhere
        static public string url = "";
    }
    void setData()
    {
        gameDataFileName = "jsondata/Target.json";
    }
    private void LoadTargetData()
    {
        if (BetterStreamingAssets.FileExists(gameDataFileName))
        {
            loadedData = JsonUtility.FromJson<TargetData>(BetterStreamingAssets.ReadAllText(gameDataFileName));
            targets = loadedData.targets;
            print("loadtargetdata successfull");
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
