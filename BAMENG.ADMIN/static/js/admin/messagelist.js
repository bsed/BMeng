
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

var messageHelper = {
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
            action: "getMessageList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
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
                            tempHtml = tempHtml.replace("{NO}", (i + 1));
                            tempHtml = tempHtml.replace("{Title}", item.Title);
                            tempHtml = tempHtml.replace(/{ID}/gm, item.ID);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            tempHtml = tempHtml.replace("{StatusText}", item.IsSend == 1 ? "<span style='color:red;'>已发送</span>" : "未发送")
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                messageHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        messageHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        messageHelper.loadList(1);
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
    edit: function (dataId) {
        var url = "admin/messageedit.html?messageid=" + dataId;
        var data = this.getModel(dataId);
        if (data != null)
            hotUtil.newTab(url, "编辑消息通知[" + data.Title + "]");
        else
            hotUtil.newTab(url, "添加消息通知");
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
                action: "DeleteMessage",
                messageid: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(messageHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功！", "您已经永久删除了这条信息。", "success");
                        messageHelper.loadList(messageHelper.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    pageInit: function () {
        messageHelper.loadList(messageHelper.pageIndex);
    }
};

$(function () {
    messageHelper.pageInit();
});


