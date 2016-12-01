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
    public class SystemDAL : AbstractDAL, ISystemDAL
    {
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="type">0总后台菜单，1总门店菜单，2分店菜单</param>
        /// <returns></returns>
        public List<SystemMenuModel> GetMenuList(int type)
        {
            string strSql = "select ID,ItemType,ItemCode,ItemNavLabel,ItemSort,ItemParentCode,ItemUrl,ItemShow,ItemIcons,CreateTime from BM_MenuList  where ItemType=@ItemType   order by ItemCode,ItemSort";
            var param = new[] {
                new SqlParameter("@ItemType",type)
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntityList<SystemMenuModel>(dr);
            }
        }

        /// <summary>
        /// 添加我的位置
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="myLocation">My location.</param>
        /// <param name="lnglat">The lnglat.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool AddMyLocation(int userId, string myLocation, string lnglat)
        {
            string strSql = "insert into BM_MyLocation(UserId,MyLocation,lnglat) values(@UserId,@MyLocation,@lnglat)";
            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@MyLocation",myLocation),
                new SqlParameter("@lnglat",lnglat)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }



        /// <summary>
        /// 获取新增盟友数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        public int GetNewAllyCount(int shopId, int userIdentity, bool today = true)
        {
            string strSql = "select count(1) from BM_User_extend where  CONVERT(nvarchar(10),CreateTime,121)=@Date and UserIdentity=0 ";

            if (shopId > 0)
            {
                if (userIdentity == 1)
                    strSql += " and BelongShopId=@ShopId";
                else if (userIdentity == 2)
                    strSql += " and ShopId=@ShopId";
            }

            DateTime dtnow = DateTime.Now;
            if (!today)
                dtnow = DateTime.Now.AddDays(-1);

            var param = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Date",dtnow.ToString("yyyy-MM-dd"))
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));

        }
        /// <summary>
        /// 获取新增有效客户数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        public int GetNewCustomerCount(int shopId, int userIdentity, bool today = true)
        {
            string strSql = "select count(1) from BM_CustomerLog where  CONVERT(nvarchar(10),CreateTime,121)=@Date and OperationType=1";
            if (shopId > 0)
            {
                if (userIdentity == 1)
                    strSql += " and BelongShopId=@ShopId";
                else if (userIdentity == 2)
                    strSql += " and ShopId=@ShopId";
            }

            DateTime dtnow = DateTime.Now;
            if (!today)
                dtnow = DateTime.Now.AddDays(-1);

            var param = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Date",dtnow.ToString("yyyy-MM-dd"))
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));

        }


        /// <summary>
        /// 获取新增审核资讯数(此方法只有总后台调用)
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        public int GetNewArticleCount(bool today = true)
        {
            string strSql = "select COUNT(1) from BM_ArticleList where  CONVERT(nvarchar(10),CreateTime,121)=@Date and AuthorIdentity in (1,2) and IsDel=0 ";

            DateTime dtnow = DateTime.Now;
            if (!today)
                dtnow = DateTime.Now.AddDays(-1);

            var param = new[] {
                new SqlParameter("@Date",dtnow.ToString("yyyy-MM-dd"))
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }

        /// <summary>
        /// 获取新增信息数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        public int GetNewMessageCount(int shopId, int userIdentity, bool today = true)
        {
            string strSql = "select COUNT(1) from BM_MessageManage M where CONVERT(nvarchar(10),M.CreateTime,121)=@Date and M.IsDel = 0  and IsSend=1 ";
            if (userIdentity != 0)
            {
                strSql = @"select COUNT(1) from BM_MessageManage M 
                            inner join BM_MessageSendTarget T on T.MessageId = M.ID
                            where CONVERT(nvarchar(10),M.CreateTime,121)=@Date and M.IsDel = 0  and IsSend=1 ";
                strSql += " and T.SendTargetShopId=@ShopId";
            }
            else
                strSql += " and M.IsSendBelongShopId=1 ";

            DateTime dtnow = DateTime.Now;
            if (!today)
                dtnow = DateTime.Now.AddDays(-1);

            var param = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Date",dtnow.ToString("yyyy-MM-dd"))
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));

        }


    }
}
