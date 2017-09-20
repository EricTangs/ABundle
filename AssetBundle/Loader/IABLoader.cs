using System;
using  UnityEngine ;

using  System.Collections ;


// 每一帧 的回调
   public   delegate   void  LoaderProgrecess(string  bundle, float  progress) ;
//  load  完成了 才回调
    public   delegate   void  LoadFinish(string  bundle);

  public  class IABLoader
    {

        private string bundleName;


        private string commonBundlePath ;

        private WWW commonLoaer;


        private float commResLoaderProcess;

        private LoadFinish loadFinish;   
        private LoaderProgrecess loadProgress;


        private IABResLoader abResloader;
         
        public IABLoader(LoaderProgrecess  tmpProgess, LoadFinish tmpFinish)
        {
            commonBundlePath = "";
            bundleName = ""  ;

            commResLoaderProcess = 0;

            loadProgress = tmpProgess;

            loadFinish = tmpFinish;

            abResloader = null;

        }

      // 设置包名 

      // scencesone/test.prefab
        public void SetBundleName(string bundel)
        {

            Debug.Log("set bundle Name =="+bundleName);
            this.bundleName = bundel;
        }

      /// <summary>
      ///  要求上层传递  完整 路径
      /// </summary>
      /// <param name="path"></param>
        public void LoadResources(string path)
        {
            commonBundlePath = path;


        }



      // 协程加载
        public IEnumerator CommonLoad()
        {

            Debug.Log("commonBundlePath ==" + commonBundlePath);

            // 从persistent path 里 先加载 ？
            // 如果有 就从 里面加载。
          //  AssetBundle.LoadFromFile

            commonLoaer = new WWW(commonBundlePath);

            while (!commonLoaer.isDone)
            {

                commResLoaderProcess = commonLoaer.progress;


                if (loadProgress != null)
                {
                    loadProgress(bundleName,commResLoaderProcess);
                }

                yield return commonLoaer.progress;

                commResLoaderProcess = commonLoaer.progress;

            }


            if (commResLoaderProcess >= 1.0f) //表示已经加载完成
            {

                Debug.Log("load 11111111111111 finish =="+bundleName);

                //yujie fixed
                abResloader = new IABResLoader(commonLoaer.assetBundle);

                if (loadProgress != null)
                {
                    loadProgress(bundleName, commResLoaderProcess);
                }

                if (loadFinish != null)
                this.loadFinish(bundleName);

              //  abResloader = new IABResLoader(commonLoaer.assetBundle);
            }
            else
            {
                Debug.LogError("load  bundle error =="+bundleName);
            }

            commonLoaer = null;

        }




        #region   下层提供功能




        //Debug  


        public void DebugerLoader()
        {
            if (abResloader != null)
            {
                abResloader.DebugAllRes();
            }
        }






      //    获取单个资源
        public UnityEngine.Object GetResources(string name)
        {
            if (abResloader != null)

                return abResloader[name];

            else
            {
                Debug.Log("IABLoader null");
                return null;
            }
                
        }

      // 获取多个资源

        public UnityEngine.Object[] GetMutiRes(string name)
        {


            if (abResloader != null)

                return abResloader.LoadResources(name);

            else

                return null;
        }



      // 释放功能

        public void Dispose()
        {
            if (abResloader != null)
            {
                abResloader.Dispose();
                abResloader = null;
            }
               

              
              
        }


        // 卸载单个资源
        public void UnLoadAssetRes(UnityEngine.Object tmpObj)
        {
            if (abResloader != null)
                abResloader.UnloadRes(tmpObj);
        }



        #endregion

    }

