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
    }
}
