
using UnityEngine;

using System.IO;


using System.Collections;

using System.Collections.Generic;

//  对一个场景的 所有bundle 包的管理



public  delegate   void  LoadAssetBundleCallBack(string scenceName, string bundleName) ;



// 单个物体  有可能 多个 存取
public  class  AssetObj
{



    public  List<Object> objs ;

    
    public   AssetObj(params  Object[]  tmpObj)
    {
          objs =  new List<Object>();


         objs.AddRange(tmpObj);
    }


    public  void ReleaseObj()
    {
         for(int i =0 ; i < objs.Count ;i++)
         {
               Resources.UnloadAsset(objs[i]);
         }
    }



}


// 存的一个bundle 包里面的 obj
public class AssetResObj
{

    public Dictionary<string, AssetObj> resObjs;


    public AssetResObj(string name, AssetObj tmp)
    {
        resObjs = new Dictionary<string, AssetObj>();

        resObjs.Add(name,tmp);
    }


    public void AddResObj(string name, AssetObj tmpObj)
    {
        resObjs.Add(name,tmpObj);
    }

    // 释放 多个
    public void ReleaseAllResObj()
    {
        List<string> keys = new List<string>();

        keys.AddRange(resObjs.Keys);

        for (int i = 0; i < keys.Count; i++)
        {

            ReleaseResObj(keys[i]);
        }


    }

