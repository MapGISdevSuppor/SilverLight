using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Browser;

namespace Logistics_system
{
    public class checkTextLegality
    {


        

        public bool checkNumIsEmpty(string NumberValue)
        {
            if (NumberValue == null || NumberValue == "")
            {
                MessageBox.Show("输入不能为空！");
                return false;

            }
            else
            {
                if (!m_blnIsNUmber(NumberValue))
                {
                    MessageBox.Show("输入字符必须为数字！");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool checkTextIsEmpty(string ChineseLetterValue)
        {
            if (ChineseLetterValue == null || ChineseLetterValue == "")
            {
                MessageBox.Show("输入不能为空！");
                return false;
            }
            else
            {
                if (!this.IsChineseLetter(ChineseLetterValue))
                {
                    MessageBox.Show("输入字符必须中文！");
                    return false;

                }
                else
                {
                    return true;
                }
            }
        }
        private   bool m_blnIsNUmber(string p_strVaule)
        {

            Regex m_regex = new System.Text.RegularExpressions.Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");
            return m_regex.IsMatch(p_strVaule);

        }
        /// <summary>
        /// 判断是否为中文
        /// </summary>
        /// <param name="input">关键字</param>
        /// <returns></returns>
        private bool IsChineseLetter(string CString)
        {
            bool BoolValue = true;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    return BoolValue = false;
                }
            }
            return BoolValue;
        }
    }
}
