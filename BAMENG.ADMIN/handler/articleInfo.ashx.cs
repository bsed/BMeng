using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// articleInfo 的摘要说明
    /// </summary>
    public class articleInfo : BaseLogicFactory, IHttpHandler
    {

        public new void ProcessRequest(HttpContext context)
        {
            int articleId = GetFormValue("articleId", 0);
            string auth = GetFormValue("auth", "");
            string json = string.Empty;
            int userId = 0;
            if (!string.IsNullOrEmpty(auth))
            {
                userId = UserLogic.GetUserIdByAuthToken(auth);
                if(userId==0)
                {
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.用户信息丢失));
                    context.Response.ContentType = "application/json";
                    context.Response.Write(json);
                    context.Response.End();
                }
            }
            else
            {
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.用户信息丢失));
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                context.Response.End();
            }
            ArticleModel data = ArticleLogic.GetModel(articleId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));


            //相同的资讯是否被一个人浏览（cookie + Ip）
            string clientip = GetClientIP;
            string cookie = CookieHelper.GetCookieVal("HOTBMUSER");
            if (string.IsNullOrEmpty(cookie))
            {
                cookie = Guid.NewGuid().ToString("n");
                CookieHelper.SetCookieValByCurrentDomain("HOTBMUSER", 1, cookie);
            }
            if (cookie.Length != 32)
            {
                goto Finish;
            }

            ReadLogModel logModel = new ReadLogModel()
            {
                UserId = userId,
                cookie = cookie,
                ClientIp = clientip,
                ArticleId = articleId,
                IsRead = 1,
                ReadTime = DateTime.Now
            };
            int type = data.AuthorIdentity;
            //判断是否已经阅读
            if (!LogLogic.IsRead(articleId, type, userId, cookie, clientip))
            {
                //添加阅读日志
                if (type == 3 || type == 4)
                    LogLogic.UpdateReadStatus(userId, articleId);
                else
                    LogLogic.AddReadLog(logModel);

                ArticleLogic.UpdateArticleAmount(articleId);
            }

            goto Finish;

        Finish:
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string GetClientIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理  
                    if (result.IndexOf(".") == -1) //非IPv4格式  
                        result = string.Empty;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。  
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i]; //找到不是内网的地址  
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式  
                            return result;
                        else
                            result = string.Empty; //代理中的内容 非IP，取IP  
                    }

                }

                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;
                return result;
            }
        }
        private bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
    }
}