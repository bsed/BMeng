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

var customerHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    isAlly: hotUtil.getQuery("type"),
    pageIndex: 1,
    reset: null,
    loadList: function (page) {
        var self = this;
        self.loaclData = [];
        this.pageIndex = page;
        var postData = {
            action: "GetCustomerList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
            searchType: $("#stdType").val(),
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
                            var tempHtml = $("#templist").html();
                            tempHtml = tempHtml.replace(/{ID}/gm, item.ID);
                            tempHtml = tempHtml.replace("{Name}", item.Name);
                            tempHtml = tempHtml.replace("{BelongOneName}", item.BelongOneName);
                            tempHtml = tempHtml.replace("{BelongTwoName}", item.BelongTwoName);
                            tempHtml = tempHtml.replace("{Mobile}", item.Mobile);
                            tempHtml = tempHtml.replace("{Addr}", item.Addr + " " + item.ShopName);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                customerHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        customerHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        customerHelper.loadList(1);
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
    edit: function () {
        var param = hotUtil.serializeForm("#signupForm .form-control");
        param.action = "EditCustomerInfo";
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("提交成功！", "", "success");
                    customerHelper.loadList(customerHelper.pageIndex);
                    $(".close").click();
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
            hotUtil.loading.close();
        });
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
                action: "DeleteCustomerInfo",
                customerid: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(customerHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功！", "您已经永久删除了这条信息。", "success");
                        customerHelper.loadList(customerHelper.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    dialog: function (dataId) {
        if (this.reset)
            this.reset.resetForm();
        var data = this.getModel(dataId);
        if (data != null) {            
            $("#customerid").val(dataId);
            $("#username").val(data.Name);
            $("#useraddress").val(data.Addr);
            $("#usermobile").val(data.Mobile);
        }
    },
    pageInit: function () {
        customerHelper.loadList(customerHelper.pageIndex);
        customerHelper.validate();
    },
    validate: function () {
        var e = "<i class='fa fa-times-circle'></i> ";
        this.reset = $("#signupForm").validate({
            rules: {
                username: {
                    required: !0,
                    minlength: 2
                },
                usermobile: "required",
                useraddress: "required"
            },
            messages: {
                username: {
                    required: e + "请输入客户姓名",
                    minlength: e + "客户姓名必须两个字符以上"
                },
                usermobile: e + "请输入您的手机号码",
                useraddress: e + "请输入客户地址"
            },
            submitHandler: function (form) {
                customerHelper.edit();
            }
        })
    }
};



$(function () {
    customerHelper.pageInit();
});


