/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    /// <summary>
    /// 门店实体对象
    /// </summary>
    public class ShopModel
    {
        /// <summary>
        /// Gets or sets the shop identifier.
        /// </summary>
        /// <value>The shop identifier.</value>
        public int ShopID { get; set; }

        /// <summary>
        /// Gets or sets the name of the shop.
        /// </summary>
        /// <value>The name of the shop.</value>
        public string ShopName { get; set; }

        /// <summary>
        /// Gets or sets the name of the belong one shop.
        /// </summary>
        /// <value>The name of the belong one shop.</value>
        public string BelongOneShopName { get; set; }

        /// <summary>
        /// 门店类型1 总店 2分店
        /// </summary>
        public int ShopType { get; set; }
        /// <summary>
        /// 门店所属总店ID，门店类型为总店时，此值0
        /// </summary>
        public int ShopBelongId { get; set; }
        /// <summary>
        /// Gets or sets the shop prov.
        /// </summary>
        /// <value>The shop prov.</value>
        public string ShopProv { get; set; }
        /// <summary>
        /// Gets or sets the shop city.
        /// </summary>
        /// <value>The shop city.</value>
        public string ShopCity { get; set; }
        /// <summary>
        /// Gets or sets the shop area.
        /// </summary>
        /// <value>The shop area.</value>
        public string ShopArea { get; set; }
        /// <summary>
        /// 店铺地址
        /// </summary>
        public string ShopAddress { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactWay { get; set; }
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }
        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        public string LoginPassword { get; set; }
        /// <summary>
        /// 1活动 0结束
        /// </summary>
        public int IsActive { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
