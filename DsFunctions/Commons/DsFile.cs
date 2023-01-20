using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace DsFunctions.Commons
{
    public class DsFile
    {
        public static string Join(params string[] filepath)
        {
            string fullpath = "";
            foreach (string path in filepath)
            {
                fullpath += path + "\\";
            }
            fullpath = fullpath.Substring(0, fullpath.Length - 1);

            if (fullpath.Substring(0, 2) == "\\\\")
            {
                return "\\\\" + fullpath.Substring(2).Replace("\\\\", "\\").Replace("\\\\", "\\");
            }
            else
            {
                return fullpath.Replace("\\\\", "\\").Replace("\\\\", "\\");
            }
        }

        public static bool ValidFileName(string s)
        {
            char[] invalidch = Path.GetInvalidFileNameChars();
            foreach (char c in invalidch)
            {
                if (s.IndexOf(c) >= 0) return false;
            }
            return true;
        }

        public static string GetXmlFileData(string FilePath)
        {
            if (FilePath == null || FilePath == string.Empty) return "";
            if (!File.Exists(FilePath)) return "";

            return File.ReadAllText(FilePath);
        }

        public static MemoryStream GetXmlToStream(string XmlData)
        {
            if (XmlData == null || XmlData == String.Empty) return null;
            byte[] ba = Encoding.UTF8.GetBytes(XmlData);
            return new MemoryStream(ba);
        }

        public static long GetDirectorySize(string path)
        {
            long size = 0;
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                size += fi.Length;
            }

            return size;
        }

        public static string FindFilePath(string path, bool showError)
        {
            string text = "\\";
            for (int i = 0; i <= 10; i++)
            {
                if (File.Exists(Application.StartupPath + text + path))
                {
                    return Application.StartupPath + text + path;
                }

                text += "..\\";
            }

            if (showError)
            {
                MessageBox.Show("File " + path + " is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            return string.Empty;
        }

        public static DriveInfo[] GetDrives(bool isReadOnly = false)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            if (isReadOnly)
            {
                List<DriveInfo> lstDrive = new List<DriveInfo>();
                foreach (DriveInfo drvInfo in drives)
                {
                    if (drvInfo.DriveType == DriveType.CDRom)
                    {
                        lstDrive.Add(drvInfo);
                    }
                }
                return lstDrive.ToArray();
            }
            else
            {
                return drives;
            }

        }

        public static DriveInfo GetDrive(string driveName)
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Network)
                    {
                        if (drive.Name.Contains(driveName.ToUpper()))
                        {
                            return drive;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 폴더 생성
        /// </summary>
        /// <param name="FilePath"></param>
        public static bool CreateFolder(string FilePath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(FilePath);
                if (!di.Exists)
                    di.Create();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 파일이 없는 폴더를 삭제하는 함수
        /// </summary>
        /// <param name="folderPath"></param>
        public static bool DeleteBlankFolder(string folderPath, bool isRoot = true)
        {
            try
            {
                string[] getFolders = Directory.GetDirectories(folderPath);
                string[] getFiles = Directory.GetFiles(folderPath);

                if (getFolders.Length > 0)
                {
                    foreach (string fPath in getFolders)
                    {
                        bool bRet = DeleteBlankFolder(fPath, false);
                        if (!bRet) return bRet; // 한개 폴더라도 파일이 있으면 리턴한다. 
                    }
                }

                if (getFiles.Length == 0)
                {
                    if (!isRoot) DeleteFolder(folderPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 폴더를 강제로 삭제하는 함수
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeleteFolder(string folderPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);

                if (dir.Attributes == (FileAttributes.Directory | FileAttributes.ReadOnly))
                {
                    dir.Attributes = FileAttributes.Directory;
                }

                DirectoryInfo[] dirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);
                FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                foreach (DirectoryInfo dirItem in dirs)
                {
                    DeleteFolder(dirItem.FullName);
                }

                foreach (FileInfo file in files)
                {
                    file.Attributes = FileAttributes.Normal;
                }

                Directory.Delete(folderPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 파일삭제 하고 해당폴더에 파일이 없으면 폴더도 삭제하는 함수
        /// </summary>
        /// <param name="FilePath"></param>
        public static void DeleteFile(string FilePath)
        {
            try
            {
                File.Delete(FilePath);
                string FolderPath = Path.GetDirectoryName(FilePath);
                string[] FolderFiles = Directory.GetFiles(FolderPath);
                if (FolderFiles.Length == 0)
                {
                    Directory.Delete(FolderPath);
                }
            }
            catch (Exception ex)
            {
                // 보통 쓰기권한이 없을때 삭제가 안된다.
                Console.WriteLine(ex);
            }
        }

        public static void MoveFolder(string srcPath, string dstPath, bool OverWrite = true)
        {
            try
            {
                ProcCopyDir(srcPath, dstPath);
                DeleteFolder(srcPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 파일 이동하는 함수
        /// </summary>
        public static void MoveFile(string srcPath, string dstPath, bool OverWrite = true)
        {
            try
            {
                if (File.Exists(dstPath))
                {
                    if (OverWrite)
                    {
                        DeleteFile(dstPath);
                    }
                    else
                    {
                        return;
                    }
                }
                File.Move(srcPath, dstPath);
            }
            catch (Exception ex)
            {
                // 보통 쓰기권한이 없을때 삭제가 안된다.
                CreateFolder(Path.GetDirectoryName(dstPath));
                Console.WriteLine(ex);
                CopyFile(srcPath, dstPath, true);
                DeleteFile(srcPath);
            }
        }

        public static void CopyFile(string srcPath, string dstPath, bool OverWrite = true)
        {
            try
            {
                File.Copy(srcPath, dstPath, OverWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static string[] GetFilePathsFromFileInfoArray(FileInfo[] fileInfos)
        {
            List<string> lst = new List<string>();
            foreach (FileInfo fileInfo in fileInfos)
            {
                lst.Add(fileInfo.FullName);
            }
            return lst.ToArray();
        }

        public static int GetLocalStorageRatio(string LocalStroagePath)
        {
            try
            {
                if (LocalStroagePath == null) return 0;
                if (LocalStroagePath == string.Empty) return 0;

                string Drive = Path.GetPathRoot(LocalStroagePath).Substring(0, 1);
                if (Drive == "\\")
                {
                    return 0;
                }
                DriveInfo drvInfo = GetDrive(Drive);
                return Convert.ToInt32((Convert.ToSingle(drvInfo.TotalSize) - Convert.ToSingle(drvInfo.AvailableFreeSpace)) / Convert.ToSingle(drvInfo.TotalSize) * 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Path의 하위폴더들 중 파일만 가져와 리스트로 반환하는 함수
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Ext"></param>
        /// <returns></returns>
        public static List<string> GetFiles(string Path, List<string> lstFiles = null)
        {
            if (lstFiles == null)
            {
                lstFiles = new List<string>();
            }

            string[] getDirectory = Directory.GetDirectories(Path);
            string[] getFiles = Directory.GetFiles(Path);

            if (getFiles.Length > 0) lstFiles.AddRange(getFiles);
            if (getDirectory.Length > 0)
            {
                foreach (string dirPath in getDirectory)
                {
                    lstFiles = GetFiles(dirPath, lstFiles);
                }
            }

            return lstFiles;
        }

        public static string SearchFile(string path, string searchFileName)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    if (searchFileName.ToUpper() == Path.GetFileName(file).ToUpper())
                    {
                        return Path.Combine(path, file);
                    }
                }

                if (dirs.Length > 0)
                {
                    foreach (string dir in dirs)
                    {
                        return SearchFile(dir, searchFileName);
                    }
                }
            }
            catch
            {
                return "";
            }
            return "";
        }

        /// <summary>
        /// sourceFolder에서 destFolder로 하위폴더 및 파일을 포함한 모두를 복사
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        public static void ProcCopyDir(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);

                // 읽기전용이면 일반으로 변경
                FileAttributes fas = File.GetAttributes(dest);
                if ((fas & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(dest, FileAttributes.Normal);
                }
            }

            foreach (string folder in folders)
            {
                string namename = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, namename);
                ProcCopyDir(folder, dest);
            }
        }
    }
}
