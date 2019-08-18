using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Threading;

namespace inp2k
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileInf = string.Empty;
            if (args.Length>0)
            {
                fileInf = args[0];

            }
            else
            {
                Console.WriteLine("请输入要转换的Abqus文件全路径：\n");
                fileInf = Console.ReadLine();
                while (!File.Exists(fileInf)|| !fileInf.EndsWith(".inp"))
                {
                    Console.WriteLine("输入文件路径错误或文件格式错误，请重新输入：\n");
                    fileInf = Console.ReadLine();
                }
            }
            string endLine = GetEndline(fileInf);
        }
        public static string GetEndline(string filepath)
        {
            string outFileName = filepath.Split('.')[0] + ".k";
            StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
            FileStream fsMyFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
            StreamWriter swMyFile = new StreamWriter(fsMyFile);

            string LineStr = string.Empty;
            int typeFlag = 0;
            int startFlag = 0;
            int pid = 0;
            bool recordFlag = false;
            swMyFile.WriteLine("*KEYWORD");
            while (!sr.EndOfStream)//获取最后一行数据
            {
                LineStr = sr.ReadLine();
                LineStr = LineStr.Replace(" ", "");
                if (LineStr.Equals("*Node"))
                {
                    typeFlag = 1;
                    recordFlag = true;
                    swMyFile.WriteLine(LineStr);
  
                    continue;
                }
                else if (LineStr.Equals("*Element,type=T3D2"))
                {
                    typeFlag = 2;
                    recordFlag = true;
                    swMyFile.WriteLine("*ELEMENT_BEAM");
                    pid++;
                    continue;

                }
                else if (LineStr.Equals("*Element,type=C3D4"))
                {

                    typeFlag = 3;
                    recordFlag = true;
                    swMyFile.WriteLine("*ELEMENT_SOLID");
                    pid++;
                    continue;

                }
                else if (LineStr.StartsWith("*"))
                {
                    typeFlag = 0;
                    recordFlag = false;
                    continue;
                }
                if (recordFlag)
                {
                    switch (typeFlag)
                    {
                        case 1:
                            {
                                swMyFile.WriteLine(LineStr);
                            }
                            break;
                        case 2:
                            {
                                //LineStr = new Regex("[\\s]+").Replace(LineStr, " ");//正则表达式去除多余空格
                                LineStr = LineStr.Replace(" ","");
                                string[] origiArray = LineStr.Split(',');
                                string convertLine = string.Empty;
                                convertLine += origiArray[0] +","+ pid.ToString();
                                for (int i = 1; i < origiArray.Length; i++)
                                {
                                    convertLine += "," + origiArray[i];
                                }
                                convertLine += ",1,0,0,0,0";
                                swMyFile.WriteLine(convertLine);
                            }
                            break;
                        case 3:
                            {
                                LineStr = LineStr.Replace(" ", "");
                                string[] origiArray = LineStr.Split(',');
                                string convertLine = string.Empty;
                                convertLine += origiArray[0] + "," + pid.ToString();
                                for (int i = 1; i < origiArray.Length; i++)
                                {
                                    convertLine += "," + origiArray[i];
                                }
                                for (int j = 0; j < 4; j++)
                                {
                                    convertLine += "," + origiArray[origiArray.Length - 1];
                                }
                                swMyFile.WriteLine(convertLine);
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            swMyFile.WriteLine("*END");
            sr.Close();
            swMyFile.Flush();
            swMyFile.Close();
            fsMyFile.Close();
            string message = "文件转换完成，位置为：" + outFileName + "\n";
            Console.WriteLine(message);
            Console.WriteLine("按回车键结束程序...");
            Console.ReadLine();
            return LineStr;
        }

        public void WriteMessAppend(string FileName, string Path, string LineData, int AlignNO)
        {

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            string filepath = Path + FileName;//
            FileStream fsMyFile = new FileStream(filepath, FileMode.Append, FileAccess.Write);
            StreamWriter swMyFile = new StreamWriter(fsMyFile);
            swMyFile.WriteLine(LineData);
            swMyFile.Flush();
            swMyFile.Close();
            fsMyFile.Close();

        }
        public string AlignMixText(string MixText, int Length, int Alignment)//构建中英混合对齐方式
        {
            string tmItem = null;
            if (Length < Encoding.Default.GetByteCount(MixText))
            {
                Length = Encoding.Default.GetByteCount(MixText);
            }
            try
            {
                if (Alignment == 1)//右对齐
                {
                    tmItem = new string(' ', Length - Encoding.Default.GetByteCount(MixText)) + MixText;
                }
                else//左对齐
                {
                    tmItem = MixText + new string(' ', Length - Encoding.Default.GetByteCount(MixText));
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return tmItem;
        }

    }
}
