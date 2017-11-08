﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2
{
    static class ConsoleHelper
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void Show()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }

        public static void Hide()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
    }
}
