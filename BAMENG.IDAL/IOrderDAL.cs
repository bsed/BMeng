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
      List<OrderModel> GetOrderList(int userId, int status, long lastId);


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
         bool Update(string orderId, int status, string memo);

        OrderModel GetModel(string orderId);

        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int CountOrders(int userId);


        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        int CountOrders(int userId, int orderStatus);
    }
}
