using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.CONFIG
{
    /// <summary>
    /// 接口业务状态码
    /// 作者:郭孟稳
    /// </summary>
    public enum ApiStatusCode
    {
        /// <summary>
        /// 服务器成功处理了请求，但没有返回任何内容。
        /// </summary>
        [Description("服务器成功处理了请求，但没有返回任何内容")]
        无返回 = 204,
        /// <summary>
        /// 要完成请求，需要进一步操作
        /// </summary>
        [Description("要完成请求，需要进一步操作")]
        失败 = 300,
        /// <summary>
        /// 请求要求身份验证
        /// </summary>
        [Description("请求要求身份验证")]
        未授权 = 401,
        /// <summary>
        /// 服务器拒绝请求。
        /// </summary>
        [Description("服务器拒绝请求")]
        禁止请求 = 403,
        /// <summary>
        /// 服务器找不到请求的网页。
        /// </summary>
        [Description("服务器找不到请求的网页")]
        地址错误 = 404,

        /// <summary>
        /// 服务器遇到错误
        /// </summary>
        [Description("服务器开小差了，请稍后再试!")]
        SERVICEERROR = 500,
        /// <summary>
        /// 无数据
        /// </summary>
        [Description("无数据")]
        NULL = 0,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        OK = 200,

        [Description("更新失败")]
        更新失败 = 6001,
        [Description("操作失败")]
        操作失败 = 6002,
        [Description("删除失败")]
        删除失败 = 6003,
        [Description("添加失败")]
        添加失败 = 6004,
        [Description("发送失败")]
        发送失败 = 6005,
        [Description("缺少发送目标")]
        缺少发送目标 = 6006,
        [Description("客户已存在")]
        客户已存在 = 6007,

        [Description("保存失败")]
        保存失败 = 6008,

        [Description("无操作权限")]
        无操作权限 = 6009,

        [Description("找回密码失败")]
        找回密码失败 = 6010,

        [Description("密码修改失败")]
        密码修改失败 = 6011,

        [Description("旧密码不正确")]
        旧密码不对 = 6012,

        [Description("无效手机号")]
        无效手机号 = 6013,

        [Description("姓名不能为空")]
        姓名不能为空 = 6014,

        [Description("地址不能为空")]
        地址不能为空 = 6015,
        [Description("无效的盟主ID")]
        无效的盟主ID = 6016,

        [Description("账户已存在")]
        账户已存在 = 7000,
        [Description("账户不存在")]
        账户不存在 = 7001,
        [Description("账户或密码不正确")]
        账户密码不正确 = 7002,
        [Description("账户已禁用")]
        账户已禁用 = 7003,

        [Description("用户名已存在")]
        用户名已存在 = 7004,

        [Description("手机用户已存在")]
        手机用户已存在 = 7005,

        [Description("你已申请，请耐心等待")]
        你已申请请耐心等到审核 = 7006,

        [Description("非法请求")]
        非法请求 = 7007,

        [Description("用户信息丢失，请重新登录")]
        没有登录 = 70034,
        [Description("你的账号已在另一台设备登录。如非本人操作，则密码可能已泄露，建议修改密码。")]
        令牌失效 = 70035,

        [Description("兑换审核存在异常")]
        兑换审核存在异常 = 70036,

        [Description("用户信息丢失。")]
        用户信息丢失 = 70037,
        [Description("内容不能为空。")]
        内容不能为空 = 70038,


        [Description("请上传图片")]
        请上传图片 = 71000,
        [Description("请上传凭证")]
        请上传凭证 = 71001,

        [Description("订单状态为已成交后上传")]
        订单状态为已成交后上传 = 71002,



        [Description("订单存在问题")]
        订单存在问题 = 71003,
        [Description("订单目前状态存在异常")]
        订单目前状态存在异常 = 71004,
        [Description("兑换的盟豆数量不能少于100")]
        兑换的盟豆数量不能少于100 = 71005,
        [Description("你的盟豆不够")]
        你的盟豆不够 = 71006,
        [Description("请先上传成交凭证")]
        请先上传成交凭证 = 71007,

        /// <summary>
        /// 操作过于频繁
        /// </summary>
        [Description("操作过于频繁,请一分钟后再试")]
        INVALID_OPTION_CODE = 72000,
        /// <summary>
        /// 发送验证码失败
        /// </summary>
        [Description("验证码发送失败")]
        APP_SEND_CODE = 72001,

        [Description("无效验证码")]
        无效验证码 = 72002,


        [Description("超出当天发送次数")]
        超出当天发送次数 = 72003,





        [Description("签到功能未开启")]
        签到功能未开启 = 74001,

        [Description("今日已签到")]
        今日已签到 = 74002,
        [Description("服务器开小差了, 请重新签到")]
        请重新签到 = 74003,


        [Description("现金券已过期")]
        现金券已过期 = 75001,

        [Description("您来晚了，现金券已领完!")]
        现金券已领完 = 75002,
        [Description("现金券不存在或已使用")]
        优惠券不存在 = 75003,

        [Description("该优惠券您已转发")]
        您已转发 = 75004,

        [Description("非本店现金券")]
        非本店现金券 = 75005,

        [Description("现金券已使用")]
        现金券已使用 = 75006,
    }


    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SearchType
    {
        姓名 = 1,
        昵称 = 2,
        手机 = 3,
        门店 = 4,
        标题 = 5,
    }


    public enum OSOptions
    {
        android,
        iphone
    }

    /// <summary>
    /// 用户属性枚举
    /// </summary>
    public enum UserPropertyOptions
    {
        /// <summary>
        /// The use 头像
        /// </summary>
        USER_1 = 1,
        /// <summary>
        /// The use 昵称
        /// </summary>
        USER_2 = 2,

        /// <summary>
        /// The use 手机
        /// </summary>
        USER_3 = 3,

        /// <summary>
        /// The use 姓名
        /// </summary>
        USER_4 = 4,

        /// <summary>
        /// The use 性别
        /// </summary>
        USER_5 = 5,

        /// <summary>
        /// The use 地区
        /// </summary>
        USER_6 = 6,

        /// <summary>
        /// The use 其他扩展
        /// </summary>
        USER_7 = 7,

    }




}
