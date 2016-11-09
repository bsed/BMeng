/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-$today.year. All rights reserved.
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
    public class AppServiceLogic
    {

        private static AppServiceLogic _instance = new AppServiceLogic();

        public static AppServiceLogic Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppServiceLogic();
                return _instance;
            }
        }
        /// <summary>
        /// 初始化接口
        /// </summary>
        /// <param name="clientVersion"></param>
        /// <returns></returns>
        public AppInitModel Initialize(string clientVersion, string OS)
        {
            AppInitModel data = new AppInitModel();

            if (OS.ToLower() == "android")
                data.versionData = CheckUpdate(clientVersion, "android");

            data.baseData.aboutUrl = "http://wwww.xx.com/about.html";
            data.baseData.agreementUrl = "http://wwww.xx.com/about.html";
            data.baseData.userStatus = 1;

            return data;
        }



        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <param name="os">操作系统</param>
        /// <returns></returns>
        public AppVersionModel CheckUpdate(string currentVersion, string os)
        {
            AppVersionModel verData = new AppVersionModel();
            if (os == OSOptions.android.ToString())
            {
                bool flag = GlobalProvider.IsVersionUpdate("1.0.2", currentVersion);
                if (flag)
                {
                    verData.serverVersion = "1.0.2";
                    verData.updateType = 1;
                    verData.updateTip = "测试更新";
                    verData.updateUrl = "http://www.baidu.com";
                }
            }
            return verData;
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <param name="apiCode">The API code.</param>
        /// <returns>UserModel.</returns>
        public UserModel Login(string loginName, string password, ref ApiStatusCode apiCode)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                UserModel model = dal.Login(loginName, password);
                if (model != null)
                {
                    if (model.IsActive == 1)
                    {
                        apiCode = ApiStatusCode.OK;
                        string token = EncryptHelper.MD5(StringHelper.CreateCheckCode(20));
                        if (dal.IsAuthTokenExist(model.UserId) ? dal.UpdateUserAuthToken(model.UserId, token) : dal.AddUserAuthToken(model.UserId, token))
                            model.token = token;
                        return model;
                    }
                    else
                    {
                        apiCode = ApiStatusCode.账户已禁用;
                        return null;
                    }
                }
                else
                {
                    apiCode = ApiStatusCode.账户不存在;
                    return null;
                }
            }

        }

    }
}
