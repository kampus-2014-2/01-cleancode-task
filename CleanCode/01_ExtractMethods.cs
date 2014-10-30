using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string s, byte[] d)
		{
			//open files
			var fs1 = new FileStream(s, FileMode.OpenOrCreate);
			var fs2 = new FileStream(Path.ChangeExtension(s, "bkp"), FileMode.OpenOrCreate);

			// write data
			fs1.Write(d, 0, d.Length);
			fs2.Write(d, 0, d.Length);

			// close files
			fs1.Close();
			fs2.Close();

			// save last-write time
			string tf = s + ".time";
			var fs3 = new FileStream(tf, FileMode.OpenOrCreate);
			var t = BitConverter.GetBytes(DateTime.Now.Ticks);
			fs3.Write(t, 0, t.Length);
			fs3.Close();
		}
	}
}