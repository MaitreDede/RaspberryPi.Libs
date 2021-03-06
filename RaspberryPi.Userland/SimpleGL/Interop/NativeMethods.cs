﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static RaspberryPi.Interop.NativeMethods;

namespace RaspberryPi.Userland.SimpleGL.Interop
{
    internal static class NativeMethods
    {
        static NativeMethods()
        {
            //Ensure gles/egl libs are loaded correctly
            LoadLib(GL.LIB_GLES);
            LoadLib(LIB_EGL);
        }

        public const string LIB_EGL = "libbrcmEGL.so";

        [DllImport(LIB_EGL, EntryPoint = "eglGetDisplay", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr eglGetDisplay(int display);

        [DllImport(LIB_EGL, EntryPoint = "eglGetError", CallingConvention = CallingConvention.Cdecl)]
        public static extern EglError eglGetError();

        [DllImport(LIB_EGL, EntryPoint = "eglInitialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglInitialize(IntPtr display, out int major, out int minor);

        [DllImport(LIB_EGL, EntryPoint = "eglTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglTerminate(IntPtr display);

        [DllImport(LIB_EGL, EntryPoint = "eglChooseConfig", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglChooseConfig(IntPtr display, int[] attrib_list, IntPtr config, int config_size, out int num_config);

        [DllImport(LIB_EGL, EntryPoint = "eglCreateContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr eglCreateContext(IntPtr display, IntPtr config, IntPtr share_context, IntPtr attrib_list);

        [DllImport(LIB_EGL, EntryPoint = "eglDestroyContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglDestroyContext(IntPtr display, IntPtr context);

        [DllImport(LIB_EGL, EntryPoint = "eglCreateWindowSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr eglCreateWindowSurface(IntPtr display, IntPtr config, IntPtr nativeWindow, IntPtr attrib_list);

        [DllImport(LIB_EGL, EntryPoint = "eglDestroySurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglDestroySurface(IntPtr display, IntPtr context);

        [DllImport(LIB_EGL, EntryPoint = "eglMakeCurrent", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglMakeCurrent(IntPtr display, IntPtr draw, IntPtr read, IntPtr context);

        [DllImport(LIB_EGL, EntryPoint = "eglSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglSwapBuffers(IntPtr display, IntPtr surface);
    }
}
