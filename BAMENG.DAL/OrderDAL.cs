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
 public   class OrderDAL: AbstractDAL, IOrderDAL
    {

        public List<OrderModel> GetOrderList( int userId,int status, long lastId)
        {
        
            string strSql = "select * from BM_Orders where UserId=@UserId";
            if (status > 0) strSql += " and OrderStatus="+status;
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

            return Int32.Parse(DbHelperSQL.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters).ToString())>0;
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
        public bool Update(OrderModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BM_Orders set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("Ct_BelongId=@Ct_BelongId,");
            strSql.Append("ShopId=@ShopId,");
            strSql.Append("orderTime=@orderTime,");
            strSql.Append("Memo=@Memo,");
            strSql.Append("OrderStatus=@OrderStatus,");
            strSql.Append("OrderImg=@OrderImg,");
            strSql.Append("SuccessImg=@SuccessImg,");
            strSql.Append("Ct_Name=@Ct_Name,");
            strSql.Append("Ct_Mobile=@Ct_Mobile,");
            strSql.Append("Ct_Address=@Ct_Address,");
            strSql.Append("CashCouponAmount=@CashCouponAmount,");
            strSql.Append("CashCouponBn=@CashCouponBn,");
            strSql.Append("FianlAmount=@FianlAmount,");
            strSql.Append("CreateTime=@CreateTime");
            strSql.Append(" where orderId=@orderId ");
            SqlParameter[] parameters = {
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
                    new SqlParameter("@orderId", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.Ct_BelongId;
            parameters[2].Value = model.ShopId;
            parameters[3].Value = model.orderTime;
            parameters[4].Value = model.Memo;
            parameters[5].Value = model.OrderStatus;
            parameters[6].Value = model.OrderImg;
            parameters[7].Value = model.SuccessImg;
            parameters[8].Value = model.Ct_Name;
            parameters[9].Value = model.Ct_Mobile;
            parameters[10].Value = model.Ct_Address;
            parameters[11].Value = model.CashCouponAmount;
            parameters[12].Value = model.CashCouponBn;
            parameters[13].Value = model.FianlAmount;
            parameters[14].Value = model.CreateTime;
            parameters[15].Value = model.orderId;

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
    }
}
