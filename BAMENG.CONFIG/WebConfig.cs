using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.CONFIG
{
    public class WebConfig
    {

        /// <summary>
        /// 数据库连接字符串（默认MssqlDBConnectionString）
        /// </summary>
        /// <returns>返回连接字符串</returns>
        public static string getConnectionString()
        {
            return ConfigHelper.MssqlDBConnectionString;
        }

        /// <summary>
        /// 判断判断是否debug模式
        /// </summary>
        /// <returns></returns>
        public static bool debugMode()
        {
            try
            {
                string debug = ConfigHelper.GetConfigString("debugMode", "false");
                if (!string.IsNullOrEmpty(debug))
                    return Convert.ToBoolean(debug);
                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 资源网站域名
        /// </summary>
        /// <returns>System.String.</returns>
        public static string reswebsite()
        {
            return ConfigHelper.GetConfigString("reswebsite", "");
        }

        /// <summary>
        /// 资讯详情域名
        /// </summary>
        /// <returns>System.String.</returns>
        public static string articleDetailsDomain()
        {
            return ConfigHelper.GetConfigString("articledetailsdomain", "");
        }



        #region 短信接口相关配置
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SMSURL()
        {
            return ConfigHelper.GetConfigString("SMSURL", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SMSAcount()
        {
            return ConfigHelper.GetConfigString("SMSAcount", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SMSAcountPassword()
        {
            return ConfigHelper.GetConfigString("SMSAcountPassword", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SMSVoiceAcount()
        {
            return ConfigHelper.GetConfigString("SMSVoiceAcount", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SMSVoiceAcountPassword()
        {
            return ConfigHelper.GetConfigString("SMSVoiceAcountPassword", "");
        }
        #endregion
    }
}
