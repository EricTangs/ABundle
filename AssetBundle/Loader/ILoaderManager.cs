using UnityEngine;
using System.Collections;

using  System.Collections.Generic ;


public class ILoaderManager : MonoBehaviour {

    public static ILoaderManager Instance;


    void Awake()
    {

        Instance = this;

        //第一步 加载 IABManifeset


        StartCoroutine(IABManifestLoader.Instance.LoadMainfeset());

    }


  
    //  sencename    manager
    private Dictionary<string, IABScenceManager> loadManager = new Dictionary<string, IABScenceManager>();


    //第二步 读取配置文件
    public void ReadConfiger(string scenceName)
    {
        if (!loadManager.ContainsKey(scenceName))
        {

            IABScenceManager tmpManager = new IABScenceManager(scenceName);

            tmpManager.ReadConfiger(scenceName);

            loadManager.Add(scenceName,tmpManager);
        }
 
    }

    //

    public void LoadCallBack(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {

            IABScenceManager tmpManager = loadManager[scenceName];

            StartCoroutine(tmpManager.LoadAssetSys(bundleName));
        }
        else
        {
            Debug.Log(" bundle name  is not contain =="+bundleName);
        }
    }

    // 提供 加载功能

    public void LoadAsset(string scenceName, string bundleName, LoaderProgrecess progress)
    {

        if (!loadManager.ContainsKey(scenceName))
        {
            ReadConfiger(scenceName);
        }

        IABScenceManager   tmpManager =  loadManager[scenceName] ;

        tmpManager.LoadAsset(bundleName, progress, LoadCallBack);

    }




    #region  由下层API 提供


    public string GetBundleRetateName(string scenceName, string bundleName)
    {

        IABScenceManager tmpManager = loadManager[scenceName];

        if (tmpManager != null)
        {
            return tmpManager.GetBundleReateName(bundleName);
        }

        return null;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="scenceName">ScenceOne </param>
    /// <param name="bundleName">Load</param>
    /// <param name="resName">TestTwo</param>
    /// <returns></returns>
    public Object GetSingleResources(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            return tmpManager.GetSingleResources(bundleName, resName);

        }
        else
        {
            Debug.Log(" scenceName =="+scenceName +"Bundle Name =="+ bundleName +" is not load");
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scenceName">ScenceOne </param>
    /// <param name="bundleName">Load</param>
    /// <param name="resName">TestTwo</param>
    /// <returns></returns>
    public Object[] GetMutiResources(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            return tmpManager.GetMutiResources(bundleName, resName);

        }
        else
        {
            Debug.Log(" scenceName ==" + scenceName + "Bundle Name ==" + bundleName + " is not load");
            return null;
        }

    }


   ///  释放资源
   /// 释放 某一个资源
    public void UnLoadResObj(string scenceName, string bundleName, string res)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeResObj(bundleName, res);

        }
    }

    /// <summary>
    ///   释放整个包
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>

    public void UnLoadBundleResObjs(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeBundleRes(bundleName);

        }
    }


    /// <summary>
    ///  释放 真个场景的objects
    /// </summary>
    /// <param name="scenceName"></param>
    public void UnLoadAllResObjs(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeAllRes();

        }
 
    }


    /// <summary>
    ///  释放一个bundle 
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadAssetBundle(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeBundle(bundleName);

        }
 

    }

    /// <summary>
    ///  释放一个场景的 全部bundle
    /// </summary>
    /// <param name="scenceName"></param>
    public void UnLoadAllAssetBundle(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeAllBundle();

            System.GC.Collect();
        }
    }


    /// <summary>
    ///  释放 一个场景的 全部bundle  和objects
    /// </summary>
    /// <param name="scenceName"></param>
    public void UnLoadAllAssetBundleAndResObjs(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeAllBundleAndRes();

            System.GC.Collect();
        }
    }



    public void DebugAllAssetBundle(string scenceName)
    {

        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            tmpManager.DebugAllAsset();
        }
    }



    public bool IsLoadingBundleFinish(string scenceName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(scenceName);

        if (tmpBool)
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            return tmpManager.IsLoadingFinish( bundleName);
        }


        return false;
    }

    public bool IsLoadingAssetBundle(string scenceName, string bundleName)
    {

        bool tmpBool = loadManager.ContainsKey(scenceName);

        if (tmpBool)
        {
            IABScenceManager tmpManager = loadManager[scenceName];

            return tmpManager.IsloadingAssetBundle(bundleName);
        }


        return false;
    }





    #endregion







    void OnDestroy()
    {
        loadManager.Clear();
        System.GC.Collect();
    }



    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
