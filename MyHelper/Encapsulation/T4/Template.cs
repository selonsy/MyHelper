using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Devin
{   	
	/// <summary>
	/// 书签表
	/// </summary>
    public partial class BOOKMARK
    {
        ///<summary>字段枚举</summary>	
		public enum EBOOKMARK 
		{ 	
			///<summary>书签ID</summary>
			Id,	
			///<summary>书签名称</summary>
			BMName,	
			///<summary>书签链接</summary>
			BMUrl,	
			///<summary></summary>
			BCGuid,	
			///<summary>书签小图标</summary>
			BMIcon,	
			///<summary>书签大图标</summary>
			BMIcon_big,	
			///<summary>书签优先级</summary>
			BMOrder,
		}

        ///<summary>字段列表</summary>	
        public static string[] ps = { "Id", "BMName", "BMUrl", "BCGuid", "BMIcon", "BMIcon_big", "BMOrder" };
        ///<summary>对应数据库的表名</summary>	
        public static string tablename = "BookMark";

		#region 字段成员

		///<summary>书签ID</summary>		
		public string Id { get; set; }
		///<summary>书签名称</summary>		
		public string BMName { get; set; }
		///<summary>书签链接</summary>		
		public string BMUrl { get; set; }
		///<summary></summary>		
		public string BCGuid { get; set; }
		///<summary>书签小图标</summary>		
		public string BMIcon { get; set; }
		///<summary>书签大图标</summary>		
		public string BMIcon_big { get; set; }
		///<summary>书签优先级</summary>		
		public int? BMOrder { get; set; }
		
		#endregion
    }
}

