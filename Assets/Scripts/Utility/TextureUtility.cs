using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Utility
{

    public static class TextureUtility
    {

        #region public methods

        public static Texture2D LoadPNG(String iPath)
        {
            Texture2D pTexTexture = null;
            if (File.Exists(iPath))
            {
                Byte[] pBytData = File.ReadAllBytes(iPath);
                pTexTexture = new Texture2D(2, 2);
                pTexTexture.LoadImage(pBytData);                    //..this will auto-resize the texture dimensions.
            }
            return (pTexTexture);
        }

        #endregion

    }

}
