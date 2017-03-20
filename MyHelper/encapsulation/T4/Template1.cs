using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Devin
{   	
	/// <summary>
	/// 日记表
	/// </summary>
    public partial class DIARY
    {
        ///<summary>字段枚举</summary>	
		public enum EDIARY 
		{ 	
			///<summary>主键</summary>
			Id,	
			///<summary>标题</summary>
			Title,	
			///<summary>内容</summary>
			Content,	
			///<summary>作者</summary>
			Author,	
			///<summary>天气</summary>
			Weather,	
			///<summary>位置</summary>
			Location,	
			///<summary>温度</summary>
			Temperature,	
			///<summary>用户id</summary>
			UserId,	
			///<summary>添加时间</summary>
			AddTime,	
			///<summary>修改时间</summary>
			ModifyTime,	
			///<summary>分类Id</summary>
			DiaryCategoryId,
		}

        ///<summary>字段列表</summary>	
        public static string[] ps = { "Id", "Title", "Content", "Author", "Weather", "Location", "Temperature", "UserId", "AddTime", "ModifyTime", "DiaryCategoryId" };
        ///<summary>对应数据库的表名</summary>	
        public static string tablename = "Diary";

		#region 字段成员

		///<summary>主键</summary>		
		public string Id { get; set; }
		///<summary>标题</summary>		
		public string Title { get; set; }
		///<summary>内容</summary>		
		public string Content { get; set; }
		///<summary>作者</summary>		
		public string Author { get; set; }
		///<summary>天气</summary>		
		public string Weather { get; set; }
		///<summary>位置</summary>		
		public string Location { get; set; }
		///<summary>温度</summary>		
		public string Temperature { get; set; }
		///<summary>用户id</summary>		
		public string UserId { get; set; }
		///<summary>添加时间</summary>		
		public DateTime AddTime { get; set; }
		///<summary>修改时间</summary>		
		public DateTime? ModifyTime { get; set; }
		///<summary>分类Id</summary>		
		public string DiaryCategoryId { get; set; }
		
		#endregion
    }
}

