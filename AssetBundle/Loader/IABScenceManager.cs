using UnityEngine;
using System.Collections;

using  System.IO ;

using  System.Collections.Generic ;

using U3DEventFrame;



public class IABScenceManager 
{


    IABManager abManager;
    public IABScenceManager(string scenceName)
    {

        abManager = new IABManager(scenceName);

    }

    /// <summary>
    ///  存的都是 单个场景的  所有 assetbundle 包
    /// </summary>
    private Dictionary<string, string> allAssets = new Dictionary<string, string>();



 


    /// <summary>
    ///   场景名字  ScenceOne
    /// </summary>
    /// <param name="fileName"></param>
    public void ReadConfiger(string fileName)
    {
        string textFileName = "/" + fileName + "Record.txt";


        string path = IPathTools.GetAssetBundleFilePath(textFileName) ;

        ReadConfig(path);
    }


    /// <summary>
    /// /
    ///             bw.WriteLine(readDict.Count);
             
            //foreach (string key in readDict.Keys)
            //{
            //    bw.Write(key);

            //    bw.Write(" ");
            //    bw.Write(readDict[key]);

            //    bw.Write("\n");
            //}
    /// 

    //  第一行 是总行数
    /// 
    /// 
    /// </summary>
    /// <param name="path"></param>
    private void ReadConfig(string path)
    {
        FileStream fs = new FileStream(path,FileMode.Open);

        StreamReader br = new StreamReader(fs);


        string line = br.ReadLine();

        Debug.Log("read line =="+line);

        int allCount= int.Parse(line);

        for (int i = 0; i < allCount; i++)
        {

           string  tmpStr= br.ReadLine();

      

           string[] tmpArr = tmpStr.Split(" ".ToCharArray());

           Debug.Log("Readline ==" + tmpStr + "Length=="+ tmpArr.Length);

           Debug.Log(tmpArr[0] + "==" + tmpArr[2]);

           allAssets.Add(tmpArr[0], tmpArr[2]);
        }

        br.Close();

        fs.Close();

    }



    /// <summary>
    ///   
    /// </summary>
    /// <param name="bundleName"> Load </param>
    /// <param name="progress"></param>
    /// <param name="callBack"></param>
    public void LoadAsset(string bundleName, LoaderProgrecess progress, LoadAssetBundleCallBack callBack)
    {

        //scenceone/load.ld
        if (allAssets.ContainsKey(bundleName))
        {
            string tmpValue = allAssets[bundleName];

            Debug.Log("iab scenceManager ==" +tmpValue + "bundleName=="+bundleName);

            abManager.LoadAssetBundle(tmpValue, progress, callBack);


        }
        else
        {
            Debug.Log("Dont  contain  the bundle ==" + bundleName);
        }
    }



    public string GetBundleReateName(string bundleName)
    {

        if (allAssets.ContainsKey(bundleName))
        {
            return allAssets[bundleName];
        }
        else
        {
            return null;
        }
    }


    #region 由下层提供功能


    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return abManager.LoadAssetBundles(bundleName);
    }



    public Object GetSingleResources(string bundleName, string resName)
    {
        if (allAssets.ContainsKey(bundleName))
        {

            return abManager.GetSingleResources(allAssets[bundleName], resName);
        }
        else
        {
            Debug.Log("Dont  contain  the bundle ==" + bundleName);
            return null;
        }
    }



    public Object[] GetMutiResources(string bundleName, string resName)
    {
        if (allAssets.ContainsKey(bundleName))
        {

            return abManager.GetMutiResources(allAssets[bundleName], resName);
        }
        else
        {
            Debug.Log("Dont  contain  the bundle ==" + bundleName);
            return null;
        }

    }


    /// <summary>
    ///  释放单个 资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="res"></param>
    public void DisposeResObj(string bundleName, string res)
    {

        if (allAssets.ContainsKey(bundleName))
        {

            abManager.DisposeResObj(allAssets[bundleName], res);
        }
        else
        {
            Debug.Log("Dont  contain  the bundle ==" + bundleName);

        }

    }

    public void DisposeBundleRes(string bundleName)
    {

        if (allAssets.ContainsKey(bundleName))
        {

            abManager.DisposeResObj(allAssets[bundleName]);
        }
        else
        {
            Debug.Log("Dont  contain  the bundle ==" + bundleName);

        }
    }


    public void DisposeAllRes()
    {

        abManager.DisposeAllObj();
    }




    public void DisposeBundle(string bundleName)
    {

        if (allAssets.ContainsKey(bundleName))
        {

            abManager.DisposeBundle(bundleName);
        }

    }

    public void DisposeAllBundle()
    {
        abManager.DispoaseAllBundl();


        allAssets.Clear();
    }


    public void DisposeAllBundleAndRes()
    {
        abManager.DisposeAllBundleAndRes();

        allAssets.Clear();
    }


    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();

        keys.AddRange(allAssets.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            abManager.DebugAssetBundle(allAssets[keys[i]]);
        }

    }

    // scenceone/test.ld

    //bundleName  =test
    public bool IsLoadingFinish(string bundleName)
    {
        if (allAssets.ContainsKey(bundleName))
        {
            return abManager.IsLoadingFinish(allAssets[bundleName]);
        }
        else
        {
            Debug.Log("is not  contain bundle =="+bundleName);
        }

        return false;
    }

    public bool IsloadingAssetBundle(string bundleName)
    {
        if (allAssets.ContainsKey(bundleName))
        {
            return abManager.IsLoadingAssetBundle(allAssets[bundleName]);
        }
        else
        {
            Debug.Log("is not  contain bundle ==" + bundleName);
        }

        return false;
    }



    #endregion





}
