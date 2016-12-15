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

namespace BAMENG.LOGIC
{
    public class ArticleLogic
    {
        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="AuthorIdentity">作者身份类型，0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultPageModel GetArticleList(int AuthorId, int AuthorIdentity, SearchModel model)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetArticleList(AuthorId, AuthorIdentity, model);
            }
        }

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="AuthorIdentity">作者身份类型，0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shopId">门店ID</param>
        /// <param name="userIdentity">用户身份，1盟主.0盟友</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetAppArticleList(int AuthorIdentity, int pageindex, int pageSize, int userId, int shopId, int userIdentity)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetAppArticleList(AuthorIdentity, pageindex, pageSize, userId, shopId, userIdentity);
            }
        }
        /// <summary>
        /// 获取置顶资讯数据
        /// </summary>
        /// <param name="AuthorIdentity">The author identity.</param>
        /// <param name="userIdentity">用户身份，1盟主.0盟友</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>List&lt;ArticleBaseModel&gt;.</returns>
        public static List<ArticleBaseModel> GetAppTopArticleList(int AuthorIdentity, int userIdentity, int shopId)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetAppTopArticleList(AuthorIdentity, userIdentity, shopId);
            }
        }

        /// <summary>
        /// 编辑资讯信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditArticle(ArticleModel model)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                if (model.ArticleId > 0)
                    return dal.UpdateArticle(model);
                else
                    return dal.AddArticle(model) > 0;
            }
        }

        public static int AddArticle(ArticleModel model)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.AddArticle(model);
            }
        }


        /// <summary>
        /// 设置资讯置顶状态
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static bool SetArticleEnableTop(int articleId, bool enable,int useridentity)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.SetArticleEnableTop(articleId, enable, useridentity);
            }
        }
        /// <summary>
        /// 设置资讯发布状态
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static bool SetArticleEnablePublish(int articleId, bool enable)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.SetArticleEnablePublish(articleId, enable);
            }
        }
        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public static bool DeleteArticle(int articleId)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.DeleteArticle(articleId);
            }
        }
        /// <summary>
        /// 获取资讯信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public static ArticleModel GetModel(int articleId)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetModel(articleId);
            }
        }

        /// <summary>
        /// 设置资讯审核状态
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="status">-1 审核失败 1审核成功</param>
        /// <param name="remark">The remark.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SetArticleStatus(int articleId, int status, string remark)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.SetArticleStatus(articleId, status, remark);
            }
        }


        /// <summary>
        /// 更新资讯浏览量
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateArticleAmount(int articleId)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.UpdateArticleAmount(articleId);
            }
        }

        /// <summary>
        /// 获取用户未读消息数量
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <returns>System.Int32.</returns>
        public static int GetNotReadMessageCount(int userId, int userIdentity)
        {
            using (var dal = FactoryDispatcher.ArticleFactory())
            {
                return dal.GetNotReadMessageCount(userId, userIdentity);
            }
        }
    }
}
