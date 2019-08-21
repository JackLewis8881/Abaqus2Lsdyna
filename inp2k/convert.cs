using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace inp2k
{
    class convert
    {
        public static string convert2Kfile(string filepath)
        {
            string outFileName = filepath.Split('.')[0] + ".k";
            StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
            FileStream fsMyFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
            StreamWriter swMyFile = new StreamWriter(fsMyFile);

            string LineStr = string.Empty;
            int typeFlag = 0;
            //int startFlag = 0;
            int pid = 0;
            bool recordFlag = false;
            swMyFile.WriteLine("*KEYWORD");
            while (!sr.EndOfStream)//获取到最后一行数据
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
                                LineStr = LineStr.Replace(" ", "");
                                string[] origiArray = LineStr.Split(',');
                                string convertLine = string.Empty;
                                convertLine += origiArray[0] + "," + pid.ToString();
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
            return outFileName;
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
