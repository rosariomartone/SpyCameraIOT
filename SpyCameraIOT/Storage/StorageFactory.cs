﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace SpyCameraIOT
{
    public static class StorageFactory
    {
        public static IStorage Get(StorageProvider provider)
        {
            switch (provider)
            {
                case StorageProvider.Azure: return new Azure();
                case StorageProvider.OneDrive: return new OneDrive();
                case StorageProvider.Local: return new LocalStorage();
                default: throw new ArgumentNullException("Set StorageProvider in config");
            }
        }
    }
}
