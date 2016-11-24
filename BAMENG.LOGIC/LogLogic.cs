/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.MODEL;
using HotCoreUtils.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
    /// <summary>
    /// Class LogLogic.
    /// </summary>
    public class LogLogic
    {
        /// <summary>
        /// 添加资讯阅读日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool AddReadLog(ReadLogModel logModel)
        {

            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.AddReadLog(logModel);
            }
        }


        /// <summary>
        /// 更新用户资讯阅读状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateReadStatus(int userId, int articleId)
        {

            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.UpdateReadStatus(userId, articleId);
            }
        }

        /// <summary>
        /// 判断当前操作用户是否阅读
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="type">资讯类型0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns>true if the specified article identifier is read; otherwise, false.</returns>
        public static bool IsRead(int articleId, int type, int userId, string cookie, string clientIp)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                if (userId > 0)
                {
                    if (type == 3 || type == 4)
                        return dal.IsReadbyIdentity(userId, articleId);
                    else
                        return dal.IsRead(userId, articleId);
                }

                if (dal.IsRead(clientIp, articleId))
                    return true;

                if (dal.IsRead(articleId, cookie))
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddLoginLog(LoginLogModel logModel)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                if (logModel.UserId > 0)
                {
                    int shopId = ShopLogic.GetBelongShopId(logModel.ShopId);
                    if (shopId > 0)
                        logModel.BelongShopId = shopId;

                    return dal.AddLoginLog(logModel);
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 添加客户操作日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddCustomerLog(LogBaseModel logModel)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                if (logModel.UserId > 0)
                {
                    int shopId = ShopLogic.GetBelongShopId(logModel.ShopId);
                    if (shopId > 0)
                        logModel.BelongShopId = shopId;

                    return dal.AddCustomerLog(logModel);
                }
                else
                    return false;
            }
        }


        /// <summary>
        /// 登录统计
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        public static StatisticsModel LoginStatistics(AdminLoginModel user, int type, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                StatisticsModel data = new StatisticsModel();
                data.yData = new List<int>();
                data.xData = new List<string>();
                List<StatisticsListModel> lst = null;
                string cacheKey = string.Format("BMLOGIN{0}{1}{2}", user.UserIndentity, type, DateTime.Now.ToString("yyyyMMdd"));
                if (type != 0)
                {
                    lst = WebCacheHelper<List<StatisticsListModel>>.Get(cacheKey);
                    if (lst == null)
                    {
                        lst = dal.LoginStatistics(user.ID, user.UserIndentity, startTime, endTime);
                        WebCacheHelper.Insert(cacheKey, lst, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                    }
                }
                else
                    lst = dal.LoginStatistics(user.ID, user.UserIndentity, startTime, endTime);
                if (lst != null && lst.Count() > 0)
                {
                    int len = lst.Count();
                    if (len < 5)
                    {
                        string t = lst[0].xData;
                        data.xData.Add(Convert.ToDateTime(t).AddDays(-1).ToString("yyyy-MM-dd"));
                        data.yData.Add(0);
                    }
                    foreach (var item in lst)
                    {
                        data.xData.Add(item.xData);
                        data.yData.Add(item.yData);
                        data.total += item.yData;
                    }
                    //string t2 = lst[len - 1].xData;
                    //data.xData.Add(Convert.ToDateTime(t2).AddDays(1).ToString("yyyy-MM-dd"));
                    //data.yData.Add(0);

                }
                return data;
            }
        }


        /// <summary>
        ///获取客户统计
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        public StatisticsModel CustomerStatistics(AdminLoginModel user, int type, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                StatisticsModel data = new StatisticsModel();
                data.yData = new List<int>();
                data.xData = new List<string>();
                List<StatisticsListModel> lst = null;
                string cacheKey = string.Format("BMCT{0}{1}{2}", user.UserIndentity, type, DateTime.Now.ToString("yyyyMMdd"));
                if (type != 0)
                {
                    lst = WebCacheHelper<List<StatisticsListModel>>.Get(cacheKey);
                    if (lst == null)
                    {
                        lst = dal.LoginStatistics(user.ID, user.UserIndentity, startTime, endTime);
                        WebCacheHelper.Insert(cacheKey, lst, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                    }
                }
                else
                    lst = dal.LoginStatistics(user.ID, user.UserIndentity, startTime, endTime);
                if (lst != null && lst.Count() > 0)
                {
                    int len = lst.Count();
                    if (len < 5)
                    {
                        string t = lst[0].xData;
                        data.xData.Add(Convert.ToDateTime(t).AddDays(-1).ToString("yyyy-MM-dd"));
                        data.yData.Add(0);
                    }
                    foreach (var item in lst)
                    {
                        data.xData.Add(item.xData);
                        data.yData.Add(item.yData);
                        data.total += item.yData;
                    }

                }
                return data;
            }
        }
    }
}
