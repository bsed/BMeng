/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2017. All rights reserved.
 * author guomw
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    public class ScoreConfigModel
    {
        /// <summary>
        /// 创建订单奖励
        /// </summary>
        public int CreateOrderScore { get; set; }

        /// <summary>
        /// 盟友提交客户信息并审核通过，盟友可获得积分
        /// </summary>
        public int SubmitCustomerToAllyScore { get; set; }

        /// <summary>
        /// 盟友提交客户信息并审核通过，盟主可获得积分
        /// </summary>
        public int SubmitCustomerToMainScore1 { get; set; }

        /// <summary>
        /// 盟主提交客户信息，盟主可获得积分
        /// </summary>
        public int SubmitCustomerToMainScore2 { get; set; }

        /// <summary>
        /// 盟主邀请盟友奖励积分
        /// </summary>
        public int InviteScore { get; set; }
    }
}
