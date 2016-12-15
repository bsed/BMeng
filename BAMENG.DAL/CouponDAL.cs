/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMENG.MODEL;
using System.Data.SqlClient;
using HotCoreUtils.DB;
using BAMENG.CONFIG;
using System.Data;

namespace BAMENG.DAL
{
    /// <summary>
    /// 优惠券相关数据操作
    /// </summary>
    /// <seealso cref="BAMENG.DAL.AbstractDAL" />
    /// <seealso cref="BAMENG.IDAL.ICouponDAL" />
    public class CouponDAL : AbstractDAL, ICouponDAL
    {

        private const string SELECT_SQL = "select CouponId,ShopId,Title,Money,StartTime,EndTime,IsEnable,CreateTime,Remark from BM_CashCoupon where IsDel=0 ";

        /// <summary>
        /// 添加现金券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int AddCashCoupon(CashCouponModel model)
        {
            string strSql = "insert into BM_CashCoupon(ShopId,Title,Money,StartTime,EndTime,IsEnable,Remark) values(@ShopId,@Title,@Money,@StartTime,@EndTime,@IsEnable,@Remark);select @@IDENTITY;";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Money", model.Money),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@IsEnable", model.IsEnable),
                new SqlParameter("@ShopId", model.ShopId),
                new SqlParameter("@Remark", model.Remark)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
            if (obj != null)
                return Convert.ToInt32(obj);
            return 0;
        }

        /// <summary>
        /// 删除现金券
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool DeleteCashCoupon(int couponId)
        {
            string strSql = "update BM_CashCoupon set IsDel=1 where CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@CouponId",couponId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }

        /// <summary>
        /// 获取现金券列表
        /// </summary>
        /// <param name="shopId">门店ID</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultPageModel GetCashCouponList(int shopId, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = SELECT_SQL;

            if (!string.IsNullOrEmpty(model.key))
                strSql += string.Format(" and Title like '%{0}%' ", model.key);


            strSql += " and ShopId=@ShopId ";

