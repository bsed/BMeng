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
using System.Transactions;

namespace BAMENG.LOGIC
{
    /// <summary>
    /// Class CouponLogic.
    /// </summary>
    public class CouponLogic
    {



        /// <summary>
        /// 获取现金券领取记录列表
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetUserCashCouponLogList(int couponId, SearchModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetUserCashCouponLogList(couponId, model);
            }
        }


        /// <summary>
        /// 添加现金券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static int AddCashCoupon(CashCouponModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                int flag = dal.AddCashCoupon(model);
                if (flag > 0)
                {
                    //添加优惠券领取操作日志
                    LogLogic.AddCouponLog(new LogBaseModel()
                    {
                        objId = flag,
                        UserId = 0,
                        ShopId = model.ShopId,
                        OperationType = 0,//0创建 1领取 2使用
                        Money = model.Money
                    });
                }
                return flag;
            }
        }

        /// <summary>
        /// 删除现金券
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool DeleteCashCoupon(int couponId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.DeleteCashCoupon(couponId);
            }
        }

        /// <summary>
        /// 修改现金券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool UpdateCashCoupon(CashCouponModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.UpdateCashCoupon(model);
            }
        }


        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool EditCashCoupon(CashCouponModel model)
        {
            if (model.CouponId > 0)
                return UpdateCashCoupon(model);
            else
                return AddCashCoupon(model) > 0;
        }


        /// <summary>
        /// 获取现金券列表
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static ResultPageModel GetCashCouponList(int shopId, SearchModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetCashCouponList(shopId, model);
            }
        }


        /// <summary>
        /// 设置优惠券启用和禁用
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool SetCouponEnable(int couponId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.SetCouponEnable(couponId);
            }
        }


        /// <summary>
        /// 我的现金券数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int32.</returns>
        public static int GetMyCashCouponCount(int userId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetMyCashCouponCount(userId);
            }
        }


        /// <summary>
        /// 根据用户ID和优惠券ID，获取优惠券记录ID
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>System.Int32.</returns>
        public static CashCouponLogModel GetCashCouponLogIDByUserID(int userId, int couponId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetCashCouponLogIDByUserID(userId, couponId);
            }
        }

        /// <summary>
        /// 获得现金卷列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public static List<CashCouponModel> getEnabledCashCouponList(int shopId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.getEnabledCashCouponList(shopId);
            }
        }

        /// <summary>
        /// 根据盟友ID，获取盟友的现金券列表
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List&lt;CashCouponModel&gt;.</returns>
        public static List<CashCouponModel> GetEnableCashCouponListByUserId(int userId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetEnableCashCouponListByUserId(userId);
            }
        }


        /// <summary>
        /// 获得优惠卷发送列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<CouponSendModel> getCouponSendList(int userId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.getCouponSendList(userId);
            }
        }


        /// <summary>
        /// 添加优惠券发送记录
        /// </summary>
        /// <param name="userId">发送人ID</param>
        /// <param name="Int32">The int32.</param>
        /// <param name="userIdentity">用户身份，0盟友  1盟主.</param>
        /// <param name="sendToUserId">接收用户ID,如果是自己转发，则为0</param>
        /// <param name="couponId">优惠券ID</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddSendCoupon(int userId, int userIdentity, int sendToUserId, int couponId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (var dal = FactoryDispatcher.CouponFactory())
                {
                    bool flag = dal.AddSendCoupon(userId, sendToUserId, couponId);

                    //如果盟主自己分享或发送给盟友，则创建现金券记录
                    if (flag && userIdentity == 1)
                    {
                        CashCouponModel couponModel = dal.GetModel(couponId);
                        if (couponModel != null)
                        {
                            int BelongOneUserId = userId;

                            int belongOneShopId = ShopLogic.GetBelongShopId(couponModel.ShopId);
                            if (belongOneShopId == 0)
                                belongOneShopId = couponModel.ShopId;

                            dal.CreateUserCashCouponLog(new CashCouponLogModel()
                            {
                                CouponId = couponId,
                                CouponNo = StringHelper.CreateCheckCode(10).ToLower(),
                                UserId = sendToUserId > 0 ? sendToUserId : userId,
                                IsShare = sendToUserId > 0 ? 0 : 1,
                                Money = couponModel.Money,
                                StartTime = couponModel.StartTime,
                                EndTime = couponModel.EndTime,
                                ShopId = couponModel.ShopId,
                                BelongOneUserId = BelongOneUserId,
                                BelongOneShopId = belongOneShopId
                            });
                        }
                    }
                    if (flag && userIdentity == 0)
                    {
                        dal.UpdateCouponShareStatus(userId, couponId);
                    }

                    scope.Complete();
                    return flag;
                }
            }
        }


        /// <summary>
        ///给盟友发送优惠券
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="TargetIds">The target ids.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddSendAllyCoupon(int userId, int couponId, string[] TargetIds)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (var dal = FactoryDispatcher.CouponFactory())
                {
                    bool flag = dal.AddSendCoupon(userId, 0, couponId);

                    //如果盟主自己分享或发送给盟友，则创建现金券记录
                    CashCouponModel couponModel = dal.GetModel(couponId);
                    if (couponModel != null)
                    {
                        int BelongOneUserId = userId;
                        string cpNo = StringHelper.CreateCheckCode(10).ToLower();
                        foreach (var item in TargetIds)
                        {
                            int sendToUserId = Convert.ToInt32(item);

                            int belongOneShopId = ShopLogic.GetBelongShopId(couponModel.ShopId);
                            if (belongOneShopId == 0)
                                belongOneShopId = couponModel.ShopId;

                            dal.CreateUserCashCouponLog(new CashCouponLogModel()
                            {
                                CouponId = couponId,
                                CouponNo = cpNo,
                                UserId = sendToUserId,
                                IsShare = 0,
                                Money = couponModel.Money,
                                StartTime = couponModel.StartTime,
                                EndTime = couponModel.EndTime,
                                ShopId = couponModel.ShopId,
                                BelongOneUserId = BelongOneUserId,
                                BelongOneShopId = belongOneShopId
                            });
                        }
                    }
                    scope.Complete();
                    return flag;
                }
            }
        }



        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="isValid">是否只获取有效的优惠券</param>
        /// <returns>CashCouponModel.</returns>
        public static CashCouponModel GetModel(int couponId, bool isValid = false)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.GetModel(couponId, isValid);
            }
        }

        /// <summary>
        /// 创建用户现金券(盟主分享现金券时调用)
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static int CreateUserCashCouponLog(CashCouponLogModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.CreateUserCashCouponLog(model);
            }
        }

        /// <summary>
        /// 更新现金券的领取记录
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateUserCashCouponGetLog(CashCouponLogModel model)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.UpdateUserCashCouponGetLog(model);
            }
        }

        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="couponNo">The coupon no.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Boolean.</returns>
        public static bool DeleteUserCashCoupon(string couponNo, int couponId, int userId)
        {
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                return dal.DeleteUserCashCoupon(couponNo, couponId, userId);
            }
        }
    }
}
