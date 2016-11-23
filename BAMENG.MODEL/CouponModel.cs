/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    /// <summary>
    /// 现金券实体
    /// </summary>
    public class CashCouponModel
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value>The coupon identifier.</value>
        public int CouponId { get; set; }



        /// <summary>
        /// 门店
        /// </summary>
        /// <value>The shop identifier.</value>
        public int ShopId { get; set; }

        /// <summary>
        /// 现金券标题
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }



        /// <summary>
        /// 现金券额度
        /// </summary>
        /// <value>The money.</value>
        public decimal Money { get; set; }



        /// <summary>
        /// 开始时间
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }



        /// <summary>
        /// 过期时间
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }



        /// <summary>
        /// 是否启用
        /// </summary>
        /// <value>The is enable.</value>
        public int IsEnable { get; set; }



        /// <summary>
        /// 状态名称
        /// </summary>
        /// <value>The name of the status.</value>
        public string StatusName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
    }



    /// <summary>
    /// 现金券领取记录实体
    /// </summary>
    public class CashCouponLogModel
    {
        /// <summary>
        /// 现金券码ID 
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }


        /// <summary>
        /// 现金券码
        /// </summary>
        /// <value>The coupon no.</value>
        public int CouponNo { get; set; }


        /// <summary>
        /// 现金券规则ID
        /// </summary>
        /// <value>The coupon identifier.</value>
        public int CouponId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// 客户手机号码
        /// </summary>
        /// <value>The mobile.</value>
        public int Mobile { get; set; }

        /// <summary>
        /// 是否获取
        /// </summary>
        /// <value>The is get.</value>
        public int IsGet { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <value>The get time.</value>
        public int GetTime { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        /// <value>The is use.</value>
        public int IsUse { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        /// <value>The use time.</value>
        public DateTime UseTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 现金券额度
        /// </summary>
        /// <value>The money.</value>
        public decimal Money { get; set; }



        /// <summary>
        /// 开始时间
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }



        /// <summary>
        /// 过期时间
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }
    }


    public class MyCouponListModel
    {
        /// <summary>
        /// 现金优惠券Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal money { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public string due { get; set; }
    }
}
