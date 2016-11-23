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

        private const string SELECT_SQL = "select CouponId,Title,Money,StartTime,EndTime,IsEnable,CreateTime from BM_CashCoupon where IsDel=0 ";

        /// <summary>
        /// 添加现金券
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int AddCashCoupon(CashCouponModel model)
        {
            string strSql = "insert into BM_CashCoupon(ShopId,Title,Money,StartTime,EndTime,IsEnable) values(@ShopId,@Title,@Money,@StartTime,@EndTime,@IsEnable)";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Money", model.Money),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@IsEnable", model.IsEnable),
                new SqlParameter("@ShopId", model.ShopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
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
                    if (DateTime.Compare(item.EndTime, DateTime.Now) > 0)
                    {
                        if (item.IsEnable == 1)
                            item.StatusName = "启用";
                        else
                            item.StatusName = "未启用";
                    }
                    else
                        item.StatusName = "已过期";


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
            string strSql = "update BM_CashCoupon set Title=@Title,Money=@Money,StartTime=@StartTime,EndTime=@EndTime,IsEnable=@IsEnable where CouponId=@CouponId";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Money", model.Money),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@IsEnable", model.IsEnable),
                new SqlParameter("@CouponId", model.CouponId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }

        /// <summary>
        /// 修改现金券的获取状态
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="couponNo">The coupon no.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateGetStatus(int couponId, string couponNo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改现金券使用状态
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateUseStatus(int couponId)
        {
            throw new NotImplementedException();
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
        /// <param name="userId">The user identifier.</param>
        /// <param name="couponId">The coupon identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public int CreateUserCashCouponLog(int userId, int couponId)
        {
            string strSql = "insert into BM_GetCashCouponLog(UserId,CouponNo,CouponId) values(@UserId,@CouponId);select @@IDENTITY";
            var parm = new[] {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@CouponId", couponId)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
            if (obj != null)
                return Convert.ToInt32(obj);
            return 0;
        }

        /// <summary>
        /// 获取现金券领取列表
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public ResultPageModel GetUserCashCouponLogList(int couponId, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = "select L.ID,L.UserId,L.CouponNo,L.CouponId,L.Name,L.Mobile,L.IsGet,L.GetTime,L.IsUse,L.UseTime,L.CreateTime from BM_GetCashCouponLog L with(nolock) where 1=1 ";


            strSql += " and CouponId=@CouponId ";


            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),CreateTime,121)<=@endTime ";
            var param = new[] {
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime),
                new SqlParameter("@CouponId", couponId)
            };
            //生成sql语句
            return getPageData<CashCouponLogModel>(model.PageSize, model.PageIndex, strSql, "CreateTime", param);
        }


        /// <summary>
        /// 获取可用的优惠卷信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cashNo"></param>
        /// <returns></returns>
        public CashCouponLogModel getEnableCashCouponLogModel(string mobile, string cashNo)
        {

            CashCouponLogModel model = new CashCouponLogModel();
            string strSql = "select * from BM_GetCashCouponLog where CouponNo=@CouponNo and Mobile=@Mobile and IsGet=1 and IsDel=0 and IsUse=0";
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
        /// 更新现金券的领取记录
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateUserCashCouponGetLog(CashCouponLogModel model)
        {
            throw new NotImplementedException();
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
        /// 获得现金卷列表
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public List<CashCouponModel> getEnabledCashCouponList(int shopId)
        {
            List<CashCouponModel> list = new List<CashCouponModel>();
           string strSql = "select * from BM_CashCoupon where ShopId=@ShopId and StartTime<@Date and EndTime>@Date order by CouponId desc";
            var parms = new[] {
                new SqlParameter("@ShopId",shopId),
                new SqlParameter("@Date",DateTime.Now)
            };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, null))
            {
                list = DbHelperSQLP.GetEntityList<CashCouponModel>(dr);
            }
            return list;

        }

        /// <summary>
        /// 获得优惠卷发送列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CouponSendModel> getCouponSendList(int userId)
        {
            List<CouponSendModel> list = new List<CouponSendModel>();
            string strSql = "select * from BM_CouponSend where UserId=" + userId + " order by id desc";
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, null))
            {
                list = DbHelperSQLP.GetEntityList<CouponSendModel>(dr);
            }
            return list;
        }

    }
}
