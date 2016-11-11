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
}
