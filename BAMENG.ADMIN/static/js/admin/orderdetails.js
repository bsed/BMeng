/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />

/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

var orderdetailsHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    orderId: hotUtil.getQuery("orderid"),
    load: function () {
        var self = this;
        var postData = {
            action: "getorderinfo",
            orderid: this.orderId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        $.each(ret.data, function (k, v) {
                            if (k == "OrderImg" || k == "SuccessImg")
                                $("#" + k).attr("src", v);
                            else
                                $("#" + k).text(v);

                            if (k == "OrderStatus" && v == 0) {
                                $(".btn-outline").show();
                            }

                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    updateStatus: function (status) {
        var self = this;
        var postData = {
            action: "UpdateOrderStatus",
            orderid: this.orderId,
            type: status
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("操作成功", "", "success");
                    $(".btn-outline").hide();
                    $("#OrderStatusName").text(status == 1 ? "已成交" : "已退单");
                }
                else
                    swal(ret.statusText, "", "error");
            }
            hotUtil.loading.close();
        });
    }
}


$(function () {
    $(".btn-outline").hide();
    orderdetailsHelper.load();
});