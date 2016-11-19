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
            , int sex, ref ApiStatusCode apiCode)
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
                else
                {
                    dal.SaveApplyFriend(userId, mobile, EncryptHelper.MD5(password), nickname, userName, sex);
                    apiCode = ApiStatusCode.OK;
                }
                return true;
            }
        }

        /// <summary>
        /// 给用户加盟豆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static int addUserMoney(int userId, decimal money)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.addUserMoney(userId, money);
            }
        }


        /// <summary>
        /// 更新盟友等级
        /// </summary>
        /// <param name="userId"></param>
        public static void userUpdate(int userId)
        {
            int amount = OrderLogic.CountOrders(userId);
            using (var dal = FactoryDispatcher.UserFactory())
            {
                List<MallUserLevelModel> levels = dal.GeUserLevelList(userId, 0);
                foreach (MallUserLevelModel level in levels)
                {
                    if (amount > level.UL_MemberNum)
                    {
                        //更新用户等级
                        dal.updateUserLevel(userId, level.UL_ID);
                    }
                }
            }
        }

        /// <summary>
        /// 更新盟主等级
        /// </summary>
        /// <param name="userId"></param>
        public static void masterUpdate(int userId)
        {

            using (var dal = FactoryDispatcher.UserFactory())
            {
                int amount = dal.countByBelongOne(userId);
                List<MallUserLevelModel> levels = dal.GeUserLevelList(userId, 1);
                foreach (MallUserLevelModel level in levels)
                {
                    if (amount > level.UL_MemberNum)
                    {
                        //更新用户等级
                        dal.updateUserLevel(userId, level.UL_ID);
                    }
                }
            }
        }

        /// <summary>
        /// 盟豆兑换
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="code">The code.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool ConvertToBean(int userId, int amount, ref ApiStatusCode code)
        {
            if (amount < 100)
            {
                code = ApiStatusCode.兑换的盟豆数量不能少于100;
                return false;
            }

            UserModel userModel = null;
            using (var dal = FactoryDispatcher.UserFactory())
            {
                userModel = dal.GetUserModel(userId);
            }

            decimal userAmount = userModel.MengBeans - userModel.MengBeansLocked;
            if (userAmount < amount)
            {
                code = ApiStatusCode.你的盟豆不够;
                return false;
            }

            using (var dal = FactoryDispatcher.UserFactory())
            {
                dal.addMengBeansLocked(userId, amount);
                dal.insertBeansConvert(userId, userModel.BelongOne, amount);
                code = ApiStatusCode.OK;
            }
            return true;
        }

        public static List<ConvertFlowModel> getMasterConvertFlow(int masterUserId, int lastId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return toConvertFlowModel(dal.getBeansConvertListByMasterModel(masterUserId, lastId));
            }
        }

        public static List<ConvertFlowModel> getConvertFlow(int userId, int lastId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return toConvertFlowModel(dal.getBeansConvertListModel(userId, lastId));
            }
        }

        private static List<ConvertFlowModel> toConvertFlowModel(List<BeansConvertModel> list)
        {
            List<ConvertFlowModel> result = new List<ConvertFlowModel>();
            foreach (BeansConvertModel convert in list)
            {
                ConvertFlowModel convertFlow = new ConvertFlowModel();
                convertFlow.money = convert.Amount;
                convertFlow.name = convert.UserRealName;
                convertFlow.time = StringHelper.GetUTCTime(convert.CreateTime);
                convertFlow.status = convert.Status;

                result.Add(convertFlow);
            }
            return result;
        }

        /// <summary>
        /// 兑换审核
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="status">1同意2拒绝</param>
        /// <returns></returns>
        public static bool ConvertAudit(int userId, int id, int status, ref ApiStatusCode code)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                BeansConvertModel model = dal.getBeansConvertModel(id);
                if (model == null || model.Status != 0 || model.UserMasterId != userId)
                {
                    code = ApiStatusCode.兑换审核存在异常;
                    return false;
                }

                if (status == 1)
                {
                    dal.addMengBeansLocked(model.UserId, -model.Amount);
                    dal.addUserMoney(model.UserId, -model.Amount);
                    dal.updateBeansConvertStatus(id, 1);
                }
                else if (status == 2)
                {
                    dal.addMengBeansLocked(model.UserId, -model.Amount);
                    dal.updateBeansConvertStatus(id, 2);
                }

                dal.updateBeansConvertStatus(id, status);
            }
            return true;
        }


        /// <summary>
        ///   盟友申请审核
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="status">1成功2拒绝</param>
        /// <returns></returns>
        public static bool AllyApplyAudit(int userId, int id, int status, ref ApiStatusCode code)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                ApplyFriendModel model = dal.getApplyFriendModel(id);
                if (model == null || model.UserId != userId)
                {
                    code = ApiStatusCode.操作失败;
                    return false;
                }

                if (status == 1)
                {
                    dal.updateApplyFriendStatus(id, 1);

                    UserRegisterModel register = new UserRegisterModel();
                    register.belongOne = userId;
                    register.loginName = model.Mobile;
                    register.loginPassword = model.Password;
                    register.mobile = model.Mobile;
                    register.nickname = model.NickNname;
                    register.ShopId = dal.getUserShopId(userId);
                    register.storeId = ConstConfig.storeId;
                    register.UserIdentity = 0;
                    register.username = model.UserName;
                    dal.AddUserInfo(register);
                }
                else if (status == 2)
                {
                    dal.updateApplyFriendStatus(id, 2);
                }
            }
            return true;
        }


        /// <summary>
        /// APP端修改用户信息
        /// </summary>
        /// <param name="opt">The opt.</param>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateUserInfo(UserPropertyOptions opt, UserModel model)
        {

            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.UpdateUserInfo(opt, model);
            }
        }


        /// <summary>
        /// 获取兑换数量(只对盟主)
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">状态 0,未审核 1已审核 2,拒绝</param>
        /// <returns>System.Int32.</returns>
        public static int GetConvertCount(int userid, int status)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetConvertCount(userid, status);
            }
        }



        /// <summary>
        /// 我的业务汇总
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIdentity">用户身份0盟友 1盟主</param>
        /// <returns>MyUserBusinessModel.</returns>
        public static MyUserBusinessModel MyBusinessAmount(int userId, int userIdentity)
        {
            MyUserBusinessModel model = new MyUserBusinessModel();

            //订单数量
            model.orderAmount = OrderLogic.CountOrdersByAllyUserId(userId, 0);

            //客户数量
            model.customerAmount = CustomerLogic.GetCustomerCount(userId, 0, 0);

            //兑换数量
            model.exchangeAmount = GetConvertCount(userId, 0);

            //现金券数量
            model.cashCouponAmount = CouponLogic.GetMyCashCouponCount(userId);

            return model;
        }


        public static bool SignIn(int userId)
        {
            string outputMsg = string.Empty;
            /**
            * 输出签到积分，从区间中随机获取
            */
            int Integral = 0;
            /**
             * 输出签到额外奖励积分
             */
            int RewardIntegral = 0;


            /**
            * 获取商家签到配置信息
            */
            SignInConfig signConfig = ConfigLogic.GetSignInConfig();

            /**
             * 判断商户是否开启签到功能
             */
            if (signConfig == null || !signConfig.EnableSign)
            {                
                return false;
            }


            // bool flag = SignMemberBLL.Instance.TrySignIn(this.CustomerId, this.UserId, out Integral, out RewardIntegral, out outputMsg);


            return false;
        }


        /// <summary>
        /// 获得盟豆流水记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static BeansRecordsListIndexModel getBeansRecordsList(int userId, int lastId) {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                BeansRecordsListIndexModel result = new BeansRecordsListIndexModel();
                if (lastId <= 0)
                {
                    result.outcome = countBeansMoney(userId, 0, 0);
                    result.income = countBeansMoney(userId, 0, 1);
                }
                else {
                    result.outcome = 0;
                    result.income = 0;
                }
                result.list = toBeansRecordsList(dal.getBeansRecordsList(userId, lastId,0));
                return result;
            }
        }

        /// <summary>
        /// 获得积分流水
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static List<BeansRecordsListModel> getScoreList(int userId, int lastId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return toBeansRecordsList(dal.getBeansRecordsList(userId, lastId, 1));
            }
        }

        /// <summary>
        /// 获取临时盟豆列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static List<TempBeansRecordsListModel> getTempBeansRecordsList(int userId, int lastId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return toTempBeansRecordsList(dal.getTempBeansRecordsList(userId, lastId, 0));
            }
        }


        private static List<BeansRecordsListModel> toBeansRecordsList(List<BeansRecordsModel> list)
        {
            List<BeansRecordsListModel> result = new List<BeansRecordsListModel>();
            foreach (BeansRecordsModel model in list)
            {
                BeansRecordsListModel item = new BeansRecordsListModel();
                item.id = model.ID;
                item.money = model.Amount;
                item.status = model.Income;
                item.time = StringHelper.GetUTCTime(model.CreateTime);
                item.remark = model.Remark;
                result.Add(item);
            }
            return result;
        }

        private static List<TempBeansRecordsListModel> toTempBeansRecordsList(List<TempBeansRecordsModel> list)
        {
            List<TempBeansRecordsListModel> result = new List<TempBeansRecordsListModel>();
            foreach (TempBeansRecordsModel model in list)
            {
                TempBeansRecordsListModel item = new TempBeansRecordsListModel();
                item.id = model.ID;
                item.money = model.Amount;
                item.status = model.Income;
                item.time = StringHelper.GetUTCTime(model.CreateTime);
                result.Add(item);
            }
            return result;
        }

        public static decimal countBeansMoney(int userId, int LogType, int income)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return countBeansMoney(userId, LogType, income);
            }
        }

        public static decimal countTempBeansMoney(int userId, int LogType, int income)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return countTempBeansMoney(userId, LogType, income);
            }
        }
    }
}
