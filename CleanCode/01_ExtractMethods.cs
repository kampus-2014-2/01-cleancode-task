using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string s, byte[] d)
		{
            WriteToFileStream(s,d);
            WriteToFileStream(Path.ChangeExtension(s, "bkp"), d);
            WriteToFileStream(s + ".time", BitConverter.GetBytes(DateTime.Now.Ticks));
		}

	    private static void WriteToFileStream(string path, byte[] data)
	    {
	        var fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.Write(data, 0, data.Length);
            fs.Close();
	    }
	}
}