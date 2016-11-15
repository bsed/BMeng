/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/


using BAMENG.CONFIG;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAMENG.LOGIC
{
    public class UserLogic
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="ShopId"></param>
        /// <param name="UserIdentity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultPageModel GetUserList(int ShopId, int UserIdentity, SearchModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetUserList(ShopId, UserIdentity, model);
            }
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditUserInfo(UserRegisterModel model, ref ApiStatusCode apiCode)
        {
            apiCode = ApiStatusCode.OK;
            using (var dal = FactoryDispatcher.UserFactory())
            {
                if (model.UserId > 0)
                {
                    bool b = dal.UpdateUserInfo(model);
                    if (!b)
                        apiCode = ApiStatusCode.更新失败;
                    return b;
                }
                else
                {
                    int flag = dal.AddUserInfo(model);
                    if (flag == -1)
                        apiCode = ApiStatusCode.账户已存在;
                    else if (flag == 0)
                        apiCode = ApiStatusCode.添加失败;

                    return flag > 0;
                }
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool DeleteUser(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.DeleltUserInfo(userId);
            }
        }

        /// <summary>
        /// 冻结或解冻账户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static bool UpdateUserActive(int userId, int active)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.UpdateUserActive(userId, active);
            }
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserModel GetModel(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetUserModel(userId);
            }
        }

        /// <summary>
        /// 获取他的盟友列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultPageModel GetAllyList(SearchModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetAllyList(model);
            }
        }

        /// <summary>
        /// 获取等级列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ResultPageModel GetLevelList(int type)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetLevelList(ConstConfig.storeId, type);
            }
        }

        public static bool DeleteLevel(int levelId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.DeleteLevel(levelId, ConstConfig.storeId);
            }
        }

        /// <summary>
        /// 编辑等级
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="levelType"></param>
        /// <param name="levelname"></param>
        /// <param name="upgradeCount"></param>
        /// <returns></returns>
        public static bool EditLevel(int levelId, int levelType, string levelname, int upgradeCount)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                MallUserLevelModel model = new MallUserLevelModel()
                {
                    IntegralPreID = 0,
                    PricePreID = 0,
                    UL_BelongOne_Content = "",
                    UL_BelongTwo_Content = "",
                    UL_CustomerID = ConstConfig.storeId,
                    UL_DefaultLevel = 0,
                    UL_Description = "",
                    UL_DirectTeamNum = 0,
                    UL_Gold = 0,
                    UL_GuidetLevel = -1,
                    UL_ID = levelId,
                    UL_IndirectTeamNum = 0,
                    UL_Integral = 0,
                    UL_Level = 1,
                    UL_LevelName = levelname,
                    UL_MemberNum = upgradeCount,
                    UL_Money = 0,
                    UL_OpenLevel_One = false,
                    UL_OpenLevel_Two = false,
                    UL_Type = levelType,

                };
                if (levelId > 0)
                {
                    model.UL_Level = dal.GetLevelCount(ConstConfig.storeId, levelType) + 1;
                    return dal.UpdateLevel(model);
                }
                else
                {
                    model.UL_Level = dal.GetMaxLevel(ConstConfig.storeId, levelType) + 1;
                    return dal.InsertLevel(model) > 0;
                }
            }
        }


        /// <summary>
        /// 后台登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPassword"></param>
        /// <param name="IsShop">是否是门店登录</param>
        /// <returns></returns>
        public static AdminLoginModel Login(string loginName, string loginPassword, bool IsShop)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.Login(loginName, loginPassword, IsShop);
            }
        }


        /// <summary>
        /// Gets the user identifier by authentication token.
        /// </summary>
        /// <param name="Token">The token.</param>
        /// <returns>System.Int32.</returns>
        public static int GetUserIdByAuthToken(string Token)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetUserIdByAuthToken(Token);
            }
        }


        /// <summary>
        /// 设置盟友奖励
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="creward">客户资料提交奖励</param>
        /// <param name="orderreward">订单成交奖励</param>
        /// <param name="shopreward">客户进店奖励.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool SetAllyRaward(int userId, decimal creward, decimal orderreward, decimal shopreward)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                RewardsSettingModel model = new RewardsSettingModel()
                {
                    UserId = userId,
                    CustomerReward = creward,
                    OrderReward = orderreward,
                    ShopReward = shopreward
                };
                if (dal.IsRewarExist(userId))
                    return dal.UpdateRewardSetting(model);
                else
                {
                    return dal.AddRewardSetting(model) > 0;
                }
            }
        }


        /// <summary>
        ///根据盟主，获取盟友奖励设置信息
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>RewardsSettingModel.</returns>
        public static RewardsSettingModel GetRewardModel(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetRewardModel(userId);
            }
        }


        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="password">The password.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool ForgetPwd(string mobile, string password)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.ForgetPwd(mobile, password);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">The user identifier.</param>        
        /// <param name="oldPassword">The old password.</param>
        /// <param name="password">The password.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool ChanagePassword(int userId, string oldPassword, string password)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                bool flag = dal.ChanagePassword(userId, oldPassword, password);
                string token = EncryptHelper.MD5(StringHelper.CreateCheckCode(20));
                if (dal.IsAuthTokenExist(userId))
                    dal.UpdateUserAuthToken(userId, token);
                else
                    dal.AddUserAuthToken(userId, token);

                return flag;
            }
        }


        public static bool AllyApply(int userId, string mobile, string password
            , string nickname, string userName
            , int sex,ref ApiStatusCode apiCode)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                if (dal.ExistApplyFriend(mobile))
                {
                    apiCode = ApiStatusCode.你已申请请耐心等到审核;
                }
                else if (dal.UserExist(mobile, ConstConfig.storeId))
                {
                    apiCode = ApiStatusCode.手机用户已存在;
                }
                else {
                    dal.SaveApplyFriend(userId, mobile, EncryptHelper.MD5(password), nickname, userName, sex);
                    apiCode = ApiStatusCode.OK;
                }
                return true;
            }
        }
    }
}
