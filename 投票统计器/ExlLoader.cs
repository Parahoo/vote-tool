using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace 投票统计器
{
    class ExlLoader
    {
        public static List<VotUser> LoadFromFile(string filename)
        {
            List<VotUser> usrs = new List<VotUser>();

            return null;
        }

        public static void  SaveToFile(string filename, List<VotUser> usrs)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);    //创建工作簿（WorkBook：即Excel文件主体本身）  
            Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];   //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出  

            //excelWS.Cells.NumberFormat = "@";     //  如果数据中存在数字类型 可以让它变文本格式显示  
            //将数据导入到工作表的单元格  
            for (int i = 0; i < usrs.Count; i++)
            {
                    excelWS.Cells[i + 1, 1] = usrs[i].UserName;   //Excel单元格第一个从索引1开始  
                    excelWS.Cells[i + 1, 2] = usrs[i].CountNum.ToString();   //Excel单元格第一个从索引1开始  
            }

            if (File.Exists(filename))
                File.Delete(filename);
            excelWB.SaveAs(filename);  //将其进行保存到指定的路径  
            excelWB.Close();
            excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程  
        }
    }
}
