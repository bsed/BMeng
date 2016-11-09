using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : BaseLogicFactory, IHttpHandler
    {

        public new void ProcessRequest(HttpContext context)
        {
            string loginName = GetFormValue("loginName", "");
            string loginPassword = EncryptHelper.MD5(GetFormValue("password", ""));
            int loginType = GetFormValue("loginType", 0);
            string json = string.Empty;
            AdminLoginModel data = UserLogic.Login(loginName, loginPassword, loginType == 1);
            if (data != null)
            {
                //判断账户是否已启用
                if (data.UserStatus == 1)
                {
                    WriteCookies(data);
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
                }
                else
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.账户已禁用));
            }
            else
            {
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.账户密码不正确));
            }

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




    }
}