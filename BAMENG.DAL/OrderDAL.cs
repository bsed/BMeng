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
 public class OrderDAL: AbstractDAL, IOrderDAL
    {

        /// <summary>
        /// 获取盟主的订单列表
        /// </summary>
        /// <param name="masterUserId"></param>
        /// <param name="status"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrderList( int masterUserId,int status, long lastId)
        {
        
            string strSql = "select top 10 * from BM_Orders where UserId=@UserId";
            if (status > 0) strSql += " and OrderStatus="+status;
            if (lastId > 0) strSql += " and CreateTime<" + StringHelper.GetTimeFromUTC(lastId);
            SqlParameter[] parameters = {
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

            return Int32.Parse(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters).ToString())>0;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(OrderModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BM_Orders(");
            strSql.Append("orderId,UserId,Ct_BelongId,ShopId,orderTime,Memo,OrderStatus,OrderImg,SuccessImg,Ct_Name,Ct_Mobile,Ct_Address,CashCouponAmount,CashCouponBn,FianlAmount,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@orderId,@UserId,@Ct_BelongId,@ShopId,@orderTime,@Memo,@OrderStatus,@OrderImg,@SuccessImg,@Ct_Name,@Ct_Mobile,@Ct_Address,@CashCouponAmount,@CashCouponBn,@FianlAmount,@CreateTime)");
            SqlParameter[] parameters = {
                    new SqlParameter("@orderId", SqlDbType.NVarChar,50),
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
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
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
        public bool Update(string orderId,int status,string memo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BM_Orders set "); 
            strSql.Append("Memo=@Memo,OrderStatus=@OrderStatus");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Memo", memo),
                    new SqlParameter("@OrderStatus", status),                   
                    new SqlParameter("@orderId", orderId)};     

            int rows = DbHelperSQL.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text,strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        /// 计算订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int CountOrders(int userId,int orderStatus)
        {
            string strSql = "select count(*) from BM_Orders where UserId=@UserId and OrderStatus=@OrderStatus";
            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@OrderStatus",orderStatus)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }


    }
}
