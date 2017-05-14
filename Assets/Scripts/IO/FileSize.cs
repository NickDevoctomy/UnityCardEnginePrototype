using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.IO
{

    public class FileSize
    {

        #region public enums

        public enum UnitSize
        {
            None = 0,
            Bytes = 1,
            Kilobytes = 2,
            Megabytes = 3,
            Gigabytes = 4
        }

        #endregion

        #region private objects

        private Int64 cIntBytes;

        #endregion

        #region constructor / destructor

        public FileSize(Int64 iCount,
            UnitSize iUnitSize)
        {
            switch(iUnitSize)
            {
                case UnitSize.Bytes:
                    {
                        cIntBytes = iCount;
                        break;
                    }
                case UnitSize.Kilobytes:
                    {
                        cIntBytes = iCount * 1024;
                        break;
                    }
                case UnitSize.Megabytes:
                    {
                        cIntBytes = (iCount * 1024) * 1024;
                        break;
                    }
                case UnitSize.Gigabytes:
                    {
                        cIntBytes = ((iCount * 1024) * 1024) * 1024;
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        #endregion

        #region public methods

        public static Int64 Convert(Int64 iCount,
            UnitSize iSourceUnitSize,
            UnitSize iDestUnitSize)
        {
            FileSize pFSeSize = new FileSize(iCount, iSourceUnitSize);
            return (pFSeSize.Convert(iDestUnitSize));
        }

        public Int64 Convert(UnitSize iUnitSize)
        {
            switch (iUnitSize)
            {
                case UnitSize.Bytes:
                    {
                        return(cIntBytes);
                    }
                case UnitSize.Kilobytes:
                    {
                        return (cIntBytes / 1024);
                    }
                case UnitSize.Megabytes:
                    {
                        return ((cIntBytes / 1024) / 1024);
                    }
                case UnitSize.Gigabytes:
                    {
                        return (((cIntBytes / 1024) / 1024) / 1024);
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        #endregion

    }

}
