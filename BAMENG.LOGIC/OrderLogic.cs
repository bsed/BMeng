using BAMENG.CONFIG;
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
    public class OrderLogic
    {
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        private static string createOrderNo(int userId)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(3) + userId;
        }

        public static List<OrderListModel> GetMyOrderList(int userId, int type, long lastId, int userIdentity)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return toOrderListModel(dal.GetOrderList(userId, type, lastId, userIdentity));
            }
        }

        private static List<OrderListModel> toOrderListModel(List<OrderModel> list)
        {
            List<OrderListModel> result = new List<OrderListModel>();
            foreach (OrderModel order in list)
            {
                OrderListModel orderList = new OrderListModel();
                orderList.userName = order.Ct_Name;
                orderList.mobile = order.Ct_Mobile;
                orderList.money = order.FianlAmount;
                orderList.pictureUrl = WebConfig.reswebsite() + order.OrderImg;
                orderList.status = order.OrderStatus;
                orderList.ID = StringHelper.GetUTCTime(order.CreateTime);
                orderList.orderId = order.orderId;
                orderList.remark = order.Memo;
                orderList.note = order.Note;
                orderList.mengbeans = order.MengBeans;
                if (orderList.status == 0)
                {
                    orderList.statusName = "未成交";
                    orderList.pictureUrl = WebConfig.reswebsite() + order.OrderImg;
                }
                else if (orderList.status == 1)
                {
                    orderList.statusName = "已成交";
                    orderList.pictureUrl = WebConfig.reswebsite() + order.SuccessImg;
                }
                else
                {
                    orderList.statusName = "退单";
                    orderList.pictureUrl = WebConfig.reswebsite() + order.OrderImg;
                }
                result.Add(orderList);
            }
            return result;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="address">The address.</param>
        /// <param name="cashNo">The cash no.</param>
        /// <param name="memo">The memo.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="apiCode">The API code.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool saveOrder(int userId, int shopId, string userName, string mobile, string address, string cashNo, string memo, string filename, int cid, ref ApiStatusCode apiCode)
        {

            try
            {
                OrderModel model = new OrderModel();
                model.orderId = createOrderNo(userId);
                model.UserId = userId;
                model.Ct_BelongId = userId;
                model.orderTime = DateTime.Now;
                model.Memo = memo;
                model.OrderStatus = 0;
                model.OrderImg = filename;
                model.SuccessImg = "";
                model.Ct_Name = userName;
                model.Ct_Mobile = mobile;
                model.Ct_Address = address;
                model.FianlAmount = 0;
                model.CreateTime = DateTime.Now;

                //判断订单是否使用优惠券
                CashCouponLogModel coupon = null;
                int cashUserId = 0;
                using (var dal = FactoryDispatcher.CouponFactory())
                {
                    if (!string.IsNullOrEmpty(cashNo))
                    {
                        coupon = dal.getEnableCashCouponLogModel(mobile, cashNo);
                        if (coupon != null && coupon.ID > 0)
                        {
                            if (coupon.ShopId != shopId)
                            {
                                apiCode = ApiStatusCode.非本店现金券;
                                return false;
                            }
                            if (coupon.IsUse != 0)
                            {
                                apiCode = ApiStatusCode.现金券已使用;
                                return false;
                            }
                            model.CashCouponAmount = coupon.Money;
                            model.CashCouponBn = cashNo;
                            model.ShopId = coupon.ShopId;
                            //设置订单归属用户
                            model.Ct_BelongId = coupon.UserId;
                            model.UserId = coupon.BelongOneUserId;
                            cashUserId = coupon.UserId;
                        }
                        else
                        {
                            apiCode = ApiStatusCode.优惠券不存在;
                            return false;
                        }
                    }
                }
                //如果没有优惠
                if (cashUserId <= 0)
                {
                    //根据手机号或地址，查找客户
                    CustomerModel customer =cid>0?CustomerLogic.getCustomerModel(cid): CustomerLogic.getCustomerModel(mobile, address);
                    if (customer != null)
                    {
                        //设置订单归属用户
                        model.Ct_BelongId = customer.BelongOne;
                        model.UserId = customer.BelongTwo;
                        model.ShopId = customer.ShopId;
                        cashUserId = customer.BelongOne;
                    }
                }
                TempBeansRecordsModel model1 = null;
                BeansRecordsModel model2 = null;
                if (cashUserId > 0)
                {
                    using (var dal = FactoryDispatcher.UserFactory())
                    {
                        var user = dal.GetUserModel(cashUserId);
                        if (user.UserIdentity == 0)
                        {
                            model.UserId = user.BelongOne;
                            model.Ct_BelongId = user.UserId;


                            var shopData = ShopLogic.GetShopModel(model.ShopId);
                            RewardsSettingModel rewardSettingModel = null;
                            //判断当前客户是否所属分店
                            if (shopData != null && shopData.ShopType == 2)
                                rewardSettingModel = UserLogic.GetRewardModel(model.ShopId);

                            if (rewardSettingModel != null && rewardSettingModel.OrderReward > 0)
                            {
                                //订单成交需付盟豆
                                model.MengBeans = rewardSettingModel.OrderReward;
                                //插入盟友订单成交临时奖励
                                model1 = new TempBeansRecordsModel();
                                model1.Amount = rewardSettingModel.OrderReward;
                                model1.UserId = cashUserId;
                                model1.LogType = 0;
                                model1.Income = 1;
                                model1.CreateTime = DateTime.Now;
                                model1.Status = 0;
                                model1.Remark = "下单";
                            }
                        }
                        else
                        {
                            model.UserId = user.UserId;
                            model.Ct_BelongId = user.UserId;
                        }

                        //获取积分奖励配置
                        ScoreConfigModel scoreCfg = ConfigLogic.GetScoreConfig();
                        //添加盟主创建订单，奖励积分                    
                        if (scoreCfg.CreateOrderScore > 0 && dal.addUserIntegral(userId, scoreCfg.CreateOrderScore) > 0)
                        {
                            model2 = new BeansRecordsModel();
                            model2.Amount = scoreCfg.CreateOrderScore;
                            model2.UserId = userId;
                            model2.LogType = 1;
                            model2.Income = 1;
                            model2.Remark = "创建订单奖励";
                            model2.OrderId = "";
                            model2.CreateTime = DateTime.Now;
                        }
                    }
                }
                else
                {
                    using (var dal = FactoryDispatcher.UserFactory())
                    {
                        var user = dal.GetUserModel(userId);
                        model.UserId = userId;
                        model.Ct_BelongId = userId;
                        model.ShopId = user.ShopId;
                    }
                }
                bool flag = false;

                using (TransactionScope scope = new TransactionScope())
                {

                    using (var dal = FactoryDispatcher.OrderFactory())
                    {
                        model.BelongOneShopId = ShopLogic.GetBelongShopId(model.ShopId);
                        if (model.BelongOneShopId == 0)
                            model.BelongOneShopId = model.ShopId;
                        flag = dal.Add(model);
                    }
                    if (flag)
                    {

                        //

                        //根据手机号或地址，查找客户，修改客户状态为已生成订单
                        CustomerModel customer = cid > 0 ? CustomerLogic.getCustomerModel(cid) : CustomerLogic.getCustomerModel(mobile, address);
                        if (customer != null)
                        {
                            using (var dal = FactoryDispatcher.CustomerFactory())
                            {
                                dal.UpdateStatus(customer.ID, 4);
                            }

                        }
                        //添加
                        using (var dald = FactoryDispatcher.UserFactory())
                        {
                            if (model1 != null)
                                dald.AddTempBeansRecords(model1);

                            //添加日志
                            if (model2 != null)
                                dald.AddBeansRecords(model2);
                        }

                        //添加优惠券使用记录
                        using (var cpDal = FactoryDispatcher.CouponFactory())
                        {
                            if (coupon != null)
                            {
                                if (cpDal.UpdateUserCashCouponUseStatus(coupon.ID))
                                {
                                    //添加优惠券领取操作日志
                                    LogLogic.AddCouponLog(new LogBaseModel()
                                    {
                                        objId = coupon.CouponId,
                                        UserId = coupon.UserId,
                                        ShopId = coupon.ShopId,
                                        OperationType = 2,//0创建 1领取 2使用
                                        Money = coupon.Money
                                    });
                                }
                            }
                        }
                    }
                    scope.Complete();
                    return flag;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("saveOrder==>Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                apiCode = ApiStatusCode.SERVICEERROR;
                return false;
            }


        }


        public static OrderDetailModel getOrderDetail(string id)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return toOrderDetail(dal.GetModel(id));
            }
        }

        public static OrderDetailModel toOrderDetail(OrderModel order)
        {
            OrderDetailModel result = null;
            if (order != null)
            {
                result = new OrderDetailModel();
                result.userName = order.Ct_Name;
                result.mobile = order.Ct_Mobile;
                result.pictureUrl = WebConfig.reswebsite() + order.OrderImg;
                result.successUrl = WebConfig.reswebsite() + order.SuccessImg;
                result.status = order.OrderStatus;
                result.orderId = order.orderId;
                result.orderTime = StringHelper.GetUTCTime(order.orderTime);
                result.address = order.Ct_Address;
                result.remark = order.Memo;
                result.note = order.Note;
                result.cashcouponmoney = order.CashCouponAmount;
                result.fianlamount = order.FianlAmount;
            }
            return result;
        }

        public static bool Update(int userId, string orderId, int status, ref ApiStatusCode code)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                OrderModel orderModel = dal.GetModel(orderId);
                if (orderModel == null)
                {
                    code = ApiStatusCode.订单存在问题;
                    return false;
                }
                if (orderModel.OrderStatus != 0)
                {
                    code = ApiStatusCode.订单目前状态存在异常;
                    return false;
                }

                if (status == 1 && string.IsNullOrEmpty(orderModel.SuccessImg))
                {
                    code = ApiStatusCode.请先上传成交凭证;
                    return false;
                }


                //改订单为已处理
                if (status == 1 && orderModel.UserId != orderModel.Ct_BelongId)
                {
                    //更新盟友用户等级
                    UserLogic.userUpdate(orderModel.Ct_BelongId);
                    //更新盟主用户等级
                    UserLogic.masterUpdate(orderModel.UserId);
                    if (orderModel.MengBeans > 0)
                    {
                        //给盟友加盟豆
                        UserLogic.addUserMoney(orderModel.Ct_BelongId, orderModel.MengBeans);

                        using (var dal1 = FactoryDispatcher.UserFactory())
                        {

                            TempBeansRecordsModel model1 = new TempBeansRecordsModel();
                            model1.Amount = -orderModel.MengBeans;
                            model1.UserId = orderModel.Ct_BelongId;
                            model1.LogType = 0;
                            model1.Income = 0;
                            model1.CreateTime = DateTime.Now;
                            model1.Status = 0;
                            model1.OrderId = orderModel.orderId;
                            model1.Remark = "转正";
                            dal1.AddTempBeansRecords(model1);

                            BeansRecordsModel model2 = new BeansRecordsModel();
                            model2.Amount = orderModel.MengBeans;
                            model2.UserId = orderModel.Ct_BelongId;
                            model2.LogType = 0;
                            model2.Income = 1;
                            model2.Remark = "订单奖励";
                            model2.OrderId = orderModel.orderId;
                            model2.CreateTime = DateTime.Now;
                            dal1.AddBeansRecords(model2);
                        }
                    }
                }
                if (status == 2 && orderModel.UserId != orderModel.Ct_BelongId && orderModel.MengBeans > 0)
                {

                    using (var dal1 = FactoryDispatcher.UserFactory())
                    {
                        TempBeansRecordsModel model1 = new TempBeansRecordsModel();
                        model1.Amount = -orderModel.MengBeans;
                        model1.UserId = orderModel.Ct_BelongId;
                        model1.LogType = 0;
                        model1.Income = 0;
                        model1.CreateTime = DateTime.Now;
                        model1.Status = 0;
                        model1.OrderId = orderModel.orderId;
                        model1.Remark = "退单";
                        dal1.AddTempBeansRecords(model1);
                    }
                }

                bool flag = dal.Update(orderId, status) == 1;
                if (status == 1)
                {
                    if (orderModel.UserId != orderModel.Ct_BelongId && orderModel.UserId > 0 && orderModel.Ct_BelongId > 0)
                    {
                        //添加用户订单量
                        UserLogic.AddUserOrderSuccessAmount(orderModel.UserId);
                        UserLogic.AddUserOrderSuccessAmount(orderModel.Ct_BelongId);
                    }
                    else
                    {
                        //添加用户订单量
                        UserLogic.AddUserOrderSuccessAmount(orderModel.UserId);
                    }
                }

                return flag;
            }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>OrderModel.</returns>
        public static OrderModel GetModel(string orderId)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.GetModel(orderId);
            }
        }

        /// <summary>
        /// 获取订单完整详情
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>OrderModel.</returns>
        public static OrderModel GetOrderDetail(string orderId)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.GetOrderDetail(orderId);
            }
        }


        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int CountOrders(int userId)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.CountOrders(userId);
            }
        }

        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public static int CountOrders(int userId, int orderStatus)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.CountOrders(userId, orderStatus);
            }
        }

        /// <summary>
        /// 根据盟友ID，获取盟友的订单数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderStatus">订单状态0 未成交 1 已成交 2退单</param>
        /// <returns>System.Int32.</returns>
        public static int CountOrdersByAllyUserId(int userId, int orderStatus)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.CountOrdersByAllyUserId(userId, orderStatus);
            }
        }



        /// <summary>
        /// 获取盟友的订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static List<OrderModel> GetUserOrderList(int userId, int status, long lastId)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.GetUserOrderList(userId, status, lastId);
            }
        }

        /// <summary>
        /// 更新上传凭证信息
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="price">The price.</param>
        /// <param name="memo">The memo.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.Int32.</returns>
        public static int UploadVoucher(string orderId, string customer
            , string mobile, decimal price, string memo, string fileName)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.UploadVoucher(orderId, customer
            , mobile, price, memo, fileName);
            }
        }


        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="shopType">门店类型1 总店 0分店</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetOrderList(int shopId, int shopType, SearchModel model)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.GetOrderList(shopId, shopType, model);
            }
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="status">0未成交，1已成交，2退单</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateOrderStatus(string orderId, int status)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.UpdateOrderStatus(orderId, status);
            }
        }
    }
}
