using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maticsoft.BLL;
using Maticsoft.Model;
using Maticsoft.DBUtility;
using 养生池;

namespace 自动化库存管理
{
    struct LibraryProperty
    {
        public bool isFull;//true:有货；false:无货
        public int X;//界面坐标
        public int Y;//界面坐标
        public int VerticalNo;//库位层号
        public int HorizontalNo;//库位列号
        public int MingCheng;//货物名称
        public int BianMa;//货物编码
        public int HuoJiaNo;//货架号
        public Maticsoft.Model.tb_KuCun kucun;//当前库位具体信息
        
    }
    public partial class KuWeiForm : Form
    {
        int Lastindex;//上次选中仓位；
        int HuoJiaShu;
        int CengShu;
        int LieShu;
        int ZongKuWeiShu;
        //string MingCheng = "";
        LibraryProperty[] KuWeiPro ;
        PictureBox[] KuWeiPic ;
        Maticsoft.BLL.tb_KuCun KuCunBLL;
        Maticsoft.BLL.tb_KuWeiDetail KuWeiDetailBLL;
        List<Maticsoft.Model.tb_KuCun> listKuCunModel ;
        List<Maticsoft.Model.tb_KuWeiDetail> listKuWeiDetailModel;
        int unitWidth;
        int unitHeight;
        public KuWeiForm()
        {
            InitializeComponent();
            this.Resize += KuWeiForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            //toolTipKuCun.AutoPopDelay = 25000;
            toolTipKuCun.InitialDelay = 100;
            Lastindex = -1;//初始化-1

            KuCunBLL = new Maticsoft.BLL.tb_KuCun();
            KuWeiDetailBLL = new Maticsoft.BLL.tb_KuWeiDetail();
            listKuCunModel = new List<Maticsoft.Model.tb_KuCun>();
            listKuWeiDetailModel = new List<Maticsoft.Model.tb_KuWeiDetail>();
           
        }

        private void KuWeiForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

