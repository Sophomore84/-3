using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自动化库存管理.Model
{
    /// <summary>
    /// 缓存任务
    /// </summary>
    public class Assignment
    {
        private string id;//任务id
        private string location;//货物位置
        private string code;//货物编码
        private string name;//货物名称
        private string number;//货物数量
        private string contractNum;//货物合同号
        private string operation;//机器或人工
        private string platform;//上货台

        public string Location { get => location; set => location = value; }
        public string Code { get => code; set => code = value; }
        public string Number { get => number; set => number = value; }
        public string ContractNum { get => contractNum; set => contractNum = value; }
        public string Operation { get => operation; set => operation = value; }
        public string Platform { get => platform; set => platform = value; }
        public string Name { get => name; set => name = value; }
        public string Id { get => id; set => id = value; }
    }
}
