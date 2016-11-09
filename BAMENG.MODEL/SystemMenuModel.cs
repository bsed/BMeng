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
    /// <summary>
    /// /系统菜单实体
    /// </summary>
    public class SystemMenuModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }
        /// <summary>
        /// 0总后台菜单，1总门店菜单，2分店菜单
        /// </summary>
        public int ItemType { get; set; }


        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        /// <value>The item code.</value>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item nav label.
        /// </summary>
        /// <value>The item nav label.</value>
        public string ItemNavLabel { get; set; }


        /// <summary>
        /// Gets or sets the item parent code.
        /// </summary>
        /// <value>The item parent code.</value>
        public string ItemParentCode { get; set; }

        /// <summary>
        /// Gets or sets the item URL.
        /// </summary>
        /// <value>The item URL.</value>
        public string ItemUrl { get; set; }

        /// <summary>
        /// Gets or sets the item show.
        /// </summary>
        /// <value>The item show.</value>
        public int ItemShow { get; set; }


        /// <summary>
        /// Gets or sets the item icons.
        /// </summary>
        /// <value>The item icons.</value>
        public string ItemIcons { get; set; }


        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// Class SystemLeftModel.
    /// </summary>
    public class SystemLeftModel
    {
        /// <summary>
        /// Gets or sets the menu data.
        /// </summary>
        /// <value>The menu data.</value>
        public List<SystemMenuModel> menuData { get; set; }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        /// <value>The user data.</value>
        public AdminLoginModel userData { get; set; }
        /// <summary>
        /// 用户菜单权限代码，用隔开,如 |10001|10000101|
        /// </summary>
        /// <value>The authority.</value>
        public string authority { get; set; }
    }



    public class SwtSmsVerificationModel
    {
        /// <summary>
        /// SSV_ID
        /// </summary>		
        private int _ssv_id;
        public int SSV_ID
        {
            get { return _ssv_id; }
            set { _ssv_id = value; }
        }
        /// <summary>
        /// SSV_Verification
        /// </summary>		
        private string _ssv_verification;
        public string SSV_Verification
        {
            get { return _ssv_verification; }
            set { _ssv_verification = value; }
        }
        /// <summary>
        /// SSV_AddTime
        /// </summary>		
        private DateTime _ssv_addtime;
        public DateTime SSV_AddTime
        {
            get { return _ssv_addtime; }
            set { _ssv_addtime = value; }
        }
        /// <summary>
        /// 0代表未失效，1代表失效
        /// </summary>		
        private int _ssv_isinvalid;
        public int SSV_IsInvalid
        {
            get { return _ssv_isinvalid; }
            set { _ssv_isinvalid = value; }
        }
        /// <summary>
        /// SSV_Phone
        /// </summary>		
        private string _ssv_phone;
        public string SSV_Phone
        {
            get { return _ssv_phone; }
            set { _ssv_phone = value; }
        }

        public int SSV_CustomerId { set; get; }
    }



    public class ConfigModel
    {
        /// <summary>
        /// 配置编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 配置值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

    }

}
