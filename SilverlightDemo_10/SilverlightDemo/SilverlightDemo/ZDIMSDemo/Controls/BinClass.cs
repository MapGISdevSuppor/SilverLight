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
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Generic;

namespace ZDIMSDemo.Controls
{
    /// <summary>
    /// 万能绑定类
    /// </summary>
    public class BindClass : IEditableObject                 //实现这个接口的目的是需要在界面被编辑时处理一些事件
    {
        public String key0 { get; set; }public String key1 { get; set; }public String key2 { get; set; }public String key3 { get; set; }public String key4 { get; set; }public String key5 { get; set; }public String key6 { get; set; }public String key7 { get; set; }public String key8 { get; set; }public String key9 { get; set; }public String key10 { get; set; }public String key11 { get; set; }public String key12 { get; set; }public String key13 { get; set; }public String key14 { get; set; }public String key15 { get; set; }public String key16 { get; set; }public String key17 { get; set; }public String key18 { get; set; }public String key19 { get; set; }public String key20 { get; set; }public String key21 { get; set; }public String key22 { get; set; }public String key23 { get; set; }public String key24 { get; set; }public String key25 { get; set; }public String key26 { get; set; }public String key27 { get; set; }public String key28 { get; set; }public String key29 { get; set; }public String key30 { get; set; }public String key31 { get; set; }public String key32 { get; set; }public String key33 { get; set; }public String key34 { get; set; }public String key35 { get; set; }public String key36 { get; set; }public String key37 { get; set; }public String key38 { get; set; }public String key39 { get; set; }public String key40 { get; set; }public String key41 { get; set; }public String key42 { get; set; }public String key43 { get; set; }public String key44 { get; set; }public String key45 { get; set; }public String key46 { get; set; }public String key47 { get; set; }public String key48 { get; set; }public String key49 { get; set; }public String key50 { get; set; }public String key51 { get; set; }public String key52 { get; set; }public String key53 { get; set; }public String key54 { get; set; }public String key55 { get; set; }public String key56 { get; set; }public String key57 { get; set; }public String key58 { get; set; }public String key59 { get; set; }public String key60 { get; set; }public String key61 { get; set; }public String key62 { get; set; }public String key63 { get; set; }public String key64 { get; set; }public String key65 { get; set; }public String key66 { get; set; }public String key67 { get; set; }public String key68 { get; set; }public String key69 { get; set; }public String key70 { get; set; }public String key71 { get; set; }public String key72 { get; set; }public String key73 { get; set; }public String key74 { get; set; }public String key75 { get; set; }public String key76 { get; set; }public String key77 { get; set; }public String key78 { get; set; }public String key79 { get; set; }public String key80 { get; set; }public String key81 { get; set; }public String key82 { get; set; }public String key83 { get; set; }public String key84 { get; set; }public String key85 { get; set; }public String key86 { get; set; }public String key87 { get; set; }public String key88 { get; set; }public String key89 { get; set; }public String key90 { get; set; }public String key91 { get; set; }public String key92 { get; set; }public String key93 { get; set; }public String key94 { get; set; }public String key95 { get; set; }public String key96 { get; set; }public String key97 { get; set; }public String key98 { get; set; }public String key99 { get; set; }public String key100 { get; set; }public String key101 { get; set; }public String key102 { get; set; }public String key103 { get; set; }public String key104 { get; set; }public String key105 { get; set; }public String key106 { get; set; }public String key107 { get; set; }public String key108 { get; set; }public String key109 { get; set; }public String key110 { get; set; }public String key111 { get; set; }public String key112 { get; set; }public String key113 { get; set; }public String key114 { get; set; }public String key115 { get; set; }public String key116 { get; set; }public String key117 { get; set; }public String key118 { get; set; }public String key119 { get; set; }public String key120 { get; set; }public String key121 { get; set; }public String key122 { get; set; }public String key123 { get; set; }public String key124 { get; set; }public String key125 { get; set; }public String key126 { get; set; }public String key127 { get; set; }public String key128 { get; set; }public String key129 { get; set; }public String key130 { get; set; }public String key131 { get; set; }public String key132 { get; set; }public String key133 { get; set; }public String key134 { get; set; }public String key135 { get; set; }public String key136 { get; set; }public String key137 { get; set; }public String key138 { get; set; }public String key139 { get; set; }public String key140 { get; set; }public String key141 { get; set; }public String key142 { get; set; }public String key143 { get; set; }public String key144 { get; set; }public String key145 { get; set; }public String key146 { get; set; }public String key147 { get; set; }public String key148 { get; set; }public String key149 { get; set; }public String key150 { get; set; }public String key151 { get; set; }public String key152 { get; set; }public String key153 { get; set; }public String key154 { get; set; }public String key155 { get; set; }public String key156 { get; set; }public String key157 { get; set; }public String key158 { get; set; }public String key159 { get; set; }public String key160 { get; set; }public String key161 { get; set; }public String key162 { get; set; }public String key163 { get; set; }public String key164 { get; set; }public String key165 { get; set; }public String key166 { get; set; }public String key167 { get; set; }public String key168 { get; set; }public String key169 { get; set; }public String key170 { get; set; }public String key171 { get; set; }public String key172 { get; set; }public String key173 { get; set; }public String key174 { get; set; }public String key175 { get; set; }public String key176 { get; set; }public String key177 { get; set; }public String key178 { get; set; }public String key179 { get; set; }public String key180 { get; set; }public String key181 { get; set; }public String key182 { get; set; }public String key183 { get; set; }public String key184 { get; set; }public String key185 { get; set; }public String key186 { get; set; }public String key187 { get; set; }public String key188 { get; set; }public String key189 { get; set; }public String key190 { get; set; }public String key191 { get; set; }public String key192 { get; set; }public String key193 { get; set; }public String key194 { get; set; }public String key195 { get; set; }public String key196 { get; set; }public String key197 { get; set; }public String key198 { get; set; }public String key199 { get; set; }
        //以下可能足够长， 可以放心 这些字段不必要全部初始化 留null不会占用内存空间。
        public String[] keyarr = new string[200];                 //这个集合保存上面所有key0～key19的引用
        public String[] keystrarr =                                   //属性的字符表示
                            {
                                "key0","key1","key2","key3","key4","key5","key6","key7","key8","key9","key10","key11","key12","key13","key14","key15","key16","key17","key18","key19","key20","key21","key22","key23","key24","key25","key26","key27","key28","key29","key30","key31","key32","key33","key34","key35","key36","key37","key38","key39","key40","key41","key42","key43","key44","key45","key46","key47","key48","key49","key50","key51","key52","key53","key54","key55","key56","key57","key58","key59","key60","key61","key62","key63","key64","key65","key66","key67","key68","key69","key70","key71","key72","key73","key74","key75","key76","key77","key78","key79","key80","key81","key82","key83","key84","key85","key86","key87","key88","key89","key90","key91","key92","key93","key94","key95","key96","key97","key98","key99","key100","key101","key102","key103","key104","key105","key106","key107","key108","key109","key110","key111","key112","key113","key114","key115","key116","key117","key118","key119","key120","key121","key122","key123","key124","key125","key126","key127","key128","key129","key130","key131","key132","key133","key134","key135","key136","key137","key138","key139","key140","key141","key142","key143","key144","key145","key146","key147","key148","key149","key150","key151","key152","key153","key154","key155","key156","key157","key158","key159","key160","key161","key162","key163","key164","key165","key166","key167","key168","key169","key170","key171","key172","key173","key174","key175","key176","key177","key178","key179","key180","key181","key182","key183","key184","key185","key186","key187","key188","key189","key190","key191","key192","key193","key194","key195","key196","key197","key198","key199"
                            };
        /// <summary>
        /// 很重要的refresh方法
        /// </summary>
        public void Refresh()
        {
            key0 = keyarr[0]; key1 = keyarr[1]; key2 = keyarr[2]; key3 = keyarr[3]; key4 = keyarr[4]; key5 = keyarr[5]; key6 = keyarr[6]; key7 = keyarr[7]; key8 = keyarr[8]; key9 = keyarr[9]; key10 = keyarr[10]; key11 = keyarr[11]; key12 = keyarr[12]; key13 = keyarr[13]; key14 = keyarr[14]; key15 = keyarr[15]; key16 = keyarr[16]; key17 = keyarr[17]; key18 = keyarr[18]; key19 = keyarr[19]; key20 = keyarr[20]; key21 = keyarr[21]; key22 = keyarr[22]; key23 = keyarr[23]; key24 = keyarr[24]; key25 = keyarr[25]; key26 = keyarr[26]; key27 = keyarr[27]; key28 = keyarr[28]; key29 = keyarr[29]; key30 = keyarr[30]; key31 = keyarr[31]; key32 = keyarr[32]; key33 = keyarr[33]; key34 = keyarr[34]; key35 = keyarr[35]; key36 = keyarr[36]; key37 = keyarr[37]; key38 = keyarr[38]; key39 = keyarr[39]; key40 = keyarr[40]; key41 = keyarr[41]; key42 = keyarr[42]; key43 = keyarr[43]; key44 = keyarr[44]; key45 = keyarr[45]; key46 = keyarr[46]; key47 = keyarr[47]; key48 = keyarr[48]; key49 = keyarr[49]; key50 = keyarr[50]; key51 = keyarr[51]; key52 = keyarr[52]; key53 = keyarr[53]; key54 = keyarr[54]; key55 = keyarr[55]; key56 = keyarr[56]; key57 = keyarr[57]; key58 = keyarr[58]; key59 = keyarr[59]; key60 = keyarr[60]; key61 = keyarr[61]; key62 = keyarr[62]; key63 = keyarr[63]; key64 = keyarr[64]; key65 = keyarr[65]; key66 = keyarr[66]; key67 = keyarr[67]; key68 = keyarr[68]; key69 = keyarr[69]; key70 = keyarr[70]; key71 = keyarr[71]; key72 = keyarr[72]; key73 = keyarr[73]; key74 = keyarr[74]; key75 = keyarr[75]; key76 = keyarr[76]; key77 = keyarr[77]; key78 = keyarr[78]; key79 = keyarr[79]; key80 = keyarr[80]; key81 = keyarr[81]; key82 = keyarr[82]; key83 = keyarr[83]; key84 = keyarr[84]; key85 = keyarr[85]; key86 = keyarr[86]; key87 = keyarr[87]; key88 = keyarr[88]; key89 = keyarr[89]; key90 = keyarr[90]; key91 = keyarr[91]; key92 = keyarr[92]; key93 = keyarr[93]; key94 = keyarr[94]; key95 = keyarr[95]; key96 = keyarr[96]; key97 = keyarr[97]; key98 = keyarr[98]; key99 = keyarr[99]; key100 = keyarr[100]; key101 = keyarr[101]; key102 = keyarr[102]; key103 = keyarr[103]; key104 = keyarr[104]; key105 = keyarr[105]; key106 = keyarr[106]; key107 = keyarr[107]; key108 = keyarr[108]; key109 = keyarr[109]; key110 = keyarr[110]; key111 = keyarr[111]; key112 = keyarr[112]; key113 = keyarr[113]; key114 = keyarr[114]; key115 = keyarr[115]; key116 = keyarr[116]; key117 = keyarr[117]; key118 = keyarr[118]; key119 = keyarr[119]; key120 = keyarr[120]; key121 = keyarr[121]; key122 = keyarr[122]; key123 = keyarr[123]; key124 = keyarr[124]; key125 = keyarr[125]; key126 = keyarr[126]; key127 = keyarr[127]; key128 = keyarr[128]; key129 = keyarr[129]; key130 = keyarr[130]; key131 = keyarr[131]; key132 = keyarr[132]; key133 = keyarr[133]; key134 = keyarr[134]; key135 = keyarr[135]; key136 = keyarr[136]; key137 = keyarr[137]; key138 = keyarr[138]; key139 = keyarr[139]; key140 = keyarr[140]; key141 = keyarr[141]; key142 = keyarr[142]; key143 = keyarr[143]; key144 = keyarr[144]; key145 = keyarr[145]; key146 = keyarr[146]; key147 = keyarr[147]; key148 = keyarr[148]; key149 = keyarr[149]; key150 = keyarr[150]; key151 = keyarr[151]; key152 = keyarr[152]; key153 = keyarr[153]; key154 = keyarr[154]; key155 = keyarr[155]; key156 = keyarr[156]; key157 = keyarr[157]; key158 = keyarr[158]; key159 = keyarr[159]; key160 = keyarr[160]; key161 = keyarr[161]; key162 = keyarr[162]; key163 = keyarr[163]; key164 = keyarr[164]; key165 = keyarr[165]; key166 = keyarr[166]; key167 = keyarr[167]; key168 = keyarr[168]; key169 = keyarr[169]; key170 = keyarr[170]; key171 = keyarr[171]; key172 = keyarr[172]; key173 = keyarr[173]; key174 = keyarr[174]; key175 = keyarr[175]; key176 = keyarr[176]; key177 = keyarr[177]; key178 = keyarr[178]; key179 = keyarr[179]; key180 = keyarr[180]; key181 = keyarr[181]; key182 = keyarr[182]; key183 = keyarr[183]; key184 = keyarr[184]; key185 = keyarr[185]; key186 = keyarr[186]; key187 = keyarr[187]; key188 = keyarr[188]; key189 = keyarr[189]; key190 = keyarr[190]; key191 = keyarr[191]; key192 = keyarr[192]; key193 = keyarr[193]; key194 = keyarr[194]; key195 = keyarr[195]; key196 = keyarr[196]; key197 = keyarr[197]; key198 = keyarr[198]; key199 = keyarr[199];
        }
        
