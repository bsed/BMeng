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

        public static List<OrderListModel> GetMyOrderList(int userId, int type, long lastId)
        {
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return toOrderListModel(dal.GetOrderList(userId, type, lastId));
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
                orderList.id = StringHelper.GetUTCTime(order.CreateTime);
                result.Add(orderList);
            }
            return result;
        }

        public static bool saveOrder(int userId, string userName, string mobile, string address, string cashNo, string memo, string filename)
        {

            OrderModel model = new OrderModel();
            model.orderId = createOrderNo(userId);
            model.UserId = userId;
            model.Ct_BelongId = userId;

            CustomerModel customer = CustomerLogic.getCustomerModel(mobile, address);
            if (customer != null)
            {
                model.Ct_BelongId = customer.BelongOne;
                model.ShopId = customer.ShopId;
            }
            model.orderTime = DateTime.Now;
            model.Memo = memo;
            model.OrderStatus = 0;
            model.OrderImg = filename;
            model.SuccessImg = "";
            model.Ct_Name = userName;
            model.Ct_Mobile = mobile;
            model.Ct_Address = address;
            using (var dal = FactoryDispatcher.CouponFactory())
            {
                CashCouponLogModel coupon = dal.getEnableCashCouponLogModel(mobile, cashNo);
                if (coupon != null)
                {
                    model.CashCouponAmount = coupon.Money;
                    model.CashCouponBn = cashNo;
                }
            }
            model.FianlAmount = 0;
            model.CreateTime = DateTime.Now;
            using (var dal = FactoryDispatcher.OrderFactory())
            {
                return dal.Add(model);
            }
        }


        public static OrderDetailModel getOrderDetail(int id)
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
                result.status = order.OrderStatus;
                result.orderId = order.orderId;
                result.orderTime = StringHelper.GetUTCTime(order.orderTime);
                result.address = order.Ct_Address;
            }
            return result;
         }
    }
}
