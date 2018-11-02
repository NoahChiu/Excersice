using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace DeleteProgram
{
    class Delete
    {
        static void Main(string[] args)
        {
            try
            {
                // Console.WriteLine("請輸入目錄名稱:");
                String strDirectoryName = args[0]; // Console.ReadLine();
                if (Directory.Exists(strDirectoryName) == false )
                {
                    throw new OutofDirectory();//路徑輸入錯誤直接離開程式
                }
                if(strDirectoryName.Substring(strDirectoryName.Length - 1, 1) != "\\")
                {
                    strDirectoryName = strDirectoryName + "\\";
                }
                // Console.WriteLine("請輸入限制期限(日):");
                int time = int.Parse(args[1]);  //int.Parse(Console.ReadLine());
                if (time <= 0)
                {
                    throw new OutofRang();//數值輸入錯誤直接離開
                }

                // Console.WriteLine("請輸入附檔名:");
                string extension = args[2];  //int.Parse(Console.ReadLine());

                Delete myDelete = new Delete();
                myDelete.CheckFile(strDirectoryName, time, extension);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (OutofDirectory od)
            {
                Console.WriteLine(od.ToString());
                Console.WriteLine("請檢查路徑是否正確");
            }
            catch (OutofRang or)
            {
                Console.WriteLine(or.ToString());
            }

        }
        public void CheckFile(string Path, int Time, string Extension)
        {
            DirectoryInfo myDirectoryInfo = new DirectoryInfo(Path);
            Regex defaultRegex = new Regex(Extension);
            var AllFiles = myDirectoryInfo.GetFiles(); //取得目錄下所有檔案，為矩陣形式
            for (int i = 0; i < AllFiles.Length; i++)
            {
                string AllFileName = AllFiles[i].Name; //將名字取出
                DateTime dtCreate = AllFiles[i].CreationTime; //將建立時間取出
                string AllExtension = AllFiles[i].Extension; //取出附檔名

                TimeSpan ts = DateTime.Now - dtCreate;
                int differenceInDays = (int)ts.TotalDays; //"今日時間"減去"建立時間"後以"日"表示
                if (differenceInDays >= Time && defaultRegex.IsMatch(AllFileName + "." + AllExtension) == true)
                    File.Delete(Path + AllFileName); //相減大於指定時間且為指定副檔名則刪除
            }

            var CheckFolder = myDirectoryInfo.GetDirectories(); //檢查子目錄
            for (int k = 0; k < CheckFolder.Length; k++)
            {
                string Folder = CheckFolder[k].FullName+"\\";
                CheckFile(Folder, Time, Extension); //遞迴
            }
        }
    }
    class OutofDirectory : ApplicationException
    {
        string strMessage;
        public OutofDirectory() : base()
        {
            this.strMessage = "路徑輸入錯誤，此路徑不存在";
        }
        public override string ToString()
        {
            return strMessage;
        }
    }
    class OutofRang : ApplicationException
    {
        string strMessage;
        public OutofRang() : base()
        {
            this.strMessage = "數值輸入錯誤，不接受小於等於零數值";
        }
        public override string ToString()
        {
            return strMessage;
        }
    }
}
