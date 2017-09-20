using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAssetBundle : MonoBehaviour {



    public GameObject parentCanvas;
    
	// Use this for initialization
	void Start () {

        gameObject.AddComponent<ILoaderManager>();



        ILoaderManager.Instance.LoadAsset("ScenceOne", "Loading", LoaderProgrecess);
		
	}


    public void LoaderProgrecess(string bundle, float progress)
    {

        if ( progress >= 1)
        {

            Debug.Log("bundle name=="+bundle  );

        }
           
    }
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0))
        {
            Object tmpObj = ILoaderManager.Instance.GetSingleResources("ScenceOne", "Loading", "Button");


            GameObject tmpGame = GameObject.Instantiate(tmpObj) as GameObject;


            tmpGame.transform.SetParent(parentCanvas.transform,false);
           // tmpGame.transform.parent = parentCanvas.transform;
        }
		
	}
}
