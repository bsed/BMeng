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
    /// Class ConfigLogic.
    /// </summary>
    public class ConfigLogic
    {
        private const string cacheKey = "appbaseconfig";

        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <returns>List&lt;ConfigModel&gt;.</returns>
        public static List<ConfigModel> GetConfigList()
        {
            List<ConfigModel> lst = WebCacheHelper<List<ConfigModel>>.Get(cacheKey);
            if (lst == null)
            {
                using (var dal = FactoryDispatcher.ConfigFactory())
                {
                    lst = dal.List();
                    WebCacheHelper.Insert(cacheKey, lst, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                }
            }
            return lst;
        }


        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateValue(ConfigModel model)
        {
            using (var dal = FactoryDispatcher.ConfigFactory())
            {
                bool flag = dal.UpdateValue(model);

                if (flag)
                    WebCacheHelper.RefreshCache(cacheKey);

                return flag;
            }
        }

        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="lst">The LST.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateValue(List<ConfigModel> lst)
        {
            using (var dal = FactoryDispatcher.ConfigFactory())
            {
                foreach (var item in lst)
                {
                    dal.UpdateValue(new ConfigModel()
                    {
                        Code = item.Code,
                        Value = item.Value,
                        Remark = item.Remark
                    });
                }
                WebCacheHelper.RefreshCache(cacheKey);
                return true;
            }
        }


        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        public static string GetValue(string code)
        {
            List<ConfigModel> lst = GetConfigList();
            if (lst == null)
                return "";
            ConfigModel configModel = lst.Find((item) =>
             {
                 return item.Code == code;
             });

            if (configModel != null)
                return configModel.Value;
            else
                return "";
        }
    }
}
