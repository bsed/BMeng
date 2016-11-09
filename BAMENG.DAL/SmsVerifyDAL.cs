/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
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
    public class SmsVerifyDAL : AbstractDAL, ISmsProvider
    {
        public bool Exists(string verifyCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Swt_SmsVerification");
            strSql.Append(" where ");
            strSql.Append(" SSV_Verification = @SSV_Verification and SSV_IsInvalid=0");
            var parameters = new[]{
                    new SqlParameter("@SSV_Verification", verifyCode)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parameters)) > 0;
        }
        /// <summary>
        /// 新增或者修改短信验证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(SwtSmsVerificationModel model)
        {
            string sql = string.Format(@"if not exists(select * from Swt_SmsVerification where SSV_Phone='{0}' AND SSV_CustomerId={3})
                                        begin insert into Swt_SmsVerification (SSV_Verification,SSV_AddTime,SSV_IsInvalid,SSV_Phone,SSV_CustomerId) 
                                        values('{1}',getdate(),{2},'{0}',{3}) end
                                        else begin update Swt_SmsVerification set SSV_Verification='{1}',SSV_AddTime=getdate(),
                                        SSV_IsInvalid={2} where SSV_Phone='{0}' AND SSV_CustomerId={3} end",
                                       model.SSV_Phone,
                                       model.SSV_Verification,
                                       model.SSV_IsInvalid,
                                       model.SSV_CustomerId);

            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 是否可以发送，60秒之后可以发送
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="customerid"></param>
        /// <param name="difSeconds"></param>
        /// <returns></returns>
        public bool Sendable(string phone, int customerid, int difSeconds = 60)
        {
            string sql = string.Format("SELECT DATEDIFF(SECOND,SSV_AddTime,GETDATE()) AS differ FROM Swt_SmsVerification WHERE SSV_Phone='{0}' AND SSV_CustomerId={1}", phone, customerid);

            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, sql);
            if (obj == null)
            {
                return true;
            }
            else
            {
                long differ = Convert.ToInt64(obj);
                if (differ > difSeconds) return true;
                else return false;
            }
        }
        /// <summary>
        /// 判断是否通过验证
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public bool IsPassVerify(string verifyCode, string phone)
        {
            string strSql = "select count(1) from Swt_SmsVerification where SSV_Verification=@SSV_Verification and SSV_IsInvalid=0 and SSV_Phone=@SSV_Phone and DATEDIFF(SECOND,SSV_AddTime,GETDATE()) < 600 ";
            var parameters = new[]{
                    new SqlParameter("@SSV_Verification", verifyCode),
                    new SqlParameter("@SSV_Phone", phone)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, parameters)) > 0;
        }


        /// <summary>
        /// 修改验证码失效
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateVerifyCodeInvalid(string verifyCode, string phone)
        {
            string strSql = "update Swt_SmsVerification set SSV_IsInvalid=1 where SSV_Verification=@SSV_Verification and SSV_Phone=@SSV_Phone";
            var parameters = new[]{
                    new SqlParameter("@SSV_Verification", verifyCode),
                    new SqlParameter("@SSV_Phone", phone)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, parameters) > 0;
        }
    }
}
