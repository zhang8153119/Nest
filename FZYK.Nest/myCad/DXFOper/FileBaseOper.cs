using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.DXFOper
{
    public class FileBaseOper
    {
        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="prompt">提示</param>
        /// <returns></returns>
        protected string select(string prompt)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = prompt;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据文件类型选择文件
        /// </summary>
        /// <param name="prompt">选择提示</param>
        /// <param name="fileClass">选择的文件类型</param>
        protected string select(string prompt, string[] fileClass)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = prompt;
            string filter = "";         //过滤器
            for (int i = 0; i < fileClass.Length; i++)
            {
                filter = filter + "*." + fileClass[i] + "|*." + fileClass[i];
                if (i != fileClass.Length - 1)
                {
                    filter = filter + "|";
                }
            }
            op.Filter = filter;
            if (op.ShowDialog() == DialogResult.OK)
            {
                return op.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 批量返回选择的文件
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="fileClass"></param>
        /// <returns></returns>
        protected List<string> selectList(string prompt, string[] fileClass)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = prompt;
            op.Multiselect = true;
            string filter = "";         //过滤器
            for (int i = 0; i < fileClass.Length; i++)
            {
                filter = filter + "*." + fileClass[i] + "|*." + fileClass[i];
                if (i != fileClass.Length - 1)
                {
                    filter = filter + "|";
                }
            }
            op.Filter = filter;
            if (op.ShowDialog() == DialogResult.OK)
            {
                string[] listFileInfo = op.FileNames;
                return op.FileNames.ToList<string>();
            }
            else
            {
                return new List<string>();
            }
        }


        protected void open()
        {

        }

        protected void create(string fileUrl)
        {

        }

        protected void copy()
        {

        }
    }
}
