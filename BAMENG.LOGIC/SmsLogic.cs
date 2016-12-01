/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.CONFIG;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BAMENG.LOGIC
{
    /// <summary>
    /// Class SmsLogic.
    /// </summary>
    public class SmsLogic
    {
        /// <summary>
        /// 判断验证码是否有效
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static bool IsPassVerify(string mobile, string verifyCode)
        {
            using (var dal = FactoryDispatcher.SmsFactory())
            {
                return dal.IsPassVerify(verifyCode, mobile);
            }
        }
        /// <summary>
        /// 设置验证码失效
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static bool UpdateVerifyCodeInvalid(string mobile, string verifyCode)
        {
            using (var dal = FactoryDispatcher.SmsFactory())
            {
                return dal.UpdateVerifyCodeInvalid(verifyCode, mobile);
            }
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="type">1普通短信  2语音短信</param>
        /// <param name="mobile"></param>        
        /// <param name="AppCode"></param>
        /// <returns></returns>
        public static bool SendSms(int type, string mobile, out ApiStatusCode AppCode)
        {
            int customerid = ConstConfig.storeId;
            try
            {
                using (var dal = FactoryDispatcher.SmsFactory())
                {
                    if (dal.TodaySendCount(mobile, customerid) > 3)
                    {
                        LogHelper.Log("超出当前发送次数", LogHelperTag.INFO, WebConfig.debugMode());
                        AppCode = ApiStatusCode.超出当天发送次数;
                        return false;
                    }

                    if (!dal.Sendable(mobile, customerid))
                    {
                        LogHelper.Log("操作太频繁,请稍后再试!", LogHelperTag.INFO, WebConfig.debugMode());
                        AppCode = ApiStatusCode.INVALID_OPTION_CODE;
                        return false;
                    }

                    string verificationNum = StringHelper.GetRandomNumber(100000, 999999).ToString();

                    while (dal.Exists(verificationNum))
                    {
                        verificationNum = StringHelper.GetRandomNumber(100000, 999999).ToString();
                    }
                    using (TransactionScope scope = new TransactionScope())
                    {
                        SwtSmsVerificationModel model = new SwtSmsVerificationModel();
                        model.SSV_Verification = verificationNum;
                        model.SSV_AddTime = DateTime.Now;
                        model.SSV_IsInvalid = 0;
                        model.SSV_Phone = mobile;
                        model.SSV_CustomerId = customerid;
                        string content = string.Empty;
                        string msg = "";
                        if (dal.Edit(model))
                        {
                            if (type == 1)
                                content = string.Format("您的验证码为：{0} ，有效期10分钟，祝您生活愉快!", verificationNum);
                            else
                                content = string.Format("您的验证码是{0}", verificationNum);
                            if (send(type, mobile, content, out msg))
                            {
                                AppCode = ApiStatusCode.OK;
                                LogHelper.Log(content, LogHelperTag.INFO, WebConfig.debugMode());
                                scope.Complete();
                                return true;
                            }
                            AppCode = ApiStatusCode.APP_SEND_CODE;
                            LogHelper.Log(string.Format("mobile:{0}, content:{1}, result:{2}", mobile, verificationNum, "发送失败 " + msg), LogHelperTag.INFO, WebConfig.debugMode());
                        }
                        else
                        {
                            AppCode = ApiStatusCode.APP_SEND_CODE;
                            LogHelper.Log(string.Format("mobile:{0}, content:{1}, result:{2}", mobile, verificationNum, "发送失败"), LogHelperTag.INFO, WebConfig.debugMode());
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppCode = ApiStatusCode.APP_SEND_CODE;
                LogHelper.Log(string.Format("mobile:{0}, Message:{1}, StackTrace:{2}", mobile, ex.Message, ex.StackTrace), LogHelperTag.ERROR);

            }
            return false;
        }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="type">1 普通，2语音</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="content">The content.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool send(int type, string mobile, string content, out string msg)
        {
            int code = 10000;
            msg = "";
            string url = WebConfig.SMSURL() + "?";
            if (type == 1)
                url += "account=" + WebConfig.SMSAcount() + "&pswd=" + WebConfig.SMSAcountPassword();
            else
            {
                url += "account=" + WebConfig.SMSVoiceAcount() + "&pswd=" + WebConfig.SMSVoiceAcountPassword();
            }
            url += "&mobile=" + mobile;
            url += "&msg=" + content;
            url += "&needstatus=true&extno=";
            bool result = false;
            try
            {
                string text = HttpPost(url, "", "get");
                code = Convert.ToInt32(text.Split('\n')[0].Split(',')[1]);
                switch (code)
                {
                    case 0:
                        result = true;
                        msg = "发送成功";
                        break;
                    case 101:
                        msg = "无此用户";
                        break;
                    case 102:
                        msg = "密码错误";
                        break;
                    case 103:
                        msg = "提交过快（提交速度超过流速限制）";
                        break;
                    case 104:
                        msg = "系统忙（因平台侧原因，暂时无法处理提交的短信）";
                        break;
                    case 105:
                        msg = "敏感短信（短信内容包含敏感词）";
                        break;
                    case 106:
                        msg = "消息长度错（>536或<=0）";
                        break;
                    case 107:
                        msg = "包含错误的手机号码";
                        break;
                    case 108:
                        msg = "手机号码个数错（群发>50000或<=0;单发>200或<=0）";
                        break;
                    case 109:
                        msg = "无发送额度（该用户可用短信数已使用完）";
                        break;
                    case 110:
                        msg = "不在发送时间内";
                        break;
                    case 111:
                        msg = "超出该账户当月发送额度限制";
                        break;
                    case 112:
                        msg = "无此产品，用户没有订购该产品";
                        break;
                    case 113:
                        msg = "extno格式错（非数字或者长度不对）";
                        break;

                    case 115:
                        msg = "自动审核驳回";
                        break;
                    case 116:
                        msg = "签名不合法，未带签名（用户必须带签名的前提下）";
                        break;
                    case 117:
                        msg = "IP地址认证错,请求调用的IP地址不是系统登记的IP地址";
                        break;
                    case 118:
                        msg = "用户没有相应的发送权限";
                        break;
                    case 119:
                        msg = "用户已过期";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("mobile:{0}, Message:{1}, StackTrace:{2}", mobile, ex.Message, ex.StackTrace), LogHelperTag.ERROR);
            }
            return result;
        }

        private static string HttpPost(string url, string body, string method, string contentType = "charset=utf-8")
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //设置请求消息头信息
            request.Method = method;
            request.ContentType = contentType;
            request.Timeout = 30 * 1000;
            //获取回应数据 接收方开始接受数据
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
