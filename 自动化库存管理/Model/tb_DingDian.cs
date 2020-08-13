using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Maticsoft.Model{
	 	//tb_DingDian
		public class tb_DingDian
	{
   		     
      	/// <summary>
		/// DingDianName
        /// </summary>		
		private string _dingdianname;
        public string DingDianName
        {
            get{ return _dingdianname; }
            set{ _dingdianname = value; }
        }        
		/// <summary>
		/// X
        /// </summary>		
		private string _x;
        public string X
        {
            get{ return _x; }
            set{ _x = value; }
        }        
		/// <summary>
		/// Y
        /// </summary>		
		private string _y;
        public string Y
        {
            get{ return _y; }
            set{ _y = value; }
        }        
		/// <summary>
		/// Z
        /// </summary>		
		private string _z;
        public string Z
        {
            get{ return _z; }
            set{ _z = value; }
        }
        /// <summary>
        /// Precision
        /// </summary>		
        private string _precision;
        public string Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

    }
}

