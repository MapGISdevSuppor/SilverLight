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
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls.Comm
{
    public interface IFeatureStyleEditor
    {
        GInfoType FeatureGinfoType { get; }
        void Update();  
    }
}
