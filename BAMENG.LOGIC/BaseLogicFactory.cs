/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.CONFIG;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
    public class BaseLogicFactory : PageBaseHelper
    {

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public static bool CheckLogin(ref ApiStatusCode appCode)
        {
            AdminLoginModel data = GetCurrentUser();
            if (data != null)
            {
                data = UserLogic.Login(data.LoginName, data.LoginPassword, data.UserIndentity != 0);
                if (data != null)
                {
                    if (data.UserStatus == 1)
                    {
                        WriteCookies(data);
                        appCode = ApiStatusCode.OK;
                        return true;
                    }
                    else
                        appCode = ApiStatusCode.账户已禁用;
                }
                else
                    appCode = ApiStatusCode.账户密码不正确;
            }
            else
                appCode = ApiStatusCode.没有登录;
            return false;
        }


        /// <summary>
        /// 反序列化cookie的值返回user对象
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public static AdminLoginModel GetCurrentUser()
        {
            try
            {
                int userid = Convert.ToInt32(CookieHelper.GetCookieVal("SHOPID"));
                int indentity = Convert.ToInt32(CookieHelper.GetCookieVal("SHOP_INDENTITY"));
                return SerializeHelper.BinaryDeserializeBase64StringToObject<AdminLoginModel>(CookieHelper.GetCookieVal(GetAdminUserCookieKey(userid, indentity)));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 前端 user对象序列化后写入cookie,保留两周
        /// </summary>
        /// <param name="user"></param>
        public static void WriteCookies(AdminLoginModel user)
        {
            CookieHelper.SetCookieValByCurrentDomain(GetAdminUserCookieKey(user.ID, user.UserIndentity), 20160, SerializeHelper.BinarySerializeObjectToBase64String(user));
            CookieHelper.SetCookieValByCurrentDomain("SHOPID", 20160, user.ID.ToString());
            CookieHelper.SetCookieValByCurrentDomain("SHOP_INDENTITY", 20160, user.UserIndentity.ToString());
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCookies()
        {
            try
            {
                int userid = Convert.ToInt32(CookieHelper.GetCookieVal("SHOPID"));
                int indentity = Convert.ToInt32(CookieHelper.GetCookieVal("SHOP_INDENTITY"));
                CookieHelper.DelCookieValByCurrentDomain(GetAdminUserCookieKey(userid, indentity));
                CookieHelper.DelCookieValByCurrentDomain("SHOPID");
                CookieHelper.DelCookieValByCurrentDomain("SHOP_INDENTITY");
            }
            catch (Exception)
            {

            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="indexntity"></param>
        /// <returns></returns>
        private static string GetAdminUserCookieKey(int userid, int indexntity)
        {
            return "USER_" + indexntity + "_" + userid;
        }
    }
}
