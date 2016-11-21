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
//using System.Web.Http;
using System.Web.Mvc;

namespace BAMENG.API.Controllers
{
    /// <summary>
    /// 全局接口
    /// </summary>
    public class SysController : BaseController
    {


        /// <summary>
        /// 初始化接口 POST: sys/init
        /// </summary>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize(AuthLogin = false)]
        public ActionResult Init()
        {
            try
            {
                AppInitModel data = AppServiceLogic.Instance.Initialize(Version, OS);
                data.userData = GetUserData();
                if (data.userData != null)
                {
                    data.baseData.userStatus = data.userData.IsActive;                    
                }
                else
                    data.baseData.userStatus = -1;

                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("Init error--->StackTrace:{0} message:{1}", ex.StackTrace, ex.Message), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 检查更新 POST: sys/checkupdate
        /// </summary>
        /// <param name="clientVersion">客户的版本号</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{ AppVersionModel }}]]></returns>
        public ActionResult CheckUpdate(string clientVersion)
        {
            try
            {
                var versionData = AppServiceLogic.Instance.CheckUpdate(clientVersion, "android");

                return Json(new ResultModel(ApiStatusCode.OK, versionData));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("CheckUpdate error--->StackTrace:{0} message:{1}", ex.StackTrace, ex.Message), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 发送短信 POST: sys/sendsms
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="type">1普通短信  2语音短信</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        public ActionResult SendSms(string mobile, int type)
        {
            ApiStatusCode apiCode;
            SmsLogic.SendSms(type, mobile, out apiCode);
            return Json(new ResultModel(apiCode));
        }
        /// <summary>
        /// 焦点图片 POST: sys/focuspic
        /// </summary>
        /// <param name="type">0集团轮播图，2 首页轮播图</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:[{FocusPicModel},{FocusPicModel}]}]]></returns>
        [ActionAuthorize]
        public ActionResult FocusPic(int type)
        {
            var data = FocusPicLogic.GetAppList(type);
            return Json(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 上传图片 POST: sys/uploadpic
        /// </summary>
        /// <returns><![CDATA[{ picurl:"/resource/bameng/image/xxxxx.jpg" }]]></returns>
        [ActionAuthorize]
        public ActionResult UploadPic()
        {
            string imgContent = string.Empty;
            HttpPostedFileBase oFile = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (oFile == null)
            {
                return Json(new ResultModel(ApiStatusCode.请上传图片));
            }
            string fileName = "/resource/bameng/image/" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(6) + ".jpg";
            Stream stream = oFile.InputStream;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            if (FileUploadHelper.UploadFile(bytes, fileName))
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["picurl"] = fileName;

                return Json(new ResultModel(ApiStatusCode.OK, dict));
            }
            else
                return Json(new ResultModel(ApiStatusCode.请上传图片));
        }



        /// <summary>
        /// 我的位置
        /// </summary>
        /// <param name="mylocation">我的位置</param>
        /// <param name="lnglat">经纬度,精度和纬度，用英文逗号隔开</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult MyLocation(string mylocation, string lnglat)
        {
            SystemLogic.AddMyLocation(GetAuthUserId(), mylocation, lnglat);
            return Json(new ResultModel(ApiStatusCode.OK));
        }


    }
}