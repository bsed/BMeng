using BAMENG.CONFIG;
using BAMENG.IDAL;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.DAL
{
    public class OrderDAL : AbstractDAL, IOrderDAL
    {

        /// <summary>
        /// 获取盟主的订单列表
        /// </summary>
        /// <param name="masterUserId"></param>
        /// <param name="status"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrderList(int masterUserId, int status, long lastId, int userIdentity)
        {

            string strSql = "select top 10 * from BM_Orders where 1=1 ";
            if (userIdentity == 1)
                strSql += " and UserId=@UserId";
            else
                strSql += " and Ct_BelongId=@UserId";

            if (status >= 0) strSql += " and OrderStatus=" + status;
            if (lastId > 0) strSql += " and CreateTime< @Date ";
            strSql += " order by CreateTime desc";
            SqlParameter[] parameters = {
                    new SqlParameter("@Date",StringHelper.GetTimeFromUTC(lastId)),
                    new SqlParameter("@UserId", masterUserId)
            };

            List<OrderModel> list = new List<OrderModel>();
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters))
            {
                list = DbHelperSQLP.GetEntityList<OrderModel>(dr);
            }
            return list;
        }


        /// <summary>
        /// 获取盟友的订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public List<OrderModel> GetUserOrderList(int userId, int status, long lastId)
        {

            string strSql = "select top 10 * from BM_Orders where Ct_BelongId=@UserId";
            if (status > 0) strSql += " and OrderStatus=" + status;
            if (lastId > 0) strSql += " and CreateTime<" + StringHelper.GetTimeFromUTC(lastId);
            strSql += " order by CreateTime desc";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId)
            };

            List<OrderModel> list = new List<OrderModel>();
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters))
            {
                list = DbHelperSQLP.GetEntityList<OrderModel>(dr);
            }
            return list;
        }

        public DateTime ConvertIntDatetime(double utc)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                startTime = startTime.AddSeconds(utc);
                return startTime;
            }
            catch (Exception)
            {
                return DateTime.Parse("1979-01-01 00:00:00");
            }
        }



        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string orderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BM_Orders");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@orderId", SqlDbType.NVarChar,50)         };
            parameters[0].Value = orderId;

            return Int32.Parse(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters).ToString()) > 0;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(OrderModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BM_Orders(");
            strSql.Append("orderId,UserId,Ct_BelongId,ShopId,orderTime,Memo,OrderStatus,OrderImg,SuccessImg,Ct_Name,Ct_Mobile,Ct_Address,CashCouponAmount,CashCouponBn,FianlAmount,CreateTime,MengBeans,BelongOneShopId)");
            strSql.Append(" values (");
            strSql.Append("@orderId,@UserId,@Ct_BelongId,@ShopId,@orderTime,@Memo,@OrderStatus,@OrderImg,@SuccessImg,@Ct_Name,@Ct_Mobile,@Ct_Address,@CashCouponAmount,@CashCouponBn,@FianlAmount,@CreateTime,@MengBeans,@BelongOneShopId)");
            SqlParameter[] parameters = {
                    new SqlParameter("@orderId", SqlDbType.NVarChar),
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@Ct_BelongId", SqlDbType.Int,4),
                    new SqlParameter("@ShopId", SqlDbType.Int,4),
                    new SqlParameter("@orderTime", SqlDbType.DateTime),
                    new SqlParameter("@Memo", SqlDbType.NVarChar,500),
                    new SqlParameter("@OrderStatus", SqlDbType.Int,4),
                    new SqlParameter("@OrderImg", SqlDbType.NVarChar,200),
                    new SqlParameter("@SuccessImg", SqlDbType.NVarChar,200),
                    new SqlParameter("@Ct_Name", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ct_Mobile", SqlDbType.NVarChar,20),
                    new SqlParameter("@Ct_Address", SqlDbType.NVarChar,200),
                    new SqlParameter("@CashCouponAmount", SqlDbType.Decimal,9),
                    new SqlParameter("@CashCouponBn", SqlDbType.NVarChar,50),
                    new SqlParameter("@FianlAmount", SqlDbType.Decimal,9),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@MengBeans",model.MengBeans),
                    new SqlParameter("@BelongOneShopId",model.BelongOneShopId)
            };
            parameters[0].Value = model.orderId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.Ct_BelongId;
            parameters[3].Value = model.ShopId;
            parameters[4].Value = model.orderTime;
            parameters[5].Value = model.Memo;
            parameters[6].Value = model.OrderStatus;
            parameters[7].Value = model.OrderImg;
            parameters[8].Value = model.SuccessImg;
            parameters[9].Value = model.Ct_Name;
            parameters[10].Value = model.Ct_Mobile;
            parameters[11].Value = model.Ct_Address;
            parameters[12].Value = model.CashCouponAmount;
            parameters[13].Value = model.CashCouponBn;
            parameters[14].Value = model.FianlAmount;
            parameters[15].Value = model.CreateTime;

            int rows = DbHelperSQL.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(string orderId, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BM_Orders set ");
            strSql.Append(" OrderStatus=@OrderStatus,FinishedTime=@FinishedTime");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderStatus", status),
                    new SqlParameter("@FinishedTime",DateTime.Now),
                    new SqlParameter("@orderId", orderId)};

            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters);
        }


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
        public int UploadVoucher(string orderId, string customer
            , string mobile, decimal price, string note, string fileName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BM_Orders set ");
            strSql.Append("Note=@Note,Ct_Mobile=@Ct_Mobile,Ct_Name=@Ct_Name,FianlAmount=@FianlAmount,SuccessImg=@SuccessImg");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Note", note),
                    new SqlParameter("@SuccessImg", fileName),
                    new SqlParameter("@Ct_Mobile", mobile),
                    new SqlParameter("@Ct_Name", customer),
                    new SqlParameter("@FianlAmount", price),
                    new SqlParameter("@orderId", orderId)};

            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters);
        }





        public OrderModel GetModel(string orderId)
        {
            string strSql = "select * from BM_Orders where orderId=@orderId";

            SqlParameter[] parameters = {
                    new SqlParameter("@orderId", orderId)
            };

            OrderModel model = null;
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters))
            {
                model = DbHelperSQLP.GetEntity<OrderModel>(dr);
            }
            return model;
        }



        /// <summary>
        /// 获取订单完整详情
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>OrderModel.</returns>
        public OrderModel GetOrderDetail(string orderId)
        {
            string strSql = @"select O.*,S.ShopName,U.UB_UserRealName as BelongOneName,UB.UB_UserRealName as BelongTwoName from BM_Orders O with(nolock)
                                left join BM_ShopManage S with(nolock)  on S.ShopID=O.ShopId
                                left join Hot_UserBaseInfo U with(nolock) on U.UB_UserID=O.Ct_BelongId
                                left join Hot_UserBaseInfo UB with(nolock) on UB.UB_UserID=O.UserId
                                where O.orderId=@orderId";

            SqlParameter[] parameters = {
                    new SqlParameter("@orderId", orderId)
            };

            OrderModel model = null;
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters))
            {
                model = DbHelperSQLP.GetEntity<OrderModel>(dr);
                if (model != null)
                {
                    if (model.OrderStatus == 0)
                        model.OrderStatusName = "未成交";
                    else if (model.OrderStatus == 1)
                        model.OrderStatusName = "已成交";
                    else
                        model.OrderStatusName = "已退单";
                }
            }
            return model;
        }




        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CountOrders(int userId)
        {
            string strSql = "select count(*) from BM_Orders where UserId=@UserId";
            var param = new[] {
                new SqlParameter("@UserId",userId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }

        /// <summary>
        /// 根据盟友ID，获取盟友的订单数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderStatus">0 未成交 1 已成交 2退单</param>
        /// <returns>System.Int32.</returns>
        public int CountOrdersByAllyUserId(int userId, int orderStatus)
        {
            string strSql = "select count(*) from BM_Orders where Ct_BelongId=@UserId and OrderStatus=@OrderStatus";
            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@OrderStatus",orderStatus)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }


        /// <summary>
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int CountOrders(int userId, int orderStatus)
        {
            string strSql = "select count(*) from BM_Orders where UserId=@UserId and OrderStatus=@OrderStatus";
            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@OrderStatus",orderStatus)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }


        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="shopType">门店类型1 总店 0分店</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public ResultPageModel GetOrderList(int shopId, int shopType, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = string.Empty;
            if (shopType == 0)//如果是分店情况下，查询
            {
                strSql = @"select O.*,S.ShopName from BM_Orders O
                            left join BM_ShopManage S on S.ShopID=O.ShopId 
                            where 1=1 ";
                strSql += " and O.ShopId=@ShopId";
            }
            else
            {
                strSql = @"select O.*,S.ShopName from BM_Orders O
                            left join BM_ShopManage S on S.ShopBelongId=O.ShopId 
                            where 1=1 ";
                strSql += " and (O.ShopId=@ShopId or S.ShopBelongId=@ShopId) ";
            }
            if (!string.IsNullOrEmpty(model.key))
                strSql += string.Format(" and O.Ct_Name like '%{0}%' ", model.key);
            if (model.type != -1)
                strSql += "  and OrderStatus=@OrderStatus";

            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),O.CreateTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),O.CreateTime,121)<=@endTime ";


            var param = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@OrderStatus", model.type),
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime)
            };
            //生成sql语句
            return getPageData<OrderModel>(model.PageSize, model.PageIndex, strSql, "O.CreateTime", param, (items =>
             {
                 items.ForEach(item =>
                 {
                     if (item.OrderStatus == 0)
                         item.OrderStatusName = "未成交";
                     else if (item.OrderStatus == 1)
                         item.OrderStatusName = "已成交";
                     else
                         item.OrderStatusName = "已退单";
                 });
             }));
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="status">0未成交，1已成交，2退单</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateOrderStatus(string orderId, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BM_Orders set ");
            strSql.Append("OrderStatus=@OrderStatus");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderStatus", status),
                    new SqlParameter("@orderId", orderId)};

            int rows = DbHelperSQL.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
