using UnityEngine;
using System.Collections;

using  System.Collections.Generic ;

using U3DEventFrame;


public class IABRelationManager 
{



    /// <summary>
    /// 
    ///              ---->    yy  
    ///       xx                       xx  dependence   YY  AA                
    ///            -----  >  AA 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    List<string> depedenceBundle ;


    
    /// 
    /// 
    ///  表示   yy  aa    refer    XX
    /// 
    /// </summary>

    List<string> referBundle;



    IABLoader assetLoader;


    LoaderProgrecess loaderProgress;


    string theBundleName;


    public IABRelationManager()
    {

        depedenceBundle = new List<string>();
        referBundle = new List<string>();
    }


    //  添加  ref 关系
    public void AddRefference(string bundleName)
    {
        referBundle.Add(bundleName);
    }


    //  获取 ref 关系
    public List<string> GetRefference()
    {
        return referBundle;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns>  表示 是否释放了 自己</returns>
    public bool RemoveReference(string bundleName)
    {
        for (int i = 0; i < referBundle.Count; i++)
        {
            if (bundleName.Equals(referBundle[i]))
            {
                referBundle.RemoveAt(i);
            }
        }

        if (referBundle.Count <= 0)
        {
            Dispose();

            return true;
        }

        return false;
    }



    public void SetDepedences(string[] depence)
    {
        if (depence.Length > 0)
        {
            depedenceBundle.AddRange(depence);
        }
    }


    public List<string> GetDepedence()
    {
        return depedenceBundle;
    }


    public void RemoveDepence(string bundleName)
    {
        for (int i = 0; i < depedenceBundle.Count; i++)
        {

            if (bundleName.Equals(depedenceBundle[i]))
            {
                depedenceBundle.RemoveAt(i);
            }
        }
    }


    bool IsLoadFinish;

    public void BundleLoadFinish(string bundleName)
    {
        IsLoadFinish = true;
    }


    public bool IsBundleLoadFinish()
    {
        return IsLoadFinish;
    }


    public string GetBundleName()
    {
        return theBundleName;
    }


    public void Initial(string bundle, LoaderProgrecess progress)
    {

      
        IsLoadFinish = false;

        theBundleName = bundle;

        loaderProgress = progress;
      
        assetLoader = new IABLoader(progress, BundleLoadFinish) ;

       
        //yujie fixed

        assetLoader.SetBundleName(bundle);

        string bundlePath = IPathTools.GetAssetBundleWWWStreamPath(bundle); // IPathTools.GetWWWAssetBundlePath()+"/"+bundle;
        assetLoader.LoadResources(bundlePath);
    }


    public LoaderProgrecess GetProgress()
    {

        return loaderProgress;
    }



    #region  由下层 提供 API


    public void DebuggerAsset()
    {

        if (assetLoader != null)
        {
            assetLoader.DebugerLoader();
        }
        else
        {
            Debug.Log(" asset  load  is null");
        }
    }

    //  unity3d  5.3 以上 协程  才可以

    public IEnumerator LoadAssetBundle()
    {

        yield return assetLoader.CommonLoad();
    }



    // 释放 过程

    public void Dispose()
    {


        assetLoader.Dispose();
    }

    public Object GetSingleResource(string bundleName)
    {

        Debug.Log("iabreation manager");
        return assetLoader.GetResources(bundleName);
    }


    public Object[] GetMutiResources(string bundleName)
    {
        return assetLoader.GetMutiRes(bundleName);
    }



    #endregion

}
