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
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Interface;

namespace SilverlightDemo.components
{
    /// <summary>
    /// 标注接口
    /// </summary>
    public interface ImyMark
    {
        /// <summary>
        /// 定位时需要偏移的x轴像素
        /// </summary>
        double OffsetX { get; set; }
        /// <summary>
        /// 定位时需要偏移的y轴像素
        /// </summary>
        double OffsetY { get; set; }
        /// <summary>
        /// 在相对于标注图层左距位置(标注隐藏时，该属性值无效)
        /// </summary>
        double Left { get; }
        /// <summary>
        /// 在相对于标注图层上距位置(标注隐藏时，该属性值无效)
        /// </summary>
        double Top { get; }
        /// <summary>
        /// x坐标位置
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// y坐标位置
        /// </summary>
        double Y { get; set; }
        /// <summary>
        ///  获取该标注是否在可视范围内
        /// </summary>
        bool IsInMapViewBound { get; }
        /// <summary>
        /// 关联的目标标注（一般用于聚合标注）
        /// </summary>
        ImyMark TargetMark { get; set; }
        /// <summary>
        /// 关联目标对象
        /// </summary>
        object Tag { get; set; } 
        /// <summary>
        /// 是否允许拖拽标注
        /// </summary>
        bool EnableDrag { get; set; }
        /// <summary>
        /// 标注图层对象
        /// </summary>
        MarkLayer MarkLayer { get; }
        /// <summary>
        /// 是否允许在地图变化时修正标注（注意：只在坐标系为逻辑坐标系时生效，默认允许）
        /// </summary>
        bool EnableRevisedPos { get; set; }
        /// <summary>
        /// 要显示的标注点元素对象
        /// </summary>
        FrameworkElement MarkControl { get; }
        /// <summary>
        /// 设置是否可见
        /// </summary>
        Visibility Visibility { get; set; }
        /// <summary>
        /// 设置坐标类型（默认是屏幕坐标，如果是逻辑坐标，在绘图时会根据地图容器的LogicToScreen方法转换坐标）
        /// </summary>
        CoordinateType CoordinateType { get; set; }
        /// <summary>
        /// 修正位置（只在坐标系是逻辑坐标时有效）
        /// </summary>
        /// <param name="isZoom">是否是放缩时修正位置</param>
        /// <param name="enableMarkHiden">是否允许判断并隐藏不在可视范围的标注</param>
        void RevisedPosition(bool isZoom = true, bool enableMarkHiden = true);
        /// <summary>
        /// 闪烁图形
        /// </summary>
        /// <param name="timeSpan">时间间隔(毫秒单位)</param>
        /// <param name="num">闪烁次数(如果传入“-1”将一直闪烁，传入“0”停止闪烁)</param>
        void Flicker(int timeSpan = 500, int num = 6);
        /// <summary>
        /// 停止闪烁
        /// </summary>
        void FlickerStop();
        /// <summary>
        /// 释放相关资源
        /// </summary>
        void Dispose();
    }
}
