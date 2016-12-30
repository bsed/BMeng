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
        /// 添加站内信阅读日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddMailReadLog(ReadLogModel logModel)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.AddMailReadLog(logModel);
            }
        }

        /// <summary>
        /// 根据用户ID判断当前信息是否已阅读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mailId">The mail identifier.</param>
        /// <returns>true if [is mail read] [the specified user identifier]; otherwise, false.</returns>
        public static bool IsMailRead(int userId, int mailId)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.IsMailRead(userId, mailId);
            }
        }

        /// <summary>
        /// 更新用户站内信阅读状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mailId">The mail identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateMailReadStatus(int userId, int mailId)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.UpdateMailReadStatus(userId, mailId);
            }
        }
        /// <summary>
        /// 根据条件，修改阅读状态为未读
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mailId">The mail identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateMailNotReadStatus(int userId, int mailId)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.UpdateMailNotReadStatus(userId, mailId);
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


        public static bool IsRead(int userId, int articleId)
        {

            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.IsRead(userId, articleId);
            }
        }


        public static bool IsRead(string clientIp, int articleId)
        {

            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.IsRead(clientIp, articleId);
            }
        }
        public static bool IsRead(int articleId, string cookie)
        {

            using (var dal = FactoryDispatcher.LogFactory())
            {
                return dal.IsRead(articleId, cookie);
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
                    else
                        logModel.BelongShopId = logModel.ShopId;

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
                    else
                        logModel.BelongShopId = logModel.ShopId;

                    return dal.AddCustomerLog(logModel);
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 添加优惠券操作日志
        /// </summary>
        /// <param name="logModel">The log model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddCouponLog(LogBaseModel logModel)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                if (logModel.UserId > 0)
                {
                    int shopId = ShopLogic.GetBelongShopId(logModel.ShopId);
                    if (shopId > 0)
                        logModel.BelongShopId = shopId;
                    else
                        logModel.BelongShopId = logModel.ShopId;

                    return dal.AddCouponLog(logModel);
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

                if (data.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data.xData.Add(dtime);
                    data.yData.Add(0);
                }

                return data;
            }
        }

        /// <summary>
        /// 获取签到统计
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="type">The type.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        public static StatisticsModel UserSignStatistics(AdminLoginModel user, int type, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                StatisticsModel data = new StatisticsModel();
                List<StatisticsListModel> lst = null;
                string cacheKey = string.Format("BMSIGN{0}{1}{2}", user.UserIndentity, type, DateTime.Now.ToString("yyyyMMdd"));
                if (type != 0)
                {
                    lst = WebCacheHelper<List<StatisticsListModel>>.Get(cacheKey);
                    if (lst == null)
                    {
                        lst = dal.UserSignStatistics(user.ID, user.UserIndentity, startTime, endTime);
                        WebCacheHelper.Insert(cacheKey, lst, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                    }
                }
                else
                    lst = dal.UserSignStatistics(user.ID, user.UserIndentity, startTime, endTime);
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

                if (data.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data.xData.Add(dtime);
                    data.yData.Add(0);
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
        public static List<StatisticsModel> CustomerStatistics(AdminLoginModel user, int type, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsModel> result = new List<StatisticsModel>();
                StatisticsModel data1 = new StatisticsModel();
                StatisticsModel data2 = new StatisticsModel();
                StatisticsModel data3 = new StatisticsModel();
                List<StatisticsListModel> lst = null;
                string cacheKey = string.Format("BMCT{0}{1}{2}", user.UserIndentity, type, DateTime.Now.ToString("yyyyMMdd"));
                if (type != 0)
                {
                    lst = WebCacheHelper<List<StatisticsListModel>>.Get(cacheKey);
                    if (lst == null)
                    {
                        lst = dal.CustomerStatistics(user.ID, user.UserIndentity, startTime, endTime);
                        WebCacheHelper.Insert(cacheKey, lst, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                    }
                }
                else
                    lst = dal.CustomerStatistics(user.ID, user.UserIndentity, startTime, endTime);
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {
                        if (!data1.xData.Contains(item.xData))
                        {
                            if (item.Code == 0)
                            {
                                data1.xData.Add(item.xData);
                                data1.yData.Add(item.yData);
                                data1.total += item.yData;
                            }
                            else
                            {
                                data1.xData.Add(item.xData);
                                data1.yData.Add(0);
                                data1.total += 0;
                            }
                        }
                        if (!data2.xData.Contains(item.xData))
                        {
                            if (item.Code == 1)
                            {

                                data2.xData.Add(item.xData);
                                data2.yData.Add(item.yData);
                                data2.total += item.yData;
                            }
                            else
                            {
                                data2.xData.Add(item.xData);
                                data2.yData.Add(0);
                                data2.total += 0;
                            }
                        }
                        if (!data3.xData.Contains(item.xData))
                        {
                            if (item.Code == 2)
                            {

                                data3.xData.Add(item.xData);
                                data3.yData.Add(item.yData);
                                data3.total += item.yData;
                            }
                            else
                            {
                                data3.xData.Add(item.xData);
                                data3.yData.Add(0);
                                data3.total += 0;
                            }
                        }

                    }

                    if (data1.xData.Count() == 0)
                    {
                        string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                        data1.xData.Add(dtime);
                        data1.yData.Add(0);
                        data2.xData.Add(dtime);
                        data2.yData.Add(0);
                        data3.xData.Add(dtime);
                        data3.yData.Add(0);
                    }


                    result.Add(data1);
                    result.Add(data2);
                    result.Add(data3);
                }
                return result;
            }

        }

        public static List<StatisticsMoneyModel> CouponStatistics(AdminLoginModel user, int type, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsMoneyModel> result = new List<StatisticsMoneyModel>();
                StatisticsMoneyModel data1 = new StatisticsMoneyModel();
                StatisticsMoneyModel data2 = new StatisticsMoneyModel();
                StatisticsMoneyModel data3 = new StatisticsMoneyModel();
                List<StatisticsMoneyListModel> lst = dal.CouponStatistics(user.ID, user.UserIndentity, startTime, endTime);
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {

                        if (!data1.xData.Contains(item.xData))
                        {
                            if (item.Code == 0)
                            {
                                data1.xData.Add(item.xData);
                                data1.yData.Add(item.yData);
                                data1.total += item.yData;
                            }
                            else
                            {
                                data1.xData.Add(item.xData);
                                data1.yData.Add(0);
                                data1.total += 0;
                            }
                        }

                        if (!data2.xData.Contains(item.xData))
                        {
                            if (item.Code == 1)
                            {
                                data2.xData.Add(item.xData);
                                data2.yData.Add(item.yData);
                                data2.total += item.yData;
                            }
                            else
                            {
                                data2.xData.Add(item.xData);
                                data2.yData.Add(0);
                                data2.total += 0;
                            }
                        }
                        if (!data3.xData.Contains(item.xData))
                        {
                            if (item.Code == 2)
                            {
                                data3.xData.Add(item.xData);
                                data3.yData.Add(item.yData);
                                data3.total += item.yData;
                            }
                            else
                            {
                                data3.xData.Add(item.xData);
                                data3.yData.Add(0);
                                data3.total += 0;
                            }
                        }
                    }

                }

                if (data1.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data1.xData.Add(dtime);
                    data1.yData.Add(0);
                    data2.xData.Add(dtime);
                    data2.yData.Add(0);
                    data3.xData.Add(dtime);
                    data3.yData.Add(0);
                }


                result.Add(data1);
                result.Add(data2);
                result.Add(data3);

                return result;
            }
        }

        public static StatisticsMoneyPieModel CouponStatisticsPie(AdminLoginModel user, string startTime, string endTime)
        {
            StatisticsMoneyPieModel data1 = new StatisticsMoneyPieModel();


            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsMoneyListModel> lst = null;
                if (user.UserIndentity == 0)
                    lst = dal.CouponStatisticsPieByAdmin(startTime, endTime);
                else if (user.UserIndentity == 1)
                    lst = dal.CouponStatisticsPieByBelongShop(user.ID, startTime, endTime);
                else if (user.UserIndentity == 2)
                    lst = dal.CouponStatisticsPieByShop(user.ID, startTime, endTime);

                if (lst != null && lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {
                        data1.xData.Add(item.xData);
                        PieModel pie = new PieModel();
                        pie.name = item.xData;
                        pie.value = item.yData;
                        data1.yData.Add(pie);
                        data1.total += item.yData;
                    }
                }


                if (data1.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data1.xData.Add(dtime);
                    PieModel pie = new PieModel();
                    pie.name = "";
                    pie.value = 0;
                    data1.yData.Add(pie);
                }

                return data1;
            }
        }

        public static List<StatisticsModel> OrderStatistic(AdminLoginModel user, string startTime, string endTime)
        {
            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsModel> result = new List<StatisticsModel>();

                List<StatisticsListModel> listTotal = dal.OrderStatistics(user.ID, user.UserIndentity, startTime, endTime);
                //List<StatisticsListModel> listFinish = dal.OrderFinishStatistics(user.ID, user.UserIndentity, startTime, endTime);

                StatisticsModel data1 = new StatisticsModel();
                StatisticsModel data2 = new StatisticsModel();

                foreach (var item in listTotal)
                {
                    if (!data1.xData.Contains(item.xData))
                    {
                        if (item.Code == 0)
                        {
                            data1.xData.Add(item.xData);
                            data1.yData.Add(item.yData);
                            data1.total += item.yData;
                        }
                        else
                        {
                            data1.xData.Add(item.xData);
                            data1.yData.Add(0);
                            data1.total += 0;
                        }
                    }
                    if (item.Code == 1)
                    {
                        if (!data2.xData.Contains(item.xData))
                        {
                            data2.xData.Add(item.xData);
                            data2.yData.Add(item.yData);
                            data2.total += item.yData;
                        }
                        else
                        {
                            data2.xData.Add(item.xData);
                            data2.yData.Add(0);
                            data2.total += 0;
                        }
                    }

                    //data1.xData.Add(item.xData);
                    //data1.yData.Add(item.yData);
                    //data1.total += item.yData;

                    //data2.xData.Add(item.xData);
                    //int finishAmount = getFininshAmount(item.xData, listFinish);
                    //data2.yData.Add(finishAmount);
                    //data2.total += finishAmount;
                }

                if (data1.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data1.xData.Add(dtime);
                    data1.yData.Add(0);
                    data2.xData.Add(dtime);
                    data2.yData.Add(0);
                }


                result.Add(data1);
                result.Add(data2);
                return result;
            }
        }

        private static int getFininshAmount(string xData, List<StatisticsListModel> listFinish)
        {
            foreach (StatisticsListModel model in listFinish)
            {
                if (model.xData == xData)
                {
                    return model.yData;
                }
            }
            return 0;
        }


        public static StatisticsPieModel OrderStatisticsPie(AdminLoginModel user, string startTime, string endTime)
        {
            StatisticsPieModel data1 = new StatisticsPieModel();


            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsListModel> lst = null;
                if (user.UserIndentity == 0)
                    lst = dal.OrderStatisticsPieByAdmin(startTime, endTime);
                else if (user.UserIndentity == 1)
                    lst = dal.OrderStatisticsPieByBelongShop(user.ID, startTime, endTime);
                else if (user.UserIndentity == 2)
                    lst = dal.OrderStatisticsPieByShop(user.ID, startTime, endTime);

                if (lst != null && lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {
                        data1.xData.Add(item.xData);
                        PieCountModel pie = new PieCountModel();
                        pie.name = item.xData;
                        pie.value = item.yData;
                        data1.yData.Add(pie);
                        data1.total += item.yData;
                    }
                }


                if (data1.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data1.xData.Add(dtime);
                    PieCountModel pie = new PieCountModel();
                    pie.name = "";
                    pie.value = 0;
                    data1.yData.Add(pie);
                }

                return data1;
            }
        }




        /// <summary>
        ///客户饼状图
        /// </summary>
        /// <param name="shopid">The shopid.</param>
        /// <param name="useridentity">The useridentity.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        public static StatisticsPieModel CustomerStatisticsPie(AdminLoginModel user, string startTime, string endTime)
        {
            StatisticsPieModel data1 = new StatisticsPieModel();


            using (var dal = FactoryDispatcher.LogFactory())
            {
                List<StatisticsListModel> lst = dal.CustomerStatisticsPie(user.ID, user.UserIndentity, startTime, endTime);

                if (lst != null && lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {
                        data1.xData.Add(item.xData);
                        PieCountModel pie = new PieCountModel();
                        pie.name = item.xData;
                        pie.value = item.yData;
                        data1.yData.Add(pie);
                        data1.total += item.yData;
                    }
                }


                if (data1.xData.Count() == 0)
                {
                    string dtime = DateTime.Now.ToString("yyyy-MM-dd");
                    data1.xData.Add(dtime);
                    PieCountModel pie = new PieCountModel();
                    pie.name = "";
                    pie.value = 0;
                    data1.yData.Add(pie);
                }

                return data1;
            }
        }









        /// <summary>
        /// 登录流水
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetUserLoginList(int shopId, int userIdentity, SearchModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetUserLoginList(shopId, userIdentity, model);
            }
        }

        /// <summary>
        /// 签到流水.
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetSignLoginList(int shopId, int userIdentity, SearchModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetSignLoginList(shopId, userIdentity, model);
            }
        }

    }
}

