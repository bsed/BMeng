/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAMENG.API.Base
{
    /// <summary>
    /// 重写JsonResult方法，出来json时间格式问题
    /// </summary>
    /// <seealso cref="System.Web.Mvc.JsonResult" />
    public class CustomJsonResult : JsonResult
    {
        /// <summary>
        /// 通过从 <see cref="T:System.Web.Mvc.ActionResult" /> 类继承的自定义类型，启用对操作方法结果的处理。
        /// </summary>
        /// <param name="context">执行结果时所处的上下文。</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (response.StatusCode == 200)
            {
                response.ContentType = "application/json";
                if (Data != null)
                {
                    var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };//这里使用自定义日期格式，默认是ISO8601格式        
                    response.Write(JsonConvert.SerializeObject(Data, timeConverter));
                }
            }
        }
    }
}