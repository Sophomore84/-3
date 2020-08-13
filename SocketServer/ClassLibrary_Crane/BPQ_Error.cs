using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Crane
{
    public class BPQ_Error
    {
        //一键撤销任务
        public bool ChexiaoTask { get; set; }

        //小车变频器故障信号
        public bool QiShengBPQError { get; set; }

        //大车变频器故障信号
        public bool DaCheBPQError { get; set; }

        //小车变频器故障信号
        public bool XiaoCheBPQError { get; set; }

        //旋转变频器故障信号
        public bool XuanZhuanBPQError { get; set; }

        //夹具通讯故障信号
        public bool JiaJuTongXunError { get; set; }

    }
}
