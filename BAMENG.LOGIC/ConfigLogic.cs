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
                    WebCacheHelper.DeleteDepFile(cacheKey);

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
                WebCacheHelper.DeleteDepFile(cacheKey);
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


        /// <summary>
        /// 获取签到配置
        /// </summary>
        /// <returns>SignInConfig.</returns>
        public static SignInConfig GetSignInConfig()
        {
            SignInConfig model = new SignInConfig();

            string v = GetValue("EnableSign");
            if (string.IsNullOrEmpty(v))
                model.EnableSign = false;
            else
                model.EnableSign = Convert.ToInt32(v) == 1;


            string v1 = GetValue("EnableContinuousSign");
            if (string.IsNullOrEmpty(v1))
                model.EnableContinuousSign = false;
            else
                model.EnableContinuousSign = Convert.ToInt32(v1) == 1;



            string v2 = GetValue("ContinuousSignDay");
            if (string.IsNullOrEmpty(v2))
                model.ContinuousSignDay = 0;
            else
                model.ContinuousSignDay = Convert.ToInt32(v2);


            string v3 = GetValue("ContinuousSignRewardScore");
            if (string.IsNullOrEmpty(v3))
                model.ContinuousSignRewardScore = 0;
            else
                model.ContinuousSignRewardScore = Convert.ToInt32(v3);


            string v4 = GetValue("SignScore");
            if (string.IsNullOrEmpty(v4))
                model.SignScore = 0;
            else
                model.SignScore = Convert.ToInt32(v4);


            return model;

        }


        /// <summary>
        /// 获取积分奖励配置
        /// </summary>
        /// <returns></returns>
        public static ScoreConfigModel GetScoreConfig()
        {
            ScoreConfigModel result = new ScoreConfigModel();
            List<ConfigModel> lst = GetConfigList();
            if (lst == null)
                return result;

            foreach (var item in lst)
            {
                //
                if (item.Code.Equals("CreateOrderScore"))
                    result.CreateOrderScore = string.IsNullOrEmpty(item.Value) ? 0 : Convert.ToInt32(item.Value);

                if (item.Code.Equals("InviteScore"))
                    result.InviteScore = string.IsNullOrEmpty(item.Value) ? 0 : Convert.ToInt32(item.Value);

                if (item.Code.Equals("SubmitCustomerToAllyScore"))
                    result.SubmitCustomerToAllyScore = string.IsNullOrEmpty(item.Value) ? 0 : Convert.ToInt32(item.Value);

                if (item.Code.Equals("SubmitCustomerToMainScore1"))
                    result.SubmitCustomerToMainScore1 = string.IsNullOrEmpty(item.Value) ? 0 : Convert.ToInt32(item.Value);

                if (item.Code.Equals("SubmitCustomerToMainScore2"))
                    result.SubmitCustomerToMainScore2 = string.IsNullOrEmpty(item.Value) ? 0 : Convert.ToInt32(item.Value);
            }
            return result;
        }

    }
}