    //释放单个
    public void ReleaseResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];

            tmpObj.ReleaseObj();

        }
        else
        {
            Debug.Log("release  object name  is not  exit =="+name);
        }
    }

    public List<Object> GetResObj(string name)
    {

        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];

            return tmpObj.objs;

        }
        else
        {
            return null;
        }


    }
}






  public  class IABManager
    {

      // 把每一个包 都存起来 

      Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();


      // 加载出来的object 都存这
      Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();


  


    



      public IABManager(string tmpSenceName)
      {

          scenceName = tmpSenceName;
      }

      string scenceName;


      /// <summary>
      ///   表示 是否 加载了  bundle
      /// </summary>
      /// <param name="bundleName"></param>
      /// <returns></returns>

      public bool IsLoadingAssetBundle(string bundleName)
      {
          if (!loadHelper.ContainsKey(bundleName))
          {
              return false;
          }
          else
          {
              return true;
          }
      }




      #region  释放 缓存物体

      public void DisposeResObj(string bundleName, string resName)
      {
          if (loadObj.ContainsKey(bundleName))
          {
              AssetResObj tmpObj = loadObj[bundleName];

              tmpObj.ReleaseResObj(resName);
          }
      }


      public void DisposeResObj(string bundleName)
      {

          if (loadObj.ContainsKey(bundleName))
          {
              AssetResObj tmpObj = loadObj[bundleName];

              tmpObj.ReleaseAllResObj();
          }

          Resources.UnloadUnusedAssets();
 
      }

      public void  DisposeAllObj()
      {
          List<string> keys = new List<string>();

          keys.AddRange(loadObj.Keys);

          for (int i = 0; i < loadObj.Count; i++)
          {
              DisposeResObj(keys[i]);
          }

          loadObj.Clear();

      }




      /// <summary>
      ///   循环的 处理  依赖关系
      /// </summary>
      /// <param name="bundleName"></param>
      public void DisposeBundle(string bundleName)
      {
          if (loadHelper.ContainsKey(bundleName))
          {

              IABRelationManager loader = loadHelper[bundleName];

              List<string> depences = loader.GetDepedence();

              for (int i = 0; i < depences.Count; i++)
              {

                  if (loadHelper.ContainsKey(depences[i]))
                  {
 
                     IABRelationManager   depedence = loadHelper[depences[i]] ;

                     if (depedence.RemoveReference(bundleName))
                     {
                         DisposeBundle(depedence.GetBundleName());
                     }
                  }
              }

              if (loader.GetRefference().Count <= 0)
              {
                  loader.Dispose();

                  loadHelper.Remove(bundleName);
              }

          }
      }


      public void DispoaseAllBundl()
      {


          List<string> keys = new List<string>();


          keys.AddRange(loadHelper.Keys);


          for (int i = 0; i < loadHelper.Count; i++)
          {
              IABRelationManager loader = loadHelper[keys[i]];

              loader.Dispose();
          }

          loadHelper.Clear();
      }

      public void DisposeAllBundleAndRes()
      {
          DisposeAllObj();

          DispoaseAllBundl();
      }

      #endregion




      string[] GetDependences(string  bundleName)
      {
          return IABManifestLoader.Instance.GetDepences(bundleName);
      }


      //对外的接口
      public void LoadAssetBundle(string bundleName, LoaderProgrecess progress, LoadAssetBundleCallBack callBack)

      {
          if (!loadHelper.ContainsKey(bundleName))
          {

              IABRelationManager loader = new IABRelationManager();

              loader.Initial(bundleName, progress);


              loadHelper.Add(bundleName, loader);

              callBack(scenceName, bundleName);
          }
          else
          {
              Debug.Log("IABManaager  have contain  bundle name =="+bundleName);
          }
 
      }



      public IEnumerator LoadAssetBundleDependences(string bundlName, string refName, LoaderProgrecess progress)
      {

          if (!loadHelper.ContainsKey(bundlName))
          {

              IABRelationManager loader = new IABRelationManager();

              loader.Initial(bundlName, progress);


              if (refName != null)
              {
                  loader.AddRefference(refName);
              }


              loadHelper.Add(bundlName, loader);

              yield return LoadAssetBundles(bundlName);


          }
          else
          {
              if (refName != null)
              {
                  IABRelationManager loader = loadHelper[bundlName];

                  loader.AddRefference(bundlName);
              }
          }

      }


      /// <summary>
      ///  加载 assetbundle 必须先加载 manifest
      /// </summary>    callBack(scenceName, bundleName);  返回上层 调用这个api
      /// <param name="bundleName"></param>
      /// <returns></returns>

      public IEnumerator LoadAssetBundles(string bundleName)
      {
          while (!IABManifestLoader.Instance.IsLoadFinsh())
          {
              yield return null;
          }


          IABRelationManager loader = loadHelper[bundleName];

          string[] depences = GetDependences(bundleName);

          loader.SetDepedences(depences);


          for (int i = 0; i < depences.Length; i++)
          {
              yield return LoadAssetBundleDependences(depences[i],bundleName,loader.GetProgress());
          }


          yield return loader.LoadAssetBundle();

      }






      # region   由下层提供 API

      /// <summary>
      ///    
      /// </summary>
      /// <param name="bundleName"> scenceon/test.prefab</param>
      public void DebugAssetBundle(string bundleName)
      {
          if (loadHelper.ContainsKey(bundleName))
          {
               IABRelationManager  loader = loadHelper[bundleName] ;

               loader.DebuggerAsset();

          }
      }


      public bool IsLoadingFinish(string bundleName)
      {
          if (loadHelper.ContainsKey(bundleName))
          {
              IABRelationManager loader = loadHelper[bundleName];

              return loader.IsBundleLoadFinish();

          }
          else
          {

              Debug.Log("IABRelatioin  no contain  bundle=="+bundleName);
              return false;
          }
      }


      public Object GetSingleResources(string bundleName, string resName)
      {

          //表示 是否已经缓存了物体
          if (loadObj.ContainsKey(bundleName))
          {

              AssetResObj tmpRes = loadObj[bundleName];

              List<Object> tmpObj = tmpRes.GetResObj(resName);

              if (tmpObj != null)
              {
                  return tmpObj[0];
              }

          }


          // 表示已经加载过bundle
          if (loadHelper.ContainsKey(bundleName))
          {
              IABRelationManager loader = loadHelper[bundleName];

              Object tmpObj = loader.GetSingleResource(resName);


              AssetObj tmpAssetObj = new AssetObj(tmpObj);

              //缓存里面 是否已经有这个包
              if (loadObj.ContainsKey(bundleName))
              {

                  AssetResObj tmpRes = loadObj[bundleName];

                  tmpRes.AddResObj(resName, tmpAssetObj);
              }
              else
              {
                  //没有加载过 这个包
                  AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);

                  loadObj.Add(bundleName, tmpRes);
              }

              return tmpObj;

          }
          else
          {

              Debug.Log("iabmanager error =="+bundleName);
              return null;
          }



      }




      public Object[] GetMutiResources(string bundleName, string resName)
      {

          //表示 是否已经缓存了物体
          if (loadObj.ContainsKey(bundleName))
          {

              AssetResObj tmpRes = loadObj[bundleName];

              List<Object> tmpObj = tmpRes.GetResObj(resName);

              if (tmpObj != null)
              {
                  return tmpObj.ToArray();
              }

          }


          // 表示已经加载过bundle
          if (loadHelper.ContainsKey(bundleName))
          {
              IABRelationManager loader = loadHelper[bundleName];

              Object[] tmpObj = loader.GetMutiResources(resName);


              AssetObj tmpAssetObj = new AssetObj(tmpObj);

              //缓存里面 是否已经有这个包
              if (loadObj.ContainsKey(bundleName))
              {

                  AssetResObj tmpRes = loadObj[bundleName];

                  tmpRes.AddResObj(resName, tmpAssetObj);
              }
              else
              {
                  //没有加载过 这个包
                  AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);

                  loadObj.Add(bundleName, tmpRes);
              }

              return tmpObj;

          }
          else
          {
              return null;
          }



      }




     




        #endregion





    }

