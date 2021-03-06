﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPMANX_RESOURCE_HANDLE_T
    {
        public IntPtr Handle;

        public static readonly DISPMANX_RESOURCE_HANDLE_T Null = new DISPMANX_RESOURCE_HANDLE_T() { Handle = IntPtr.Zero };
    }

}
