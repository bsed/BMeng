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
           return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param)>0;
        }
    }
}
