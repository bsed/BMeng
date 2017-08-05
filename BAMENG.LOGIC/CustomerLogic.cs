/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerLogic
    {
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultPageModel GetCustomerList(SearchModel model, int shopId, bool isvalid = true)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetCustomerList(model, shopId, isvalid);
            }
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="identity">0盟友  1盟主</param>
        /// <param name="type">0所有客户 1未处理  2已处理</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetAppCustomerList(int UserId, int identity, int type, int pageIndex, int pageSize)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetAppCustomerList(UserId, identity, type, pageIndex, pageSize);
            }
        }
        /// <summary>
        ///获取客户资源列表
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetAppCustomerResList(int UserId, int pageIndex, int pageSize)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetAppCustomerResList(UserId, pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int InsertCustomerInfo(CustomerModel model)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                int flag = dal.InsertCustomerInfo(model);
                BeansRecordsModel model2 = null;
                if (flag > 0)
                {
                    //如果相等于，表示当前是盟主添加客户信息
                    if (model.BelongOne == model.BelongTwo)
                    {
                        //获取积分奖励配置
                        ScoreConfigModel scoreCfg = ConfigLogic.GetScoreConfig();
                        using (var dal1 = FactoryDispatcher.UserFactory())
                        {
                            //盟友提交客户信息，盟主奖励
                            if (scoreCfg.SubmitCustomerToMainScore2 > 0 && dal1.addUserIntegral(model.BelongOne, scoreCfg.SubmitCustomerToMainScore2) > 0)
                            {
                                model2 = new BeansRecordsModel();
                                model2.Amount = scoreCfg.SubmitCustomerToMainScore1;
                                model2.UserId = model.BelongTwo;
                                model2.LogType = 1;
                                model2.Income = 1;
                                model2.Remark = "盟友提交客户信息，盟主奖励";
                                model2.OrderId = "";
                                model2.CreateTime = DateTime.Now;
                            }
                        }
                    }
                }
                return flag;
            }
        }

        /// <summary>
        /// 添加客户资源
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        public static int InsertCustomerRes(CustomerResModel model)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.InsertCustomerRes(model);
            }
        }



        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool UpdateCustomerInfo(CustomerModel model)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.UpdateCustomerInfo(model);
            }
        }



        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteCustomerInfo(int customerId)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.DeleteCustomerInfo(customerId);
            }
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="status">审核状态 1已同意  2已拒绝 3未生成订单  4已生成订单，5已失效</param>
        /// <param name="userId">操作人ID(此方法只有盟主操作)</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool UpdateStatus(int customerId, int status, int userId)
        {
            try
            {
                using (var dal = FactoryDispatcher.CustomerFactory())
                {
                    bool flag = dal.UpdateStatus(customerId, status == 1 ? 3 : status, userId);

                    //如果客户审核同意
                    if (flag && status == 1)
                    {
                        var model = dal.GetModel(customerId);
                        if (model != null && model.BelongOne > 0)
                        {
                            //添加用户的客户量
                            UserLogic.AddUserCustomerAmount(model.BelongOne);
                            if (model.BelongOne != model.BelongTwo)
                                UserLogic.AddUserCustomerAmount(model.BelongTwo);


                            var shopData = ShopLogic.GetShopModel(model.ShopId);
                            RewardsSettingModel rewardSettingModel = null;
                            //判断当前客户是否所属分店
                            if (shopData != null && shopData.ShopType == 2)
                                rewardSettingModel = UserLogic.GetRewardModel(model.ShopId);

                            using (var dal1 = FactoryDispatcher.UserFactory())
                            {
                                BeansRecordsModel model2 = null;
                                if (rewardSettingModel != null && rewardSettingModel.CustomerReward > 0)
                                {
                                    //给盟友加盟豆
                                    UserLogic.addUserMoney(model.BelongOne, rewardSettingModel.CustomerReward);
                                    model2 = new BeansRecordsModel();
                                    model2.Amount = rewardSettingModel.CustomerReward;
                                    model2.UserId = model.BelongOne;
                                    model2.LogType = 0;
                                    model2.Income = 1;
                                    model2.Remark = "客户信息奖励";
                                    model2.OrderId = model.ID.ToString();
                                    model2.CreateTime = DateTime.Now;
                                    dal1.AddBeansRecords(model2);
                                }
                                //获取积分奖励配置
                                ScoreConfigModel scoreCfg = ConfigLogic.GetScoreConfig();
                                //审核盟友提交的客户信息                  
                                if (scoreCfg.SubmitCustomerToAllyScore > 0 && dal1.addUserIntegral(model.BelongOne, scoreCfg.SubmitCustomerToAllyScore) > 0)
                                {
                                    model2 = new BeansRecordsModel();
                                    model2.Amount = scoreCfg.SubmitCustomerToAllyScore;
                                    model2.UserId = model.BelongOne;
                                    model2.LogType = 1;
                                    model2.Income = 1;
                                    model2.Remark = "盟友提交客户信息,盟友奖励";
                                    model2.OrderId = "";
                                    model2.CreateTime = DateTime.Now;
                                    dal1.AddBeansRecords(model2);
                                }
                                //盟友提交客户信息，盟主奖励
                                if (scoreCfg.SubmitCustomerToMainScore1 > 0 && dal1.addUserIntegral(model.BelongTwo, scoreCfg.SubmitCustomerToMainScore1) > 0)
                                {
                                    model2 = new BeansRecordsModel();
                                    model2.Amount = scoreCfg.SubmitCustomerToMainScore1;
                                    model2.UserId = model.BelongTwo;
                                    model2.LogType = 1;
                                    model2.Income = 1;
                                    model2.Remark = "盟友提交客户信息，盟主奖励";
                                    model2.OrderId = "";
                                    model2.CreateTime = DateTime.Now;
                                    dal1.AddBeansRecords(model2);
                                }

                            }
                        }
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("UpdateStatus:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }











        /// <summary>
        /// 判断客户是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="addr">地址</param>
        /// <returns></returns>
        public static bool IsExist(string mobile, string addr)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.IsExist(mobile, addr);
            }
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>CustomerModel.</returns>
        public static CustomerModel GetModel(int customerId)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetModel(customerId);
            }
        }

        /// <summary>
        /// 更新客户进店状态
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="status">1进店 0未进店</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateInShopStatus(int customerId, int status)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                bool flag = dal.UpdateInShopStatus(customerId, status);
                if (flag)
                {
                    var model = dal.GetModel(customerId);
                    if (model != null)
                    {
                        //RewardsSettingModel rewardSettingModel = UserLogic.GetRewardModel(model.BelongTwo);

                        //if (rewardSettingModel == null || rewardSettingModel.ShopReward <= 0) return flag;

                        //using (var dal1 = FactoryDispatcher.UserFactory())
                        //{
                        //    //给盟友加盟豆
                        //    UserLogic.addUserMoney(model.BelongOne, rewardSettingModel.ShopReward);
                        //    BeansRecordsModel model2 = new BeansRecordsModel();
                        //    model2.Amount = rewardSettingModel.ShopReward;
                        //    model2.UserId = model.BelongOne;
                        //    model2.LogType = 0;
                        //    model2.Income = 1;
                        //    model2.Remark = "进店奖励";
                        //    model2.OrderId = model.ID.ToString();
                        //    model2.CreateTime = DateTime.Now;
                        //    dal1.AddBeansRecords(model2);
                        //}
                    }
                }
                return flag;
            }
        }

        public static CustomerModel getCustomerModel(string mobile, string address)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.getCustomerModel(mobile, address);
            }
        }


        /// <summary>
        /// 获取用户的客户数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIdentity">0盟友 1盟主</param>
        /// <param name="status"> 0 审核中，1已同意  2已拒绝</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static int GetCustomerCount(int userId, int userIdentity, int status)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetCustomerCount(userId, userIdentity, status);
            }
        }


        /// <summary>
        /// 添加客户维护信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddCustomerAssert(CustomerAssertModel model)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.AddCustomerAssert(model);
            }
        }

        /// <summary>
        /// 获取客户最新的一条维护信息
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static CustomerAssertModel GetCustomerAssertModel(int CID)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetCustomerAssertModel(CID);
            }
        }

        /// <summary>
        /// 获取客户的维护信息
        /// </summary>
        /// <param name="CID">客户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageModel GetCustomerAssertList(int CID, int pageIndex, int pageSize)
        {
            using (var dal = FactoryDispatcher.CustomerFactory())
            {
                return dal.GetCustomerAssertList(CID, pageIndex, pageSize);
            }
        }


    }
}
