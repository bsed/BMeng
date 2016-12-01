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

var shopHelper = {
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
            action: "getshoplist",
            pageIndex: page,
            pageSize: 20,
            prov: $("#province").val(),
            city: $("#city").val(),
            key: $("#keyword").val(),
            type: self.type
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
                            tempHtml = tempHtml.replace("{LoginName}", item.LoginName);
                            tempHtml = tempHtml.replace(/{ShopID}/gm, item.ShopID);
                            if (self.type == 2)
                                tempHtml = tempHtml.replace("{ShopName}", item.ShopName + "/【" + item.BelongOneShopName + "】");
                            else
                                tempHtml = tempHtml.replace("{ShopName}", item.ShopName);
                            tempHtml = tempHtml.replace("{Contacts}", item.Contacts);
                            tempHtml = tempHtml.replace("{ShopProv}", item.ShopProv + item.ShopCity + item.ShopArea + item.ShopAddress);
                            tempHtml = tempHtml.replace("{ContactWay}", item.ContactWay);
                            tempHtml = tempHtml.replace("{IsActive}", item.IsActive);
                            tempHtml = tempHtml.replace("{ActiveStatus}", item.IsActive == 1 ? "<span style='color:red;'>激活</span>" : "已冻结")
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                shopHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        shopHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        $("#province").val("");
        $("#city").val("");
        shopHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        if (this.loaclData != null && this.loaclData.length > 0) {
            $.each(this.loaclData, function (i, item) {
                if (item.ShopID == dataId) {
                    model = item;
                    return false;
                }
            });
        }
        return model;
    },
    edit: function () {
        var param = hotUtil.serializeForm("#signupForm .form-control");
        param.action = "updateShop";
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("提交成功！", "", "success");
                    shopHelper.loadList(shopHelper.pageIndex);
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
                action: "deleteshop",
                shopId: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(shopHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功！", "您已经永久删除了这条信息。", "success");
                        shopHelper.loadList(shopHelper.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    updateActive: function (dataId, active) {
        var self = this;
        if (active == 1 && parseInt(this.type) != 2) {
            swal({
                title: "您确定要冻结该总店账号吗",
                text: "请确保它的分店全部已冻结，否则冻结失败！",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "确定",
                closeOnConfirm: false,
            }, function () {
                var param = {
                    action: "UPDATESHOPACTIVE",
                    shopId: dataId,
                    active: active == 1 ? 0 : 1
                }
                hotUtil.loading.show();
                hotUtil.ajaxCall(self.ajaxUrl, param, function (ret, err) {
                    if (ret) {
                        if (ret.status == 200) {
                            swal(param.active == 0 ? "冻结成功" : "激活成功", "", "success");
                            self.loadList(self.pageIndex);
                        }
                        else {
                            swal(param.active == 0 ? "冻结失败" : "冻结成功", param.active == 0 ? "请检查所属分店是否已全部冻结" : "", "warning");
                        }
                    }
                    hotUtil.loading.close();
                });
            });
        }
        else {
            var param = {
                action: "UPDATESHOPACTIVE",
                shopId: dataId,
                active: active == 1 ? 0 : 1
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(self.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal(param.active == 0 ? "冻结成功" : "激活成功", "", "success");
                        self.loadList(self.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        }
    },
    dialog: function (dataId) {
        if (this.reset)
            this.reset.resetForm();
        var data = this.getModel(dataId);
        if (data != null) {
            $("#modal-title").text("编辑");
            $("#shopId").val(dataId);
            $("#shopname").val(data.ShopName);
            $("#shopprov").val(data.ShopProv);
            $("#shopcity").val(data.ShopCity);
            $("#shopaddress").val(data.ShopAddress);
            $("#username").val(data.Contacts);
            $("#usermobile").val(data.ContactWay);
            if (!hotUtil.isNullOrEmpty(data.LoginName))
                $("#userloginname").val(data.LoginName).attr("readonly", "readonly");
        }
        else {
            $("#userloginname").removeAttr("readonly");
            $("#modal-title").text("添加");
            $("#signupForm input").val("");
        }
    }
};


$.validator.setDefaults({
    highlight: function (e) {
        $(e).closest(".form-group").removeClass("has-success").addClass("has-error")
    },
    success: function (e) {
        e.closest(".form-group").removeClass("has-error").addClass("has-success")
    },
    errorElement: "span",
    errorPlacement: function (e, r) {
        e.appendTo(r.is(":radio") || r.is(":checkbox") ? r.parent().parent().parent() : r.parent())
    },
    errorClass: "help-block m-b-none",
    validClass: "help-block m-b-none"
});


$(function () {
    shopHelper.loadList(shopHelper.pageIndex);
    var cityPicker = new IIInsomniaCityPicker({
        data: cityData,//数据在citylist.js 中
        target: '.cityChoice',
        hideCityInput: "#city",
        hideProvinceInput: "#province"
    }).init();


    if (parseInt(shopHelper.type) != 2)
        $("#btnShop").show();



    new IIInsomniaCityPicker({
        data: cityData,//数据在citylist.js 中
        target: '.modal-cityChoice',
        hideCityInput: "#shopcity",
        hideProvinceInput: "#shopprov"
    }).init();


    var e = "<i class='fa fa-times-circle'></i> ";
    shopHelper.reset = $("#signupForm").validate({
        rules: {
            shopname: "required",
            shopcity: "required",
            username: {
                required: !0,
                minlength: 2
            },
            usermobile: "required",
            userloginname: {
                required: !0,
                minlength: 5
            },
            password: {
                minlength: 6
            }
        },
        messages: {
            shopname: e + "请输入门店名称",
            shopcity: e + "请选择所在城市",
            username: {
                required: e + "请输入联系人",
                minlength: e + "联系人必须两个字符以上"
            },
            usermobile: e + "请输入您的手机号码",
            userloginname: {
                required: e + "请输入您的登录名",
                minlength: e + "登录名必须5个字符以上"
            },
            password: {
                minlength: e + "密码必须6个字符以上"
            }
        },
        submitHandler: function (form) {
            shopHelper.edit();
        }
    })
});


