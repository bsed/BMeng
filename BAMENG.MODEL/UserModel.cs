/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-$today.year. All rights reserved.
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }
        /// <summary>
        /// 用户身份，0盟友  1盟主
        /// </summary>
        public int UserIdentity { get; set; }
        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>The merchant identifier.</value>
        public int MerchantID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ShopId { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>The is active.</value>
        public int IsActive { get; set; }

        /// <summary>
        /// 性别 男M 女F 未知
        /// </summary>
        /// <value>The user sex.</value>
        public string UserGender { get; set; }

        /// <summary>
        /// Gets or sets the user city.
        /// </summary>
        /// <value>The user city.</value>
        public string UserCity { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the score locked.
        /// </summary>
        /// <value>The score locked.</value>
        public decimal ScoreLocked { get; set; }
        /// <summary>
        /// 盟豆
        /// </summary>
        /// <value>The meng beans.</value>
        public decimal MengBeans { get; set; }
        /// <summary>
        /// 锁定盟豆
        /// </summary>
        /// <value>The meng beans locked.</value>
        public decimal MengBeansLocked { get; set; }


        /// <summary>
        /// 待结算盟豆
        /// </summary>
        /// <value>The temporary meng beans.</value>
        public decimal TempMengBeans { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }
        /// <summary>
        /// Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        public string RealName { get; set; }
        /// <summary>
        /// Gets or sets the name of the nick.
        /// </summary>
        /// <value>The name of the nick.</value>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the user mobile.
        /// </summary>
        /// <value>The user mobile.</value>
        public string UserMobile { get; set; }
        /// <summary>
        /// Gets or sets the user head img.
        /// </summary>
        /// <value>The user head img.</value>
        public string UserHeadImg { get; set; }

        /// <summary>
        /// Gets or sets the name of the shop.
        /// </summary>
        /// <value>The name of the shop.</value>
        public string ShopName { get; set; }
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
        /// Gets or sets the name of the level.
        /// </summary>
        /// <value>The name of the level.</value>
        public string LevelName { get; set; }
        /// <summary>
        /// 订单成交量
        /// </summary>
        public int OrderSuccessAmount { get; set; }
        /// <summary>
        /// 客户信息提交数量
        /// </summary>
        public int CustomerAmount { get; set; }

        /// <summary>
        /// 用户唯一令牌,调用其他接口是，需要使用该令牌
        /// </summary>
        /// <value>The token.</value>
        public string token { get; set; }

        /// <summary>
        /// 盟主ID
        /// </summary>
        /// <value>The belong one.</value>
        public int BelongOne { get; set; }

    }


    /// <summary>
    /// Class UserRegisterModel.
    /// </summary>
    public class UserRegisterModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        public int ShopId { get; set; }

        public string username { get; set; }

        public string nickname { get; set; }

        public int storeId { get; set; }

        public string mobile { get; set; }
        /// <summary>
        /// 用户身份，0盟友  1盟主
        /// </summary>
        public int UserIdentity { get; set; }

        public string loginName { get; set; }

        public string loginPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int belongOne { get; set; }

    }

    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class UserBaseInfoModel
    {
        #region User BaseInfo model
        private int _ub_userid;
        private string _ub_userloginname;
        private string _ub_userloginpassword;
        private string _ub_usernickname;
        private string _ub_userface;
        private string _ub_userrealname;
        private string _ub_usergender;
        private int _ub_userage;
        private string _ub_userincome;
        private string _ub_usermobile;
        private string _ub_useremail;
        private string _ub_usercardid;
        private string _ub_userprovince;
        private string _ub_usercity;
        private string _ub_userarea;
        private string _ub_useraddress;
        private int _ub_userintegral;
        private int _ub_usertempintegral;
        private decimal _ub_userbalance;
        private DateTime _ub_userregtime;
        private DateTime _ub_userlastlogintime;
        private int _ub_isdelete;
        private int _ub_customerid;
        private string _ub_userbirthday;
        private int _ub_hascard;
        private int _UB_ShareCount;

        public int UB_ShareCount
        {
            get { return _UB_ShareCount; }
            set { _UB_ShareCount = value; }
        }

        /// <summary>
        /// UB_UserID
        /// </summary>
        public int UB_UserID
        {
            get { return _ub_userid; }
            set { _ub_userid = value; }
        }
        /// <summary>
        /// 用户登陆名
        /// </summary>
        public string UB_UserLoginName
        {
            get { return _ub_userloginname; }
            set { _ub_userloginname = value; }
        }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string UB_UserLoginPassword
        {
            get { return _ub_userloginpassword; }
            set { _ub_userloginpassword = value; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UB_UserNickName
        {
            get { return _ub_usernickname; }
            set { _ub_usernickname = value; }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string UB_UserFace
        {
            get { return _ub_userface; }
            set { _ub_userface = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UB_UserRealName
        {
            get { return _ub_userrealname; }
            set { _ub_userrealname = value; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string UB_UserGender
        {
            get { return _ub_usergender; }
            set { _ub_usergender = value; }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public int UB_UserAge
        {
            get { return _ub_userage; }
            set { _ub_userage = value; }
        }
        /// <summary>
        /// 收入
        /// </summary>
        public string UB_UserIncome
        {
            get { return _ub_userincome; }
            set { _ub_userincome = value; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string UB_UserMobile
        {
            get { return _ub_usermobile; }
            set { _ub_usermobile = value; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UB_UserEmail
        {
            get { return _ub_useremail; }
            set { _ub_useremail = value; }
        }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string UB_UserCardID
        {
            get { return _ub_usercardid; }
            set { _ub_usercardid = value; }
        }
        /// <summary>
        /// 省
        /// </summary>
        public string UB_UserProvince
        {
            get { return _ub_userprovince; }
            set { _ub_userprovince = value; }
        }
        /// <summary>
        /// 市
        /// </summary>
        public string UB_UserCity
        {
            get { return _ub_usercity; }
            set { _ub_usercity = value; }
        }
        /// <summary>
        /// 地区/镇
        /// </summary>
        public string UB_UserArea
        {
            get { return _ub_userarea; }
            set { _ub_userarea = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string UB_UserAddress
        {
            get { return _ub_useraddress; }
            set { _ub_useraddress = value; }
        }
        /// <summary>
        /// 正式积分
        /// </summary>
        public int UB_UserIntegral
        {
            get { return _ub_userintegral; }
            set { _ub_userintegral = value; }
        }
        /// <summary>
        /// 临时积分
        /// </summary>
        public int UB_UserTempIntegral
        {
            get { return _ub_usertempintegral; }
            set { _ub_usertempintegral = value; }
        }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal UB_UserBalance
        {
            get { return _ub_userbalance; }
            set { _ub_userbalance = value; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime UB_UserRegTime
        {
            get { return _ub_userregtime; }
            set { _ub_userregtime = value; }
        }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime UB_UserLastLoginTime
        {
            get { return _ub_userlastlogintime; }
            set { _ub_userlastlogintime = value; }
        }
        /// <summary>
        /// 是否删除，1表示删除
        /// </summary>
        public int UB_IsDelete
        {
            get { return _ub_isdelete; }
            set { _ub_isdelete = value; }
        }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int UB_CustomerID
        {
            get { return _ub_customerid; }
            set { _ub_customerid = value; }
        }
        /// <summary>
        /// 用户生日
        /// </summary>
        public string UB_UserBirthday
        {
            get { return _ub_userbirthday; }
            set { _ub_userbirthday = value; }
        }
        /// <summary>
        /// 是否领取会员卡，1表示领取，0表示未领取
        /// </summary>
        public int UB_HasCard
        {
            get { return _ub_hascard; }
            set { _ub_hascard = value; }
        }

        /// <summary>
        /// 会员类型，0表示普通会员，1表示小伙伴，-1表示超级小伙伴
        /// </summary>
        public int UB_UserType { set; get; }

        /// <summary>
        /// 等级关联ID
        /// </summary>
        public int UB_LevelID { set; get; }
        public string LevelName { set; get; }

        /// <summary>
        /// 从属字段，-1表示无上级（废除）
        /// </summary>
        public int UB_BelongTo { set; get; }

        public string UserAllArea { set; get; }

        public int UB_LockedIntegral { set; get; }

        public decimal UB_LockedGold { get; set; }

        public decimal UB_LockedBalance { set; get; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public int UB_SourceID { set; get; }

        /// <summary>
        /// 来源说明
        /// </summary>
        public string UB_SourceDesc { set; get; }

        //是否在列表和详情页显示获得的积分，1表示显示，默认为1
        public int UB_IntegralVisible { set; get; }

        /// <summary>
        /// 剩余邀请次数
        /// </summary>
        public int UB_InviteCount { set; get; }

        /// <summary>
        /// 场景ID
        /// </summary>
        public int UB_SenceID { set; get; }

        /// <summary>
        /// 来源任务ID
        /// </summary>
        public int UB_ShareTaskID { set; get; }
        /// <summary>
        /// 来源任务类型
        /// </summary>
        public int UB_ShareTaskType { set; get; }

        public int UB_IsStore { set; get; }

        public string UB_StoreAddr { set; get; }

        public int UB_ParentID { set; get; }

        /// <summary>
        /// 来源路径，最初的在最前面，用,隔开
        /// </summary>
        public string UB_SourcePath { set; get; }

        /// <summary>
        /// 深度
        /// </summary>
        public int UB_SourceDepth { set; get; }

        public string ParentName { set; get; }
        public string BelongName { set; get; }

        public int UB_Represent { set; get; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }


        /// <summary>
        /// 师傅ID
        /// </summary>
        public int UB_MasterID { get; set; }

        /// <summary>
        /// 信息保护
        /// </summary>
        public int UB_InformationProtection { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public int UB_SourceType { set; get; }

        /// <summary>
        /// 上线ID
        /// </summary>
        public int UB_BelongOne { set; get; }
        /// <summary>
        /// 上线的上线ID
        /// </summary>
        public int UB_BelongTwo { set; get; }
        /// <summary>
        /// 上线的上线的上线的ID
        /// </summary>
        public int UB_BelongThree { set; get; }
        /// <summary>
        /// 业务分组ID
        /// </summary>
        public int UB_GroupId { set; get; }
        /// <summary>
        /// 是否可以拿返利，1表示可以，默认是1
        /// </summary>
        public int UB_RebateEnabled { set; get; }

        /// <summary>
        /// 实际分组id
        /// </summary>
        public int UB_UserGroupId { set; get; }

        public int UB_AccountSrc { set; get; }
        public int UB_MobileToBeBind { set; get; }
        #endregion

        #region 扩展参数

        public string[] ExtParameterValues { set; get; }

        public string WxNickName { get; set; }
        public string WxHeadImg { get; set; }

        #endregion

        private int _ub_DownloadAppStatus = 0;
        /// <summary>
        /// 是否下载登录过App,0表示没有登录过app，1表示登录过app
        /// </summary>
        public int UB_DownloadAppStatus
        {
            get { return _ub_DownloadAppStatus; }
            set { _ub_DownloadAppStatus = value; }
        }
        /// <summary>
        /// 是否开启支付密码 1开启
        /// </summary>
        public int EnabledPayPassword { get; set; }

    }

    /// <summary>
    /// 用户关系简单实体
    /// </summary>
    public class UserRelationViewEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登录账号名
        /// </summary>
        public string LoginName { get; set; }


        /// <summary>
        /// 上线ID
        /// </summary>
        public int BelongOne { get; set; }

        /// <summary>
        /// 上上线ID
        /// </summary>
        public int BelongTwo { get; set; }

        /// <summary>
        /// 上上上线ID
        /// </summary>
        public int BelongThree { get; set; }

        public int UB_CustomerID { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        public int CustomerId { get; set; }
    }

    /// <summary>
    /// 会员从属关系变动日志
    /// </summary>
    public class MemberChangeLogModel
    {

        /// <summary>
        ///  
        /// </summary>
        public int Log_Id { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int Member_Id { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Int16 Change_Type { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public DateTime Add_Time { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int Customer_Id { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BelongOne { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BelongTwo { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BelongThree { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BeforeBelongOne { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BeforeBelongTwo { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int BeforeBelongThree { get; set; }

        public int ParentId { get; set; }

        public int BeforeParentId { get; set; }

        public int LevelId { get; set; }

        public int GroupId { get; set; }

        public int BeforeLevelId { get; set; }

        public int BeforeGroupId { get; set; }

        public string Reason { get; set; }
        /// <summary>
        /// 待处理 下面字段去除
        /// </summary>
        public int Orignal_RelId { get; set; }
        public int Target_RelId { get; set; }
    }


    /// <summary>
    /// 用户等级
    /// </summary>
    [Serializable]
    public class MallUserLevelModel
    {
        /// <summary>
        /// UL_ID
        /// </summary>		
        private int _ul_id;
        public int UL_ID
        {
            get { return _ul_id; }
            set { _ul_id = value; }
        }
        /// <summary>
        /// 用户等级，0表示1级
        /// </summary>		
        private int _ul_level;
        public int UL_Level
        {
            get { return _ul_level; }
            set { _ul_level = value; }
        }
        /// <summary>
        /// 等级名称
        /// </summary>		
        private string _ul_levelname;
        public string UL_LevelName
        {
            get { return _ul_levelname; }
            set { _ul_levelname = value; }
        }
        /// <summary>
        /// 等级类型，0表示普通会员，1表示小伙伴
        /// </summary>		
        private int _ul_type;
        public int UL_Type
        {
            get { return _ul_type; }
            set { _ul_type = value; }
        }
        /// <summary>
        /// 商户ID
        /// </summary>		
        private int _ul_customerid;
        public int UL_CustomerID
        {
            get { return _ul_customerid; }
            set { _ul_customerid = value; }
        }

        public string UL_Description { set; get; }

        public int UL_Integral { set; get; }


        public decimal UL_Money { get; set; }

        /// <summary>
        /// 会员数
        /// </summary>
        public int UL_MemberNum { get; set; }
        /// <summary>
        /// 直接团队
        /// </summary>
        public int UL_DirectTeamNum { get; set; }
        /// <summary>
        /// 间接团队
        /// </summary>
        public int UL_IndirectTeamNum { get; set; }

        public int UL_DefaultLevel { get; set; }


        public int IntegralPreID { set; get; }
        public int PricePreID { set; get; }

        /// <summary>
        /// 一级分销商有没有开启等级个性化
        /// </summary>
        public bool UL_OpenLevel_One { get; set; }
        /// <summary>
        /// 一级分销商等级个性化设置
        /// </summary>
        public string UL_BelongOne_Content { get; set; }
        /// <summary>
        /// 二级分销商有没有开启等级个性化
        /// </summary>
        public bool UL_OpenLevel_Two { get; set; }
        /// <summary>
        /// 二级分销商等级个性化设置
        /// </summary>
        public string UL_BelongTwo_Content { get; set; }
        /// <summary>
        /// 系统默认-1,该等级引导人引导进来的会员等级
        /// </summary>
        public int UL_GuidetLevel { get; set; }
        /// <summary>
        /// 小金库充值
        /// </summary>
        public decimal UL_Gold { get; set; }

    }




    /// <summary>
    /// 后台登录用户实体
    /// </summary>
    [Serializable]
    public class AdminLoginModel
    {
        public int ID { get; set; }

        public string LoginName { get; set; }

        public string LoginPassword { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }


        public string UserName { get; set; }

        public string UserMobile { get; set; }

        public int UserStatus { get; set; }


        public string UserEmail { get; set; }


        public int ShopBelongId { get; set; }

        public DateTime LastLoginTime { get; set; }


        public DateTime CreateTime { get; set; }



        /// <summary>
        /// 0集团，1总店，2分店
        /// </summary>
        public int UserIndentity { get; set; }
    }



    /// <summary>
    /// 盟友奖励实体
    /// </summary>
    public class RewardsSettingModel
    {

        public int ID { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 提交客户信息奖励
        /// </summary>
        /// <value>The customer reward.</value>
        public decimal CustomerReward { get; set; }


        /// <summary>
        /// 订单成交奖励
        /// </summary>
        /// <value>The order reward.</value>
        public decimal OrderReward { get; set; }


        /// <summary>
        /// 客户进店奖励
        /// </summary>
        /// <value>The shop reward.</value>
        public decimal ShopReward { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        /// <value>The update time.</value>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }
    }


    public class ApplyFriendModel
    {

        public int ID { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int Sex { get; set; }

        public string Mobile { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public string NickNname { get; set; }

        public string Password { get; set; }

    }

    public class ConvertFlowModel
    {
        /// <summary>
        /// 客户
        /// </summary>
        public string name;
        public decimal money;
        public long time;
        /// <summary>
        /// 审核状态 0未审核 1已审核 2,拒绝
        /// </summary>
        public int status;
    }

    public class BeansConvertModel
    {
        public int ID { get; set; }
     
        public int UserId { get; set; }

        public int UserMasterId { get; set; }

        public int Amount { get; set; }


        public int Status { get; set; }

        public DateTime UpdateTime { get; set; }


        public DateTime CreateTime { get; set; }

        public string UserRealName { get; set; }

    }
}
