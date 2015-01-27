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
using ZDIMS.Interface;
using ZDIMS.Map;
using ZDIMS.Drawing;
using System.Collections.Generic;
using System.Windows.Threading;
using ZDIMS.Util;
using ZdimsExtends.DrawingArrow;
using System.ComponentModel;
using ZdimsExtends.style;
using ZdimsExtends.util;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class Military_Standard : Page
    {
        string TileLayerAddress = "";
        private DrawingBase drawingBase = new DrawingBase();
        public Military_Standard()
        {
            InitializeComponent();
            this.colorPicker1.ColorChangedOverCallBack += new ColorsPicker.SelectedColorHander(colorPicker1_ColorChangedOverCallBack);
            this.colorPicker2.ColorChangedOverCallBack += new ColorsPicker.SelectedColorHander(colorPicker2_ColorChangedOverCallBack);
        }
        //填充颜色
        void colorPicker1_ColorChangedOverCallBack(Color strColor)
        {
            if (drawingBase != null)
            {
                drawingBase.FillColor = strColor;
                drawingBase.reDrawByChangeStyle(true,false,false,false,false,false);
            }
        }
        //边框颜色
        void colorPicker2_ColorChangedOverCallBack(Color strColor)
        {
            if (drawingBase != null)
            {
                drawingBase.StrokeColor = strColor;
                drawingBase.reDrawByChangeStyle(false,false,false,true,false,false);
            }
        }
        //填充颜色透明度
        private void ChangeFillColorOpacity(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (drawingBase != null)
            {
                drawingBase.FillOpacity = ((Slider)sender).Value;
                drawingBase.reDrawByChangeStyle(false,true,false,false,false,false);
            }
        }
        //边框颜色透明度
        private void ChangeStorkeColorOpacity(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (drawingBase != null)
            {
                drawingBase.StrokeOpacity = ((Slider)sender).Value ;
                drawingBase.reDrawByChangeStyle(false,false,false,false,true,false);
            }
        }
        //边框宽度
        private void ChangeStorkeWeightOpacity(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (drawingBase != null)
            {
                drawingBase.StrokeWeight = ((Slider)sender).Value;
                drawingBase.reDrawByChangeStyle(false, false, true, false, false, false);
            }
        }
        //单个删除按钮
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (drawingBase != null)
            {
                drawingBase.ReMoveElement();
            }
        }
        //删除所有绘制的图形
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (drawingBase != null)
            {
                drawingBase.ReMoveAll();

            }
        }

        //简单箭头
        private void onLoadHandle(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();

            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.SimpleArrow);
        }
        //燕尾箭头
        private void TailedArrow_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.TailedArrow);
        }
        //自定义箭头
        private void CustomArrow_Click(object sender, MouseButtonEventArgs e)
        {

            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.CustomArrow);

        }
        //自定义燕尾箭头
        private void CustomTailedArrow_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.CustomTailedArrow);
        }
        //直箭头
        private void StraightArrow_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.StraightArrow);
        }
        //双箭头
        private void DoubleArrow_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.DoubleArrow);

        }
        //集结区域
        private void AssemblyArea_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.AssemblyArea);
        }
        //圆形区域
        private void Circle_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.Circle);
        }
        //曲线旗标
        private void CurveFlag_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.CurveFlag);
        }
        //直角旗标
        private void RectFlag_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.RectFlag);
        }
        //三角旗标
        private void TriangleFlag_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.TriangleFlag);
        }
        //尖角指北针
        private void ClosedangleCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.ClosedangleCompass);
        }
        //四角指北针
        private void FourstarCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.FourstarCompass);
        }
        //菱形指北针
        private void RhombusCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.RhombusCompass);
        }
        //三角指北针
        private void TriangleCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.TriangleCompass);
        }
        //十字箭头指北针
        private void ArrowcrossCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.ArrowcrossCompass);
        }
        //圆形尖角指北针
        private void CircleClosedangleCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.CircleClosedangleCompass);
        }
        //双向尖角指北针
        private void DoubleClosedangleCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.DoubleClosedangleCompass);
        }
        //同向尖角指北针
        private void SameDirectionClosedangleCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.SameDirectionClosedangleCompass);
        }
        //风向标指北针
        private void VaneCompass_Click(object sender, MouseButtonEventArgs e)
        {
            if (drawingBase == null)
            {
                drawingBase = new DrawingBase();
            }
            drawingBase.enableChangeStyleCompass = true;
            drawingBase.imsMap = this.iMSMap1;
            drawingBase.initDraw(DrawingBaseTyep.VaneCompass);
        }
        //imageEnter1--imageEnter20均为鼠标移入到相关按钮上更改图片
        private void imageEnter1(object sender, MouseEventArgs e)
        {   
            this.image01.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/arrowTemp.png", UriKind.Relative));
        }
        //imageLeave1--imageLeave20均为鼠标移出当前按钮更改图片
        private void imageLeave1(object sender, MouseEventArgs e)
        {
            this.image01.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/arrow.png", UriKind.Relative));
        }
        private void imageEnter2(object sender, MouseEventArgs e)
        {     
            this.image02.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/tail_arrowTemp.png", UriKind.Relative));
        }

        private void imageLeave2(object sender, MouseEventArgs e)
        {
            this.image02.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/tail_arrow.png", UriKind.Relative));
        }

        private void imageEnter3(object sender, MouseEventArgs e)
        {   
            this.image03.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/custom_arrowTemp.png", UriKind.Relative));
        }

        private void imageLeave3(object sender, MouseEventArgs e)
        {
            this.image03.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/custom_arrow.png", UriKind.Relative));
        }
        private void imageEnter4(object sender, MouseEventArgs e)
        {     
            this.image04.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/custom_tail_arrowTemp.png", UriKind.Relative));
        }

        private void imageLeave4(object sender, MouseEventArgs e)
        {
            this.image04.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/custom_tail_arrow.png", UriKind.Relative));
        }

        private void imageEnter5(object sender, MouseEventArgs e)
        {     
            this.image05.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/stright_arrowTemp.png", UriKind.Relative));
        }

        private void imageLeave5(object sender, MouseEventArgs e)
        {
            this.image05.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/stright_arrow.png", UriKind.Relative));
        }

        private void imageEnter6(object sender, MouseEventArgs e)
        {  
            this.image06.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/double_arrowTemp.png", UriKind.Relative));
        }

        private void imageLeave6(object sender, MouseEventArgs e)
        {
            this.image06.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/double_arrow.png", UriKind.Relative));
        }

        private void imageEnter7(object sender, MouseEventArgs e)
        {    
            this.image07.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/assemblyTemp.png", UriKind.Relative));
        }

        private void imageLeave7(object sender, MouseEventArgs e)
        {
            this.image07.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/assembly.png", UriKind.Relative));
        }

        private void imageEnter8(object sender, MouseEventArgs e)
        {   
            this.image08.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/circleTemp.png", UriKind.Relative));
        }

        private void imageLeave8(object sender, MouseEventArgs e)
        {
            this.image08.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/circle.png", UriKind.Relative));
        }
        private void imageEnter9(object sender, MouseEventArgs e)
        {
            this.image09.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/curve_flagTemp.png", UriKind.Relative));
        }

        private void imageLeave9(object sender, MouseEventArgs e)
        {
            this.image09.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/curve_flag.png", UriKind.Relative));
        }

        private void imageEnter10(object sender, MouseEventArgs e)
        {    
            this.image10.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/rect_flagTemp.png", UriKind.Relative));
        }

        private void imageLeave10(object sender, MouseEventArgs e)
        {
            this.image10.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/rect_flag.png", UriKind.Relative));
        }
        private void imageEnter11(object sender, MouseEventArgs e)
        {  
            this.image11.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/triangle_flagTemp.png", UriKind.Relative));
        }

        private void imageLeave11(object sender, MouseEventArgs e)
        {
            this.image11.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/triangle_flag.png", UriKind.Relative));
        }

        private void imageEnter12(object sender, MouseEventArgs e)
        {    
            this.image12.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/closedangleTemp.png", UriKind.Relative));
        }

        private void imageLeave12(object sender, MouseEventArgs e)
        {
            this.image12.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/closedangle.png", UriKind.Relative));
        }

        private void imageEnter13(object sender, MouseEventArgs e)
        {   
            this.image13.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/fourstarTemp.png", UriKind.Relative));
        }

        private void imageLeave13(object sender, MouseEventArgs e)
        {
            this.image13.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/fourstar.png", UriKind.Relative));
        }

        private void imageEnter14(object sender, MouseEventArgs e)
        {
    
            this.image14.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/rhombusTemp.png", UriKind.Relative));
        }

        private void imageLeave14(object sender, MouseEventArgs e)
        {
            this.image14.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/rhombus.png", UriKind.Relative));
        }
        private void imageEnter15(object sender, MouseEventArgs e)
        {  
            this.image15.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/triangleTemp.png", UriKind.Relative));
        }

        private void imageLeave15(object sender, MouseEventArgs e)
        {
            this.image15.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/triangle.png", UriKind.Relative));
        }

        private void imageEnter16(object sender, MouseEventArgs e)
        {   
            this.image16.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/arrowcrossTemp.png", UriKind.Relative));
        }

        private void imageLeave16(object sender, MouseEventArgs e)
        {
            this.image16.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/arrowcross.png", UriKind.Relative));
        }

        private void imageEnter17(object sender, MouseEventArgs e)
        {   
            this.image17.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/CircleClosedangleCompassTemp.png", UriKind.Relative));
        }

        private void imageLeave17(object sender, MouseEventArgs e)
        {
            this.image17.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/CircleClosedangleCompass.png", UriKind.Relative));
        }
        private void imageEnter18(object sender, MouseEventArgs e)
        {    
            this.image18.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/DoubleClosedangleCompassTemp.png", UriKind.Relative));
        }

        private void imageLeave18(object sender, MouseEventArgs e)
        {
            this.image18.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/DoubleClosedangleCompass.png", UriKind.Relative));
        }
        private void imageEnter19(object sender, MouseEventArgs e)
        {
    
            this.image19.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/SameDirectionClosedangleCompassTemp.png", UriKind.Relative));
        }

        private void imageLeave19(object sender, MouseEventArgs e)
        {
            this.image19.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/SameDirectionClosedangleCompass.png", UriKind.Relative));
        }

        private void imageEnter20(object sender, MouseEventArgs e)
        {    
            this.image20.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/VaneCompassTemp.png", UriKind.Relative));
        }

        private void imageLeave20(object sender, MouseEventArgs e)
        {
            this.image20.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/arrow/VaneCompass.png", UriKind.Relative));
        }
        //页面加载的时候给地图的地址ServerAddress赋值
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
        }
        //鼠标移入清除按钮上更改图片
        private void clearEnter(object sender, MouseEventArgs e)
        {
            this.clearImage.Cursor = Cursors.Hand;
            this.clearImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/clear1Temp.png", UriKind.Relative));
           
        }
        //鼠标移出清除按钮更改图片
        private void clearLeave(object sender, MouseEventArgs e)
        {
            this.clearImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/images/clear1.png", UriKind.Relative));
        }
    }
}


