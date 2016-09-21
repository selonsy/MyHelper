// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016年9月21日20:48:09  
// Modifyed：selonsy  
// ModifyTime：2016年9月21日20:48:15  
// Desc：
// 压缩帮助类,使用DotNet原生方法.
// 命名空间:System.IO.Compression 类:ZipFile
// </summary> 

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;

namespace Devin
{
    /// <summary>
    /// Zip帮助类:System.IO.Compression.ZipFile
    /// </summary>
    public static class ZipHelper1
    {
        /// <summary>
        /// 压缩文件，将指定文件夹中的文件压缩成一个指定文件名的压缩文件
        /// </summary>
        /// <param name="sourceDirectoryName">源目标文件夹</param>
        /// <param name="destinationArchiveFileName">目标压缩文件</param>
        public static void Zip(string sourceDirectoryName, string destinationArchiveFileName)
        {
            try
            {
                System.IO.Compression.ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
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
                System.IO.Compression.ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
            }
        }

        //Todo:添加文件到指定的压缩文件中，待实现
        /// <summary>
        /// 添加文件到指定的压缩文件中
        /// </summary>
        public static void AddFileToZip() 
        {
            
        }

        //Todo:添加文件到指定的压缩文件中，待实现
        /// <summary>
        /// 从压缩文件中获取指定名称的文件
        /// </summary>
        public static void GetFileFromZip() 
        { 
            
        }

        //DotNetZip https://dotnetzip.codeplex.com/
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
        //        //using (ZipFile zip = new ZipFile(Encoding.Default))//设置编码，解决压缩文件时中文乱码
        //        //{
        //        //    foreach (string path in list)
        //        //    {
        //        //        string fileName = Path.GetFileName(path);//取目录名称
        //        //        //如果是目录
        //        //        if (Directory.Exists(path))
        //        //        {
        //        //            if (IsDirStruct)//按目录结构压缩
        //        //            {
        //        //                zip.AddDirectory(path, fileName);
        //        //            }
        //        //            else//目录下的文件都压缩到Zip的根目录
        //        //            {
        //        //                zip.AddDirectory(path);
        //        //            }
        //        //        }
        //        //        if (File.Exists(path))//如果是文件
        //        //        {
        //        //            zip.AddFile(path);
        //        //        }
        //        //    }
        //        //    zip.Save(strZipName);//压缩
        //        //    return true;
        //        //}
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
}
