using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;


namespace SilverlightDemo.components
{
    /// <summary>
    /// 聚合标注接口
    /// </summary>
    public interface ImyPolymericMark : ImyMark
    {
        /// <summary>
        /// 获取聚合标注列表
        /// </summary>
        List<ImyMark> MarkList { get; }
        /// <summary>
        /// 添加要聚合标注
        /// </summary>
        void AddMark(ImyMark mark);
        /// <summary>
        /// 删除要聚合标注
        /// </summary>
        /// <param name="mark">要删除标注对象</param>
        /// <returns>删除成功返回“true”，否则返回“false”</returns>
        bool RemoveMark(ImyMark mark);
        /// <summary>
        /// 删除所有聚合标注
        /// </summary>
        void RemoveAll();
    }
}
