using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class IOManager {

    public static void SaveFile(string path, string content) {
	
		using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
		
			byte[] bytes = Encoding.Default.GetBytes(content);

			fs.SetLength(0);

			fs.Write(bytes, 0, bytes.Length);
		
		}
	
	}

    public static void SaveFileLanguage(string path, string content) {
    
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {

            //Encoding.

            byte[] bytes = UTF8Encoding.UTF8.GetBytes(content);

            fs.Write(bytes, 0, bytes.Length);

        }
    
    }

	public static void CopyDirectory(string srcdir, string desdir)
	{

		Debug.Log(srcdir);

	    string[] filenames = Directory.GetFileSystemEntries(srcdir);

	    foreach (string file in filenames)
	    {
			string currentdir = file + "\\";

			if (Directory.Exists(currentdir))
	        {
				string name = file.Substring(file.LastIndexOf("\\") + 1);

				Directory.CreateDirectory(desdir + name);

				CopyDirectory(currentdir, desdir + name);

			}else{
			
				string name = file.Substring(file.LastIndexOf("\\") + 1);

				File.Copy(file, desdir + name, true);
			
			}

//	        else // 否则直接copy文件
//	        {
//	            string srcfileName = file.Substring(file.LastIndexOf("\\")+1);
//
//				srcfileName = desdir + "\\" + srcfileName;
//
//
//				if (!Directory.Exists(desdir))
//	            {
//					Directory.CreateDirectory(desdir);
//	            }
//	            
//	            
//	            File.Copy(file, srcfileName);
//	        }
	    }
	}

	public static void SaveByte(string path, byte[] bytes) {

        FileInfo file = new FileInfo(path);
        file.Directory.Create();
		
		using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite)) {
			
			fs.Write(bytes, 0, bytes.Length);
			
		}
		
	}

	public static byte[] GetByte(string path) {

		byte[] bytes;

		using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {

			StreamReader streamReader = new StreamReader(fs);
			
			string content = streamReader.ReadToEnd();

			bytes = System.Text.Encoding.UTF8.GetBytes(content);

		}

		return bytes;
	
	}

    public static string GetResourceTxt(string path)
    {
        var ta = Resources.Load(path) as TextAsset;
        return ta.text;
    }

	public static string GetTxt(string path) {
	
		string content = "";

        if(File.Exists(path)) {

    		using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
    		
    			StreamReader streamReader = new StreamReader(fs);

    			content = streamReader.ReadToEnd();
    		
    		}
        }

		return content;
	
	}

	public static TextReader GetTextReader(string path) {
	
		FileStream fs = new FileStream (path, FileMode.Open, FileAccess.Read);
		
		StreamReader streamReader = new StreamReader(fs);

		return streamReader;

	}

    public static void GetDirectoryFiles(string srcdir, string strfilter) {

        string[] directories = Directory.GetDirectories(srcdir);

        string info = "";

        foreach(string directory in directories) {

            string[] filenames = Directory.GetFileSystemEntries(directory);

            foreach (string file in filenames)
            {
                if(!file.Contains("manifest") && !file.Contains("meta")) {
                
                    info += file + "\r\n";
                
                }

            }
        }

        Debug.Log("Info:" + info);

    }

    public static string[] GetFiles(string srcdir) {
    
        string[] files = Directory.GetFiles(srcdir);

        return files;
    
    }

	public static void DeleteFile(string path) {
	
		Directory.Delete(path, true);
	
	}

}
