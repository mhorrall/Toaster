using System;
using System.Runtime.InteropServices;

namespace Toaster
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PROPERTYKEY
    {
        public Guid fmtid;
        public uint pid;

        public PROPERTYKEY(Guid guid, uint id)
        {
            fmtid = guid;
            pid = id;
        }

        /// <summary>PKEY_AppUserModel_ID</summary>
        public static readonly PROPERTYKEY AppUserModel_ID = new PROPERTYKEY(new Guid("e686efbc-1d2c-4d68-801a-9819fba854e3"), 5);
        public static readonly PROPERTYKEY AppUserModel_ToastActivatorCLSID = new PROPERTYKEY(new Guid("94697601-C2EF-4097-A0EC-800B4DB37E4E"), 26);
    }
}
