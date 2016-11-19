
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

var orderHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    type: hotUtil.getQuery("type"),
    picDir: "bameng/focuspic/",
    pageIndex: 1,
    reset: null,
    loadList: function (page) {
        var self = this;
        self.loaclData = [];
        this.pageIndex = page;
        var postData = {
            action: "getorderlist",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
            startTime: $("#beginTime").val(),
            endTime: $("#endTime").val(),
            type: this.type
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        self.loaclData = ret.data.Rows;
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist").html();                            
                            tempHtml = tempHtml.replace(/{OrderId}/gm, item.orderId);
                            tempHtml = tempHtml.replace("{Name}", item.Ct_Name);
                            tempHtml = tempHtml.replace("{Mobile}", item.Ct_Mobile);
                            tempHtml = tempHtml.replace("{Address}", item.Ct_Mobile);
                            tempHtml = tempHtml.replace("{OrderStatusName}", item.OrderStatusName);
                            tempHtml = tempHtml.replace("{ShopName}", item.ShopName);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                orderHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        orderHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        orderHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        if (!hotUtil.isNullOrEmpty(dataId) && this.loaclData != null && this.loaclData.length > 0) {
            $.each(this.loaclData, function (i, item) {
                if (item.orderId == dataId) {
                    model = item;
                    return false;
                }
            });
        }
        return model;
    },
    edit: function (dataId) {
        var url = "admin/orderdetails.html?orderid=" + dataId;        
        hotUtil.newTab(url, "订单详情-[" + dataId + "]");
    },
    switchTab: function (type) {
        this.type = type;
        orderHelper.loadList(1);
    },
    pageInit: function () {
        orderHelper.loadList(orderHelper.pageIndex);
    }
};

$(function () {
    orderHelper.pageInit();
});


