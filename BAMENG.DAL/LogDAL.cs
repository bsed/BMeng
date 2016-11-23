/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using System.Data.SqlClient;
using BAMENG.CONFIG;
using System.Data;

namespace BAMENG.DAL
{
    /// <summary>
    /// 日志数据库操作
    /// </summary>
    /// <seealso cref="BAMENG.DAL.AbstractDAL" />
    /// <seealso cref="BAMENG.IDAL.ILogDAL" />
    public class LogDAL : AbstractDAL, ILogDAL
    {
        /// <summary>
        /// 添加资讯阅读日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool AddReadLog(ReadLogModel logModel)
        {
            string strSql = "insert into BM_ReadLog(UserId,ArticleId,IsRead,ClientIp,cookie,ReadTime) values(@UserId,@ArticleId,@IsRead,@ClientIp,@cookie,@ReadTime)";
            var param = new[] {
                new SqlParameter("@UserId",logModel.UserId),
                new SqlParameter("@ArticleId",logModel.ArticleId),
                new SqlParameter("@IsRead",logModel.IsRead),
                new SqlParameter("@ClientIp",logModel.ClientIp),
                new SqlParameter("@cookie",logModel.cookie),
                new SqlParameter("@ReadTime",logModel.ReadTime)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 根据cookie判断当前资讯是否已阅读
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="cookie">The cookie.</param>
        /// <returns>true if the specified article identifier is read; otherwise, false.</returns>
        public bool IsRead(int articleId, string cookie)
        {
            string strSql = "select COUNT(1) from BM_ReadLog where ArticleId=@ArticleId and cookie=@cookie";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId),
                new SqlParameter("@cookie",cookie)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 根据客户的ip判断，当前资讯是已阅读
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified client identifier is read; otherwise, false.</returns>
        public bool IsRead(string clientId, int articleId)
        {
            string strSql = "select COUNT(1) from BM_ReadLog where ArticleId=@ArticleId and ClientIp=@ClientIp";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId),
                new SqlParameter("@ClientIp",clientId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 根据用户ID判断当前资讯是否已阅读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified user identifier is read; otherwise, false.</returns>
        public bool IsRead(int userId, int articleId)
        {
            string strSql = "select COUNT(1) from BM_ReadLog where ArticleId=@ArticleId and UserId=@UserId";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId),
                new SqlParameter("@UserId",userId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 根据用户ID判断当前资讯是否已阅读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified user identifier is reads; otherwise, false.</returns>
        public bool IsReadbyIdentity(int userId, int articleId)
        {
            string strSql = "select COUNT(1) from BM_ReadLog where ArticleId=@ArticleId and UserId=@UserId and IsRead=1";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId),
                new SqlParameter("@UserId",userId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 更新用户阅读状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateReadStatus(int userId, int articleId)
        {
            string strSql = "update BM_ReadLog set IsRead=1 where UserId=@UserId and ArticleId=@ArticleId";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId),
                new SqlParameter("@UserId",userId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }


        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool AddLoginLog(LoginLogModel logModel)
        {
            string strSql = @"insert into BM_UserLoginLog(UserId,UserIdentity,BelongOne,ShopId,AppSystem) values(@UserId,@UserIdentity,@BelongOne,@ShopId,@AppSystem)";
            var param = new[] {
                new SqlParameter("@UserId",logModel.UserId),
                new SqlParameter("@UserIdentity",logModel.UserIdentity),
                new SqlParameter("@BelongOne",logModel.BelongOne),
                new SqlParameter("@ShopId",logModel.ShopId),
                new SqlParameter("@AppSystem",logModel.AppSystem)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

    }
}
