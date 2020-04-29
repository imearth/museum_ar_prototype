using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoading : MonoBehaviour
{
    public GameObject LoadingCircle;
    public GameObject Error;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFinishedLoading()
    {
        LoadingCircle.SetActive(false);
    }
    public void OnErrorLoading()
    {
        LoadingCircle.SetActive(false);
        Error.SetActive(true);
    }
}
