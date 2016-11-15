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
        public static List<OrderListModel> GetMyOrderList(int userId,int type, long lastId)
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
                orderList.pictureUrl = WebConfig.reswebsite();
                orderList.status = order.OrderStatus;
                orderList.id = StringHelper.GetUTCTime(order.CreateTime);
                result.Add(orderList);
            }
            return result;        
        }
    }
}
