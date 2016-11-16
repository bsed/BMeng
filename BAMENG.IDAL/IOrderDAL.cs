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
         bool Update(OrderModel model);

        OrderModel GetModel(int id);
    }
}
