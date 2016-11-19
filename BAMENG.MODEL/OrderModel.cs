using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    public  class OrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ct_BelongId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ShopId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime orderTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Memo { get; set; }
    /// <summary>
    /// 
    /// </summary>
        public int OrderStatus { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string OrderImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SuccessImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal CashCouponAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CashCouponBn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FianlAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    public class OrderListModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public long id;

        /// <summary>
        /// 图片地址
        /// </summary>
        public string pictureUrl { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 盟豆
        /// </summary>
        public decimal money { get; set; }

        /// <summary>
        /// 订单状态 0 未成交 1 已成交 2退单
        /// </summary>
        public int status { get; set; }

        public string orderId { get; set; }
    }


    public class OrderDetailModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId;
        /// <summary>
        /// 下单时间
        /// </summary>
        public long orderTime;

        /// <summary>
        /// 图片地址
        /// </summary>
        public string pictureUrl { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string address { get; set; }

 
        /// <summary>
        /// 订单状态 0 未成交 1 已成交 2退单
        /// </summary>
        public int status { get; set; }

       
    }
}
