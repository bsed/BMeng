/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMENG.MODEL;
using System.Data.SqlClient;
using BAMENG.CONFIG;
using HotCoreUtils.DB;
using System.Data;
using HotCoreUtils.Helper;

namespace BAMENG.DAL
{
    public class ArticleDAL : AbstractDAL, IArticleDAL
    {

        private const string APP_SELECT = @"select A.ArticleId,A.AuthorId,a.AuthorName,a.AuthorIdentity,a.SendTargetId,a.SendType,a.ArticleSort,a.ArticleType,a.ArticleClassify
                                            ,a.ArticleTitle,a.ArticleIntro,a.ArticleCover,a.ArticleBody,a.EnableTop,a.EnablePublish,a.BrowseAmount,a.ArticleStatus,a.IsDel,a.IsRead,a.TopTime,a.UpdateTime,a.PublishTime,a.CreateTime,S.ShopName,s.ShopProv,s.ShopCity,A.Remark
                                             from BM_ArticleList A with(nolock)
                                            left join BM_ShopManage S with(nolock) on S.ShopID=AuthorId
                                            where a.IsDel=0 ";


        /// <summary>
        /// 添加资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddArticle(ArticleModel model)
        {
            string strSql = @"insert into BM_ArticleList(AuthorId,AuthorName,AuthorIdentity,SendTargetId,SendType,ArticleSort,ArticleType,ArticleClassify
                                ,ArticleTitle,ArticleIntro,ArticleCover,ArticleBody,EnableTop,EnablePublish,ArticleStatus,TopTime,PublishTime)
                                values(@AuthorId,@AuthorName,@AuthorIdentity,@SendTargetId,@SendType,@ArticleSort,@ArticleType,@ArticleClassify
                                ,@ArticleTitle,@ArticleIntro,@ArticleCover,@ArticleBody,@EnableTop,@EnablePublish,@ArticleStatus,@TopTime,@PublishTime);select @@IDENTITY";
            var param = new[] {
                        new SqlParameter("@AuthorId", model.AuthorId),
                        new SqlParameter("@AuthorName", model.AuthorName),
                        new SqlParameter("@AuthorIdentity", model.AuthorIdentity),
                        new SqlParameter("@SendTargetId", model.SendTargetId),
                        new SqlParameter("@SendType", model.SendType),
                        new SqlParameter("@ArticleSort", model.ArticleSort),
                        new SqlParameter("@ArticleType", model.ArticleType),
                        new SqlParameter("@ArticleClassify", model.ArticleClassify),
                        new SqlParameter("@ArticleTitle", model.ArticleTitle),
                        new SqlParameter("@ArticleIntro", model.ArticleIntro),
                        new SqlParameter("@ArticleCover", model.ArticleCover),
                        new SqlParameter("@ArticleBody",model.ArticleBody),
                        new SqlParameter("@EnableTop",model.EnableTop),
                        new SqlParameter("@EnablePublish",model.EnablePublish),
                        new SqlParameter("@ArticleStatus",model.ArticleStatus),
                        new SqlParameter("@TopTime",model.TopTime),
                        new SqlParameter("@PublishTime",model.PublishTime)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), param);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool DeleteArticle(int articleId)
        {
            string strSql = "update BM_ArticleList set IsDel=1  where ArticleId=@ArticleId";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="AuthorIdentity">作者身份类型，0集团，1总店，2分店  3盟主 4盟友</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultPageModel GetArticleList(int AuthorId, int AuthorIdentity, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = APP_SELECT;

            if (!string.IsNullOrEmpty(model.key))
            {
                switch (model.searchType)
                {
                    case (int)SearchType.标题:
                        strSql += string.Format(" and A.ArticleTitle like '%{0}%' ", model.key);
                        break;
                    default:
                        break;
                }
            }
            if (model.Status != -100)
            {
                /* 总后台下
                 * 资讯列表只获取总后台发布的资讯
                 * Status=1标题当前在资讯列表,否则在审核列表
                 * 审核列表的数据，一般只有总店和分店提交的资讯数据，没有总店添加的资讯数据
                 * 门店后台下
                 * 资讯列表只获取当前门店发布的资讯，包含审核通过、申请中、审核失败的资讯
                 * 
                */

                //AuthorIdentity 0集团，1总店，2分店  3盟主 4盟友
                if (model.Status == 1)
                {
                    //如果不是总后台身份
                    if (AuthorIdentity != 0)
                        strSql += " and A.AuthorIdentity=@AuthorIdentity  and A.AuthorId=@AuthorId ";
                    else
                        strSql += " and A.ArticleStatus=1  and A.AuthorIdentity=0 ";
                }
                else if (model.Status == 0)
                {
                    //这里只有总后台才有入口进来
                    if (AuthorIdentity == 0)
                    {
                        //TODO:过滤总后台发布的资讯数据，只获取总店和分店的数据
                        strSql += " and A.AuthorIdentity in (1,2) ";
                    }
                    else
                        strSql += " and A.AuthorIdentity=@AuthorIdentity and A.AuthorId=@AuthorId ";
                }
            }


            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),A.CreateTime,121)>=@startTime ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),A.CreateTime,121)<=@endTime ";
            var param = new[] {
                new SqlParameter("@startTime", model.startTime),
                new SqlParameter("@endTime", model.endTime),
                new SqlParameter("@AuthorIdentity", AuthorIdentity),
                new SqlParameter("@AuthorId",AuthorId)
            };
            //生成sql语句
            return getPageData<ArticleModel>(model.PageSize, model.PageIndex, strSql, "A.CreateTime", false, param);
        }


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
        public ResultPageModel GetAppArticleList(int AuthorIdentity, int pageindex, int pageSize, int userId, int shopId, int userIdentity)
        {
            ResultPageModel result = new ResultPageModel();
            string strSql = @"select A.ArticleId,a.ArticleTitle,a.ArticleIntro,a.ArticleCover,a.BrowseAmount,a.PublishTime {1}
                                 from BM_ArticleList A with(nolock)
                                 {0}
                                 where a.IsDel=0 and a.EnableTop=0  and a.EnablePublish=1 and A.ArticleStatus=1";

            string whereSql = string.Empty, wherefield = string.Empty;
            string orderbyField = "A.PublishTime";

            if (AuthorIdentity == 3 || AuthorIdentity == 4)
            {

                wherefield = ",ISNULL(R.IsRead,0) as IsRead";
                whereSql = " left join BM_ReadLog R with(nolock) on R.ArticleId=A.ArticleId ";
                strSql += "  and R.UserId=@UserId ";
            }
            else
            {
                strSql += " and (A.SendTargetId=0 or A.SendTargetId=@SendTargetId) ";
                if (shopId > 0)
                    strSql += " and A.AuthorId=@AuthorId";
            }
            strSql += " and A.AuthorIdentity=@AuthorIdentity";


            strSql = string.Format(strSql, whereSql, wherefield);
            var param = new[] {
                new SqlParameter("@AuthorIdentity", AuthorIdentity),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@SendTargetId",  userIdentity==1?1:2),
                new SqlParameter("@AuthorId", shopId),
            };
            //生成sql语句
            return getPageData<ArticleBaseModel>(pageSize, pageindex, strSql, orderbyField, param, (items) =>
            {
                items.ForEach((item) =>
                {
                    item.ArticleUrl = string.Format("{0}/app/details.html?articleId={1}&idt={2}", WebConfig.articleDetailsDomain(), item.ArticleId, AuthorIdentity);
                    item.ArticleCover = WebConfig.reswebsite() + item.ArticleCover;
                    item.PublishTimeText = StringHelper.GetConvertFriendlyTime(item.PublishTime.ToString());
                });
            });
        }

        /// <summary>
        /// 获取置顶资讯数据
        /// </summary>
        /// <param name="AuthorIdentity">资讯类型</param>
        /// <param name="userIdentity">用户身份，1盟主.0盟友</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>List&lt;ArticleBaseModel&gt;.</returns>
        public List<ArticleBaseModel> GetAppTopArticleList(int AuthorIdentity, int userIdentity, int shopId)
        {
            string strSql = @"select A.ArticleId,a.ArticleTitle,a.ArticleIntro,a.ArticleCover,a.BrowseAmount,a.PublishTime
                                 from BM_ArticleList A with(nolock)
                                where a.IsDel=0 and a.EnableTop=1 and a.EnablePublish=1 and A.ArticleStatus=1 and A.AuthorIdentity=@AuthorIdentity and (A.SendTargetId=0 or A.SendTargetId=@SendTargetId)";



            if (shopId > 0)
                strSql += " and A.AuthorId=@AuthorId";

            strSql += " order by A.TopTime desc";


            var param = new[] {
                new SqlParameter("@AuthorIdentity", AuthorIdentity),
                new SqlParameter("@AuthorId", shopId),
                new SqlParameter("@SendTargetId", userIdentity==1?1:2),
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                List<ArticleBaseModel> data = DbHelperSQLP.GetEntityList<ArticleBaseModel>(dr);
                if (data != null)
                {
                    data.ForEach((item) =>
                    {
                        item.ArticleUrl = WebConfig.articleDetailsDomain() + "/app/details.html?articleId=" + item.ArticleId;
                        item.ArticleCover = WebConfig.reswebsite() + item.ArticleCover;
                        item.PublishTimeText = StringHelper.GetConvertFriendlyTime(item.PublishTime.ToString());
                    });
                }
                return data;
            }
        }



        /// <summary>
        /// 获取资讯信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public ArticleModel GetModel(int articleId)
        {
            string strSql = APP_SELECT + "  and ArticleId=@ArticleId";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId)
            };

            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntity<ArticleModel>(dr);
            }
        }

        /// <summary>
        /// 设置资讯发布状态
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool SetArticleEnablePublish(int articleId, bool enable)
        {
            string strSql = "update BM_ArticleList set EnablePublish=@EnablePublish";
            if (enable)
                strSql += ",PublishTime=@PublishTime ";

            strSql += " where ArticleId=@ArticleId";

            var param = new[] {
                new SqlParameter("@EnablePublish", enable ? 1 : 0),
                new SqlParameter("@PublishTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@ArticleId",articleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }
        /// <summary>
        /// 设置资讯置顶状态
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool SetArticleEnableTop(int articleId, bool enable)
        {
            string strSql = "update BM_ArticleList set EnableTop=@EnableTop";
            if (enable)
                strSql += ",TopTime=@TopTime ";

            strSql += " where ArticleId=@ArticleId";

            var param = new[] {
                new SqlParameter("@EnableTop", enable ? 1 : 0),
                new SqlParameter("@TopTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@ArticleId",articleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 设置审核状态
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="remark">备注</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool SetArticleStatus(int articleId, int status, string remark)
        {
            string strSql = "update BM_ArticleList set ArticleStatus=@ArticleStatus,Remark=@Remark  where ArticleId=@ArticleId";
            var param = new[] {
                new SqlParameter("@ArticleStatus",status),
                new SqlParameter("@Remark",remark),
                new SqlParameter("@ArticleId",articleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 修改资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateArticle(ArticleModel model)
        {
            string strSql = @"update BM_ArticleList set SendTargetId=@SendTargetId,SendType=@SendType,ArticleSort=@ArticleSort,ArticleType=@ArticleType,ArticleClassify=@ArticleClassify
                            ,ArticleTitle=@ArticleTitle,ArticleIntro=@ArticleIntro,ArticleCover=@ArticleCover,ArticleBody=@ArticleBody,EnableTop=@EnableTop,EnablePublish=@EnablePublish,ArticleStatus=@ArticleStatus,UpdateTime=@UpdateTime";

            if (model.EnablePublish == 1)
                strSql += ",PublishTime=@PublishTime";

            if (model.EnableTop == 1)
                strSql += ",TopTime=@TopTime";

            strSql += " where ArticleId=@ArticleId";
            var param = new[] {
                        new SqlParameter("@SendTargetId", model.SendTargetId),
                        new SqlParameter("@SendType", model.SendType),
                        new SqlParameter("@ArticleSort", model.ArticleSort),
                        new SqlParameter("@ArticleType", model.ArticleType),
                        new SqlParameter("@ArticleClassify", model.ArticleClassify),
                        new SqlParameter("@ArticleTitle", model.ArticleTitle),
                        new SqlParameter("@ArticleIntro", model.ArticleIntro),
                        new SqlParameter("@ArticleCover", model.ArticleCover),
                        new SqlParameter("@ArticleBody",model.ArticleBody),
                        new SqlParameter("@EnableTop",model.EnableTop),
                        new SqlParameter("@EnablePublish",model.EnablePublish),
                        new SqlParameter("@ArticleStatus",model.ArticleStatus),
                        new SqlParameter("@TopTime",model.TopTime),
                        new SqlParameter("@PublishTime",model.PublishTime),
                        new SqlParameter("@UpdateTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new SqlParameter("@ArticleId",model.ArticleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 更新资讯浏览量
        /// </summary>
        /// <param name="articleId">The article identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool UpdateArticleAmount(int articleId)
        {

            string strSql = "update BM_ArticleList set BrowseAmount=BrowseAmount+1 where ArticleId=@ArticleId";
            var param = new[] {
                new SqlParameter("@ArticleId",articleId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }
    }
}
