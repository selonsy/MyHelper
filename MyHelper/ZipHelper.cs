using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Devin
{
    public static class ZipHelper
    {
        //调用示例
        private static void MyInstance()
        {
            string startPath = @"c:\example\start";
            string zipPath = @"c:\example\result.zip";
            string extractPath = @"c:\example\extract";

            ZipFile.CreateFromDirectory(startPath, zipPath);

            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        
        /// <summary>
        /// 压缩文件，将指定文件夹中的文件压缩成一个指定文件名的压缩文件
        /// </summary>
        /// <param name="sourceDirectoryName">源目标文件夹</param>
        /// <param name="destinationArchiveFileName">目标压缩文件</param>
        public static void Zip(string sourceDirectoryName, string destinationArchiveFileName)
        {
            try
            {
                ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
            }
            catch(Exception ex)
            {
                LogHelper.WriteException(ex);
            }
        }
        
        /// <summary>
        /// 解压文件，将指定的压缩文件解压到指定的文件夹
        /// </summary>
        /// <param name="sourceArchiveFileName">源压缩文件</param>
        /// <param name="destinationDirectoryName">目标文件夹</param>
        public static void UnZip(string sourceArchiveFileName, string destinationDirectoryName)
        {            
            try
            {
                ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
            }
        }

        /// <summary>
        /// 添加文件到指定的压缩文件中，待实现
        /// </summary>
        public static void AddFileToZip() 
        {
            
        }

        /// <summary>
        /// 从压缩文件中获取指定名称的文件，待实现
        /// </summary>
        public static void GetFileFormZip() 
        { 
            
        }

        ///// <summary>
        ///// 支持多文件和多目录，或是多文件和多目录一起压缩
        ///// </summary>
        ///// <param name="list">待压缩的文件或目录集合</param>
        ///// <param name="strZipName">压缩后的文件名</param>
        ///// <param name="IsDirStruct">是否按目录结构压缩</param>
        ///// <returns>成功：true/失败：false</returns>
        //public static bool CompressMulti(List<string> list, string strZipName, bool IsDirStruct)
        //{
        //    try
        //    {
        //        using (ZipFile zip = new ZipFile(Encoding.Default))//设置编码，解决压缩文件时中文乱码
        //        {
        //            foreach (string path in list)
        //            {
        //                string fileName = Path.GetFileName(path);//取目录名称
        //                //如果是目录
        //                if (Directory.Exists(path))
        //                {
        //                    if (IsDirStruct)//按目录结构压缩
        //                    {
        //                        zip.AddDirectory(path, fileName);
        //                    }
        //                    else//目录下的文件都压缩到Zip的根目录
        //                    {
        //                        zip.AddDirectory(path);
        //                    }
        //                }
        //                if (File.Exists(path))//如果是文件
        //                {
        //                    zip.AddFile(path);
        //                }
        //            }
        //            zip.Save(strZipName);//压缩
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 解压ZIP文件
        ///// </summary>
        ///// <param name="strZipPath">待解压的ZIP文件</param>
        ///// <param name="strUnZipPath">解压的目录</param>
        ///// <param name="overWrite">是否覆盖</param>
        ///// <returns>成功：true/失败：false</returns>
        //public static bool Decompression(string strZipPath, string strUnZipPath, bool overWrite)
        //{
        //    try
        //    {
        //        ReadOptions options = new ReadOptions();
        //        options.Encoding = Encoding.Default;//设置编码，解决解压文件时中文乱码
        //        using (ZipFile zip = ZipFile.Read(strZipPath, options))
        //        {
        //            foreach (ZipEntry entry in zip)
        //            {
        //                if (string.IsNullOrEmpty(strUnZipPath))
        //                {
        //                    strUnZipPath = strZipPath.Split('.').First();
        //                }
        //                if (overWrite)
        //                {
        //                    entry.Extract(strUnZipPath, ExtractExistingFileAction.OverwriteSilently);//解压文件，如果已存在就覆盖
        //                }
        //                else
        //                {
        //                    entry.Extract(strUnZipPath, ExtractExistingFileAction.DoNotOverwrite);//解压文件，如果已存在不覆盖
        //                }
        //            }
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

    }

    /// <summary>
    /// Zip操作基于 DotNetZip 的封装
    /// </summary>
    //public static class ZipUtils
    //{
    //    /// <summary>
    //    /// 得到指定的输入流的ZIP压缩流对象【原有流对象不会改变】
    //    /// </summary>
    //    /// <param name="sourceStream"></param>
    //    /// <returns></returns>
    //    public static Stream ZipCompress(Stream sourceStream, string entryName = "zip")
    //    {
    //        MemoryStream compressedStream = new MemoryStream();
    //        if (sourceStream != null)
    //        {
    //            long sourceOldPosition = 0;
    //            try
    //            {
    //                sourceOldPosition = sourceStream.Position;
    //                sourceStream.Position = 0;
    //                using (ZipFile zip = new ZipFile())
    //                {
    //                    zip.AddEntry(entryName, sourceStream);
    //                    zip.Save(compressedStream);
    //                    compressedStream.Position = 0;
    //                }
    //            }
    //            catch
    //            {
    //            }
    //            finally
    //            {
    //                try
    //                {
    //                    sourceStream.Position = sourceOldPosition;
    //                }
    //                catch
    //                {
    //                }
    //            }
    //        }
    //        return compressedStream;
    //    }

    //    /// <summary>
    //    /// 得到指定的字节数组的ZIP解压流对象
    //    /// 当前方法仅适用于只有一个压缩文件的压缩包，即方法内只取压缩包中的第一个压缩文件
    //    /// </summary>
    //    /// <param name="sourceStream"></param>
    //    /// <returns></returns>
    //    public static Stream ZipDecompress(byte[] data)
    //    {
    //        Stream decompressedStream = new MemoryStream();
    //        if (data != null)
    //        {
    //            try
    //            {
    //                MemoryStream dataStream = new MemoryStream(data);
    //                using (ZipFile zip = ZipFile.Read(dataStream))
    //                {
    //                    if (zip.Entries.Count > 0)
    //                    {
    //                        zip.Entries.First().Extract(decompressedStream);
    //                        // Extract方法中会操作ms，后续使用时必须先将Stream位置归零，否则会导致后续读取不到任何数据
    //                        // 返回该Stream对象之前进行一次位置归零动作
    //                        decompressedStream.Position = 0;
    //                    }
    //                }
    //            }
    //            catch
    //            {
    //            }
    //        }
    //        return decompressedStream;
    //    }
    //}


    //需要引用以下的dll  还是可以的。
    //ICSharpCode.SharpZipLib

    /// <summary>
    /// Zip压缩与解压缩 
    /// </summary>
    //public class ZipHelper1
    //{
    //    /// <summary>
    //    /// 压缩单个文件
    //    /// </summary>
    //    /// <param name="fileToZip">要压缩的文件</param>
    //    /// <param name="zipedFile">压缩后的文件</param>
    //    /// <param name="compressionLevel">压缩等级</param>
    //    /// <param name="blockSize">每次写入大小</param>
    //    public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
    //    {
    //        //如果文件没有找到，则报错
    //        if (!System.IO.File.Exists(fileToZip))
    //        {
    //            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
    //        }

    //        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
    //        {
    //            using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
    //            {
    //                using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
    //                {
    //                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

    //                    ZipEntry ZipEntry = new ZipEntry(fileName);

    //                    ZipStream.PutNextEntry(ZipEntry);

    //                    ZipStream.SetLevel(compressionLevel);

    //                    byte[] buffer = new byte[blockSize];

    //                    int sizeRead = 0;

    //                    try
    //                    {
    //                        do
    //                        {
    //                            sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
    //                            ZipStream.Write(buffer, 0, sizeRead);
    //                        }
    //                        while (sizeRead > 0);
    //                    }
    //                    catch (System.Exception ex)
    //                    {
    //                        throw ex;
    //                    }

    //                    StreamToZip.Close();
    //                }

    //                ZipStream.Finish();
    //                ZipStream.Close();
    //            }

    //            ZipFile.Close();
    //        }
    //    }

    //    /// <summary>
    //    /// 压缩单个文件
    //    /// </summary>
    //    /// <param name="fileToZip">要进行压缩的文件名</param>
    //    /// <param name="zipedFile">压缩后生成的压缩文件名</param>
    //    public static void ZipFile(string fileToZip, string zipedFile)
    //    {
    //        //如果文件没有找到，则报错
    //        if (!File.Exists(fileToZip))
    //        {
    //            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
    //        }

    //        using (FileStream fs = File.OpenRead(fileToZip))
    //        {
    //            byte[] buffer = new byte[fs.Length];
    //            fs.Read(buffer, 0, buffer.Length);
    //            fs.Close();

    //            using (FileStream ZipFile = File.Create(zipedFile))
    //            {
    //                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
    //                {
    //                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
    //                    ZipEntry ZipEntry = new ZipEntry(fileName);
    //                    ZipStream.PutNextEntry(ZipEntry);
    //                    ZipStream.SetLevel(5);

    //                    ZipStream.Write(buffer, 0, buffer.Length);
    //                    ZipStream.Finish();
    //                    ZipStream.Close();
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 压缩多层目录
    //    /// </summary>
    //    /// <param name="strDirectory">The directory.</param>
    //    /// <param name="zipedFile">The ziped file.</param>
    //    public static void ZipFileDirectory(string strDirectory, string zipedFile)
    //    {
    //        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
    //        {
    //            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
    //            {
    //                ZipSetp(strDirectory, s, "");
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 递归遍历目录
    //    /// </summary>
    //    /// <param name="strDirectory">The directory.</param>
    //    /// <param name="s">The ZipOutputStream Object.</param>
    //    /// <param name="parentPath">The parent path.</param>
    //    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
    //    {
    //        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
    //        {
    //            strDirectory += Path.DirectorySeparatorChar;
    //        }
    //        Crc32 crc = new Crc32();

    //        string[] filenames = Directory.GetFileSystemEntries(strDirectory);

    //        foreach (string file in filenames)// 遍历所有的文件和目录
    //        {

    //            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
    //            {
    //                string pPath = parentPath;
    //                pPath += file.Substring(file.LastIndexOf("\\") + 1);
    //                pPath += "\\";
    //                ZipSetp(file, s, pPath);
    //            }

    //            else // 否则直接压缩文件
    //            {
    //                //打开压缩文件
    //                using (FileStream fs = File.OpenRead(file))
    //                {

    //                    byte[] buffer = new byte[fs.Length];
    //                    fs.Read(buffer, 0, buffer.Length);

    //                    string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
    //                    ZipEntry entry = new ZipEntry(fileName);

    //                    entry.DateTime = DateTime.Now;
    //                    entry.Size = fs.Length;

    //                    fs.Close();

    //                    crc.Reset();
    //                    crc.Update(buffer);

    //                    entry.Crc = crc.Value;
    //                    s.PutNextEntry(entry);

    //                    s.Write(buffer, 0, buffer.Length);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 解压缩一个 zip 文件。
    //    /// </summary>
    //    /// <param name="zipedFile">The ziped file.</param>
    //    /// <param name="strDirectory">The STR directory.</param>
    //    /// <param name="password">zip 文件的密码。</param>
    //    /// <param name="overWrite">是否覆盖已存在的文件。</param>
    //    public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
    //    {

    //        if (strDirectory == "")
    //            strDirectory = Directory.GetCurrentDirectory();
    //        if (!strDirectory.EndsWith("\\"))
    //            strDirectory = strDirectory + "\\";

    //        using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
    //        {
    //            s.Password = password;
    //            ZipEntry theEntry;

    //            while ((theEntry = s.GetNextEntry()) != null)
    //            {
    //                string directoryName = "";
    //                string pathToZip = "";
    //                pathToZip = theEntry.Name;

    //                if (pathToZip != "")
    //                    directoryName = Path.GetDirectoryName(pathToZip) + "\\";

    //                string fileName = Path.GetFileName(pathToZip);

    //                Directory.CreateDirectory(strDirectory + directoryName);

    //                if (fileName != "")
    //                {
    //                    if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
    //                    {
    //                        using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
    //                        {
    //                            int size = 2048;
    //                            byte[] data = new byte[2048];
    //                            while (true)
    //                            {
    //                                size = s.Read(data, 0, data.Length);

    //                                if (size > 0)
    //                                    streamWriter.Write(data, 0, size);
    //                                else
    //                                    break;
    //                            }
    //                            streamWriter.Close();
    //                        }
    //                    }
    //                }
    //            }

    //            s.Close();
    //        }
    //    }

    //}
}