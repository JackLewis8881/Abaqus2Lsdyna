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
            if (args.Length>0) //判断程序是否有参数传入（拖动文件至程序是一种传入参数的行为）
            {
                fileInf = args[0];//获取传入的第一个参数

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
            string outFileName=convert.convert2Kfile(fileInf);//调用转换函数
            string message = "文件转换完成，位置为：" + outFileName + "\n";
            Console.WriteLine(message);
            Console.WriteLine("按回车键结束程序...");
            Console.ReadLine();
        }
    }
}
