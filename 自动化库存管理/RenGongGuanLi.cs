using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 养生池;

namespace 自动化库存管理
{
    public partial class RenGongGuanLi : Form
    {
        
        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;

        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();

        Maticsoft.Model.tb_KuCunRecords? tb_KuCunRecordsModel = new Maticsoft.Model.tb_KuCunRecords();
        Maticsoft.BLL.tb_KuCunRecords tb_KuCunRecordsBLL = new Maticsoft.BLL.tb_KuCunRecords();

        Maticsoft.Model.tb_WuPinDetail? tb_WuPinDetailModel = new Maticsoft.Model.tb_WuPinDetail();
        Maticsoft.BLL.tb_WuPinDetail tb_WuPinDetailBLL = new Maticsoft.BLL.tb_WuPinDetail();

        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();

        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();

        Maticsoft.Model.tb_KuCun tb_KuCunModel = new Maticsoft.Model.tb_KuCun();
        Maticsoft.BLL.tb_KuCun tb_KuCunBLL = new Maticsoft.BLL.tb_KuCun();
        Maticsoft.Model.StockGoods stockGoodsModel = new Maticsoft.Model.StockGoods();
        Maticsoft.BLL.StockGoods stockGoodsBLL = new Maticsoft.BLL.StockGoods();
        public RenGongGuanLi()
        {
            InitializeComponent();
            this.Resize += RenGongGuanLi_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);

        }
        //public RenGongGuanLi(int nx, int ny)
        //{
        //    InitializeComponent();
        //    this.X = (float)base.Width;
        //    this.Y = (float)base.Height;
        //    this.asc.setTag(this);
        //    base.Width = nx;
        //    base.Height = ny;
        //    float newx = (float)nx / this.X;
        //    float newy = (float)ny / this.Y;
        //    this.asc.setControls(newx, newy, this);

        //}


