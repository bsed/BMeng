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

    public interface ILogDAL : IDisposable
    {
        /// <summary>
        /// 添加资讯阅读日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool AddReadLog(ReadLogModel logModel);



        /// <summary>
        /// 更新用户阅读状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateReadStatus(int userId, int articleId);


        /// <summary>
        /// 根据用户ID判断当前资讯是否已阅读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified user identifier is read; otherwise, false.</returns>
        bool IsRead(int userId, int articleId);

        /// <summary>
        /// 根据用户ID判断当前资讯是否已阅读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified user identifier is reads; otherwise, false.</returns>
        bool IsReadbyIdentity(int userId, int articleId);

        /// <summary>
        /// 根据客户的ip判断，当前资讯是已阅读
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if the specified client identifier is read; otherwise, false.</returns>
        bool IsRead(string clientId, int articleId);

        /// <summary>
        /// 根据cookie判断当前资讯是否已阅读
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="cookie">The cookie.</param>
        /// <returns>true if the specified article identifier is read; otherwise, false.</returns>
        bool IsRead(int articleId, string cookie);


        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool AddLoginLog(LoginLogModel logModel);


        /// <summary>
        /// 添加客户操作日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool AddCustomerLog(LogBaseModel logModel);

        /// <summary>
        /// 添加优惠券操作日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool AddCouponLog(LogBaseModel logModel);

        /// <summary>
        /// 获取登录统计
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        List<StatisticsListModel> LoginStatistics(int shopId, int userIdentity, string startTime, string endTime);




        /// <summary>
        ///获取客户统计
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        List<StatisticsListModel> CustomerStatistics(int shopId, int userIdentity, string startTime, string endTime);


        /// <summary>
        /// 获取优惠券统计
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="userIdentity"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        List<StatisticsMoneyListModel> CouponStatistics(int shopId, int userIdentity, string startTime, string endTime);


        /// <summary>
        /// 获得总店优惠券饼图统计
        /// </summary>
        /// <param name="belongShopId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        List<StatisticsMoneyListModel> CouponStatisticsPieByBelongShop(int belongShopId, string startTime, string endTime);

        /// <summary>
        /// 获取管理员优惠券统计图
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        List<StatisticsMoneyListModel> CouponStatisticsPieByAdmin(string startTime, string endTime);

        /// <summary>
        /// 获取分店优惠券饼图统计
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        List<StatisticsMoneyListModel> CouponStatisticsPieByShop(int shopId, string startTime, string endTime);
    }
}
