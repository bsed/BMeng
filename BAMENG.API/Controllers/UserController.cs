using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using HotCoreUtils.Uploader;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BAMENG.API.Controllers
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    public class UserController : BaseController
    {

        /// <summary>
        /// 登陆接口 POST:  user/login
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize(AuthLogin = false)]
        public ActionResult Login(string loginName, string password)
        {
            ApiStatusCode apiCode = ApiStatusCode.OK;
            UserModel userData = AppServiceLogic.Instance.Login(loginName, password, ref apiCode);
            return Json(new ResultModel(apiCode, userData));
        }
        /// <summary>
        /// 签到  POST: user/signin
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult SignIn()
        {
            ApiStatusCode apiCode = ApiStatusCode.OK;
            UserLogic.SignIn(GetAuthUserId(), ref apiCode);
            return Json(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 忘记密码   POST: user/forgetpwd
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="password">The password.</param>
        /// <param name="verifyCode">The verify code.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize(AuthLogin = false)]
        public ActionResult ForgetPwd(string mobile, string password, string verifyCode)
        {
            if (SmsLogic.IsPassVerify(mobile, verifyCode))
            {
                if (UserLogic.ForgetPwd(mobile, password))
                    return Json(new ResultModel(ApiStatusCode.OK));
                else
                    return Json(new ResultModel(ApiStatusCode.找回密码失败));
            }
            return Json(new ResultModel(ApiStatusCode.无效验证码));
        }

        /// <summary>
        /// 待结算盟豆列表 POST: user/tempsettlebeanlist
        /// </summary>
        /// <returns><![CDATA[{ status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult TempSettleBeanList(int lastId)
        {
            int userId = GetAuthUserId();
            var data = UserLogic.getTempBeansRecordsList(userId, lastId);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 兑换盟豆 POST: user/ConvertToBean
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult ConvertToBean(int amount)
        {
            int userId = GetAuthUserId();
            ApiStatusCode code = ApiStatusCode.OK;
            UserLogic.ConvertToBean(userId, amount, ref code);
            return Json(new ResultModel(code));
        }


        /// <summary>
        /// 兑换记录流水 POST: user/ConvertFlow
        /// </summary>
        /// <param name="lastId"></param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult ConvertFlow(int lastId)
        {
            int userId = GetAuthUserId();
            var data = UserLogic.getConvertFlow(userId, lastId);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 盟友列表 POST: user/allylist
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public JsonResult allylist(int pageIndex, int pageSize)
        {
            var data = UserLogic.GetAllyList(new SearchModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                UserId = GetAuthUserId()
            });
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 兑换审核列表 POST: user/ConvertAuditList
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult ConvertAuditList(int lastId)
        {
            int userId = GetAuthUserId();
            var data = UserLogic.getMasterConvertFlow(userId, lastId);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 兑换审核 POST: user/ConvertAudit
        /// </summary>
        /// <param name="id">兑换转换记录Id</param>
        /// <param name="status">1同意2拒绝</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult ConvertAudit(int id, int status)
        {
            ApiStatusCode code = ApiStatusCode.OK;
            int userId = GetAuthUserId();
            UserLogic.ConvertAudit(userId, id, status, ref code);
            return Json(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 个人信息 POST: user/myinfo
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult MyInfo()
        {
            var data = GetUserData();
            if (data != null)
                data.UserHeadImg = WebConfig.reswebsite() + data.UserHeadImg;
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 修改用户信息 POST: user/UpdateInfo
        /// </summary>
        /// <param name="type">修改内容类型</param>
        /// <param name="content">修改内容</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult UpdateInfo(int type, string content)
        {
            UserModel userInfo = new UserModel();
            userInfo.UserId = GetAuthUserId();
            switch (type)
            {
                case (int)UserPropertyOptions.USER_1:
                    {
                        HttpPostedFileBase oFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                        if (oFile == null)
                            return Json(new ResultModel(ApiStatusCode.请上传图片));
                        string fileName = "/resource/bameng/image/" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(6) + ".jpg";
                        Stream stream = oFile.InputStream;
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        // 设置当前流的位置为流的开始
                        stream.Seek(0, SeekOrigin.Begin);
                        if (FileUploadHelper.UploadFile(bytes, fileName))
                            userInfo.UserHeadImg = fileName;
                        else
                            return Json(new ResultModel(ApiStatusCode.请上传图片));
                    }
                    break;
                case (int)UserPropertyOptions.USER_2:
                    userInfo.NickName = content;
                    break;
                case (int)UserPropertyOptions.USER_3:
                    {

                        userInfo.UserMobile = content;
                    }
                    break;
                case (int)UserPropertyOptions.USER_4:
                    userInfo.RealName = content;
                    break;
                case (int)UserPropertyOptions.USER_5:
                    userInfo.UserGender = content;
                    break;
                case (int)UserPropertyOptions.USER_6:
                    userInfo.UserCity = content;
                    break;
            }
            UserPropertyOptions opt;
            bool flg = Enum.TryParse(type.ToString(), out opt);

            UserLogic.UpdateUserInfo(opt, userInfo);
            return Json(new ResultModel(ApiStatusCode.OK));


        }

        /// <summary>
        /// 设置盟友奖励 POST: user/setallyraward
        /// </summary>
        /// <param name="creward">客户资料提交奖励</param>
        /// <param name="orderreward">订单成交奖励</param>
        /// <param name="shopreward">客户进店奖励</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult setallyRaward(decimal creward, decimal orderreward, decimal shopreward)
        {
            var user = GetUserData();
            if (user.UserIdentity == 1)
            {
                bool flag = UserLogic.SetAllyRaward(user.UserId, creward, orderreward, shopreward);
                return Json(new ResultModel(flag ? ApiStatusCode.OK : ApiStatusCode.保存失败));
            }
            else
                return Json(new ResultModel(ApiStatusCode.无操作权限));
        }

        /// <summary>
        /// 获取盟主的盟友奖励设置
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult GetAllyReward()
        {
            var data = UserLogic.GetRewardModel(GetAuthUserId());
            return Json(new ResultModel(ApiStatusCode.OK, data));

        }


        /// <summary>
        /// 积分列表 POST: user/scoreList
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult scoreList(int lastId)
        {
            int userId = GetAuthUserId();
            var data = UserLogic.getScoreList(userId, lastId);
            return Json(new ResultModel(ApiStatusCode.OK));
        }


        /// <summary>
        /// 盟豆流水列表 POST: user/BeanFlowList
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult BeanFlowList(int lastId)
        {
            int userId = GetAuthUserId();
            var data = UserLogic.getBeansRecordsList(userId, lastId);


            return Json(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 盟友详情 POST: user/AllyInfo
        /// </summary>
        /// <param name="userid">盟友用户ID</param>
        /// <returns>JsonResult.</returns>
        [ActionAuthorize]
        public JsonResult AllyInfo(int userid)
        {
            var data = UserLogic.GetModel(userid);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 申请盟友接口 POST: user/AllyApply
        /// todo 
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult AllyApply(int userId, string mobile, string password
            , string nickname, string userName
            , int sex)
        {
            ApiStatusCode apiCode = ApiStatusCode.OK;
            UserLogic.AllyApply(userId, mobile, password, nickname, userName, sex, ref apiCode);
            return Json(new ResultModel(apiCode));
        }
        /// <summary>
        /// 盟友申请列表 POST: user/AllyApplylist
        /// </summary>
        /// <param name="type">0盟友申请列表，1我的盟友列表</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult AllyApplylist(int type, int pageIndex, int pageSize)
        {
            int userId = GetAuthUserId();
            if (type == 1)
            {
                var data = UserLogic.GetAllyList(new SearchModel()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    UserId = userId
                });
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            else
            {

                var data = UserLogic.GetApplyFriendList(new SearchModel()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    UserId = userId
                });
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
        }
        /// <summary>
        /// 盟友申请审核 POST: user/AllyApplyAudit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">1成功2拒绝</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult ApllyApplyAudit(int id, int status)
        {
            ApiStatusCode code = ApiStatusCode.OK;
            int userId = GetAuthUserId();
            UserLogic.AllyApplyAudit(userId, id, status, ref code);
            return Json(new ResultModel(ApiStatusCode.OK, code));
        }
        /// <summary>
        /// 修改密码 POST: user/ChanagePassword
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult ChanagePassword(string oldPassword, string newPassword)
        {
            var user = GetUserData();
            if (UserLogic.ChanagePassword(user.UserId, oldPassword, newPassword))
                return Json(new ResultModel(ApiStatusCode.OK));
            else
                return Json(new ResultModel(ApiStatusCode.密码修改失败));

        }

        /// <summary>
        /// 修改手机号 POST: user/ChanageMobile
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="verifyCode">The verify code.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult ChanageMobile(string mobile, string verifyCode)
        {
            if (SmsLogic.IsPassVerify(mobile, verifyCode))
            {
                SmsLogic.UpdateVerifyCodeInvalid(mobile, verifyCode);
                UserModel userInfo = new UserModel();
                userInfo.UserId = GetAuthUserId();
                userInfo.UserMobile = mobile;
                if (UserLogic.UpdateUserInfo(UserPropertyOptions.USER_3, userInfo))
                    return Json(new ResultModel(ApiStatusCode.OK));
            }
            return Json(new ResultModel(ApiStatusCode.无效验证码));
        }

        /// <summary>
        /// 我的现金券列表 POST: user/MyCashCouponList
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult MyCashCouponList()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 我的业务 POST: user/MyBusiness         
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult MyBusiness()
        {
            var user = GetUserData();
            var data = UserLogic.MyBusinessAmount(user.UserId, user.UserIdentity);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 我的财富 POST: user/MyTreasure
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult MyTreasure()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 盟友首页汇总 POST: user/AllyHomeSummary
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult AllyHomeSummary()
        {
            return Json(new ResultModel(ApiStatusCode.OK));

        }
    }
}
