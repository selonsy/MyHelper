using System;

namespace Devin.MongoDB.MongoDbConfig
{
    #region Mongo实体标签

    /// <summary>
    /// Mongo实体标签
    /// </summary>
    public class MongoAttribute : Attribute
    {
        public MongoAttribute(string collection)
        {
            Database = Config.DBMongoStr.Split(';')[1];
            Collection = collection;
        }

        public MongoAttribute(string database, string collection)
        {
            Database = database;
            Collection = collection;
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// 集合名称
        /// </summary>
        public string Collection { get; private set; }

    }

    #endregion
}
