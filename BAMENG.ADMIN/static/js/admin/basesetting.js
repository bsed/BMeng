/// <reference path="../plugins/switchery/switchery.js" />
/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


var settingHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    load: function () {
        var self = this;
        var postData = {
            action: "getconfiglist"
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200 && ret.data) {
                    $.each(ret.data, function (idx, item) {
                        $("#" + item.Code).val(item.Value);

                        if (item.Code == "EnableSign" || item.Code == "EnableContinuousSign" || item.Code == "EnableAppCoerceUpdate")
                            $("#" + item.Code).setChecked(item.Value == 1);

                    });
                }
            }
            self.initCheck();
            hotUtil.loading.close();
        });
    },
    initCheck: function () {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html);
        });
    },
    edit: function () {
        var self = this;

        var configJson = [
            {
                Code: "LoginDay",
                Value: $("#LoginDay").val(),
                Remark: "连续n天未登录，系统自动冻结账号"
            },
            {
                Code: "EnableSign",
                Value: $("#EnableSign").attr("checked") ? 1 : 0,
                Remark: "启用签到功能"
            },
            {
                Code: "SignScore",
                Value: $("#SignScore").val(),
                Remark: "每次签到，可获得n积分"
            },
            {
                Code: "EnableContinuousSign",
                Value: $("#EnableContinuousSign").attr("checked") ? 1 : 0,
                Remark: "启用连续签到功能"
            },
            {
                Code: "ContinuousSignDay",
                Value: $("#ContinuousSignDay").val(),
                Remark: "连续签到满n天"
            },
            {
                Code: "ContinuousSignRewardScore",
                Value: $("#ContinuousSignRewardScore").val(),
                Remark: "可额外赠送n积分"
            },
            {
                Code: "BindMobile",
                Value: $("#BindMobile").val(),
                Remark: "绑定手机,绑定后，总后台收到资讯审核及消息通知会收到短信通知"
            },
            {
                Code: "EnableAppCoerceUpdate",
                Value: $("#EnableAppCoerceUpdate").attr("checked") ? 1 : 0,
                Remark: "是否启用强制更新"
            },
            {
                Code: "AppVersion",
                Value: $("#AppVersion").val(),
                Remark: "最新版本号"
            },
            {
                Code: "AppUpateUrl",
                Value: $("#AppUpateUrl").val(),
                Remark: "更新地址"
            },
            {
                Code: "AppUpateContent",
                Value: $("#AppUpateContent").val(),
                Remark: "更新内容"
            },
            {
                Code: "CreateOrderScore",
                Value: $("#CreateOrderScore").val(),
                Remark: "盟主创建订单,可获得n积分"
            },
            {
                Code: "SubmitCustomerToAllyScore",
                Value: $("#SubmitCustomerToAllyScore").val(),
                Remark: "盟友提交客户信息并审核通过，盟友可获得n积分"
            },
            {
                Code: "SubmitCustomerToMainScore1",
                Value: $("#SubmitCustomerToMainScore1").val(),
                Remark: "盟友提交客户信息并审核通过，盟主可获得n积分"
            },
            {
                Code: "SubmitCustomerToMainScore2",
                Value: $("#SubmitCustomerToMainScore2").val(),
                Remark: "盟主提交客户信息，盟主可获得n积分"
            },
            {
                Code: "InviteScore",
                Value: $("#InviteScore").val(),
                Remark: "盟主邀请盟友，可获得n积分"
            }
        ];

        var postData = {
            action: "EDITCONFIG",
            config: hotUtil.encode(JSON.stringify(configJson))
        }


        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("保存成功", "", "success");
                }
            }
            hotUtil.loading.close();
        });
    }
}


$(function () {

    $('.OnlyNum').OnlyNum();
    settingHelper.load();

    $("#EnableSign,#EnableContinuousSign,#EnableAppCoerceUpdate").change(function () {
        if ($(this).attr("checked"))
            $(this).setChecked(false);
        else
            $(this).setChecked(true);
    });
    var e = "<i class='fa fa-times-circle'></i> ";
    $("#signupForm").validate({
        rules: {
            articleTitle: {
                required: !0,
                minlength: 2
            },
            articleIntro: "required"
        },
        messages: {
            articleTitle: {
                required: e + "请输入名称",
                minlength: e + "联系人必须两个字符以上"
            },
            articleIntro: e + "请输入您的手机号码"
        },
        submitHandler: function (form) {
            settingHelper.edit();
        }
    });
});