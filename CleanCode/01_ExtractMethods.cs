using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string s, byte[] d)
		{
            SaveFile(s, d);
            SaveFile(Path.ChangeExtension(s, "bkp"), d);
            AddTime(s);
		}

	    private static void SaveFile(string s, byte[] d)
	    {
	        var fs1 = new FileStream(s, FileMode.OpenOrCreate);
            fs1.Write(d, 0, d.Length);
            fs1.Close();
	    }

	    private static void AddTime(string s)
	    {
            string tf = s + ".time";
            var t = BitConverter.GetBytes(DateTime.Now.Ticks);
            SaveFile(tf, t);
	    }
	}
}