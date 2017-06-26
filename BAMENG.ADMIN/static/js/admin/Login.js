/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-$year$. All rights reserved.
 * author $username$
**/


var loginHelper = {
    ajaxUrl: "handler/Login.ashx",
    init: function () {
        $(".btn-success").bind("click", function () {
            loginHelper.login();
        });
    },
    login: function () {
        if (hotUtil.isNullOrEmpty($("#loginName").val())) {
            swal("请输入登录账号", "", "warning")
            return false;
        }
        if (hotUtil.isNullOrEmpty($("#loginPassword").val())) {
            swal("请输入登录密码", "", "warning")
            return false;
        }

        var loginType = 0;
        $("input[name='radioInline']").each(function (i, v) {
            if ($(this).attr("data-check") == "true")
                loginType = $(this).val();
        });


        var host = location.host;
        if (host.indexOf("mellkit.com") > 0) {
            if (host.indexOf("manager") >= 0)
                loginType = 0;
            else
                loginType = 1;
        }

        var postData = {
            loginName: $("#loginName").val(),
            password: $("#loginPassword").val(),
            loginType: loginType /*0总后台，1门店**/
        }
        $(".btn-success").unbind("click");
        $(".btn-success").text("正在登录...");
        var loginSuccess = false;
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    //success     
                    loginSuccess = true;
                    window.location.href = "main.html";
                }
                else
                    swal(ret.statusText, "", "warning");
            }
            else
                swal("账户或密码不正确", "", "warning");
            if (!loginSuccess) {
                $(".btn-success").text("登录");
                $(".btn-success").bind("click", function () {
                    loginHelper.login();
                });
            }

        });
    }
};

$(function () {
    $("input[name='radioInline']").change(function () {
        $("input[name='radioInline']").removeAttr("data-check");
        if (!$(this).attr("data-check")) {
            $(this).attr("data-check", "true");
        }
    });
    var host = location.host;
    if (host.indexOf("mellkit.com") > 0) {
        $(".radio-inline").hide();
    }

    loginHelper.init();
});