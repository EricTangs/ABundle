using UnityEngine;
using System.Collections;

using System;


public class IABResLoader :IDisposable {


    private AssetBundle ABRes;


    public IABResLoader(AssetBundle tmpBundle)
    {
        ABRes = tmpBundle;
    }


    /// <summary>
    ///   加载 单个 资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object this[string resName]
    {

        get {
            if (this.ABRes == null || !this.ABRes.Contains(resName))
            {
                Debug.Log(" res not contain  ");
                return null;
            }

            Debug.Log(" res load contain  " + resName);

            Debug.Log(" res load contain  " + ABRes.LoadAsset(resName));
            return ABRes.LoadAsset(resName);


        
        }
    }



    /// <summary>
    /// 加载多个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>

    public UnityEngine.Object[] LoadResources(string resName)
    {
        if (this.ABRes == null || !this.ABRes.Contains(resName))
        {
            Debug.Log(" res not contain  ");
            return null;
        }

     //   ABRes.LoadAssetAsync

        return this.ABRes.LoadAssetWithSubAssets(resName);
    }

    /// <summary>
    ///   卸载单个资源
    /// </summary>
    /// <param name="resObj"></param>

    public void UnloadRes(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }


    /// <summary>
    ///  释放 assetbundle 包
    /// </summary>
    public void Dispose()
    {
         if (this.ABRes == null)

             return ;

        ABRes.Unload(false);
    }

    // 调试
    public void DebugAllRes()
    {
        string[] tmpAssetName = ABRes.GetAllAssetNames();

        for (int i = 0; i < tmpAssetName.Length; i++)
        {
            Debug.Log(" ABRes  Contain  asset name =="+ tmpAssetName[i]);
        }
    }
     


}
