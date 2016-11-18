/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved. 
**/


using BAMENG.CONFIG;
using BAMENG.IDAL;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.DAL
{
    public class CustomerDAL : AbstractDAL, ICustomerDAL
    {
        private const string APP_SELECT = @"select C.ID,C.BelongOne,C.BelongTwo,C.Status,C.InShop,C.Name,C.Mobile,C.Addr,C.Remark,C.ShopId,C.CreateTime,U.UB_UserRealName as BelongOneName,UB.UB_UserRealName as BelongTwoName,S.ShopName from BM_CustomerManage C 
                                                left join Hot_UserBaseInfo U with(nolock) on U.UB_UserID=C.BelongOne
                                                left join Hot_UserBaseInfo UB with(nolock) on UB.UB_UserID=C.BelongTwo
                                                left join BM_ShopManage S with(nolock) on  S.ShopID=C.ShopId
                                                where C.IsDel=0 ";
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteCustomerInfo(int customerId)
        {
            string strSql = "update BM_CustomerManage set IsDel=1 where ID=@ID";
            var param = new[] {
                new SqlParameter("@ID",customerId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 获取他的客户列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isvalid">客户是否有效</param>
        /// <returns></returns>
        public ResultPageModel GetCustomerList(SearchModel model, int shopId, bool isvalid = true)
        {
            ResultPageModel result = new ResultPageModel();
            string strSql = APP_SELECT;
            if (isvalid)
                strSql += " and C.Status= 1";
            else
                strSql += " and C.Status<>1";

            if (model.UserId > 0)
                strSql += " and C.BelongTwo= @BelongOne";

            if (shopId > 0)
                strSql += " and C.ShopId= @ShopId";

            if (!string.IsNullOrEmpty(model.key))
            {
                switch (model.searchType)
                {
                    case (int)SearchType.姓名:
                        strSql += string.Format(" and C.Name like '%{0}%' ", model.key);
                        break;
                    case (int)SearchType.手机:
                        strSql += " and C.Mobile=@Mobile";
                        break;
                    default:
                        break;
                }
            }


            var param = new[] {
                new SqlParameter("@BelongOne",model.UserId),
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Mobile",model.key)
            };
            //生成sql语句
            return getPageData<CustomerModel>(model.PageSize, model.PageIndex, strSql, "C.CreateTime", false, param);
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="identity">0盟友  1盟主</param>
        /// <param name="type">0所有客户 1未处理  2已处理</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ResultPageModel.</returns>
        public ResultPageModel GetAppCustomerList(int UserId, int identity, int type, int pageIndex, int pageSize)
        {
            string strSql = APP_SELECT;
            //Status 0 审核中，1已同意  2已拒绝

            if (type == 1)
                strSql += " and C.Status= 0";
            if (type == 2)
                strSql += " and C.Status<>0";

            if (identity == 1)
                strSql += " and (C.BelongTwo= @UserID or C.BelongOne= @UserID)";
            else
                strSql += " and C.BelongOne= @UserID";

            string orderbyField = "C.CreateTime";
            bool orderby = false;

            if (type == 2)
            {
                orderbyField = "C.Status";
            }
            var param = new[] {
                new SqlParameter("@UserID",UserId),
            };
            //生成sql语句
            return getPageData<CustomerModel>(pageSize, pageIndex, strSql, orderbyField, orderby, param);
        }



        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>CustomerModel.</returns>
        public CustomerModel GetModel(int customerId)
        {
            string strSql = APP_SELECT + " and ID=@ID";
            var param = new[] {
                new SqlParameter("@ID",customerId),
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntity<CustomerModel>(dr);
            }
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCustomerInfo(CustomerModel model)
        {
            if (!IsExist(model.Mobile, model.Addr))
            {
                string strSql = "insert into BM_CustomerManage (BelongOne,BelongTwo,Status,Name,Mobile,Addr,Remark,ShopId,InShop) values(@BelongOne,@BelongTwo,@Status,@Name,@Mobile,@Addr,@Remark,@ShopId,@InShop)";
                var param = new[] {
                    new SqlParameter("@BelongOne",model.BelongOne),
                    new SqlParameter("@BelongTwo",model.BelongTwo),
                    new SqlParameter("@Status",model.Status),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Mobile",model.Mobile),
                    new SqlParameter("@Addr",model.Addr),
                    new SqlParameter("@Remark",model.Remark),
                    new SqlParameter("@ShopId",model.ShopId),
                    new SqlParameter("@InShop",model.InShop)
                };
                return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param);
            }
            return 0;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public bool IsExist(string mobile, string addr)
        {
            string strSql = "select COUNT(1) from BM_CustomerManage where IsDel=0 and Status==1 and (Mobile =@Mobile or Addr =@Addr)";
            var param = new[] {
                new SqlParameter("@Mobile",mobile),
                new SqlParameter("@Addr",addr)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCustomerInfo(CustomerModel model)
        {
            string strSql = "update BM_CustomerManage set Name=@Name,Mobile=@Mobile,Addr=@Addr,Remark=@Remark where ID=@ID";
            var param = new[] {
                new SqlParameter("@Name",model.Name),
                new SqlParameter("@Mobile",model.Mobile),
                new SqlParameter("@Addr",model.Addr),
                new SqlParameter("@Remark",model.Remark),
                new SqlParameter("@ID",model.ID)
            };

            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 更新客户状态
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="status">0 审核中，1已同意  2已拒绝</param>
        /// <param name="userId">操作人ID(此方法只有盟主操作)</param>
        /// <returns></returns>
        public bool UpdateStatus(int customerId, int status, int userId)
        {
            string strSql = "update BM_CustomerManage set Status=@Status,BelongTwo=@BelongTwo where ID=@ID";
            var param = new[] {
                new SqlParameter("@BelongTwo",userId),
                new SqlParameter("@Status",status),
                new SqlParameter("@ID",customerId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;

        }

        /// <summary>
        /// 更新客户进店状态
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="status">1进店 0未进店</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateInShopStatus(int customerId, int status)
        {
            string strSql = "update BM_CustomerManage set InShop=@InShop,InShopTime=getdate() where ID=@ID";
            var param = new[] {
                new SqlParameter("@InShop",status),
                new SqlParameter("@ID",customerId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;

        }


        /// <summary>
        /// 获得客户信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public CustomerModel getCustomerModel(string mobile, string address)
        {
            CustomerModel model = new CustomerModel();
            string strSql = "select * from BM_CustomerManage where (Addr=@Addr or Mobile=@Mobile) and  IsDel=0 and Status=1";
            var parms = new[] {
               new SqlParameter("@Addr",address),
               new SqlParameter("@Mobile",mobile)
           };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms))
            {
                model = DbHelperSQLP.GetEntity<CustomerModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 获取用户的客户数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIdentity">0盟友 1盟主</param>
        /// <param name="status"> 0 审核中，1已同意  2已拒绝</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int GetCustomerCount(int userId, int userIdentity, int status)
        {
            string strSql = "select count(*) from BM_CustomerManage where IsDel=0 and Status=@Status";
            if (userIdentity == 1)
                strSql += " and BelongTwo=@UserId";
            else
                strSql += " and BelongOne=@UserId";

            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Status",status)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }
    }
}
