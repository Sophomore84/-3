using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Maticsoft.Model{
	 	//StockGoods
		public class StockGoods
	{
   		     
      	/// <summary>
		/// LocationName
        /// </summary>		
		private string _locationname;
        public string LocationName
        {
            get{ return _locationname; }
            set{ _locationname = value; }
        }        
		/// <summary>
		/// GoodsName
        /// </summary>		
		private string _goodsname;
        public string GoodsName
        {
            get{ return _goodsname; }
            set{ _goodsname = value; }
        }        
		/// <summary>
		/// GoodsCode
        /// </summary>		
		private string _goodscode;
        public string GoodsCode
        {
            get{ return _goodscode; }
            set{ _goodscode = value; }
        }        
		/// <summary>
		/// GoodsContract
        /// </summary>		
		private string _goodscontract;
        public string GoodsContract
        {
            get{ return _goodscontract; }
            set{ _goodscontract = value; }
        }        
		/// <summary>
		/// GoodsNum
        /// </summary>		
		private string _goodsnum;
        public string GoodsNum
        {
            get{ return _goodsnum; }
            set{ _goodsnum = value; }
        }        
		/// <summary>
		/// InTime
        /// </summary>		
		private string _intime;
        public string InTime
        {
            get{ return _intime; }
            set{ _intime = value; }
        }        
		/// <summary>
		/// OutTime
        /// </summary>		
		private string _outtime;
        public string OutTime
        {
            get{ return _outtime; }
            set{ _outtime = value; }
        }        
		/// <summary>
		/// UserName
        /// </summary>		
		private string _username;
        public string UserName
        {
            get{ return _username; }
            set{ _username = value; }
        }        
		/// <summary>
		/// Action
        /// </summary>		
		private string _action;
        public string Action
        {
            get{ return _action; }
            set{ _action = value; }
        }        
		/// <summary>
		/// Weight
        /// </summary>		
		private string _weight;
        public string Weight
        {
            get{ return _weight; }
            set{ _weight = value; }
        }
        //// �Զ������ʽת��: �� MyClass1 ���͵Ķ���ת���� float ����
        //public static explicit operator StockGoods(LocationRecord obj)
        //{   // ʵ�����ǽ� �ֶ� b ��ֵת����float����Ϊ int ת float ����ʽ�ģ����Զ����ʱ����Զ���Ϊ implicit
            
        //    return new StockGoods();
        //}
        //public static explicit operator LocationRecord(StockGoods obj)
        //{   // ʵ�����ǽ� �ֶ� b ��ֵת����float����Ϊ int ת float ����ʽ�ģ����Զ����ʱ����Զ���Ϊ implicit

        //    return new LocationRecord();
        //}
    }
}

