using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
	    public static void WriteToFile(string path, FileMode mode, byte[] d)
	    {
            var fs = new FileStream(path, mode);
            fs.Write(d, 0, d.Length);
            fs.Close();
	    }

		private static void SaveData(string s, byte[] d)
		{
            WriteToFile(s, FileMode.OpenOrCreate, d);
            WriteToFile(Path.ChangeExtension(s, "bkp"), FileMode.OpenOrCreate, d);
            WriteToFile(s + ".time", FileMode.OpenOrCreate, BitConverter.GetBytes(DateTime.Now.Ticks));
		}
	}
}