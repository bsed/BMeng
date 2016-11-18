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
    public class MessageModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

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
        /// 作者身份 0总后台，1总店，2分店
        /// </summary>
        /// <value>The author identity.</value>
        public int AuthorIdentity { get; set; }

        /// <summary>
        /// 目标ID，多个用|隔开
        /// </summary>
        /// <value>The send target ids.</value>
        public string SendTargetIds { get; set; }


        /// <summary>
        /// 发送对象名称
        /// </summary>
        /// <value>The name of the send target.</value>
        public string SendTargetName { get; set; }

        /// <summary>
        /// 消息正文
        /// </summary>
        /// <value>The message body.</value>
        public string MessageBody { get; set; }


        /// <summary>
        /// 是否给总后台发消息通知
        /// </summary>
        /// <value>The is send belong shop identifier.</value>
        public int IsSendBelongShopId { get; set;}

        /// <summary>
        /// 是否已发送
        /// </summary>
        /// <value>The is send.</value>
        public int IsSend { get; set; }


        /// <summary>
        /// 是否已读
        /// </summary>
        /// <value>The is read.</value>
        public int IsRead { get; set; }





        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
    }
}
