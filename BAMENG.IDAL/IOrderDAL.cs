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
      List<OrderModel> GetOrderList(int masterUserId, int status, long lastId, int userIdentity);


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

        /// <summary>
        /// 更新成交信息
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="price">The price.</param>
        /// <param name="note">The note.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.Int32.</returns>
        int UploadVoucher(string orderId, string customer
           , string mobile, decimal price, string note, string fileName, string fileName2, string fileName3, string fileName4, string fileName5);

        /// <summary>
        /// 获取订单完整详情
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>OrderModel.</returns>
        OrderModel GetOrderDetail(string orderId);

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="shopType">门店类型1 总店 0分店</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetOrderList(int shopId, int shopType, SearchModel model);


        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="status">0未成交，1已成交，2退单</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateOrderStatus(string orderId, int status);
    }
}
