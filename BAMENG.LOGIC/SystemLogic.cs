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
    public class SystemLogic
    {

        private static string GetCacheKey(int type)
        {
            return "SYSTEM_MENU_" + type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0总后台菜单，1总门店菜单，2分店菜单</param>
        /// <returns></returns>
        public static List<SystemMenuModel> GetMenuList(int type)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                //读取缓存数据
                List<SystemMenuModel> menuData = WebCacheHelper<List<SystemMenuModel>>.Get(GetCacheKey(type));
                if (menuData == null)
                {
                    menuData = dal.GetMenuList(type);
                    //将数据插入缓存中
                    if (menuData != null)
                        WebCacheHelper.Insert(GetCacheKey(type), menuData, "bameng/cacheMenuKey_" + type);

                }

                return menuData;
            }
        }


        /// <summary>
        /// 添加我的位置
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="myLocation">My location.</param>
        /// <param name="lnglat">The lnglat.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddMyLocation(int userId, string myLocation, string lnglat)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.AddMyLocation(userId, myLocation, lnglat);
            }
        }




        /// <summary>
        /// 获取后台首页数据
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>List&lt;AdminHomeDataModel&gt;.</returns>
        public static List<AdminHomeDataModel> GetHomeData(AdminLoginModel user)
        {
            List<AdminHomeDataModel> result = new List<AdminHomeDataModel>();
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                //获取今日数据
                string todayKey = "HOMEDATA" + DateTime.Now.ToString("yyyyMMddHH") + "_" + user.UserIndentity.ToString() + user.ID.ToString();
                AdminHomeDataModel todayData = WebCacheHelper<AdminHomeDataModel>.Get(todayKey);
                if (todayData == null)
                {
                    todayData = new AdminHomeDataModel();
                    todayData.NewAllyCount = dal.GetNewAllyCount(user.ID, user.UserIndentity);
                    todayData.NewArticleCount = dal.GetNewArticleCount(user.ID, user.UserIndentity);
                    todayData.NewCustomerCount = dal.GetNewCustomerCount(user.ID, user.UserIndentity);
                    todayData.NewMessageCount = dal.GetNewMessageCount(user.ID, user.UserIndentity);
                    WebCacheHelper.Insert(todayKey, todayData, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(todayKey)));
                }
                result.Add(todayData);


                //读取缓存数据
                string yesterdayKey = "HOMEDATA" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "_" + user.UserIndentity.ToString() + user.ID.ToString();
                AdminHomeDataModel yesterdayData = WebCacheHelper<AdminHomeDataModel>.Get(yesterdayKey);
                if (yesterdayData == null)
                {
                    yesterdayData = new AdminHomeDataModel();
                    //获取昨日数据
                    yesterdayData.NewAllyCount = dal.GetNewAllyCount(user.ID, user.UserIndentity, false);
                    yesterdayData.NewCustomerCount = dal.GetNewCustomerCount(user.ID, user.UserIndentity, false);
                    WebCacheHelper.Insert(yesterdayKey, yesterdayData, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(yesterdayKey)));
                }
                result.Add(yesterdayData);


                return result;
            }
        }


        /// <summary>
        /// 修改工作汇报内容
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <param name="title">The title.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateWorkReport(int ID, string title)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.UpdateWorkReport(ID, title);
            }
        }
        /// <summary>
        ///添加工作汇报内容
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>System.Int32.</returns>
        public static int AddWorkReport(string title)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.AddWorkReport(title);
            }
        }
        /// <summary>
        /// 获取工作汇报模板列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetWorkReportList(SearchModel model)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.GetWorkReportList(model);
            }
        }


        /// <summary>
        /// 删除工作汇报
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool DeleteWorkReport(int ID)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.DeleteWorkReport(ID);
            }
        }
    }
}
