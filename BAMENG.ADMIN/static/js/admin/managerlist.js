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

var managerHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: [],
    pageIndex: 1,
    reset: null,
    loadList: function (page) {
        var self = this;
        self.loaclData = [];
        this.pageIndex = page;
        var postData = {
            action: "GetManagerList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val()
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
                            tempHtml = tempHtml.replace(/{UserId}/gm, item.ID);
                            tempHtml = tempHtml.replace("{UserName}", item.UserName);
                            tempHtml = tempHtml.replace(/{UserEmail}/g, item.UserEmail);
                            tempHtml = tempHtml.replace("{UserMobile}", item.UserMobile);
                            tempHtml = tempHtml.replace("{LastLoginTime}", item.LastLoginTime);
                            tempHtml = tempHtml.replace("{RoleName}", item.RoleName);
                            tempHtml = tempHtml.replace("{ActiveStatus}", item.UserStatus == 1 ? "<span style='color:red;'>激活</span>" : "已冻结")
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                managerHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        managerHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        managerHelper.loadList(1);
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
        param.action = "EditManager";
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("提交成功！", "", "success");
                    managerHelper.loadList(managerHelper.pageIndex);
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
                action: "DeleteManager",
                userid: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(managerHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功！", "您已经永久删除了这条信息。", "success");
                        managerHelper.loadList(managerHelper.pageIndex);
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
        var param = {
            action: "SetManagerUserStatus",
            userid: dataId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("提交成功！", "", "success");
                    managerHelper.loadList(managerHelper.pageIndex);
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
            hotUtil.loading.close();
        });
    },
    dialog: function (dataId) {
        if (this.reset)
            this.reset.resetForm();
        var data = this.getModel(dataId);
        if (data != null) {
            $("#modal-title").text("编辑管理员信息");
            $("#userid").val(dataId);
            $("#name").val(data.UserName);
            $("#email").val(data.UserEmail);
            $("#mobile").val(data.UserMobile);
            if (!hotUtil.isNullOrEmpty(data.LoginName))
                $("#loginName").val(data.LoginName).attr("readonly", "readonly");
        }
        else {
            $("#loginName").removeAttr("readonly");
            $("#modal-title").text("添加管理员信息");
            $("#signupForm input").val("");
        }
    },
    pageInit: function () {
        managerHelper.loadList(managerHelper.pageIndex);
        managerHelper.validate();
    },
    validate: function () {
        var e = "<i class='fa fa-times-circle'></i> ";
        this.reset = $("#signupForm").validate({
            rules: {
                name: {
                    required: !0,
                    minlength: 2
                },
                mobile: "required",
                loginName: {
                    required: !0,
                    minlength: 5
                },                
                password: {
                    minlength: 6
                }
            },
            messages: {
                name: {
                    required: e + "请输入姓名",
                    minlength: e + "联系人必须两个字符以上"
                },
                mobile: e + "请输入您的手机号码",
                loginName: {
                    required: e + "请输入您的登录名",
                    minlength: e + "登录名必须5个字符以上"
                },                
                password: {
                    minlength: e + "密码必须6个字符以上"
                }
            },
            submitHandler: function (form) {
                managerHelper.edit();
            }
        })
    }
};

$(function () {
    managerHelper.pageInit();
});


