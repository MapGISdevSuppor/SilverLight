using System;
using System.Collections.Generic;
using System.Linq;

//using System.Data;
using System.Net;
//using System.Data.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Properties;
using System.Windows.Input;
using System.Windows.Controls.Data.DataForm;
using System.Windows.Controls.DataVisualization;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;

using ZDIMS.Util;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMS.Map;

namespace ZDIMSDemo.Controls
{
    public partial class Project : BaseUserControl
    {
        private SpacialAnalyse _spatial = null;
        public VectorLayerBase vectorObj = null;
        private ProjectParam param_src =null;
        private ProjectParam param_des =null;

        public IMSMap IMSMap
        {
            get;
            set;
        }
       
        public Project()
        {
            InitializeComponent();
            this.mydialog1.OnClose += new RoutedEventHandler(close);
        }
        /// <summary>
        /// 初始化坐标系
        /// </summary>
        public void ProjType_coor_src(ProjectParam Projpara, int flag)
        {
            int select = -1;
            if (flag == 0)
            {
                select = coor_src.SelectedIndex;
            }
            else
            {
                select = coor_des.SelectedIndex;
            }


            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjType = EnumType.TYPE_JWD;
                        break;
                    }
                case 1:
                    {
                        Projpara.ProjType = EnumType.TYPE_LOC;
                        break;
                    }
                case 2:
                    {
                        Projpara.ProjType = EnumType.TYPE_PRJ;
                        break;
                    }
                case 3:
                    {
                        Projpara.ProjType = EnumType.TYPE_XYZ;
                        break;
                    }
                default: break;
            }
            //  return Projpara;

        }

        /// <summary>
        /// 获取椭球体类型
        /// </summary>
        public void SphereID_ISpheroid_src(ProjectParam Projpara, int flag)
        {
            // ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = ISpheroid_src.SelectedIndex;
            }
            else
            {
                select = ISpheroid_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.SphereID = EnumSphereType.Beijing54;
                        break;
                    }
                case 1:
                    {
                        Projpara.SphereID = EnumSphereType.Xian80;
                        break;
                    }
            }
            // return Projpara;
        }
        /// <summary>
        /// 获取角度单位：ProjAngleUnit
        /// </summary>
        public void ProjAngleUnit_DAngUnit_src(ProjectParam Projpara, int flag)
        {
            //ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = DAngUnit_src.SelectedIndex;
            }
            else
            {
                select = DAngUnit_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjAngleUnit = EnumProjAngleUnit.UNIT_Degree;
                        break;
                    }
                case 1:
                    {
                        Projpara.ProjAngleUnit = EnumProjAngleUnit.UNIT_DMS;
                        break;
                    }
            }
            // return Projpara;
        }
        /// <summary>
        /// 获取投影转换类型：ProjTypeID
        /// </summary>
        public void ProjTypeID_IProjTypeId_src(ProjectParam Projpara, int flag)
        {
            //ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = IProjTypeId_src.SelectedIndex;
            }
            else
            {
                select = IProjTypeId_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjTypeID = EnumProjType.LonLat;
                        break;
                    }
                case 1:
                    {
                        Projpara.ProjTypeID = EnumProjType.Gauss_Kruger;
                        break;
                    }
                case 2:
                    {
                        Projpara. ProjTypeID = EnumProjType. Lambert;
                        break;
                    }
                case 3:
                    {
                        Projpara. ProjTypeID = EnumProjType. Albers;
                        break;
                    }
                case 4:
                    {
                        Projpara. ProjTypeID = EnumProjType. Polyconic;
                        break;
                    }
                case 5:
                    {
                        Projpara. ProjTypeID = EnumProjType. Mercator;
                        break;
                    }
                    
            }
        }
        /// <summary>
        /// 获取投影分带类型：ProjZoneType
        /// </summary>
        public void ProjZoneType_BZoneType_src(ProjectParam Projpara, int flag)
        {
            //  ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = this.BZoneType_src.SelectedIndex;
            }
            else
            {
                select = this.BZoneType_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjZoneType = 0;
                        break;
                    }
                case 1:
                    {
                        Projpara.ProjZoneType = 1;
                        break;
                    }
                case 2:
                    {
                        Projpara.ProjZoneType = 2;
                        break;
                    }
            }
            //return Projpara;
        }
        /// <summary>
        /// 获取投影带号：ProjZoneNO
        /// </summary>
        public void ProjZoneNO_IZone_src(ProjectParam Projpara, int flag)
        {
            if (flag == 0)
            {
                Projpara.ProjZoneNO = Convert.ToByte(this.IZone_src.Text);
            }
            else
            {
                Projpara.ProjZoneNO = Convert.ToByte(this.IZone_des.Text);
            }
            // return Projpara;
        }
        /// <summary>
        /// 获取比例尺：ProjRate
        /// </summary>
        public void ProjRate_Scale_src(ProjectParam Projpara, int flag)
        {
            // ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = this.Scale_src.SelectedIndex;
            }
            else
            {
                select = this.Scale_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjRate = 1;
                        break;
                    }
                case 1:
                    {
                        Projpara.ProjRate = 5000;
                        break;
                    }
                case 2:
                    {
                        Projpara.ProjRate = 10000;
                        break;
                    }
                case 3:
                    {
                        Projpara.ProjRate = 25000;
                        break;
                    }
                case 4:
                    {
                        Projpara.ProjRate = 50000;
                        break;
                    }
                case 5:
                    {
                        Projpara.ProjRate = 100000;
                        break;
                    }
                case 6:
                    {
                        Projpara.ProjRate = 250000;
                        break;
                    }
                case 7:
                    {
                        Projpara.ProjRate = 500000;
                        break;
                    }
                case 8:
                    {
                        Projpara.ProjRate = 1000000;
                        break;
                    }
            }
            //  return Projpara;

        }
        /// <summary>
        /// 水平长度单位：ProjUnit
        /// </summary>
        public void ProjUnit_unit_src(ProjectParam Projpara, int flag)
        {
            // ProjectParam Projpara = new ProjectParam();
            int select = -1;
            if (flag == 0)
            {
                select = this.unit_src.SelectedIndex;
            }
            else
            {
                select = this.unit_des.SelectedIndex;
            }
            switch (select)
            {
                case 0:
                    {
                        Projpara.ProjUnit = EnumProjUnit.UNIT_MM;
                        break;

                    }
                case 1:
                    {
                        Projpara.ProjUnit = EnumProjUnit.UNIT_CM;
                        break;

                    }
                case 2:
                    {
                        Projpara.ProjUnit = EnumProjUnit.UNIT_DM;
                        break;
                    }
                case 3:
                    {
                        Projpara.ProjUnit = EnumProjUnit.UNIT_Meter;
                        break;
                    }
                case 4:
                    {
                        Projpara.ProjUnit = EnumProjUnit.UNIT_KiloMeter;
                        break;
                    }
            }
            //return Projpara;
        }





        private void submit(object sender, RoutedEventArgs e)
        {
            if (this.IMSMap== null)
            {
                MessageBox.Show("imsmap属性未赋值。");
                return;
            }
            if ((this.ProjLat_src.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat_src.Text)) ||
                (ProjLat1_src.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat1_src.Text)) ||
                (ProjLat2_src.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat2_src.Text)) ||
                (DLon_src.Text.Length > 0 && !CommFun.IsNumber(this.DLon_src.Text)) ||
                (DLon_des.Text.Length > 0 && !CommFun.IsNumber(this.DLon_des.Text)) ||
                (ProjLat_des.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat_des.Text)) ||
                (ProjLat1_des.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat1_des.Text)) ||
                (ProjLat2_des.Text.Length > 0 && !CommFun.IsNumber(this.ProjLat2_des.Text)))
            {
                MessageBox.Show("请输入合法数字!");
                return;
            }
            param_src = new ProjectParam();
            param_des = new ProjectParam();
            param_src.ProjLon = Convert.ToDouble(this.DLon_src.Text);
            this.ProjRate_Scale_src(param_src, 0);
            this.ProjType_coor_src(param_src, 0);
            if (param_src.ProjType != EnumType.TYPE_JWD)
            {
                this.ProjTypeID_IProjTypeId_src(param_src, 0);
                this.ProjZoneNO_IZone_src(param_src, 0);
                this.ProjZoneType_BZoneType_src(param_src, 0);
            }
            this.ProjUnit_unit_src(param_src, 0);
            this.SphereID_ISpheroid_src(param_src, 0);
            this.ProjAngleUnit_DAngUnit_src(param_src, 0);
            if (param_src.ProjTypeID == EnumProjType.Lambert || param_src.ProjTypeID == EnumProjType.Albers)
            {                
                param_src.ProjLat = Convert.ToDouble(this.ProjLat_src.Text);
                param_src.ProjLat1 = Convert.ToDouble(this.ProjLat1_src.Text);
                param_src.ProjLat2 = Convert.ToDouble(this.ProjLat2_src.Text);
            }
            if (param_src.ProjTypeID == EnumProjType.Polyconic || param_src.ProjTypeID == EnumProjType.Transverse_Mecator)
            {
                param_src.ProjLat = Convert.ToDouble(this.ProjLat_src.Text);
            }

            
            param_des.ProjLon = Convert.ToDouble(this.DLon_des.Text);
            this.ProjRate_Scale_src(param_des, 1);
            this.ProjType_coor_src(param_des, 1);
            if (param_des.ProjType != EnumType.TYPE_JWD)
            {
                this.ProjTypeID_IProjTypeId_src(param_des, 1);
                this.ProjZoneNO_IZone_src(param_des, 1);
                this.ProjZoneType_BZoneType_src(param_des, 1);
            }
            this.ProjUnit_unit_src(param_des, 1);
            this.SphereID_ISpheroid_src(param_des, 1);
            this.ProjAngleUnit_DAngUnit_src(param_des, 1);
            if (param_des.ProjTypeID == EnumProjType.Lambert || param_des.ProjTypeID == EnumProjType.Albers)
            {
                param_des.ProjLat = Convert.ToDouble(this.ProjLat_des.Text);
                param_des.ProjLat1 = Convert.ToDouble(this.ProjLat1_des.Text);
                param_des.ProjLat2 = Convert.ToDouble(this.ProjLat2_des.Text);
            }
            if (param_des.ProjTypeID == EnumProjType.Polyconic || param_des.ProjTypeID == EnumProjType.Transverse_Mecator)
            {
                param_des.ProjLat = Convert.ToDouble(this.ProjLat_des.Text);
            }

            CProjectDots obj = new CProjectDots();
            obj.SrcProjParam = param_src;
            obj.DesProjParm = param_des;
            Dot_2D dot = new Dot_2D();
            dot.x = Convert.ToDouble(this.coorx_src.Text);
            dot.y = Convert.ToDouble(this.coory_src.Text);
            obj.InputDots = new Dot_2D[1];
            obj.InputDots[0] = new Dot_2D();
            obj.InputDots[0] = dot;

            this._spatial = new SpacialAnalyse(vectorObj);
            this._spatial.ProjectDots(obj, new UploadStringCompletedEventHandler(onSubmit));
        }
        public void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            this.coorx_des.Text = "";
            this.coory_des.Text = "";
            CGetProjectDots rlt = this._spatial.OnProjectDots(e);
            if (rlt != null)
            {
                this.coorx_des.Text = rlt.DesDots[0].x.ToString();
                this.coory_des.Text = rlt.DesDots[0].y.ToString();
                MessageBox.Show("转换成功");
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