        /// <summary>
        /// 列个数
        /// </summary>
        public int ColumnCount { 
            get {
                int i = 0;
                for (; i < keyarr.Length; i++)
                    if (keyarr[i] == null)
                        break;
            return i; 
        } }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<BindClass> ColumnDisplay(DataGrid grid, List<string[]> list)
        {
            grid.AutoGenerateColumns = false;
            List<BindClass> relist = new List<BindClass>();       //需要经过一系列讲List<string[]> 转换为List<BingClass>
            for (int num = 0; num < list[0].Length; num++)
            {
                //DataGridTextColumn 在C#中定义，使xaml自由一点
                DataGridTextColumn column = new DataGridTextColumn();
                Binding binding = new Binding(keystrarr[num]);      //这部分很重要 定义该列绑定指定的属性列
                grid.FrozenColumnCount = 1;
                binding.Mode = BindingMode.TwoWay;
                column.Binding = binding;
                column.Header = list[0][num];                //注意我的list第一行是中文字段名用作DataGird列名
                grid.Columns.Add(column);
            }
            for (int i = 1; i < list.Count; i++)              //List<string[]> 转换为List<BingClass>
            {
                string[] strs = list[i];
                if (strs != null)
                {
                    BindClass bing = new BindClass();
                    for (int j = 0; j < strs.Length; j++)
                    {
                        bing.keyarr[j] = strs[j];
                    }
                    bing.Refresh();                               //很重要，使集合中的值赋给各个key属性
                    relist.Add(bing);
                }
                else
                    break; //relist.Add(null);//
            }
            grid.ItemsSource = relist;                       //费这么大功夫就为了这一行……
            return relist;
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="list"></param>
        /// <param name="addColumnlHeadArr"></param>
        /// <param name="addColumnlContentArr"></param>
        /// <returns></returns>
        public List<BindClass> ColumnDisplay(DataGrid grid, string[][] list, List<string> addColumnlHeadArr = null, List<string[]> addColumnlContentArr = null)
        {
            grid.AutoGenerateColumns = false;
            List<BindClass> relist = new List<BindClass>();       //需要经过一系列讲List<string[]> 转换为List<BingClass>
            int sIndex = 0;
            if (addColumnlHeadArr != null && addColumnlContentArr != null && addColumnlHeadArr.Count == addColumnlContentArr.Count)
            {
                for (int i = 0; i < addColumnlHeadArr.Count; i++)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    Binding binding = new Binding(keystrarr[i]);      //这部分很重要 定义该列绑定指定的属性列
                    grid.FrozenColumnCount = 1;
                    binding.Mode = BindingMode.TwoWay;
                    column.Binding = binding;
                    column.Header = addColumnlHeadArr[i];//list[0][num];                //注意我的list第一行是中文字段名用作DataGird列名
                    grid.Columns.Add(column);
                    sIndex++;
                }
            }
            if (list[0] != null)
            {
                for (int num = 0; num < list[0].Length; num++)
                {
                    //DataGridTextColumn 在C#中定义，使xaml自由一点
                    DataGridTextColumn column = new DataGridTextColumn();
                    Binding binding = new Binding(keystrarr[num + sIndex]);      //这部分很重要 定义该列绑定指定的属性列
                    grid.FrozenColumnCount = 1;
                    binding.Mode = BindingMode.TwoWay;
                    column.Binding = binding;
                    column.Header = list[0][num];                //注意我的list第一行是中文字段名用作DataGird列名
                    grid.Columns.Add(column);
                }

                for (int i = 1; i < list.Length; i++)              //List<string[]> 转换为List<BingClass>
                {
                    string[] strs = list[i];
                    if (strs != null)
                    {
                        BindClass bing = new BindClass();
                        if (addColumnlHeadArr != null && addColumnlContentArr != null &&
                            addColumnlHeadArr.Count == addColumnlContentArr.Count)
                        {
                            for (int k = 0; k < addColumnlContentArr.Count; k++)
                            {
                                if (k < addColumnlContentArr[k].Length && i <= addColumnlContentArr[k].Length)
                                    bing.keyarr[k] = addColumnlContentArr[k][i - 1];
                            }
                        }
                        for (int j = 0; j < strs.Length; j++)
                        {
                            bing.keyarr[j + sIndex] = strs[j];
                        }
                        bing.Refresh();                               //很重要，使集合中的值赋给各个key属性
                        relist.Add(bing);
                    }
                    else
                        break; //relist.Add(null);//
                }
            }
            else
            {
                //string s = "";
            }
            grid.ItemsSource = relist;                       //费这么大功夫就为了这一行……
            return relist;
        }
        /// <summary>
        /// 导致对象进入编辑模式。
        /// </summary>
        public void BeginEdit(){}
        /// <summary>
        ///  导致对象退出编辑模式并恢复前一未编辑的值。
        /// </summary>
        public void CancelEdit() { }
        /// <summary>
        /// 导致对象退出编辑模式并提交已编辑的值。
        /// </summary>
        public void EndEdit() { }
    }
}
