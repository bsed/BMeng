using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
   public class OrderLogic
    {
        public static OrderListModel GetMyOrderList(int type, long lastId)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetMyOrderList(type,lastId);
            }
        }
    }
}
