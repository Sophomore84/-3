using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 养生池
{
    internal class AutoSizeFormClass
    {
        // Token: 0x0600017C RID: 380 RVA: 0x00012CC4 File Offset: 0x00010EC4
        public void setTag(Control cons)
        {
            foreach (object obj in cons.Controls)
            {
                Control control = (Control)obj;
                control.Tag = string.Concat(new object[]
                {
                    control.Width,
                    ":",
                    control.Height,
                    ":",
                    control.Left,
                    ":",
                    control.Top,
                    ":",
                    control.Font.Size
                });
                if (control.Controls.Count > 0)
                {
                    this.setTag(control);
                }
            }
        }

        // Token: 0x0600017D RID: 381 RVA: 0x00012DCC File Offset: 0x00010FCC
        public void setControls(float newx, float newy, Control cons)
        {
            try
            {
                //遍历窗体中的控件，重新设置控件的值
                foreach (object obj in cons.Controls)
                {
                    //获取控件的Tag属性值，并分割后存储字符串数组
                    Control control = (Control)obj;
                    if (control.Tag != null)
                    {
                        //根据窗体缩放的比例确定控件的值
                        this.mytag = control.Tag.ToString().Split(new char[]
                        {
                            ':'
                        });
                        float num = Convert.ToSingle(this.mytag[0]) * newx;
                        control.Width = (int)num;
                        num = Convert.ToSingle(this.mytag[1]) * newy;
                        control.Height = (int)num;
                        num = Convert.ToSingle(this.mytag[2]) * newx;
                        control.Left = (int)num;
                        num = Convert.ToSingle(this.mytag[3]) * newy;
                        control.Top = (int)num;
                        float emSize = Convert.ToSingle(this.mytag[4]) * Math.Min(newx, newy);
                        control.Font = new Font(control.Font.Name, emSize, control.Font.Style, control.Font.Unit);
                        if (control.Controls.Count > 0)
                        {
                            this.setControls(newx, newy, control);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x04000141 RID: 321
        private string[] mytag;
    }
}
