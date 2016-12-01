/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/




var pwdHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    edit: function () {
        var self = this;
        var postData = {
            action: "modifypassword",
            oldpwd: $("#txtoldpwd").val(),
            newpwd: $("#txtnewpwd").val()
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("保存成功", "", "success");
                }
                else
                    swal(ret.statusText, "", "warning");
            }
            hotUtil.loading.close();
        });
    }
}


$(function () {
    var e = "<i class='fa fa-times-circle'></i> ";
    $("#signupForm").validate({
        rules: {
            txtoldpwd: "required",
            txtnewpwd: {
                required: !0,
                minlength: 5
            },
            txtconfirmpwd: {
                required: true,
                minlength: 5,
                equalTo: "#txtnewpwd"
            }
        },
        messages: {
            txtoldpwd: e + "请输入旧密码",
            txtnewpwd: {
                required: e + "请输入密码",
                minlength: e + "密码长度不能小于 5 个字母"
            },
            txtconfirmpwd: {
                required: e + "请输入密码",
                minlength: e + "密码长度不能小于 5 个字母",
                equalTo: e + "两次密码输入不一致"
            }
        },
        submitHandler: function (form) {
            pwdHelper.edit();
        }
    });
});