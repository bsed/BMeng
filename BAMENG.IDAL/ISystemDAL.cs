/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    public interface ISystemDAL : IDisposable
    {
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="type">0总后台菜单，1总门店菜单，2分店菜单</param>
        /// <returns></returns>
        List<SystemMenuModel> GetMenuList(int type);

        /// <summary>
        /// 添加我的位置
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="myLocation">My location.</param>
        /// <param name="lnglat">The lnglat.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool AddMyLocation(int userId, string myLocation, string lnglat);


        /// <summary>
        /// 获取新增盟友数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        int GetNewAllyCount(int shopId, int userIdentity, bool today = true);


        /// <summary>
        /// 获取新增有效客户数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        int GetNewCustomerCount(int shopId, int userIdentity, bool today = true);


        /// <summary>
        /// 获取新增信息数
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        int GetNewMessageCount(int shopId, int userIdentity, bool today = true);

        /// <summary>
        /// 获取新增审核资讯数(此方法只有总后台调用)
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">0总后台 1总店，2分店</param>
        /// <param name="today">默认今天，否则昨天</param>
        /// <returns>System.Int32.</returns>
        int GetNewArticleCount(bool today = true);
    }
}
