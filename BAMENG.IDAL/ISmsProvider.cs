/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    public interface ISmsProvider : IDisposable
    {
        /// <summary>
        /// 判断生成的验证码是否已经存在
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        bool Exists(string verifyCode);
        /// <summary>
        /// 新增或者修改短信验证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Edit(SwtSmsVerificationModel model);

        /// <summary>
        /// 是否可以发送，60秒之后可以发送
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="customerid"></param>
        /// <param name="difSeconds"></param>
        /// <returns></returns>
        bool Sendable(string phone, int customerid, int difSeconds = 60);

        /// <summary>
        /// 获取当前发送次数
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <param name="customerid">The customerid.</param>
        /// <returns>System.Int32.</returns>
        int TodaySendCount(string phone, int customerid);


        /// <summary>
        /// 判断是否通过验证
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        bool IsPassVerify(string verifyCode, string phone);

        /// <summary>
        /// 修改验证码失效
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool UpdateVerifyCodeInvalid(string verifyCode, string phone);
    }
}