        private void InitCangKu()
        {
            KuWeiPro = new LibraryProperty[ZongKuWeiShu];
            KuWeiPic = new PictureBox[ZongKuWeiShu];
            Point tempP = new Point(0, 0);
            int totalWidth = this.ClientSize .Width ;
            int totalHeight = this.ClientSize .Height ;
            int CangKuBuJuHang = HuoJiaShu / 2 + HuoJiaShu % 2;//2列，CangKuBuJuHang
            //王磊添加 2020.4.30
            unitWidth = totalWidth / (LieShu * 2 + 3);
            unitHeight = (totalHeight - 100) / (CangKuBuJuHang * CengShu + CangKuBuJuHang + 1);
            ///////////////////////////////////////////////////
            //王磊更改 2020.4.30
            //int HuoJiaWidth = (totalWidth - 150) / 2;
            int HuoJiaWidth = unitWidth * LieShu;
            //int HuoJiaHeight = (totalHeight - 200) / CangKuBuJuHang;
            int HuoJiaHeight = unitHeight * CengShu;
            //int KuWeiWidth = HuoJiaWidth / LieShu;
            int KuWeiWidth = unitWidth;
            //int KuWeiHeight = HuoJiaHeight / CengShu;
            int KuWeiHeight = unitHeight;
            ////////////////////////////////////////////////////////////////////
            int HuoJiaHang = 0;
            int ZhengTiWeiYi = 0;
            if (HuoJiaShu == 1)
                ZhengTiWeiYi = (totalWidth - LieShu * KuWeiWidth) / 2;
            else
                //ZhengTiWeiYi = 50;
                ZhengTiWeiYi = unitWidth;
            int tempHuoJiaHao = 0;
            for (int i = 0; i < ZongKuWeiShu; i++)
            {
                Label label = null;
                PictureBox pb = new PictureBox();
                HuoJiaHang = (i / LieShu) / CengShu / 2;
                if (((i / LieShu) / CengShu)%2 == 0)//第一列货架
                {
                    tempP.X = (i%LieShu) * KuWeiWidth + ZhengTiWeiYi;//ZhengTiWeiYi=50
                    //tempP.X = (i % LieShu) * KuWeiWidth + 50;//ZhengTiWeiYi=50
                    //tempP.Y = HuoJiaHang * HuoJiaHeight + 80 + 20 * HuoJiaHang + ((i / LieShu) % CengShu) * KuWeiHeight;
                    tempP.Y = 70 + HuoJiaHang * HuoJiaHeight + (HuoJiaHang + 1) * unitHeight + ((i / LieShu) % CengShu) * KuWeiHeight;
                    //tempP.Y = HuoJiaHang * HuoJiaHeight + 80 + 20 * HuoJiaHang + ((i / LieShu) % CengShu) * KuWeiHeight;                
                    if(i%(LieShu*CengShu)==0)
                    {
                        tempHuoJiaHao++;
                        label = new Label();
                        label.Text = tempHuoJiaHao.ToString() + "号货架总重:";// + stockGoodsBLL.GetShelvesWeight(tempHuoJiaHao.ToString()).ToString() + "KG";
                    }
                }                
                else//第二列货架
                {
                    tempP.X = (i % LieShu) * KuWeiWidth + ZhengTiWeiYi*2 + LieShu * KuWeiWidth;//ZhengTiWeiYi*2=100
                    //tempP.X = (i % LieShu) * KuWeiWidth + 100 + LieShu * KuWeiWidth;//ZhengTiWeiYi*2=100
                    //tempP.Y = HuoJiaHang * HuoJiaHeight + 80 + 20 * HuoJiaHang + ((i / LieShu) % CengShu) * KuWeiHeight;
                    tempP.Y = 70 + HuoJiaHang * HuoJiaHeight + (HuoJiaHang + 1) * unitHeight + ((i / LieShu) % CengShu) * KuWeiHeight;
                    //tempP.Y = HuoJiaHang * HuoJiaHeight + 80 + 20 * HuoJiaHang + ((i / LieShu) % CengShu) * KuWeiHeight;
                    if (i % (LieShu * CengShu) == 0)
                    {
                        tempHuoJiaHao++;
                        label = new Label();
                        label.Text = tempHuoJiaHao.ToString() + "号货架总重:";// + stockGoodsBLL.GetShelvesWeight(tempHuoJiaHao.ToString()).ToString() + "KG";
                    }
                }
                pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pb.Image = Image.FromFile(System.IO.Path.GetFullPath("库位3.jpg"));
                pb.Location = new System.Drawing.Point(tempP.X, tempP.Y);
                pb.Name = "pictureBox,KuWei" + "," + i.ToString();
                pb.Size = new System.Drawing.Size(KuWeiWidth , KuWeiHeight );
                pb.TabStop = false;
                pb.Click += new System.EventHandler(this.pictureBox_Click);
                if (i % (LieShu * CengShu) == 0)
                {
                    float size = KuWeiHeight / 2;
                    if (size > 15)
                        size = 15;
                    label.Location = new System.Drawing.Point(tempP.X, tempP.Y - Convert.ToInt32(size + 3));
                    label.Font = new System.Drawing.Font("楷体", size, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));                  
                    label.Size = new System.Drawing.Size(500, Convert.ToInt32(size+3));
                    //label.Text = tempHuoJiaHao.ToString() + "号货架总重:" + stockGoodsBLL.GetShelvesWeight(tempHuoJiaHao.ToString()).ToString() + "KG";
                    this.Controls.Add(label);                    
                }                    
                this.Controls.Add(pb);
                KuWeiPic[i] = pb;

                KuWeiPro[i].isFull = false;
                KuWeiPro[i].HorizontalNo = CengShu - (i/LieShu)%CengShu;
                KuWeiPro[i].VerticalNo = i%LieShu+1;
                KuWeiPro[i].HuoJiaNo = i / LieShu / CengShu +1;
                KuWeiPro[i].X = tempP.X;
                KuWeiPro[i].Y = tempP.Y;            
            }
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            string name = pb.Name;
            //解析被点击库位的名称，判断被点击库位的具体信息
            string[] array = name.Split(',');
            //index: array[2];
            if(Lastindex!=-1)
                KuWeiPic[Lastindex].BorderStyle = BorderStyle.None;//清除上次点选内容

