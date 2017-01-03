/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/// <reference path="../plugins/layui/layui.js" />

/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

var mailHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    pageIndex: 1,
    reset: null,
    type: hotUtil.getQuery("type"),
    loadList: function (page) {
        var self = this;
        self.loaclData = [];
        this.pageIndex = page;
        var postData = {
            action: "GetMailList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
            startTime: "",
            endTime: ""
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        self.loaclData = ret.data.Rows;
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml =$("#templist").html();
                            tempHtml = tempHtml.replace("{NO}", i + 1);
                            tempHtml = tempHtml.replace("{Title}", item.Title);
                            tempHtml = tempHtml.replace("{BodyContent}", item.ShopName);
                            if (!hotUtil.isNullOrEmpty(item.CoverUrl))
                                tempHtml = tempHtml.replace("{CoverUrl}", item.CoverUrl);
                            else
                                tempHtml = tempHtml.replace("{CoverUrl}", "/static/img/bg.png");
                            tempHtml = tempHtml.replace(/{ID}/gm, item.ID);
                            tempHtml = tempHtml.replace("{SendTime}", item.SendTime);
                            tempHtml = tempHtml.replace("{AuthorName}", item.AuthorName);                         
                            listhtml += tempHtml;
                        });
                        if (parseInt(self.type) != 0)
                            $("#listMode").html(listhtml);
                        else
                            $("#listMode2").html(listhtml);
                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init(function (p) {
                            goTo(p, function (page) {
                                mailHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        mailHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        mailHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        if (this.loaclData != null && this.loaclData.length > 0) {
            $.each(this.loaclData, function (i, item) {
                if (item.ID == dataId) {
                    model = item;
                    return false;
                }
            });
        }
        return model;
    },
    goPage: function (dataId) {
        hotUtil.newTab("/admin/maildetail.html?articleId=" + dataId, "查看");
    }
};


$(function () {
    mailHelper.loadList(mailHelper.pageIndex);
});


