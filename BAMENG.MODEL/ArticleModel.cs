/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{


    public class ArticleBaseModel
    {
        /// <summary>
        /// 资讯id
        /// </summary>
        /// <value>The article identifier.</value>
        public int ArticleId { get; set; }


        /// <summary>
        /// 资讯封面
        /// </summary>
        public string ArticleCover { get; set; }
        /// <summary>
        /// 资讯标题
        /// </summary>
        public string ArticleTitle { get; set; }
        /// <summary>
        /// 资讯简介
        /// </summary>
        public string ArticleIntro { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        /// <value>The browse amount.</value>
        public long BrowseAmount { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        ///发布时间的友好提示
        /// </summary>
        /// <value>The publish time text.</value>
        public string PublishTimeText { get; set; }

        /// <summary>
        /// 资讯详情地址
        /// </summary>
        /// <value>The article URL.</value>
        public string ArticleUrl { get; set; }

        /// <summary>
        /// 是否阅读
        /// </summary>
        public int IsRead { get; set; }
    }

    /// <summary>
    ///资讯实体对象
    /// </summary>
    public class ArticleModel : ArticleBaseModel
    {

        /// <summary>
        /// 作者ID
        /// </summary>
        /// <value>The author identifier.</value>
        public int AuthorId { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        /// <value>The name of the author.</value>
        public string AuthorName { get; set; }

        /// <summary>
        /// 资讯类型，0集团，1总店，2分店  3盟主 4盟友
        /// </summary>
        public int AuthorIdentity { get; set; }
        /// <summary>
        /// 发送对象，0所有人，1盟主 2盟友
        /// </summary>
        public int SendTargetId { get; set; }
        /// <summary>
        /// 发送类型0向下发送，1向上发送
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <value>The article sort.</value>
        public int ArticleSort { get; set; }


        /// <summary>
        /// 资讯正文
        /// </summary>
        public string ArticleBody { get; set; }
        /// <summary>
        /// 资讯类型(暂时不用)
        /// </summary>
        public int ArticleType { get; set; }

        /// <summary>
        /// 资讯分类
        /// </summary>
        public int ArticleClassify { get; set; }



        /// <summary>
        /// 是否置顶
        /// </summary>
        public int EnableTop { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public int EnablePublish { get; set; }



        /// <summary>
        /// 资讯状态， -1审核失败  0申请中，1审核通过
        /// </summary>
        public int ArticleStatus { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }


        /// <summary>
        /// 置顶时间
        /// </summary>
        public DateTime TopTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }


        public string ShopProv { get; set; }

        public string ShopCity { get; set; }

    }
}