            Lastindex = Convert.ToInt32(array[2]);
            int huojia = Lastindex / LieShu / CengShu + 1;
            int ceng = CengShu - (Lastindex / LieShu) % CengShu;
            int lie = Lastindex % LieShu + 1;
            labelWeight.Text = huojia.ToString()+"号货架总重:"+stockGoodsBLL.GetShelvesWeight(huojia.ToString()).ToString()+"KG";
            bool flagT = false;
            for (int k = 0; k < listKuCunModel.Count; k++)
            {
                int index1 = 0;
                index1 += (Convert.ToInt32(listKuCunModel[k].HuoJiaHao) - 1) * CengShu * LieShu
                    + (CengShu - Convert.ToInt32(listKuCunModel[k].CengHao)) * LieShu +
                    Convert.ToInt32(listKuCunModel[k].LieHao) - 1;
                if (index1 == Lastindex)
                {
                    flagT = true;
                    break;
                }
            }
            if (RuChuKuForm.m_RuKuChuKuFlag )//入库
            {
                if (!flagT)
                {
                    KuWeiPic[Lastindex].BorderStyle = BorderStyle.Fixed3D;
                    labelKuWei.Text = huojia.ToString() + "号货架 " + ceng + "层 " + lie + "列";
                    locationRecordModel = locationRecordBLL.GetModelByPicBox(huojia.ToString() + "-" + ceng.ToString() + "-" + lie.ToString(),true);
                    if (locationRecordModel != null)
                    { 
                        if (locationRecordBLL.GetLocNameNum(huojia.ToString() + "-" + ceng.ToString() + "-" + lie.ToString()) > 0)//有货
                    {
                        string content = "编码：" + locationRecordModel.GoodsCode + "  名称：" + locationRecordModel.GoodsName + "\n数量：" +
                                                locationRecordModel.GoodsNum + "  合同号：" + locationRecordModel.GoodsContract + "\n入库时间：" + locationRecordModel.InTime
                                                ;
                        toolTipKuCun.SetToolTip(pb, content);
                    }
                    }
                }                    
                else
                    Lastindex = -1;
            }
            else //出库
            {
                //if (flagT)
                if (true)
                {
                    KuWeiPic[Lastindex].BorderStyle = BorderStyle.Fixed3D;
                    labelKuWei.Text =huojia.ToString() + "号货架 " + ceng + "层 " + lie +"列";
                   locationRecordModel= locationRecordBLL.GetModelByPicBox(huojia.ToString()+"-"+ceng.ToString() + "-"+ lie.ToString(),true);
                    if (locationRecordModel != null)
                    { 
                        if (locationRecordBLL.GetLocNameNum(huojia.ToString() + "-" + ceng.ToString() + "-" + lie.ToString()) > 0)//有货
                    {
                        string content = "编码：" + locationRecordModel.GoodsCode + "  名称：" + locationRecordModel.GoodsName + "\n数量：" +
                        locationRecordModel.GoodsNum + "  合同号：" + locationRecordModel.GoodsContract + "\n入库时间：" + locationRecordModel.InTime
                        ;
                        toolTipKuCun.SetToolTip(pb, content);
                    }
                    }
                }
                else
                    Lastindex = -1;
            }
            
            //查询库位上的货物信息

