﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Ejdb.DB
{
	internal class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private string _libraryPath;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern SafeLibraryHandle LoadLibrary(string dllToLoad);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		public SafeLibraryHandle() : base(true)
		{

		}

		protected override bool ReleaseHandle()
		{
			
			SafeLibraryHandle.FreeLibrary(this.handle);

			if (File.Exists(_libraryPath))
			{
				File.Delete(_libraryPath);
			}

			return true;
		}

		public static SafeLibraryHandle Load()
		{
			var libraryPath = ResourceHelper.ExportLibrary();
			var result = LoadLibrary(libraryPath);

			if (result.IsInvalid)
			{
				var error = Marshal.GetLastWin32Error();
				throw new InvalidOperationException("Win32 error " + error);
			}


			result._libraryPath = libraryPath;
			return result;
		}
	}
}