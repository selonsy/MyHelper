using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Devin
{   	
	/// <summary>
	/// 
	/// </summary>
    public partial class USERS
    {
        ///<summary>字段枚举</summary>	
		public enum EUSERS 
		{ 	
			///<summary>用户id</summary>
			UserGuid,	
			///<summary>用户名称</summary>
			UserName,	
			///<summary>手机号码</summary>
			Phone,	
			///<summary>邮箱</summary>
			Email,	
			///<summary>添加时间</summary>
			AddTime,	
			///<summary>修改时间</summary>
			ModifyTime,	
			///<summary>是否有效</summary>
			Activity,
		}

        ///<summary>字段列表</summary>	
        public static string[] ps = { "UserGuid", "UserName", "Phone", "Email", "AddTime", "ModifyTime", "Activity" };
        ///<summary>对应数据库的表名</summary>	
        public static string tablename = "Users";

		#region 字段成员

		///<summary>用户id</summary>		
		public string UserGuid { get; set; }
		///<summary>用户名称</summary>		
		public string UserName { get; set; }
		///<summary>手机号码</summary>		
		public string Phone { get; set; }
		///<summary>邮箱</summary>		
		public string Email { get; set; }
		///<summary>添加时间</summary>		
		public DateTime AddTime { get; set; }
		///<summary>修改时间</summary>		
		public DateTime? ModifyTime { get; set; }
		///<summary>是否有效</summary>		
		public int Activity { get; set; }
		
		#endregion
    }
}

