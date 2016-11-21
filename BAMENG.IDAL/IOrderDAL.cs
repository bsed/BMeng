using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    public interface IOrderDAL :IDisposable
    {
      List<OrderModel> GetOrderList(int masterUserId, int status, long lastId);


        /// <summary>
        /// 是否存在该记录
        /// </summary>
         bool Exists(string orderId);

        /// <summary>
        /// 增加一条数据
        /// </summary>
         bool Add(OrderModel model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
         int Update(string orderId, int status);

        OrderModel GetModel(string orderId);

        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int CountOrders(int userId);


        /// <summary>
        /// 根据盟友ID，获取盟友的订单数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderStatus">0 未成交 1 已成交 2退单</param>
        /// <returns>System.Int32.</returns>
        int CountOrdersByAllyUserId(int userId,int orderStatus);

        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        int CountOrders(int userId, int orderStatus);


        /// <summary>
        /// 获取盟友的订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        List<OrderModel> GetUserOrderList(int userId, int status, long lastId);

        int UploadVoucher(string orderId, string customer
           , string mobile, decimal price, string note, string fileName);
    }
}
