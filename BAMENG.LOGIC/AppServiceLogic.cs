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

            data.baseData.aboutUrl = WebConfig.articleDetailsDomain() + "/app/about.html";

            data.baseData.agreementUrl = WebConfig.articleDetailsDomain() + "/app/agreement.html";


            data.baseData.userStatus = 1;

            string v = ConfigLogic.GetValue("EnableSign");
            if (string.IsNullOrEmpty(v))
                data.baseData.enableSignIn = 0;
            else
                data.baseData.enableSignIn = Convert.ToInt32(v);




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

                var newVersion = ConfigLogic.GetValue("AppVersion");

                bool flag = GlobalProvider.IsVersionUpdate(newVersion, currentVersion);
                if (flag)
                {
                    verData.serverVersion = newVersion;
                    verData.updateType = Convert.ToInt32(ConfigLogic.GetValue("EnableAppCoerceUpdate")) == 1 ? 2 : 1;
                    verData.updateTip = ConfigLogic.GetValue("AppUpateContent");
                    verData.updateUrl = ConfigLogic.GetValue("AppUpateUrl");
                }
            }
            return verData;
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <param name="AppSystem">APP系统</param>
        /// <param name="apiCode">The API code.</param>
        /// <returns>UserModel.</returns>
        public UserModel Login(string loginName, string password, string AppSystem, ref ApiStatusCode apiCode)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                UserModel model = dal.Login(loginName, password);
                if (model != null)
                {
                    if (model.IsActive == 1 && model.ShopActive == 1)
                    {
                        apiCode = ApiStatusCode.OK;
                        if (!string.IsNullOrEmpty(model.UserHeadImg))
                            model.UserHeadImg = WebConfig.reswebsite() + model.UserHeadImg;
                        model.myqrcodeUrl = WebConfig.articleDetailsDomain() + "/app/myqrcode.html?userid=" + model.UserId;
                        model.myShareQrcodeUrl = WebConfig.articleDetailsDomain() + string.Format("/resource/app/qrcode/{0}/index.html", model.UserId);
                        model.MengBeans = model.MengBeans - model.MengBeansLocked;
                        model.Score = model.Score - model.ScoreLocked;
                        model.TempMengBeans = UserLogic.countTempBeansMoney(model.UserId, 0);

                        string token = EncryptHelper.MD5(StringHelper.CreateCheckCode(20));
                        if (dal.IsAuthTokenExist(model.UserId) ? dal.UpdateUserAuthToken(model.UserId, token) : dal.AddUserAuthToken(model.UserId, token))
                            model.token = token;

                        if (model.UserIdentity == 1)
                            UserLogic.masterUpdate(model.UserId);
                        else
                            UserLogic.userUpdate(model.UserId);

                        model.LevelName = UserLogic.GetUserLevelName(model.UserId);

                        //添加登录日志
                        LogLogic.AddLoginLog(new LoginLogModel()
                        {
                            UserId = model.UserId,
                            UserIdentity = model.UserIdentity,
                            BelongOne = model.BelongOne,
                            ShopId = model.ShopId,
                            AppSystem = AppSystem
                        });

                        //更新最后登录时间
                        dal.UpdateLastLoginTime(model.UserId);
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
                    apiCode = ApiStatusCode.账户密码不正确;
                    return null;
                }
            }

        }

    }
}
