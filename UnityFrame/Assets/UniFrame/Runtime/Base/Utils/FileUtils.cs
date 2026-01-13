using System;
using System.IO;
using UnityEngine;

public class FileUtils
{
    public static bool ReleaseFiles(string sourceDir, string[] dirOrFileList, string destDir)
    {
        for (int i = 0; i < dirOrFileList.Length; i++)
        {
            string text = sourceDir + "/" + dirOrFileList[i];
            string text2 = destDir + "/" + dirOrFileList[i];
            if (File.Exists(text))
            {
                CopyFile(text, text2);
            }
            else if (Directory.Exists(text))
            {
                CopyDirectory(text, text2);
            }
        }

        return true;
    }

    public static bool DeleteFiles(string destDir, string[] dirOrFileList)
    {
        for (int i = 0; i < dirOrFileList.Length; i++)
        {
            string text = destDir + "/" + dirOrFileList[i];
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            else if (Directory.Exists(text))
            {
                DeleteDirectory(text);
            }
        }

        return true;
    }

    public static bool CopyDirectory(string sDir, string dDir)
    {
        if (Directory.Exists(sDir))
        {
            try
            {
                if (!Directory.Exists(dDir))
                {
                    Directory.CreateDirectory(dDir);
                }

                string[] files = Directory.GetFiles(sDir, "*.*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    File.Copy(files[i], files[i].Replace(sDir, dDir), overwrite: true);
                }

                string[] directories = Directory.GetDirectories(sDir, "*.*", SearchOption.TopDirectoryOnly);
                for (int j = 0; j < directories.Length; j++)
                {
                    CopyDirectory(directories[j], directories[j].Replace(sDir, dDir));
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError((object)("CopyDirectory复制目录失败!s:" + sDir + ";;d:" + dDir));
                Debug.LogException(ex);
            }
        }
        else
        {
            Debug.LogError((object)("CopyDirectory:原始目录不存在." + sDir));
        }

        return false;
    }

    public static bool CopyFile(string source, string dest, bool isOverwrite = true)
    {
        try
        {
            if (File.Exists(source))
            {
                FileInfo fileInfo = new FileInfo(dest);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.Copy(source, dest, isOverwrite);
                return true;
            }

            Debug.LogError((object)("没有找到源文件:" + source));
        }
        catch (Exception ex)
        {
            Debug.LogError((object)("复制文件失败!s:" + source + ";;;d:" + dest));
            Debug.LogException(ex);
        }

        return false;
    }

    public static bool DeleteDirectory(string dir)
    {
        try
        {
            if (!Directory.Exists(dir))
            {
                return true;
            }

            string[] files = Directory.GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i]))
                {
                    File.Delete(files[i]);
                }
            }

            bool flag = true;
            string[] directories = Directory.GetDirectories(dir);
            for (int j = 0; j < directories.Length; j++)
            {
                flag = flag && DeleteDirectory(directories[j]);
            }

            if (flag)
            {
                string[] fileSystemEntries = Directory.GetFileSystemEntries(dir);
                if (fileSystemEntries.Length == 0)
                {
                    Directory.Delete(dir);
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError((object)("删除目录的内容时,出现异常:" + dir));
            Debug.LogException(ex);
            return false;
        }
    }
}