        private void btnRead_Click(object sender, EventArgs e)
        {
            IList<Maticsoft.Model.tb_KuWei> tb_KuWeiList = tb_KuWeiBLL.GetModelList("");
            dataGridViewKuWei.DataSource = tb_KuWeiList;

           

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKuWeiName.Text))
            {
                MessageBox.Show("库位名称输入有误！");
                return;
            }
            if (MessageBox.Show("确认要修改当前选中的库位坐标吗?", "警告？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_KuWeiModel.KuWeiName = txtKuWeiName.Text;
                tb_KuWeiModel.X = txtX.Text;
                tb_KuWeiModel.Y = tXtY.Text;
                tb_KuWeiModel.Z = txtZ.Text;
                tb_KuWeiModel.Precision = kuweiPrecision.Text;
                if (tb_KuWeiBLL.Update(tb_KuWeiModel))
                {
                    btnRead_Click(sender,e);
                    Thread.Sleep(500);
                    MessageBox.Show("已经更新成功！");
                    //本地数据库更新事件
                }
                else
                {
                    MessageBox.Show("数据更新失败!请再次检查当前数据是否确?");
                }
            }

            }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            RuChuKuForm.logForm.Show();
            
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
        private void RenGongGuanLi_Load(object sender, EventArgs e)
        {
            //this.Resize += RenGongGuanLi_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);
            //setTag(this);
            //resizeEnable = true;
            //orignalparentHeight = this.Height;
            //orignalparentWidth = this.Width;
            //this.WindowState = FormWindowState.Maximized;
        }

        private void RenGongGuanLi_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

       

        private void dataGridViewKuWei_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                tb_KuWeiModel = tb_KuWeiBLL.GetModel(dataGridViewKuWei.SelectedRows[0].Cells[0].Value.ToString()
                    //, 
                    //dataGridViewKuWei.SelectedRows[0].Cells[1].Value.ToString(),
                    //dataGridViewKuWei.SelectedRows[0].Cells[2].Value.ToString(),
                    //dataGridViewKuWei.SelectedRows[0].Cells[3].Value.ToString()
                    );
                txtKuWeiName.ReadOnly = false;
                txtKuWeiName.Text = tb_KuWeiModel.KuWeiName;
                txtKuWeiName.ReadOnly = true;
                txtX.Text = tb_KuWeiModel.X;
                tXtY.Text = tb_KuWeiModel.Y;
                txtZ.Text = tb_KuWeiModel.Z;
                kuweiPrecision.Text = tb_KuWeiModel.Precision;

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private void btnReadDingDian_Click(object sender, EventArgs e)
        {
            IList<Maticsoft.Model.tb_DingDian> tb_DingDianList = tb_DingDianBLL.GetModelList("");
            dataGridViewDingDian.DataSource = tb_DingDianList;
        }

        private void btnUpdateDingDian_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDingDian.Text))
            {
                MessageBox.Show("定点名称输入有误！");
                return;
            }
            if (MessageBox.Show("确认要修改当前选中的定点坐标吗?", "警告？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_DingDianModel.DingDianName = txtDingDian.Text;
                tb_DingDianModel.X = txtDingDianX.Text;
                tb_DingDianModel.Y = txtDingDianY.Text;
                tb_DingDianModel.Z = txtDingDianZ.Text;
                tb_DingDianModel.Precision = dingPrecision.Text;
                if (tb_DingDianBLL.Update(tb_DingDianModel))
                {
                    btnReadDingDian_Click(sender, e);
                    Thread.Sleep(500);
                    MessageBox.Show("已经更新成功！");
                }
                else
                {
                    MessageBox.Show("数据更新失败!请再次检查当前数据是否确?");
                }
            }
        }

        private void dataGridViewDingDian_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                tb_DingDianModel = tb_DingDianBLL.GetModel(dataGridViewDingDian.SelectedRows[0].Cells[0].Value.ToString()
                    //,
                    //dataGridViewDingDian.SelectedRows[0].Cells[1].Value.ToString(),
                    //dataGridViewDingDian.SelectedRows[0].Cells[2].Value.ToString(),
                    //dataGridViewDingDian.SelectedRows[0].Cells[3].Value.ToString()
                    );
                txtDingDian.ReadOnly = false;
                txtDingDian.Text = tb_DingDianModel.DingDianName;
                txtDingDian.ReadOnly = true;
                txtDingDianX.Text = tb_DingDianModel.X;
                txtDingDianY.Text = tb_DingDianModel.Y;
                txtDingDianZ.Text = tb_DingDianModel.Z;
                dingPrecision.Text = tb_DingDianModel.Precision;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

   

       

      

        private void btnReadChuKu_Click(object sender, EventArgs e)
        {
            IList<Maticsoft.Model.LocationRecord> locationRecordList = locationRecordBLL.GetModelList(" OutTime is not null and OutTime != '' order by cast(OutTime as datetime) desc");
            foreach (var item in locationRecordList)
            {
                item.GoodsNum = item.GoodsNum.Remove(0, 1);
            }
            dataGridViewOut.DataSource = locationRecordList;
        }

        private void dataGridViewKuCunRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //tb_KuCunRecordsModel = tb_KuCunRecordsBLL.GetModel(dataGridViewOut.SelectedRows[0].Cells[0].Value.ToString(),
            //    dataGridViewKuCunRecords.SelectedRows[0].Cells[1].Value.ToString(),
            //    dataGridViewKuCunRecords.SelectedRows[0].Cells[2].Value.ToString()
            //    );
            //chuKuName.Text = tb_KuCunRecordsModel.MingCheng;
            //chuKuBianMa.Text = tb_KuCunRecordsModel.BianMa;
            //chuKuGuiGe.Text = tb_KuCunRecordsModel.GuiGe;
            //chuKuHeTongHao.Text = tb_KuCunRecordsModel.HeTongHao;
            //chuKuHuoJia.Text = tb_KuCunRecordsModel.HuoJiaHao;
            //chuKuCeng.Text = dataGridViewKuCunRecords.SelectedRows[0].Cells[1].Value.ToString();
            //chuKuLie.Text = dataGridViewKuCunRecords.SelectedRows[0].Cells[2].Value.ToString();
        }

        
       

        private void btnExcel_Click(object sender, EventArgs e)
        {
            #region
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == "")
                return;
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string str = "";
            try
            {
                for (int i = 0; i < dataGridViewOut.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridViewOut.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                for (int j = 0; j < dataGridViewOut.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridViewOut.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        tempStr += dataGridViewOut.Rows[j].Cells[k].Value.ToString();
                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
            #endregion
        }

       

        private void btnUpdateIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inCode.Text))
            {
                MessageBox.Show("货物编码输入有误！");
                return;
            }
            if (MessageBox.Show("确认要修改当前选中的入库记录吗?", "警告？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (inNum.Text == "0")//如果货物数量为0，更新为空位
                {

                    if (locationRecordBLL.DeleteByLocationName(inLocation.Text,txtInTime.Text,false))//删除此行数据
                    {
                        btnIn_Click(sender, e);
                        Thread.Sleep(500);
                        inName.Text = "";
                        inCode.Text = "";
                        inContract.Text = "";
                        inNum.Text = "";
                        txtInTime.Text = "";
                        inLocation.Text = "";

                        MessageBox.Show("已经更新成功！");

                    }
                }
                else//修改库位上货物信息
                {
                    locationRecordModel.GoodsCode = inCode.Text;
                    locationRecordModel.GoodsName = inName.Text;
                    locationRecordModel.GoodsNum ="+"+ inNum.Text;
                    locationRecordModel.GoodsContract = inContract.Text;
                    locationRecordModel.InTime = txtInTime.Text;
                    locationRecordModel.LocationName = inLocation.Text;
                    locationRecordModel.UserName = txtUsrName.Text;
                    locationRecordModel.Action = comboBoxQuHuoFangShi.Text;
                    locationRecordModel.Weight = inWeight.Text;
                    if (locationRecordBLL.UpdateByLocName(locationRecordModel,false))//修改货物编码相应货物信息也要修改
                    {
                        btnIn_Click(sender, e);

                        Thread.Sleep(500);
                        comboBoxQuHuoFangShi.SelectedIndex = -1;
                        inName.Text = "";
                        inCode.Text = "";
                        inContract.Text = "";
                        inNum.Text = "";
                        txtInTime.Text = "";
                        inLocation.Text = "";
                        txtUsrName.Text = "";
                        inWeight.Text = "";
                        MessageBox.Show("已经更新成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("数据更新失败!请再次检查当前数据是否确?");
            }

            //更新为新的货物或加数量或加合同号
            //
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            IList<Maticsoft.Model.LocationRecord> locationRecordList = locationRecordBLL.GetModelList(" InTime is not null and InTime != '' order by cast(InTime as datetime) desc");
            foreach (var item in locationRecordList)
            {
                item.GoodsNum = item.GoodsNum.Remove(0,1);
            }
            dataGridViewIn.DataSource = locationRecordList;
        }

        private void dataGridViewIn_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //locationRecordModel = locationRecordBLL.GetModelByPicBox(dataGridViewIn.SelectedRows[0].Cells[0].Value.ToString()
                //    );
                //inName.Text = locationRecordModel.GoodsName;
                //inContract.Text = locationRecordModel.GoodsContract;
                //inCode.Text = locationRecordModel.GoodsCode;
                //inNum.Text = locationRecordModel.GoodsNum.Remove(0,1);
                //inLocation.Text = locationRecordModel.LocationName;
                //txtInTime.Text = locationRecordModel.InTime;
                //comboBoxQuHuoFangShi.Text = locationRecordModel.Action;
                //txtUsrName.Text = locationRecordModel.UserName;
                inName.Text = dataGridViewIn.SelectedRows[0].Cells["货物名称"].Value.ToString();
                inContract.Text = dataGridViewIn.SelectedRows[0].Cells["合同号"].Value.ToString();
                inCode.Text = dataGridViewIn.SelectedRows[0].Cells["货物编码"].Value.ToString();
                inNum.Text = dataGridViewIn.SelectedRows[0].Cells["货物数量"].Value.ToString();
                inLocation.Text = dataGridViewIn.SelectedRows[0].Cells["库位"].Value.ToString();
                txtInTime.Text = dataGridViewIn.SelectedRows[0].Cells["入库时间"].Value.ToString();
                comboBoxQuHuoFangShi.Text = dataGridViewIn.SelectedRows[0].Cells["操作"].Value.ToString();
                txtUsrName.Text = dataGridViewIn.SelectedRows[0].Cells["操作员"].Value.ToString();
                if (dataGridViewIn.SelectedRows[0].Cells["重量"].Value == null)
                {
                    inWeight.Text = "";
                }
                else
                {
                    inWeight.Text = dataGridViewIn.SelectedRows[0].Cells["重量"].Value.ToString();
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
           

        }

        private void btnUpdateChuKu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outCode.Text))
            {
                MessageBox.Show("货物编码输入有误！");
                return;
            }
            if (MessageBox.Show("确认要修改当前选中的出库记录吗?", "警告？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (outNum.Text == "0")//如果货物数量为0，更新为空位
                {

                    if (locationRecordBLL.DeleteByLocationName(outLoc.Text,outTim.Text,true))//删除此行数据
                    {
                        btnReadChuKu_Click(sender,e);
                        Thread.Sleep(500);
                        outName.Text = "";
                        outCode.Text = "";
                        outContract.Text = "";
                        outNum.Text = "";
                        outTim.Text = "";
                        outLoc.Text = "";
                        comboBoxOut.SelectedIndex = -1;
                        outPeo.Text = "";
                        outWeight.Text = "";
                        MessageBox.Show("已经更新成功！");

                    }
                }
                else//修改库位上货物信息
                {
                    locationRecordModel.GoodsCode = outCode.Text;
                    locationRecordModel.GoodsName = outName.Text;
                    locationRecordModel.GoodsNum = "-"+outNum.Text;
                    locationRecordModel.GoodsContract = outContract.Text;
                    locationRecordModel.OutTime = outTim.Text;
                    locationRecordModel.InTime = null;
                    locationRecordModel.LocationName = outLoc.Text;
                    locationRecordModel.UserName = outPeo.Text;
                    locationRecordModel.Action = comboBoxOut.Text;
                    locationRecordModel.Weight = outWeight.Text;
                    if (locationRecordBLL.UpdateByLocName(locationRecordModel,true))//修改货物编码相应货物信息也要修改
                    {
                        btnReadChuKu_Click(sender, e);

                        Thread.Sleep(500);
                        outName.Text = "";
                        outCode.Text = "";
                        outContract.Text = "";
                        outNum.Text = "";
                        outTim.Text = "";
                        outLoc.Text = "";
                        comboBoxOut.SelectedIndex = -1;
                        outPeo.Text = "";
                        outWeight.Text = "";
                        MessageBox.Show("已经更新成功！");
                    }
                    else
                    {
                        MessageBox.Show("更新失败");
                    }
                }
            }
            else
            {
                MessageBox.Show("数据更新失败!请再次检查当前数据是否确?");
            }

            //更新为新的货物或加数量或加合同号
            //
        }

        private void inCode_MouseLeave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inCode.Text))
            {

                tb_WuPinDetailModel = tb_WuPinDetailBLL.GetModel(inCode.Text);
                if (tb_WuPinDetailModel != null)
                {
                    {
                        inName.Text = tb_WuPinDetailModel.MingCheng;
                        
                    }

                }

            }
        }

        private void dataGridViewOut_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //locationRecordModel = locationRecordBLL.GetModelByPicBox(dataGridViewOut.SelectedRows[0].Cells[0].Value.ToString()
                //   );
                //outName.Text = locationRecordModel.GoodsName;
                //outContract.Text = locationRecordModel.GoodsContract;
                //outCode.Text = locationRecordModel.GoodsCode;
                //outNum.Text = locationRecordModel.GoodsNum.Remove(0, 1);
                //outLoc.Text = locationRecordModel.LocationName;
                //outTim.Text = locationRecordModel.InTime;
                //outPeo.Text = locationRecordModel.UserName;
                //comboBoxOut.Text = locationRecordModel.Action;
                outName.Text = dataGridViewOut.SelectedRows[0].Cells["出货物名称"].Value.ToString();
                outContract.Text = dataGridViewOut.SelectedRows[0].Cells["出合同号"].Value.ToString();
                outCode.Text = dataGridViewOut.SelectedRows[0].Cells["出货物编码"].Value.ToString();
                outNum.Text = dataGridViewOut.SelectedRows[0].Cells["出货物数量"].Value.ToString();
                outLoc.Text = dataGridViewOut.SelectedRows[0].Cells["出库位"].Value.ToString();
                outTim.Text = dataGridViewOut.SelectedRows[0].Cells["出出库时间"].Value.ToString();
                comboBoxOut.Text = dataGridViewOut.SelectedRows[0].Cells["出操作"].Value.ToString();
                outPeo.Text = dataGridViewOut.SelectedRows[0].Cells["出操作员"].Value.ToString();

                if (dataGridViewOut.SelectedRows[0].Cells["出重量"].Value == null)
                {
                    outWeight.Text = "";
                }
                else
                {
                    outWeight.Text = dataGridViewOut.SelectedRows[0].Cells["出重量"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
           
        }

        private void outCode_MouseLeave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inCode.Text))
            {

                tb_WuPinDetailModel = tb_WuPinDetailBLL.GetModel(outCode.Text);
                if (tb_WuPinDetailModel != null)
                {
                    {
                        outName.Text = tb_WuPinDetailModel.MingCheng;

                    }

                }

            }
        }

        private void btnExc_Click(object sender, EventArgs e)
        {
            #region
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == "")
                return;
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string str = "";
            try
            {
                for (int i = 0; i < dataGridViewIn.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridViewIn.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                for (int j = 0; j < dataGridViewIn.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridViewIn.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        tempStr += dataGridViewIn.Rows[j].Cells[k].Value.ToString();
                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
            #endregion

        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void RenGongGuanLi_SizeChanged(object sender, EventArgs e)
        {

        }

        private void inAdd_Click(object sender, EventArgs e)
        {
            try
            {
                locationRecordModel.GoodsCode = inCode.Text;
                locationRecordModel.GoodsName = inName.Text;
                locationRecordModel.GoodsNum = "+" + inNum.Text;
                locationRecordModel.GoodsContract = inContract.Text;
                locationRecordModel.InTime = txtInTime.Text;
                locationRecordModel.LocationName = inLocation.Text;
                locationRecordModel.UserName = txtUsrName.Text;
                locationRecordModel.Action = comboBoxQuHuoFangShi.Text;
                locationRecordModel.OutTime = null;
                locationRecordModel.Weight = inWeight.Text;
                //if (locationRecordBLL.UpdateByLocName(locationRecordModel, false))//修改货物编码相应货物信息也要修改
                //if (locationRecordBLL.Add(locationRecordModel))//修改货物编码相应货物信息也要修改
                //{
                locationRecordBLL.Add(locationRecordModel);
                btnIn_Click(sender, e);

                Thread.Sleep(500);
                comboBoxQuHuoFangShi.SelectedIndex = -1;
                inName.Text = "";
                inCode.Text = "";
                inContract.Text = "";
                inNum.Text = "";
                txtInTime.Text = "";
                inLocation.Text = "";
                txtUsrName.Text = "";
                inWeight.Text = "";
                MessageBox.Show("已经添加成功！");
            }
            catch (Exception ex)
            {

                //throw;
                MessageBox.Show("入库添加失败：" + ex.ToString());
            }
            
            //}
     
            //else
            //{
            //    MessageBox.Show("数据添加失败!请再次检查当前数据是否确?");
            //}
        }

        private void outAdd_Click(object sender, EventArgs e)
        {
            try
            {
                locationRecordModel.GoodsCode = outCode.Text;
                locationRecordModel.GoodsName = outName.Text;
                locationRecordModel.GoodsNum = "-" + outNum.Text;
                locationRecordModel.GoodsContract = outContract.Text;
                locationRecordModel.OutTime = outTim.Text;
                locationRecordModel.LocationName = outLoc.Text;
                locationRecordModel.UserName = outPeo.Text;
                locationRecordModel.Action = comboBoxOut.Text;
                locationRecordModel.InTime = null;
                locationRecordModel.Weight = outWeight.Text;
                locationRecordBLL.Add(locationRecordModel);//修改货物编码相应货物信息也要修改


                    btnReadChuKu_Click(sender, e);

                    Thread.Sleep(500);
                    outName.Text = "";
                    outCode.Text = "";
                    outContract.Text = "";
                    outNum.Text = "";
                    outTim.Text = "";
                    outLoc.Text = "";
                    comboBoxOut.SelectedIndex = -1;
                    outPeo.Text = "";
                outWeight.Text = "";
                    MessageBox.Show("已经更新成功！");
              
    }
            catch (Exception ex)
            {
                MessageBox.Show("出库添加失败："+ex.ToString());
                
            }
            
            
        }

        private void readStock_Click(object sender, EventArgs e)
        {
            IList<Maticsoft.Model.StockGoods> stockList = stockGoodsBLL.GetModelList(" InTime is not null and InTime != ''  order by LocationName asc ");
            foreach (var item in stockList)
            {
                item.GoodsNum = item.GoodsNum.Remove(0, 1);
            }
            dataGridViewStock.DataSource = stockList;
        }

        private void dataGridViewStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                stockName.Text = dataGridViewStock.SelectedRows[0].Cells["名称s"].Value.ToString();
                stockContract.Text = dataGridViewStock.SelectedRows[0].Cells["合同号s"].Value.ToString();
                stockCode.Text = dataGridViewStock.SelectedRows[0].Cells["编码s"].Value.ToString();
                stockNum.Text = dataGridViewStock.SelectedRows[0].Cells["数量s"].Value.ToString();
                stockLocation.Text = dataGridViewStock.SelectedRows[0].Cells["库位s"].Value.ToString();
                stockTime.Text = dataGridViewStock.SelectedRows[0].Cells["时间s"].Value.ToString();
                stockAction.Text = dataGridViewStock.SelectedRows[0].Cells["操作s"].Value.ToString();
                stockPeople.Text = dataGridViewStock.SelectedRows[0].Cells["操作员s"].Value.ToString();
                if (dataGridViewStock.SelectedRows[0].Cells["重量s"].Value == null)
                {
                    inWeight.Text = "";
                }
                else
                {
                    inWeight.Text = dataGridViewStock.SelectedRows[0].Cells["重量s"].Value.ToString();
                }


            }
            catch (Exception ex)
            {

                //throw;
            }
        }
    }
}
