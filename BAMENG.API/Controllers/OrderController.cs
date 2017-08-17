using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using HotCoreUtils.Uploader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAMENG.API.Controllers
{

    /// <summary>
    /// 订单接口
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class OrderController : BaseController
    {
        /// <summary>
        /// 我的订单列表 POST: order/myList
        /// </summary>
        /// <param name="type">-1全部 0 未成交 1 已成交 2退单</param>
        /// <param name="lastId">最后的时间</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult myList(int type, long lastId)
        {
            try
            {
                var user = GetUserData();
                int userId = user.UserId;
                var data = OrderLogic.GetMyOrderList(userId, type, lastId, user.UserIdentity);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["list"] = data;
                return Json(new ResultModel(ApiStatusCode.OK, dict));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("myList:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 订单详情 POST: order/details
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult details(string id)
        {
            try
            {
                var data = OrderLogic.getOrderDetail(id);
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("details:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        ///  创建订单 POST: order/create
        /// </summary>
        /// <param name="userName">客户名</param>
        /// <param name="mobile">手机号</param>
        /// <param name="address">客户地址</param>
        /// <param name="cashNo">现金卷编号</param>
        /// <param name="memo">备注</param>
        /// <param name="cid">客户ID</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult create(string userName, string mobile, string address, string cashNo, string memo, int cid=0)
        {

            try
            {

                if (string.IsNullOrEmpty(userName))
                    return Json(new ResultModel(ApiStatusCode.姓名不能为空));
                if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsValidMobileNo(mobile))
                    return Json(new ResultModel(ApiStatusCode.无效手机号));
                if (string.IsNullOrEmpty(address))
                    return Json(new ResultModel(ApiStatusCode.地址不能为空));

                var user = GetUserData();
                int userId = user.UserId;

                //string imgContent = string.Empty;
                HttpPostedFileBase oFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (oFile == null)
                {
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
                }
                string fileName = GetUploadImagePath();
                Stream stream = oFile.InputStream;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                if (FileUploadHelper.UploadFile(bytes, fileName))
                {
                    ApiStatusCode apiCode = ApiStatusCode.OK;
                    bool flag = OrderLogic.saveOrder(userId, user.ShopId, userName, mobile, address, cashNo, memo, fileName, cid, ref apiCode);
                    if (!flag)
                    {
                        System.IO.File.Delete(Server.MapPath(fileName));
                        if (apiCode == ApiStatusCode.OK)
                            return Json(new ResultModel(apiCode, "订单创建成功"));
                        else
                            return Json(new ResultModel(apiCode));
                    }
                    else
                        return Json(new ResultModel(apiCode));
                }
                else
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("create_order:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 修改订单 POST: order/update
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="status">状态  0 未成交 1 已成交 2退单</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult update(string orderId, int status)
        {
            try
            {
                int userId = GetAuthUserId();
                ApiStatusCode code = ApiStatusCode.OK;
                OrderLogic.Update(userId, orderId, status, ref code);
                if (code == ApiStatusCode.OK)
                    return Json(new ResultModel(code, "保存成功"));
                else
                    return Json(new ResultModel(code));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("update order:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 上传成交凭证 POST: order/UploadSuccessVoucher
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult UploadSuccessVoucher(string orderId
            , string customer, string mobile, decimal price, string memo)
        {
            try
            {
                OrderModel orderModel = OrderLogic.GetModel(orderId);
                if (orderModel.OrderStatus != 0)
                {
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
                }


                string imgContent = string.Empty;
                HttpPostedFileBase oFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (oFile == null)
                {
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
                }
                string fileName = GetUploadImagePath();
                Stream stream = oFile.InputStream;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                if (FileUploadHelper.UploadFile(bytes, fileName))
                {

                    OrderLogic.UploadVoucher(orderId, customer, mobile, price, memo, fileName);
                    return Json(new ResultModel(ApiStatusCode.OK));
                }
                else
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("UploadSuccessVoucher order:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 客户订单列表 POST: order/customerOrderList
        /// </summary>
        /// <param name="type">-1全部 0 未成交 1 已成交 2退单</param>
        /// <param name="lastId">最后的时间</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult customerOrderList(int type, long lastId)
        {
            try
            {
                int userId = GetAuthUserId();
                var data = OrderLogic.GetUserOrderList(userId, type, lastId);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["list"] = data;
                return Json(new ResultModel(ApiStatusCode.OK, dict));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("customerOrderList order:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }





    }
}