using BAMENG.CONFIG;
using BAMENG.MODEL;
using System;
using System.Collections.Generic;
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
        public ActionResult myList(int type,long lastId)
        {
            int userId = GetAuthUserId();
            var data = Order
            return Json(new ResultModel(ApiStatusCode.OK,data));
        }
        /// <summary>
        /// 订单详情 POST: order/details
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult details()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 创建订单 POST: order/create
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult create()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 修改订单 POST: order/update
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult update()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }


        /// <summary>
        /// 订单成交信息提交 POST: order/success
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult success()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 上传成交凭证 POST: order/UploadSuccessVoucher
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult UploadSuccessVoucher()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }


        /// <summary>
        /// 发送站内消息 POST: order/SendInstationMessage
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult SendInstationMessage()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 站内消息列表 POST: order/InstationMessageList
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult InstationMessageList()
        {
            return Json(new ResultModel(ApiStatusCode.OK));
        }

    }
}