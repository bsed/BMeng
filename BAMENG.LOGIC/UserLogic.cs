/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/


using BAMENG.CONFIG;
using BAMENG.MODEL;
using HotCoreUtils.Caching;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
                var user = dal.GetUserModel(userId);
                if (user != null)
                {
                    user.UserHeadImg = WebConfig.reswebsite() + user.UserHeadImg;
                    user.myqrcodeUrl = WebConfig.articleDetailsDomain() + "/app/myqrcode.html?userid=" + user.UserId;
                    user.myShareQrcodeUrl = WebConfig.articleDetailsDomain() + string.Format("/resource/app/qrcode/{0}/index.html", user.UserId);
                    user.MengBeans = user.MengBeans - user.MengBeansLocked;
                    user.Score = user.Score - user.ScoreLocked;
                }
                return user;

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

        /// <summary>
        /// Gets the master convert flow.
        /// </summary>
        /// <param name="masterUserId">The master user identifier.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>List&lt;ConvertFlowModel&gt;.</returns>
        public static List<ConvertFlowModel> getMasterConvertFlow(int masterUserId, int lastId, int type)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return toConvertFlowModel(dal.getBeansConvertListByMasterModel(masterUserId, lastId, type));
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
                convertFlow.ID = convert.ID;
                convertFlow.headimg = WebConfig.reswebsite() + convert.HeadImg;
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
                using (TransactionScope scope = new TransactionScope())
                {
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



                        BeansRecordsModel model2 = new BeansRecordsModel();
                        model2.Amount = -model.Amount;
                        model2.UserId = model.UserId;
                        model2.LogType = 0;
                        model2.Income = 0;
                        model2.Remark = "兑换";
                        model2.OrderId = "";
                        model2.CreateTime = DateTime.Now;
                        dal.AddBeansRecords(model2);

                    }
                    else if (status == 2)
                    {
                        dal.addMengBeansLocked(model.UserId, -model.Amount);
                        dal.updateBeansConvertStatus(id, 2);
                    }

                    dal.updateBeansConvertStatus(id, status);

                    scope.Complete();
                }
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
                    using (TransactionScope scope = new TransactionScope())
                    {

                        dal.updateApplyFriendStatus(id, 1);
                        UserRegisterModel register = new UserRegisterModel();
                        register.belongOne = userId;
                        register.loginName = model.Mobile;
                        register.loginPassword = model.Password;
                        register.mobile = model.Mobile;
                        register.nickname = model.NickName;
                        register.ShopId = dal.getUserShopId(userId);
                        register.storeId = ConstConfig.storeId;
                        register.UserIdentity = 0;
                        register.username = model.UserName;
                        register.userGender = model.Sex == 1 ? "M" : "F";
                        dal.AddUserInfo(register);

                        scope.Complete();
                    }
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
        /// 获取盟友数量
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>System.Int32.</returns>
        public static int GetAllyCount(int userid)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetAllyCount(userid);
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
            model.orderAmount = userIdentity == 1 ? OrderLogic.CountOrders(userId, 0) : OrderLogic.CountOrdersByAllyUserId(userId, 0);

            //客户数量
            model.customerAmount = CustomerLogic.GetCustomerCount(userId, userIdentity, 0);

            //兑换数量
            model.exchangeAmount = GetConvertCount(userId, 0);

            //现金券数量
            model.cashCouponAmount = CouponLogic.GetMyCashCouponCount(userId);

            return model;
        }


        /// <summary>
        /// 签到功能
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="apiCode">The API code.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static int SignIn(int userId, ref ApiStatusCode apiCode)
        {
            try
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
                 * 日期格式
                 */
                string dateFormat = "yyyy-MM-dd";
                /**
                * 获取商家签到配置信息
                */
                SignInConfig signCfg = ConfigLogic.GetSignInConfig();

                /**
                 * 判断商户是否开启签到功能
                 */
                if (signCfg == null || !signCfg.EnableSign)
                {
                    apiCode = ApiStatusCode.签到功能未开启;
                    return 0;
                }

                Integral = signCfg.SignScore;

                /**
                 * 连续签到天数，默认0
                 */
                int SignCount = 0;
                MemberSignModel memberSign = GetMemberSignModel(userId);
                if (memberSign == null)
                {
                    memberSign = new MemberSignModel();
                    memberSign.UserId = userId;
                }
                else
                {
                    /**
                     * 判断今天是否签到过
                     */
                    if (memberSign.lastSignTime.ToString(dateFormat).Equals(DateTime.Now.ToString(dateFormat)))
                    {
                        apiCode = ApiStatusCode.今日已签到;
                        return 0;
                    }
                }


                /**
                 * 判断是否开启连续签到功能
                 * 如果开启，则判断连续签到逻辑
                 */
                if (signCfg.EnableContinuousSign)
                {
                    /**
                       * 判断昨天是否已经满足奖励条件
                       * 如果满足，则连续签到天数置0，重新计数
                       */
                    if (memberSign.SignCount >= signCfg.ContinuousSignDay)
                    {
                        RewardIntegral = signCfg.ContinuousSignRewardScore;
                        //重新计数
                        memberSign.SignCount = 0;
                    }
                    /**
                     * 当前已签到天数
                     */
                    SignCount = memberSign.SignCount;

                    /**
                    * 连续签到天数是否大于0
                    */
                    if (SignCount > 0)
                    {
                        /**
                         * 判断昨天是否签到，如果签到过，则连续签到天数加1
                         * 否则，连续签到天数置1
                         */
                        if (memberSign.lastSignTime.ToString(dateFormat).Equals(DateTime.Now.AddDays(-1).ToString(dateFormat)))
                            SignCount += 1;
                        else
                            SignCount = 1;
                    }
                    else
                        SignCount = 1;

                    /**
                        * 判断连续签到天数是否满足奖励条件
                        * 满足条件后，获取奖励积分，并且连续签到天数置0
                        */
                    if (SignCount >= signCfg.ContinuousSignDay)
                        RewardIntegral = signCfg.ContinuousSignRewardScore;


                }
                else
                    SignCount = 0;


                using (var dal = FactoryDispatcher.UserFactory())
                {

                    /**
                     * 更新会员签到数据
                     */
                    memberSign.SignCount = SignCount;
                    memberSign.TotalSignIntegral += (Integral + RewardIntegral);
                    memberSign.lastSignTime = DateTime.Now;
                    memberSign.TotalSignDays += 1;
                    if (memberSign.ID > 0)
                        dal.UpdateMemberSignInfo(memberSign);
                    else
                        memberSign.ID = dal.AddMemberSignInfo(memberSign);

                    /**
                     * 更新签到缓存
                     */
                    RefreshMemberSignCache(userId, memberSign);
                    //将签到获得积分，冲入用户积分账号中
                    if (dal.addUserIntegral(userId, Integral + RewardIntegral) > 0)
                    {
                        /**
                         * 添加签到日志
                         */
                        BeansRecordsModel model2 = new BeansRecordsModel();
                        model2.Amount = Integral + RewardIntegral;
                        model2.UserId = userId;
                        model2.LogType = 1;
                        model2.Income = 1;
                        model2.Remark = "签到";
                        model2.OrderId = "";
                        model2.CreateTime = DateTime.Now;
                        dal.AddBeansRecords(model2);
                        apiCode = ApiStatusCode.OK;
                    }

                }
                return Integral + RewardIntegral;
            }
            catch (Exception ex)
            {
                apiCode = ApiStatusCode.请重新签到;
                LogHelper.Log(string.Format("SignIn-->StackTrace:{0},Message:{1},MemberId:{2}", ex.StackTrace, ex.Message, ex, userId));
                return 0;

            }
        }


        /// <summary>
        /// 获得盟豆流水记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static BeansRecordsListIndexModel getBeansRecordsList(int userId, int lastId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                BeansRecordsListIndexModel result = new BeansRecordsListIndexModel();
                if (lastId <= 0)
                {
                    result.outcome = countBeansMoney(userId, 0, 0) * -1;
                    result.income = countBeansMoney(userId, 0, 1);
                }
                else
                {
                    result.outcome = 0;
                    result.income = 0;
                }
                result.list = toBeansRecordsList(dal.getBeansRecordsList(userId, lastId, 0));
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
                item.ID = model.ID;
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
                item.ID = model.ID;
                item.money = model.Amount;
                item.status = model.Income;
                item.time = StringHelper.GetUTCTime(model.CreateTime);
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 统计盟豆数据
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="LogType">0盟豆 1积分</param>
        /// <param name="income">0支出 1收入.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal countBeansMoney(int userId, int LogType, int income)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.countBeansMoney(userId, LogType, income);
            }
        }

        /// <summary>
        /// 统计临时盟豆数据.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="LogType">0盟豆 1积分</param>
        /// <param name="income">0支出 1收入.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal countTempBeansMoney(int userId, int LogType, int income)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.countTempBeansMoney(userId, LogType, income);
            }
        }
        /// <summary>
        /// 获取待结算盟豆
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="LogType">Type of the log.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal countTempBeansMoney(int userId, int LogType)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.countTempBeansMoney(userId, LogType);
            }
        }


        /// 获取签到信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>MemberSignModel.</returns>
        public static MemberSignModel GetMemberSignModel(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                string cacheKey = GetMemberSignCacheKey(userId);
                MemberSignModel model = WebCacheHelper<MemberSignModel>.Get(cacheKey);
                if (model == null)
                {
                    model = dal.GetMemberSignModel(userId);
                    if (model == null) return null;
                    WebCacheHelper.Insert(cacheKey, model, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
                }
                return model;
            }
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="memberSign">签到数据</param>
        private static void RefreshMemberSignCache(int userId, MemberSignModel memberSign)
        {
            string cacheKey = GetMemberSignCacheKey(userId);
            WebCacheHelper.Insert(cacheKey, memberSign, new System.Web.Caching.CacheDependency(WebCacheHelper.GetDepFile(cacheKey)));
        }
        /// <summary>
        /// 得到签到缓存KEY
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string GetMemberSignCacheKey(int userId)
        {
            return string.Format("BMSIGN{0}{1}", userId, DateTime.Now.ToString("yyyyMMdd"));
        }

        public static int AddTempBeansRecords(TempBeansRecordsModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.AddTempBeansRecords(model);
            }

        }
        public static int AddBeansRecords(BeansRecordsModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.AddBeansRecords(model);
            }
        }



        /// <summary>
        /// 获取盟友申请列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetApplyFriendList(SearchModel model)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetApplyFriendList(model);
            }
        }


        /// <summary>
        ///添加用户客户提交量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddUserCustomerAmount(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.AddUserCustomerAmount(userId);
            }
        }

        /// <summary>
        /// 添加用户订单成交量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddUserOrderSuccessAmount(int userId)
        {
            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.AddUserOrderSuccessAmount(userId);
            }
        }




        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MyAllyIndexModel.</returns>
        public static MyAllyIndexModel GetUserRank(int userId)
        {

            using (var dal = FactoryDispatcher.UserFactory())
            {
                return dal.GetUserRank(userId);
            }
        }


        /// <summary>
        /// 获取我的现金卷列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<MyCouponListModel> getMyCashCouponList(int userId)
        {
            UserModel user = GetModel(userId);
            List<CashCouponModel> coupons = CouponLogic.getEnabledCashCouponList(user.ShopId);
            List<CouponSendModel> sendlist = CouponLogic.getCouponSendList(userId);
            List<int> sends = new List<int>();
            foreach (CouponSendModel item in sendlist)
            {
                sends.Add(item.CouponId);
            }

            List<MyCouponListModel> result = new List<MyCouponListModel>();
            foreach (CashCouponModel item in coupons)
            {
                if(!sends.Contains(item.CouponId)) result.Add(ToMyCouponList(item));
            }

            return result;
        }

        public static  MyCouponListModel ToMyCouponList(CashCouponModel item)
        {
            MyCouponListModel model = new MyCouponListModel();
            model.due = item.StartTime.ToString("yyyy.MM.dd") + "-" + item.EndTime.ToString("yyyy.MM.dd");
            model.id = item.CouponId;
            model.money = item.Money;
            model.name = item.Title;
            return model;
        }
    }
}
