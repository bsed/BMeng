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
            ResultPageModel data = ArticleLogic.GetAppArticleList(identity, pageIndex, pageSize, GetAuthUserId());

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["list"] = data;
            if (pageIndex == 1 && (identity == 0 || identity == 1 || identity == 2))
                dict["top"] = ArticleLogic.GetAppTopArticleList(identity);
            return Json(new ResultModel(ApiStatusCode.OK, dict));
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
            var user = GetUserData();
            ArticleModel model = new ArticleModel();
            model.ArticleBody = content;
            model.ArticleIntro = content;
            model.ArticleTitle = title;
            model.ArticleCover = "";
            model.ArticleStatus = 1;
            model.AuthorId = user.UserId;
            model.AuthorIdentity = user.UserIdentity == 0 ? 4 : 3;
            model.AuthorName = user.RealName;
            model.EnablePublish = 1;
            model.EnableTop = 0;
            model.PublishTime = DateTime.Now;
            model.TopTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            //如果当前创建资讯的用户身份为盟友，则发送目标为盟主的ID
            //如果当前创建资讯的用户身份为盟主时，则发送目标为 2（盟友）
            model.SendTargetId = user.UserIdentity == 1 ? 2 : user.BelongOne;

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

            int articleId = ArticleLogic.AddArticle(model);
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
                        LogLogic.AddReadLog(logModel);
                    }
                }
                else
                {
                    logModel.UserId = user.BelongOne;
                    LogLogic.AddReadLog(logModel);
                }
            }
            else
                apiCode = ApiStatusCode.发送失败;
            return Json(new ResultModel(apiCode));
        }
    }
}