/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.CONFIG;
using BAMENG.IDAL;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.DAL
{
    public class ConfigDAL : AbstractDAL, IConfigDAL
    {

        /// <summary>
        /// 基本选择定义
        /// </summary>
        private const string SQL_SELECT = @"select Code,Value,Remark from BM_BaseConfig  with(nolock) where Code=@Code";

        private const string SQL_UPDATE_VALUE = @"IF EXISTS (SELECT * FROM BM_BaseConfig WHERE Code=@Code)  
                                            BEGIN
                                                UPDATE BM_BaseConfig set Value=@Value where Code=@Code
                                            END
                                            ELSE 
                                            BEGIN
                                                INSERT INTO BM_BaseConfig(Code,Value,Remark) VALUES(@Code,@Value,@Remark)
                                            END";



        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateValue(ConfigModel model)
        {
            var parm = new[]{
                        new SqlParameter("@Code", model.Code),
                        new SqlParameter("@Value", model.Value),
                        new SqlParameter("@Remark", model.Value)
                        };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, SQL_UPDATE_VALUE, parm) > 0;
        }

        /// <summary>
        /// 根据CODE获取实体信息
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>ConfigModel.</returns>
        public ConfigModel GetModel(string code)
        {
            var parm = new[] { new SqlParameter("@Code", code) };
            ConfigModel model = null;
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, SQL_SELECT, parm))
            {
                model = DbHelperSQLP.GetEntity<ConfigModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 获取所有的配置数据
        /// </summary>
        /// <returns>List&lt;ConfigModel&gt;.</returns>
        public List<ConfigModel> List()
        {
            string strsql = @"select Code,Value,Remark from BM_BaseConfig with(nolock) ";
            List<ConfigModel> list = new List<ConfigModel>();
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strsql, null))
            {
                list = DbHelperSQLP.GetEntityList<ConfigModel>(dr);
            }
            return list;
        }

        /// <summary>
        /// 根据CODE获取值
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        public string GetValue(string code)
        {
            string val = string.Empty;
            string strsql = @"select Value from BM_BaseConfig  with(nolock) where Code=@Code";
            var parm = new[] { new SqlParameter("@Code", code) };
            SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strsql, parm);
            if (dr.Read())
            {
                val = !dr.IsDBNull(0) ? dr.GetString(0) : string.Empty;
            }
            dr.Close();
            return val;
        }
    }
}
