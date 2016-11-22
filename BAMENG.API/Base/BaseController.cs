using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BAMENG.API.Base;
using System.Reflection;
using BAMENG.MODEL;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.Routing;
using BAMENG.CONFIG;
using BAMENG.LOGIC;

namespace BAMENG.API
{
    /// <summary>
    /// Class BaseController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class BaseController : Controller
    {
        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式的 <see cref="T:System.Web.Mvc.JsonResult" /> 对象。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 JSON 结果对象。</returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new CustomJsonResult { Data = data, ContentType = contentType, MaxJsonLength = int.MaxValue, ContentEncoding = contentEncoding };
        }

        /// <summary>
        /// Jsons the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>JsonResult.</returns>
        public new JsonResult Json(object data, string contentType, JsonRequestBehavior jsonRequest)
        {
            return new CustomJsonResult { Data = data, ContentType = contentType, MaxJsonLength = int.MaxValue, JsonRequestBehavior = jsonRequest };
        }


        /// <summary>
        /// Jsons the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>JsonResult.</returns>
        public new JsonResult Json(object data, JsonRequestBehavior jsonRequest)
        {
            return new CustomJsonResult { Data = data, ContentType = "application/json", MaxJsonLength = int.MaxValue, JsonRequestBehavior = jsonRequest };
        }


        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="T:System.Web.Mvc.JsonResult" /> 对象。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 JSON 结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public new JsonResult Json(object data)
        {
            return new CustomJsonResult { Data = data, ContentType = "application/json", MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private Dictionary<string, object> Params { get; set; }

        private Dictionary<string, string> Header { get; set; }
        /// <summary>
        /// 获取 get/post 数据
        /// </summary>        
        /// <returns></returns>
        private Dictionary<string, object> GetParams(HttpRequestBase Request)
        {
            foreach (var key in Request.Headers.AllKeys)
            {
                Header.Add(key, Request.Headers[key]);
            }
            Dictionary<string, object> p = new Dictionary<string, object>();
            if (Request.HttpMethod.ToLower() == "post") //post 数据请求
            {
                NameValueCollection values = Request.Form;
                foreach (string m in values.Keys)
                {
                    p.Add(m, HttpUtility.UrlDecode(values[m]));
                }
            }
            else  //get 数据请求
            {
                string url = Request.Url.ToString();
                if (url == null || url == "")
                    return p;
                int questionMarkIndex = url.IndexOf('?');
                if (questionMarkIndex == url.Length - 1)
                    return p;
                string ps = url.Substring(questionMarkIndex + 1);
                if (ps != null && !string.IsNullOrEmpty(ps))
                {
                    ps = HttpUtility.UrlDecode(ps);
                    // 开始分析参数对   
                    Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
                    MatchCollection mc = re.Matches(ps);
                    foreach (Match m in mc)
                    {
                        p.Add(m.Result("$2"), HttpUtility.UrlDecode(m.Result("$3")));
                    }
                }
            }
            return p;
        }


        /// <summary>
        /// 版本号
        /// </summary>
        /// <value>The application version.</value>
        public string Version { get; set; }


        /// <summary>
        /// 系统 android / iphone
        /// </summary>
        /// <value>The os.</value>
        public string OS { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <value>The timestamp.</value>
        public string timestamp { get; set; }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public string GetQuery(string key, string defaultValue)
        {
            if (Params == null)
                return defaultValue;
            else
            {
                if (Params.ContainsKey(key))
                    return Params[key].ToString();
                else
                    return defaultValue;
            }
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public int GetQuery(string key, int defaultValue)
        {
            if (Params == null)
                return defaultValue;
            else
            {
                if (Params.ContainsKey(key))
                    return Convert.ToInt32(Params[key].ToString());
                else
                    return defaultValue;
            }
        }
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Decimal.</returns>
        public decimal GetQuery(string key, decimal defaultValue)
        {
            if (Params == null)
                return defaultValue;
            else
            {
                if (Params.ContainsKey(key))
                    return Convert.ToDecimal(Params[key].ToString());
                else
                    return defaultValue;
            }
        }


        /// <summary>
        /// 在调用操作方法前调用。
        /// </summary>
        /// <param name="filterContext">有关当前请求和操作的信息。</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Header = new Dictionary<string, string>();
            Params = GetParams(filterContext.HttpContext.Request);
            OS = GetQuery("os", "");
            Version = GetQuery("version", "1.0.0");
            timestamp = GetQuery("timestamp", "");
        }


        /// <summary>
        /// 只有在授权成功后，才能获取到
        /// </summary>
        /// <returns>UserModel.</returns>
        public UserModel GetUserData()
        {
            try
            {
                if (Header.ContainsKey("Authorization"))
                {
                    string Authorization = Header["Authorization"];
                    if (!string.IsNullOrEmpty(Authorization))
                    {
                        int UserId = UserLogic.GetUserIdByAuthToken(Authorization);
                        UserModel user = UserLogic.GetModel(UserId);
                        user.token = Authorization;
                        user.TempMengBeans = UserLogic.countTempBeansMoney(user.UserId, 0);
                        return user;
                    }
                }
            }
            catch (Exception)
            {

            }
            return new UserModel();
        }

        /// <summary>
        /// 获取授权用户ID
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetAuthUserId()
        {
            try
            {
                if (Header.ContainsKey("Authorization"))
                {
                    string Authorization = Header["Authorization"];
                    if (!string.IsNullOrEmpty(Authorization))
                    {
                        return UserLogic.GetUserIdByAuthToken(Authorization);
                    }
                }
            }
            catch (Exception)
            {

            }
            return 0;
        }
    }
}