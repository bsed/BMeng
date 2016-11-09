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
using System.Data.SqlClient;
using HotCoreUtils.DB;
using BAMENG.CONFIG;
using System.Data;

namespace BAMENG.DAL
{
    public class ManagerDAL : AbstractDAL, IManagerDAL
    {

        private const string SELECT_SQL = "select ID,LoginName,LoginPassword,RoleId,UserName,UserMobile,UserStatus,UserEmail,LastLoginTime,CreateTime,0 as UserIndentity from BM_Manager  where 1=1 ";

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Delete(int userId)
        {
            string strSql = "delete from BM_Manager where ID=@ID ";
            SqlParameter[] param = {
                new SqlParameter("@ID",userId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultPageModel GetManagerList(SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = SELECT_SQL;


            if (!string.IsNullOrEmpty(model.key))
            {
                strSql += string.Format(" and LoginName like '%{0}%' ", model.key);
            }

            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)<=@endTime ";
            var param = new[] {
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime)
            };
            return getPageData<AdminLoginModel>(model.PageSize, model.PageIndex, strSql, "CreateTime", false, param);
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>AdminLoginModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public AdminLoginModel GetModel(int userid)
        {
            string strSql = SELECT_SQL + " and ID=@ID";
            SqlParameter[] param = {
                new SqlParameter("@ID",userid)
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntity<AdminLoginModel>(dr);
            }
        }

        /// <summary>
        /// Inserts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Insert(AdminLoginModel model)
        {
            string strSql = "insert into BM_Manager(LoginName,LoginPassword,RoleId,UserName,UserMobile,UserEmail,UserStatus,LastLoginTime) values(@LoginName,@LoginPassword,@RoleId,@UserName,@UserMobile,@UserEmail,@UserStatus,@LastLoginTime)";
            SqlParameter[] param = {
                new SqlParameter("@LoginName",model.LoginName),
                new SqlParameter("@LoginPassword",model.LoginPassword),
                new SqlParameter("@RoleId",model.RoleId),
                new SqlParameter("@UserName",model.UserName),
                new SqlParameter("@UserMobile",model.UserMobile),
                new SqlParameter("@UserEmail",model.UserEmail),
                new SqlParameter("@UserStatus",model.UserStatus),
                new SqlParameter("@LastLoginTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param);

        }

        /// <summary>
        /// 设置用户激活状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool SetUserStatus(int userId)
        {
            string strSql = "update BM_Manager set UserStatus=ABS(UserStatus-1) where ID=@ID";
            SqlParameter[] param = {
                new SqlParameter("@ID",userId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }
        /// <summary>
        /// 更新最后登录时间
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateLastLoginTime(int userId)
        {
            string strSql = "update BM_Manager set LastLoginTime=getdate() where ID=@ID";
            SqlParameter[] param = {
                new SqlParameter("@ID",userId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }


        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Update(AdminLoginModel model)
        {
            string strSql = "update BM_Manager set LoginName=@LoginName,RoleId=@RoleId,UserName=@UserName,UserMobile=@UserMobile,UserEmail=@UserEmail";

            if (!string.IsNullOrEmpty(model.LoginPassword))
                strSql += ",LoginPassword=@LoginPassword ";


            strSql += " where ID=@ID ";

            SqlParameter[] param = {
                new SqlParameter("@ID",model.ID),
                new SqlParameter("@LoginName",model.LoginName),
                new SqlParameter("@LoginPassword",model.LoginPassword),
                new SqlParameter("@RoleId",model.RoleId),
                new SqlParameter("@UserName",model.UserName),
                new SqlParameter("@UserMobile",model.UserMobile),
                new SqlParameter("@UserEmail",model.UserEmail)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }
    }
}
