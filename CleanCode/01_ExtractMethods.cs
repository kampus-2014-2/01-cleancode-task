using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string fileName, byte[] data)
		{
		    using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
		    {
		        var backupFileName = Path.ChangeExtension(fileName, "bkp");
		        using (var backupFileStream = new FileStream(backupFileName, FileMode.OpenOrCreate))
		        {
                    fileStream.Write(data, 0, data.Length);
                    backupFileStream.Write(data, 0, data.Length);
		        }
		    }
		    SaveTime(fileName);	
		}

	    private static void SaveTime(string fileName)
	    {
            var completeFileName = fileName + ".time";
	        using (var fileStream = new FileStream(completeFileName, FileMode.OpenOrCreate))
	        {	            	        
	            var time = BitConverter.GetBytes(DateTime.Now.Ticks);
	            fileStream.Write(time, 0, time.Length);
	        }
	        
	    }
	}
}