using UnityEngine;
using System.Collections;


using U3DEventFrame;


public class IABManifestLoader 
{


    public AssetBundleManifest assetManifeset;


    public string manifesetPath;


    private bool isLoadFinish;


    public AssetBundle manifesetLoader;


    public IABManifestLoader()
    {
        assetManifeset = null;

        manifesetLoader = null;


        isLoadFinish = false;

        //fixed 
   // file:///C:/Users/lenovo/Desktop/zyj/Game/TeacherManniu/Assets/StreamingAssets/Windows/Windows
        manifesetPath = IPathTools.GetAssetBundleWWWStreamPath(IPathTools.GetRuntimeFolder());
    }



    public IEnumerator LoadMainfeset()
    {

        Debug.Log("manifesetPath ==" + manifesetPath);
        WWW mainfeset = new WWW(manifesetPath);

        yield return mainfeset;

        if (!string.IsNullOrEmpty(mainfeset.error))
        {
            Debug.Log(mainfeset.error);
        }
        else
        {
            if (mainfeset.progress >= 1.0f)
            {
                manifesetLoader = mainfeset.assetBundle;

                assetManifeset = manifesetLoader.LoadAsset("AssetBundleManifest")  as AssetBundleManifest;

                isLoadFinish = true;

                Debug.Log("manifeset  load finish");
            }
             
        }
    }


    public string[] GetDepences(string name)
    {
        return assetManifeset.GetAllDependencies(name);
    }

    public void UnloadManifest()
    {
        manifesetLoader.Unload(true);
    }



    /// <summary>
    ///   
    /// </summary>
    /// <param name="path"></param>
    public void SetManifestPath(string path)
    {

        manifesetPath = path;
 
    }


    private static IABManifestLoader instance = null;


    public static IABManifestLoader Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new IABManifestLoader();
            }

            return instance;
        }
    }


    public bool IsLoadFinsh()
    {

        return isLoadFinish;
    }

}
