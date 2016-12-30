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

var workHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    reset: null,
    pageIndex: 1,
    loadList: function (page) {
        var self = this;
        this.pageIndex = page;
        var postData = {
            action: "GetUserReportList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),            
            startTime: $("#beginTime").val(),
            endTime: $("#endTime").val()
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        workHelper.loaclData = ret.data.Rows;
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist").html();
                            tempHtml = tempHtml.replace("{NO}", i + 1);
                            tempHtml = tempHtml.replace(/{ID}/gm, item.ID);
                            tempHtml = tempHtml.replace("{ReportTitle}", item.ReportTitle);
                            tempHtml = tempHtml.replace("{UserName}", item.UserName);
                            tempHtml = tempHtml.replace("{UserMobile}", item.UserMobile);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init(function (p) {
                            goTo(p, function (page) {
                                workHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        workHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword,#beginTime,#endTime,#createTimePick").val("");
        workHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        $.each(this.loaclData, function (i, item) {
            if (item.ID == dataId) {
                model = item;
                return false;
            }
        });
        return model;
    },
    del: function (dataId) {
        swal({
            title: "您确定要删除这条信息吗",
            text: "删除后将无法恢复，请谨慎操作！",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "删除",
            closeOnConfirm: false,
        }, function () {
            var param = {
                action: "DeleteUserReport",
                workid: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(workHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功", "您已经永久删除了这条信息。", "success");
                        workHelper.loadList(workHelper.pageIndex);

                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    goPage: function (dataId) {
        var data = this.getModel(dataId);
        hotUtil.newTab("/app/reportdetail.html?workid=" + dataId, "查看【" + data.ReportTitle + "】");
    },
    pageInit: function () {
        workHelper.loadList(1);
    },

};

$(function () {
    workHelper.pageInit();
});


