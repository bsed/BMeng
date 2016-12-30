using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAMENG.API.Controllers
{
    /// <summary>
    /// 资讯接口
    /// </summary>
    public class ArticleController : BaseController
    {

        /// <summary>
        /// 资讯列表  article/list
        /// </summary>
        /// <param name="identity">资讯身份 0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ActionResult.</returns>
        /// <example>
        ///   <code>
        /// {
        /// status:200,
        /// statusText:"OK",
        /// data:{
        /// list:{
        /// ResultPageModel
        /// },
        /// top:[
        /// {
        /// ArticleBaseModel
        /// }]
        /// }
        /// }
        /// </code>
        /// </example>
        [ActionAuthorize]
        public ActionResult list(int identity, int pageIndex, int pageSize)
        {
            try
            {
                pageIndex = pageIndex > 0 ? pageIndex : 1;

                var userInfo = GetUserData();


                //当前用户所属门店ID
                int shopId = userInfo.ShopId;
                //如果当前用户所属分店，且当前其他类型为总店，那么shopid=当前用户门店所属的总店ID
                if (userInfo.ShopType == 2 && identity == 1)
                    shopId = userInfo.ShopBelongId;

                if (identity == 0)
                    shopId = 0;
                ResultPageModel data = null;// ArticleLogic.GetAppArticleList(identity, pageIndex, pageSize, userInfo.UserId, shopId, userInfo.UserIdentity);

                if (identity != 3 && identity != 4)
                    data = ArticleLogic.GetAppArticleList(identity, pageIndex, pageSize, userInfo.UserId, shopId, userInfo.UserIdentity);
                else
                    data = ArticleLogic.GetAppMailList(1, pageIndex, pageSize, userInfo.UserId, true, true);

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["list"] = data;
                if (pageIndex == 1 && (identity == 0 || identity == 1 || identity == 2))
                    dict["top"] = ArticleLogic.GetAppTopArticleList(identity, userInfo.UserIdentity, shopId);
                return Json(new ResultModel(ApiStatusCode.OK, dict));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("list:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 获取站内信列表     article/maillist
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="type">1 消息，2反馈</param>
        /// <param name="isSend">1 发送消息，0接收消息</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult maillist(int pageIndex, int pageSize, int type, int isSend = 1)
        {
            try
            {
                pageIndex = pageIndex > 0 ? pageIndex : 1;

                var userInfo = GetUserData();
                ResultPageModel data = null;
                bool isPush = isSend == 1;
                data = ArticleLogic.GetAppMailList(type, pageIndex, pageSize, userInfo.UserId, isPush);

                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("list:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 创建资讯
        /// </summary>
        /// <param name="title">资讯标题</param>
        /// <param name="content">资讯内容</param>
        /// <param name="ids">发送对象,格式如:1|2|3</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult create(string title, string content, string ids)
        {
            try
            {
                var user = GetUserData();
                //ArticleModel model = new ArticleModel();
                //model.ArticleBody = content;
                //model.ArticleIntro = title;
                //model.ArticleTitle = title;
                //model.ArticleCover = "";
                //model.ArticleStatus = 1;
                //model.AuthorId = user.UserId;
                //model.AuthorIdentity = user.UserIdentity == 0 ? 4 : 3;
                //model.AuthorName = user.RealName;
                //model.EnablePublish = 1;
                //model.EnableTop = 0;
                //model.PublishTime = DateTime.Now;
                //model.TopTime = DateTime.Now;
                //model.UpdateTime = DateTime.Now;
                ////如果当前创建资讯的用户身份为盟友，则发送目标为盟主的ID
                ////如果当前创建资讯的用户身份为盟主时，则发送目标为 2（盟友）
                //model.SendTargetId = user.UserIdentity == 1 ? 2 : user.BelongOne;

                string[] TargetIds = null;

                //如果是盟主身份，则需要判断发送目标
                if (user.UserIdentity == 1)
                {
                    if (string.IsNullOrEmpty(ids))
                        return Json(new ResultModel(ApiStatusCode.缺少发送目标));

                    TargetIds = ids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (TargetIds.Length <= 0)
                        return Json(new ResultModel(ApiStatusCode.缺少发送目标));
                }


                MailModel model = new MailModel();
                model.AuthorId = user.UserId;
                model.AuthorName = user.NickName;
                model.Title = title;
                model.BodyContent = content;
                model.CoverUrl = user.UserHeadImg;
                model.SendType = ids == "-1" ? 2 : user.UserIdentity == 1 ? 0 : 1;
                model.ReplyPid = 0;
                model.ReplyUserId = 0;

                int articleId = ArticleLogic.AddMailInfo(model);
                ApiStatusCode apiCode = ApiStatusCode.OK;
                if (articleId > 0)
                {
                    ReadLogModel logModel = new ReadLogModel()
                    {
                        ArticleId = articleId,
                        ClientIp = "",
                        cookie = "",
                        IsRead = 0,
                        ReadTime = DateTime.Now
                    };
                    if (user.UserIdentity == 1)
                    {
                        foreach (var TargetId in TargetIds)
                        {
                            logModel.UserId = Convert.ToInt32(TargetId);
                            //LogLogic.AddReadLog(logModel);
                            LogLogic.AddMailReadLog(logModel);
                        }
                    }
                    else
                    {
                        logModel.UserId = user.BelongOne;
                        LogLogic.AddMailReadLog(logModel);
                    }

                    //将自己也添加进去,这样自己就可以看到自己发布的信息
                    logModel.UserId = user.UserId;
                    logModel.IsRead = 1;
                    LogLogic.AddMailReadLog(logModel);
                }
                else
                    apiCode = ApiStatusCode.发送失败;
                return Json(new ResultModel(apiCode));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("create:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }
    }
}