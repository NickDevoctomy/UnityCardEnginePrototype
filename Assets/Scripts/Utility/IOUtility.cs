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
            Debug.Log(String.Format("Loaded streaming assets file data '{0}'.", pStrText));
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
                Debug.Log(String.Format("Loaded streaming assets file data '{0}'.", pStrText));
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

        #endregion

    }

}
