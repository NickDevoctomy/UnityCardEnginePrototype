using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{

    public static class PlatformUtility
    {

        #region public methods

        public static String GetPlatformNewLineString()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {
                        return ("\n");
                    }
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    {
                        return ("\r\n");
                    }
                default:
                    {
                        throw new PlatformNotSupportedException();
                    }
            }
        }

        public static String GetPlatformPathSeparator()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {
                        return ("/");
                    }
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    {
                        return ("\\");
                    }
                default:
                    {
                        throw new PlatformNotSupportedException();
                    }
            }
        }

        #endregion

    }

}
