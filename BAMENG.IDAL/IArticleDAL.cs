/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{

    public interface IArticleDAL : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="AuthorIdentity">作者身份类型，0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultPageModel GetArticleList(int AuthorId, int AuthorIdentity, SearchModel model);



        /// <summary>
        /// 获取资讯列表--APP
        /// </summary>
        /// <param name="AuthorIdentity">作者身份类型，0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">用户身份，1盟主.0盟友</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetAppArticleList(int AuthorIdentity, int pageindex, int pageSize, int userId, int shopId, int userIdentity);



        /// <summary>
        /// 获取站内消息
        /// </summary>
        /// <param name="AuthorIdentity">类型 1盟主和盟友，2系统反馈消息</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPush">是否获取推送的消息</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetAppMailList(int AuthorIdentity, int pageindex, int pageSize, int userId, bool isPush, bool isAll = false);


        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetMailList(SearchModel model);


        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="mailId">The mail identifier.</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetReplyMailList(int mailId, int pageindex, int pageSize);


        /// <summary>
        /// 获取置顶资讯数据
        /// </summary>
        /// <param name="AuthorIdentity">The author identity.</param>
        /// <param name="userIdentity">用户身份，1盟主.0盟友</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>List&lt;ArticleBaseModel&gt;.</returns>
        List<ArticleBaseModel> GetAppTopArticleList(int AuthorIdentity, int userIdentity, int shopId);

        /// <summary>
        /// 获取资讯信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        ArticleModel GetModel(int articleId);

        /// <summary>
        /// 获取信息实体
        /// </summary>
        /// <param name="mailId">The mail identifier.</param>
        /// <returns>MailModel.</returns>
        MailModel GetMailModel(int mailId);


        /// <summary>
        /// 添加资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddArticle(ArticleModel model);

        /// <summary>
        /// 添加站内信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        int AddMailInfo(MailModel model);


        /// <summary>
        /// 修改资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateArticle(ArticleModel model);

        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        bool DeleteArticle(int articleId);

        /// <summary>
        /// 设置资讯置顶状态
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="enable">if set to true [enable].</param>
        /// <param name="useridentity">The useridentity.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool SetArticleEnableTop(int articleId, bool enable, int useridentity);

        /// <summary>
        /// 设置资讯发布状态
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        bool SetArticleEnablePublish(int articleId, bool enable);

        /// <summary>
        /// 设置审核状态
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="remark">备注</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SetArticleStatus(int articleId, int status, string remark);


        /// <summary>
        /// 更新资讯浏览量
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateArticleAmount(int articleId);

        /// <summary>
        /// 获取用户未读消息数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <returns>System.Int32.</returns>
        int GetNotReadMessageCount(int userId, int userIdentity);
    }
}
