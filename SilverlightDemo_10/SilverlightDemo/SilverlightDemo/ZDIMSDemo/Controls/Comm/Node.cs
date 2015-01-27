//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

//namespace ZDIMSDemo.Controls.Comm
//{
//    /// <summary>
//    /// 公交换乘站点的结构类型 
//    /// </summary>
//    public struct Node
//    {
//        /// <summary>
//        /// 站点名称 
//        /// </summary>
//        public string name;
//        /// <summary>
//        /// X逻辑坐标 
//        /// </summary>
//        public double x;
//        /// <summary>
//        /// Y逻辑坐标 
//        /// </summary>	
//        public double y;
//        /// <summary>
//        /// 站点序号 — 标识车站的序列号，同时也用于标识div层的ID,起点为0 
//        /// </summary>	
//        public int seq;
//        /// <summary>
//        /// 站点类型 — 0 起点；1 乘车点(标识起点有步行的情况)；2 普通点(线路上的站点)；3 步行点;4 换乘点;5 终点  
//        /// </summary>	
//        public int type;
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="name">站点名称 </param>
//        /// <param name="logicX"> X逻辑坐标 </param>
//        /// <param name="logicY"> Y逻辑坐标 </param>
//        /// <param name="seq">站点序号 — 标识车站的序列号，同时也用于标识div层的ID,起点为0 </param>
//        /// <param name="type">站点类型 — 0 起点；1 乘车点(标识起点有步行的情况)；2 普通点(线路上的站点)；3 步行点;4 换乘点;5 终点</param>
//        public Node(string name, double logicX, double logicY, int seq, int type)
//        {
//            this.name = name;
//            this.x = logicX;
//            this.y = logicY;
//            this.seq = seq;    //标识车站的序列号，同时也用于标识div层的ID,起点为0
//            this.type = type; //0 起点；1 乘车点(标识起点有步行的情况)；2 普通点(线路上的站点)；3 步行点;4 换乘点;5 终点 
//        }
//    }
//}