            //for (int t = 0; t < listKuCunModel.Count; t++)
            //{  
            //    if (listKuCunModel[t].HuoJiaHao.Trim () == huojia.ToString() &&
            //        listKuCunModel [t].CengHao.Trim () == ceng.ToString () &&
            //        listKuCunModel [t].LieHao.Trim () == lie .ToString ())
            //    {
            //        string content = listKuCunModel[t].MingCheng +" "+ listKuCunModel[t].GuiGe + " " + listKuCunModel[t].XingHao + "\n数量：" + 
            //            listKuCunModel[t].ShuLiang + "\n入库时间：\n";
            //        for (int i=0;i<listKuWeiDetailModel.Count;i++)
            //        {
            //            if (listKuWeiDetailModel[i].HuoJiaHao.Trim() == huojia.ToString() &&
            //                 listKuWeiDetailModel[i].CengHao.Trim() == ceng.ToString() &&
            //                 listKuWeiDetailModel[i].LieHao.Trim() == lie.ToString())
            //            {
            //                content += listKuWeiDetailModel[i].RuKuTime + "\n";
            //            }
            //        }                    
            //        toolTipKuCun.SetToolTip(pb, content);
            //        return;
            //    }
            //}
        
        }
        private void KuWeiForm_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            HuoJiaShu = 4;//临时赋值
            CengShu = 2;//临时赋值
            LieShu = 6;//临时赋值
            ZongKuWeiShu = HuoJiaShu * CengShu * LieShu;
            InitCangKu();
            toolTipKuCun.IsBalloon = true;
            //所有库存物品：黑色
            try
            {
                #region 之前
                //if (DbHelperSQL.IsConnected())
                //{
                //    listKuCunModel = KuCunBLL.GetModelList("");
                //    int count = listKuCunModel.Count;
                //    for(int i=0;i<count;i++)
                //    {
                //        int index = 0;
                //        index += (Convert.ToInt32(listKuCunModel[i].HuoJiaHao)-1) * CengShu *LieShu 
                //            + (CengShu-Convert.ToInt32(listKuCunModel [i].CengHao ))*LieShu +
                //            Convert.ToInt32(listKuCunModel[i].LieHao)-1;
                //        KuWeiPic[index].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用黑.jpg"));

                //        //string whereSQL = string.Format(@"HuoJiaHao = '{0}' and CengHao = {1} and LieHao = {2}",
                //        //listKuCunModel[i].HuoJiaHao , listKuCunModel[i].CengHao , listKuCunModel[i].LieHao );                        
                //    }
                //    //用于记录一个库位放2件以上物品的情况
                //    if(count>0)
                //    {
                //        listKuWeiDetailModel = KuWeiDetailBLL.GetModelList("");
                //    }
                //}

                #endregion
                #region 数据库更改后
                stockGoodsBLL.DeleteAll(); //先清空库存记录表
                //DataSet ds = locationRecordBLL.GetList(1,);
                //1.筛选库位
                List<string> locNames = locationRecordBLL.GetListLocName();
                string[] result;
                string[] stringSeparators = new string[] { "-" };
                //2.计算数量
                foreach (string locName in locNames)
                {
                    if (locationRecordBLL.GetLocNameNum(locName)>0)//有货
                    {
                        //(Maticsoft.Model.StockGoods)
                        stockGoodsBLL.Add(locationRecordBLL.GetModelByPicBox(locName, true));//把此条数据更新到库存表中
                        int index = 0;
                        result = locName.Split(stringSeparators, StringSplitOptions.None);
                        index += (Convert.ToInt32(result[0]) - 1) * CengShu * LieShu
                            + (CengShu - Convert.ToInt32(result[1])) * LieShu +
                            Convert.ToInt32(result[2]) - 1;
                        KuWeiPic[index].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用黑.jpg")); //3.显示已用库位黑色
                        if (!RuChuKuForm.m_RuKuChuKuFlag)//如果出库，并且为出库编码
                        {
                            if (locationRecordBLL.GetModelByPicBox(locName,true).GoodsCode==RuChuKuForm.m_BianMa)
                            {
                                int index1 = 0;
                                result = locName.Split(stringSeparators, StringSplitOptions.None);
                                index1 += (Convert.ToInt32(result[0]) - 1) * CengShu * LieShu
                                    + (CengShu - Convert.ToInt32(result[1])) * LieShu +
                                    Convert.ToInt32(result[2]) - 1;
                                KuWeiPic[index1].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用.jpg")); //3.显示已用库位黑色
                            }
                        }
                    }
                    //else
                    //{
                       
                    //}
                   
                }
                if (DetailOperationRuKuForm.listLoca.Count > 0)
                {
                    foreach (string locName1 in DetailOperationRuKuForm.listLoca)
                    {
                        int index = 0;
                        result = locName1.Split(stringSeparators, StringSplitOptions.None);
                        index += (Convert.ToInt32(result[0]) - 1) * CengShu * LieShu
                            + (CengShu - Convert.ToInt32(result[1])) * LieShu +
                            Convert.ToInt32(result[2]) - 1;
                        KuWeiPic[index].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用黑.jpg")); //3.显示已用库位黑色
                        //KuWeiPic[index].Enabled = false;
                        //KuWeiPic[index].BackColor = Color.Green;
                    }
                }
                if (DetailOperationForm.listLoca.Count>0)
                {
                    foreach (string locName1 in DetailOperationForm.listLoca)
                    {
                        int index = 0;
                        result = locName1.Split(stringSeparators, StringSplitOptions.None);
                        index += (Convert.ToInt32(result[0]) - 1) * CengShu * LieShu
                            + (CengShu - Convert.ToInt32(result[1])) * LieShu +
                            Convert.ToInt32(result[2]) - 1;
                        KuWeiPic[index].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3.jpg")); //3.显示已用库位黑色
                        //KuWeiPic[index].Enabled = false;
                        //KuWeiPic[index].BackColor = Color.Green;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #region 之前
            //待出库物品：红色
            //for (int t=0;t<ZongKuWeiShu;t++)
            //{
            //    for(int i=0;i< RuChuKuForm.KuCunList .Count;i++)
            //    {
            //        if (KuWeiPro[t].HuoJiaNo.ToString () == RuChuKuForm.KuCunList[i].HuoJiaHao)
            //            if (KuWeiPro[t].HorizontalNo.ToString() == RuChuKuForm.KuCunList[i].CengHao )
            //                if(KuWeiPro[t].VerticalNo.ToString() == RuChuKuForm.KuCunList[i].LieHao )
            //                {
            //                    KuWeiPic[t].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用.jpg"));
            //                }
            //    }                
            //}

            #endregion 之后
        }



        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        public delegate void SetWeiZhiDelegate();
        public event SetWeiZhiDelegate SetWeiZhiEvent;
        private void btnWanCheng_Click(object sender, EventArgs e)
        {
            if (Lastindex != -1)
            {
                
                int huojia = Lastindex / LieShu / CengShu + 1;
                int ceng = CengShu - (Lastindex / LieShu) % CengShu;
                int lie = Lastindex % LieShu + 1;
                RuChuKuForm.m_WeiZhi = huojia.ToString() + "-" + ceng.ToString() + "-" + lie.ToString();
                //int index = 0;
                //index += (huojia - 1) * CengShu * LieShu
                //    + (CengShu - ceng) * LieShu +
                //    lie - 1;

                //KuWeiPic[index].Enabled = false;
                //KuWeiPic[index].BackColor = Color.Green;
                int index = (huojia - 1) * CengShu * LieShu + (CengShu - ceng) * LieShu + lie - 1;
                KuWeiPic[index].Image = Image.FromFile(System.IO.Path.GetFullPath("库位3 占用.jpg"));
                SetWeiZhiEvent();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 库位数据库
        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();
        Maticsoft.Model.StockGoods stockGoodsModel = new Maticsoft.Model.StockGoods();
        Maticsoft.BLL.StockGoods stockGoodsBLL = new Maticsoft.BLL.StockGoods();
        #endregion


        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void KuWeiForm_SizeChanged(object sender, EventArgs e)
        {
            float newx = (float)base.Width / this.X;
            float newy = (float)base.Height / this.Y;
            this.asc.setControls(newx, newy, this);
        }
    }
}
