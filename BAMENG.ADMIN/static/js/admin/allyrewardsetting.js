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
            action: "getAllyReward"
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200 && ret.data) {
                    $("#creward").val(ret.data.CustomerReward);
                    $("#orderreward").val(ret.data.OrderReward);
                    $("#extrareward").val(ret.data.ExtraReward);
                }
            }
            hotUtil.loading.close();
        });
    },
    edit: function () {
        var self = this;
        var postData = {
            action: "SETALLYREWARD",
            creward: $("#creward").val(),
            orderreward: $("#orderreward").val(),
            extrareward: $("#extrareward").val()
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

    $("#BtnSaveReward").click(function () {
        settingHelper.edit();
    });

});