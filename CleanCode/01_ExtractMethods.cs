using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
	    /// <summary>
	    /// Open file.
	    /// </summary>
	    /// <param name="file">File name</param>
	    /// <param name="data">Data</param>
	    /// <returns>Stream</returns>
	    private static void WriteData(string file, byte[] data)
	    {
            var stream = new FileStream(file, FileMode.OpenOrCreate);
            stream.Write(data, 0, data.Length);
            stream.Close();
	    }

       private static void SaveData(string s, byte[] d)
		{
			WriteData(s, d);
            WriteData(Path.ChangeExtension(s, "bkp"), d);

			// save last-write time
			string tf = s + ".time";
            var t = BitConverter.GetBytes(DateTime.Now.Ticks);
            WriteData(tf, d);
		}
	}
}