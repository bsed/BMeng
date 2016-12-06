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

var couponHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    couponId: hotUtil.getQuery("couponid"),
    pageIndex: 1,
    reset: null,
    loadList: function (page) {
        var self = this;
        self.loaclData = [];
        this.pageIndex = page;
        var postData = {
            action: "getcouponlogList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
            searchType: $("#sltStatus").val(),
            startTime: $("#beginTime").val(),
            endTime: $("#endTime").val(),
            couponId: this.couponId
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
                            tempHtml = tempHtml.replace("{NO}", (i + 1));
                            tempHtml = tempHtml.replace("{CouponNo}", item.CouponNo);
                            tempHtml = tempHtml.replace("{Mobile}", item.Mobile);
                            tempHtml = tempHtml.replace("{Time}", item.time);
                            tempHtml = tempHtml.replace("{GetTime}", item.GetTime);
                            if (item.IsUse == 1) {
                                tempHtml = tempHtml.replace("{UseStatusName}", "已核销");
                                tempHtml = tempHtml.replace("{UseTime}", item.UseTime);
                            }
                            else {
                                tempHtml = tempHtml.replace("{UseStatusName}", "未核销");
                                tempHtml = tempHtml.replace("{UseTime}", "--/--");
                            }

                            
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init(function (p) {
                            goTo(p, function (page) {
                                couponHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        couponHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        $("#createTimePick,#beginTime,#endTime").val("");
        couponHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        if (!hotUtil.isNullOrEmpty(dataId) && this.loaclData != null && this.loaclData.length > 0) {
            $.each(this.loaclData, function (i, item) {
                if (item.ID == dataId) {
                    model = item;
                    return false;
                }
            });
        }
        return model;
    },
    pageInit: function () {
        couponHelper.loadList(couponHelper.pageIndex);
    },
    newTab: function () {
        hotUtil.newTab("admin/cashcouponlist.html", "现金券列表");
    }

};



$(function () {
    couponHelper.pageInit();
});


