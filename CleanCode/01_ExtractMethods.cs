using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string name, byte[] data)
		{
            SaveFile(name, data);
            SaveFile(Path.ChangeExtension(name, "bkp"), data);
            AddTime(name);
		}

        private static void SaveFile(string name, byte[] data)
	    {
            var file = new FileStream(name, FileMode.OpenOrCreate);
            file.Write(data, 0, data.Length);
            file.Close();
	    }

        private static void AddTime(string name)
	    {
            string timeFile = name + ".time";
            var timeStamp = BitConverter.GetBytes(DateTime.Now.Ticks);
            SaveFile(timeFile, timeStamp);
	    }
	}
}