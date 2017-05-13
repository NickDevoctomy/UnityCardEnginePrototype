using Assets.Scripts.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{

    public static class IOUtility
    {

        #region public methods

        public static String LoadURLAsString(String iPath)
        {
            String pStrText = String.Empty;
            WWW www = new WWW(iPath);
            WaitForSeconds w;
            while (!www.isDone)
                w = new WaitForSeconds(0.1f);
            Logman.Log("Test", BaseLogger.MessageType.Information, "Loaded streaming assets file data '{0}'.", pStrText);
            pStrText = www.text;
            return (pStrText);
        }

        public static String LoadStreamingAssestsFileAsString(String iPath)
        {
            String pStrFilePath = Path.Combine(Application.streamingAssetsPath, iPath);
            String pStrText = String.Empty;
            if (pStrFilePath.Contains("://"))
            {
                WWW www = new WWW(pStrFilePath);
                WaitForSeconds w;
                while (!www.isDone)
                    w = new WaitForSeconds(0.1f);
                Logman.Log("Test", BaseLogger.MessageType.Information, "Loaded streaming assets file data '{0}'.", pStrText);
                pStrText = www.text;
            }
            else
            {
                pStrText = File.ReadAllText(pStrFilePath);
            }
            return(pStrText);
        }

        public static Texture2D LoadStreamingAssestsFileAsTexture2D(String iPath)
        {
            String pStrFilePath = Path.Combine(Application.streamingAssetsPath, iPath);
            Texture2D pTexTexture = null;
            if (pStrFilePath.Contains("://"))
            {
                WWW www = new WWW(pStrFilePath);
                WaitForSeconds w;
                while (!www.isDone)
                    w = new WaitForSeconds(0.1f);
                pTexTexture = www.texture;
            }
            else
            {
                pTexTexture = TextureUtility.LoadPNG(pStrFilePath);
            }
            return (pTexTexture);
        }

        /// <summary>
        /// Get a writable directory that we can store stuff in
        /// http://answers.unity3d.com/questions/317048/android-writing-to-applicationpersistentdatapath.html
        /// </summary>
        /// <returns></returns>
        public static String GetDataPath()
        {
            string path = "";
#if UNITY_ANDROID && !UNITY_EDITOR
 try {
          IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
          IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");
 
          using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
             using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
                IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
                IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");   
                                 
                path = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);                    
 
                if(path != null) {
                   Debug.Log("Got internal path: " + path);
                }
                else {
                   Debug.Log("Using fallback path");
                   path = "/data/data/*** YOUR PACKAGE NAME ***/files";
                }
             }
          }
       }
       catch(Exception e) {
          Debug.Log(e.ToString());
       }
#else
            path = Application.persistentDataPath;
#endif
            return (path);
        }

        #endregion

    }

}