            if (model.Status != -100)
            {
                strSql += " and IsEnable=@IsEnable";
            }


            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)<=@endTime ";
            var param = new[] {
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime),
                new SqlParameter("@ShopId", shopId),
                new SqlParameter("@IsEnable", model.Status)
            };
            //生成sql语句
            return getPageData<CashCouponModel>(model.PageSize, model.PageIndex, strSql, "CreateTime", param, ((items) =>
            {
                items.ForEach((item) =>
                {
                    if (DateTime.Compare(item.EndTime.AddHours(24), DateTime.Now) >= 0)
                    {
                        if (item.IsEnable == 1)
                            item.StatusName = "启用";
                        else
                            item.StatusName = "未启用";
                    }
                    else
                        item.StatusName = "已过期";

                    item.time = item.StartTime.ToString("yyyy.MM.dd") + " 至 " + item.EndTime.ToString("yyyy.MM.dd");
                });
            }));
        }

        /// <summary>
        /// 修改现金券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateCashCoupon(CashCouponModel model)
        {
            string strSql = "update BM_CashCoupon set Title=@Title,Money=@Money,StartTime=@StartTime,EndTime=@EndTime,IsEnable=@IsEnable,Remark=@Remark where CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Money", model.Money),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@IsEnable", model.IsEnable),
                new SqlParameter("@CouponId", model.CouponId),
                new SqlParameter("@Remark", model.Remark)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }


        /// <summary>
        /// 设置优惠券启用和禁用
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool SetCouponEnable(int couponId)
        {
            string strSql = "update BM_CashCoupon set IsEnable=ABS(IsEnable-1) where CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@CouponId",couponId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }

        /// <summary>
        /// 创建用户现金券(盟主分享现金券时调用)
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public int CreateUserCashCouponLog(CashCouponLogModel model)
        {
            string strSql = "insert into BM_GetCashCouponLog(UserId,CouponNo,CouponId,StartTime,EndTime,Money,IsShare,ShopId,BelongOneUserId,BelongOneShopId) values(@UserId,@CouponNo,@CouponId,@StartTime,@EndTime,@Money,@IsShare,@ShopId,@BelongOneUserId,@BelongOneShopId);select @@IDENTITY";
            var parm = new[] {
                new SqlParameter("@UserId", model.UserId),
                new SqlParameter("@CouponId", model.CouponId),
                new SqlParameter("@CouponNo", model.CouponNo),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@Money", model.Money),
                new SqlParameter("@IsShare", model.IsShare),
                new SqlParameter("@ShopId", model.ShopId),
                new SqlParameter("@BelongOneUserId", model.BelongOneUserId),
                new SqlParameter("@BelongOneShopId", model.BelongOneShopId)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
            if (obj != null)
                return Convert.ToInt32(obj);
            return 0;
        }

        /// <summary>
        /// 获取现金券领取记录列表
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public ResultPageModel GetUserCashCouponLogList(int couponId, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = "select L.ID,L.UserId,L.CouponNo,L.CouponId,L.Name,L.Mobile,L.IsGet,L.GetTime,L.IsUse,L.UseTime,L.CreateTime,StartTime,EndTime,L.BelongOneUserId,L.BelongOneShopId from BM_GetCashCouponLog L with(nolock) where IsDel=0 and IsGet=1 and CouponId=@CouponId";

            if (model.Status != -1)
                strSql += " and IsUse=@IsUse ";

            if (!string.IsNullOrEmpty(model.key))
                strSql += " and Mobile=@Mobile ";


            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),GetTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),GetTime,121)<=@endTime ";
            var param = new[] {
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime),
                new SqlParameter("@CouponId", couponId),
                new SqlParameter("@IsUse", model.Status),
                new SqlParameter("@Mobile", model.key)
            };
            //生成sql语句
            return getPageData<CashCouponLogModel>(model.PageSize, model.PageIndex, strSql, "GetTime", param, (items =>
            {
                items.ForEach(item =>
                {
                    item.time = item.StartTime.ToString("yyyy.MM.dd") + " 至 " + item.EndTime.ToString("yyyy.MM.dd");
                });
            }));
        }


        /// <summary>
        /// 获取优惠卷信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cashNo"></param>
        /// <returns></returns>
        public CashCouponLogModel getEnableCashCouponLogModel(string mobile, string cashNo)
        {

            CashCouponLogModel model = new CashCouponLogModel();
            string strSql = "select * from BM_GetCashCouponLog where CouponNo=@CouponNo and Mobile=@Mobile and IsGet=1 and IsDel=0 ";
            var parms = new[] {
               new SqlParameter("@CouponNo",cashNo),
               new SqlParameter("@Mobile",mobile)
           };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms))
            {
                model = DbHelperSQLP.GetEntity<CashCouponLogModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="isValid">是否只获取有效的优惠券</param>
        /// <returns>CashCouponModel.</returns>
        public CashCouponModel GetModel(int couponId, bool isValid = false)
        {
            CashCouponModel model = new CashCouponModel();
            string strSql = SELECT_SQL + " and CouponId=@CouponId";
            if (isValid)
                strSql += " and EndTime>@Date and IsEnable=1 ";
            var parms = new[] {
               new SqlParameter("@CouponId",couponId),
               new SqlParameter("@Date",DateTime.Now)
           };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms))
            {
                model = DbHelperSQLP.GetEntity<CashCouponModel>(dr);
            }
            return model;
        }



        /// <summary>
        /// 更新现金券的领取记录
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateUserCashCouponGetLog(CashCouponLogModel model)
        {
            string strSql = "update BM_GetCashCouponLog set Mobile=@Mobile,Name=@Name,IsGet=1,GetTime=GETDATE() where ID=@ID and UserId=@UserId";
            var parms = new[] {
               new SqlParameter("@Mobile",model.Mobile),
               new SqlParameter("@Name",model.Name),
               new SqlParameter("@UserId",model.UserId),
               new SqlParameter("@ID",model.ID)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms) > 0;
        }

        /// <summary>
        /// Deletes the user cash coupon.
        /// </summary>
        /// <param name="couponNo">The coupon no.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Boolean.</returns>
        public bool DeleteUserCashCoupon(string couponNo, int couponId, int userId)
        {
            string strSql = "delete from BM_GetCashCouponLog where UserId<>@UserId and CouponId=@CouponId and CouponNo=@CouponNo and IsGet=0";
            var parms = new[] {
               new SqlParameter("@CouponId",couponId),
               new SqlParameter("@CouponNo",couponNo),
               new SqlParameter("@UserId",userId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms) > 0;
        }


        /// <summary>
        /// 更新现金券的领取记录
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateUserCashCouponUseStatus(int cashlogId)
        {
            string strSql = "update BM_GetCashCouponLog set IsUse=1,UseTime=GETDATE() where ID=@ID";
            var parms = new[] {
               new SqlParameter("@ID",cashlogId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms) > 0;
        }

        /// <summary>
        /// 我的现金券数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int32.</returns>
        public int GetMyCashCouponCount(int userId)
        {
            string strSql = "select COUNT(1) from BM_GetCashCouponLog where UserId=@UserId and IsGet=0 ";
            var param = new[] {
                new SqlParameter("@UserId",userId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param));
        }

        /// <summary>
        /// 根据用户ID和优惠券ID，获取优惠券记录ID
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>System.Int32.</returns>
        public CashCouponLogModel GetCashCouponLogIDByUserID(int userId, int couponId)
        {
            string strSql = @"select top 1 L.*,C.Remark from BM_GetCashCouponLog L
                                left join BM_CashCoupon C on C.CouponId=L.CouponId
                                where L.UserId=@UserId and L.CouponId=@CouponId and L.IsGet=0  and L.IsShare=1 and L.IsDel=0";
            var param = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@CouponId",couponId)
            };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntity<CashCouponLogModel>(dr);
            }
        }

        /// <summary>
        /// 获得现金卷列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public List<CashCouponModel> getEnabledCashCouponList(int shopId)
        {
            List<CashCouponModel> list = new List<CashCouponModel>();
            string strSql = "select * from BM_CashCoupon where ShopId=@ShopId and EndTime>@Date and IsEnable=1 and  IsDel=0 order by CouponId desc";
            var parms = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Date",DateTime.Now)
            };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, parms))
            {
                list = DbHelperSQLP.GetEntityList<CashCouponModel>(dr);
            }
            return list;

        }


        /// <summary>
        /// 根据盟友ID，获取盟友的现金券列表
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List&lt;CashCouponModel&gt;.</returns>
        public List<CashCouponModel> GetEnableCashCouponListByUserId(int userId)
        {
            string strSql = @"select g.CouponId as CouponId,c.Title,g.CouponNo,g.StartTime,g.EndTime,g.Money from BM_GetCashCouponLog g
                                left join BM_CashCoupon c on c.CouponId = g.CouponId
                                where c.IsEnable = 1 and g.UserId =@UserId and g.EndTime>@Date  and isShare=0 and c.IsDel=0  order by g.ID desc";

            var parms = new[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Date",DateTime.Now)
            };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, parms))
            {
                return DbHelperSQLP.GetEntityList<CashCouponModel>(dr);
            }
        }


        /// <summary>
        /// 获得优惠卷发送列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CouponSendModel> getCouponSendList(int userId)
        {
            List<CouponSendModel> list = new List<CouponSendModel>();
            string strSql = "select * from BM_CouponSend where UserId=@UserId order by id desc";
            var parms = new[] {
                new SqlParameter("@UserId",userId)
            };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, parms))
            {
                list = DbHelperSQLP.GetEntityList<CouponSendModel>(dr);
            }
            return list;
        }




        /// <summary>
        /// 添加优惠券发送记录
        /// </summary>
        /// <param name="userId">发送人ID</param>
        /// <param name="sendToUserId">接收用户ID,如果是自己转发，则为0</param>
        /// <param name="couponId">优惠券ID</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool AddSendCoupon(int userId, int sendToUserId, int couponId)
        {
            string strSql = "insert into BM_CouponSend(UserId,CouponId,Type,SendToUserId) values(@UserId,@CouponId,@Type,@SendToUserId)";
            var parm = new[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@CouponId", couponId),
                new SqlParameter("@Type", sendToUserId==0?"0":"1"),
                new SqlParameter("@SendToUserId", sendToUserId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }


        /// <summary>
        /// 更新优惠券分享状态(只有盟友身份时才操作)
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateCouponShareStatus(int userId, int couponId)
        {
            string strSql = "update BM_GetCashCouponLog set IsShare=1 where UserId=@UserId and CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@CouponId", couponId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }


        /// <summary>
        ///判断用户是否已转发
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if [is send coupon by user identifier] [the specified user identifier]; otherwise, false.</returns>
        public bool IsSendCouponByUserId(int userId, int couponId)
        {
            string strSql = "select COUNT(1) from BM_CouponSend where UserId=@UserId and CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@CouponId", couponId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm)) > 0;
        }


    }
}
