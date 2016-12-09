using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BAMENG.API.Controllers
{
    /// <summary>
    /// 客户接口
    /// </summary>
    public class CustomerController : BaseController
    {
        /// <summary>
        /// 客户列表 POST: customer/list
        /// </summary>
        /// <param name="type">0所有客户 1未处理  2已处理</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult list(int type, int pageIndex, int pageSize)
        {
            UserModel user = GetUserData();
            if (user != null)
            {
                var data = CustomerLogic.GetAppCustomerList(user.UserId, user.UserIdentity, type, pageIndex, pageSize);
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            return Json(new ResultModel(ApiStatusCode.令牌失效));
        }
        /// <summary>
        /// 审核 POST: customer/audit
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <param name="status">审核状态 1已同意  2已拒绝</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult audit(int cid, int status)
        {
            var user = GetUserData();
            if (CustomerLogic.UpdateStatus(cid, status, user.UserId))
            {
                //添加客户操作日志
                LogLogic.AddCustomerLog(new LogBaseModel()
                {
                    objId = cid,
                    UserId = user.UserId,
                    ShopId = user.ShopId,
                    AppSystem = OS,
                    OperationType = status
                });

                return Json(new ResultModel(ApiStatusCode.OK));
            }
            else
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
        }
        /// <summary>
        /// 创建客户 POST: customer/create
        /// </summary>
        /// <param name="username">客户姓名</param>
        /// <param name="mobile">客户手机</param>
        /// <param name="address">客户地址</param>
        /// <param name="remark">客户备注</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult create(string username, string mobile, string address, string remark)
        {
            var user = GetUserData();
            //信息以电话和客户地址做为唯一性的判断标准,判断客户所属
            if (!CustomerLogic.IsExist(mobile, address))
            {
                int flag = CustomerLogic.InsertCustomerInfo(new CustomerModel()
                {
                    BelongOne = user.UserId,
                    BelongTwo = user.UserIdentity == 1 ? user.UserId : user.BelongOne,
                    Addr = address,
                    Mobile = mobile,
                    ShopId = user.ShopId,
                    Name = username,
                    Remark = remark,
                    Status = user.UserIdentity == 1 ? 1 : 0
                });

                if (flag > 0)
                {

                    //添加客户操作日志
                    LogLogic.AddCustomerLog(new LogBaseModel()
                    {
                        objId = flag,
                        UserId = user.UserId,
                        ShopId = user.ShopId,
                        AppSystem = OS,
                        OperationType = 0 //操作类型0提交，1有效 2无效
                    });

                    if (user.UserIdentity == 1)
                    {
                        UserLogic.AddUserCustomerAmount(user.UserId);
                        //如果是盟主创建客户，则需要添加一条有效客户日志
                        //添加客户操作日志
                        LogLogic.AddCustomerLog(new LogBaseModel()
                        {
                            objId = flag,
                            UserId = user.UserId,
                            ShopId = user.ShopId,
                            AppSystem = OS,
                            OperationType = 1
                        });
                    }
                }
                return Json(new ResultModel(ApiStatusCode.OK));
            }
            else
                return Json(new ResultModel(ApiStatusCode.客户已存在));
        }

        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult details(int cid)
        {
            var data = CustomerLogic.GetModel(cid);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 更新客户进店状态
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <param name="status">1进店 0未进店</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult UpdateInShop(int cid, int status)
        {
            if (CustomerLogic.UpdateInShopStatus(cid, status))
                return Json(new ResultModel(ApiStatusCode.OK, "保存成功"));
            else
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
        }
    }
}