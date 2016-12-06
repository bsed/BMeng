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

var levelHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    loaclData: {
        one: [],
        two: []
    },
    isAlly: 1,
    reset: null,
    loadlevelList: function (type) {
        var self = this;
        var postData = {
            action: "GetLevelList",
            type: type
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        if (type == 1)
                            self.loaclData.two = ret.data.Rows;
                        else
                            self.loaclData.one = ret.data.Rows;                        
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist").html();
                            tempHtml = tempHtml.replace("{NO}", i+1);
                            tempHtml = tempHtml.replace("{LevelName}", item.UL_LevelName);
                            tempHtml = tempHtml.replace("{MemberNum}", item.UL_MemberNum);
                            tempHtml = tempHtml.replace(/{LevelId}/gm, item.UL_ID);
                            tempHtml = tempHtml.replace(/{type}/gm, item.UL_Type);
                            tempHtml = tempHtml.replace("{Level}", i++);
                            tempHtml = tempHtml.replace("{WhereTitle}", type == 1 ? "盟友" : "成交订单");
                            tempHtml = tempHtml.replace("{unit}", type == 1 ? "人" : "次");
                            listhtml += tempHtml;                            
                        });
                        if (type == 1)
                            $("#listMode").html(listhtml);
                        else
                            $("#listMode2").html(listhtml);
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    getModel: function (dataId, type) {
        var model = null;
        if (type == 1) {
            if (this.loaclData.two != null && this.loaclData.two.length > 0) {
                $.each(this.loaclData.two, function (i, item) {
                    if (item.UL_ID == dataId) {
                        model = item;
                        return false;
                    }
                });
            }
        }
        else {
            if (this.loaclData.one != null && this.loaclData.one.length > 0) {
                $.each(this.loaclData.one, function (i, item) {
                    if (item.UL_ID == dataId) {
                        model = item;
                        return false;
                    }
                });
            }
        }
        return model;
    },
    edit: function () {
        var param = hotUtil.serializeForm("#signupForm .form-control");
        param.action = "editlevel";
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("提交成功", "", "success");
                    levelHelper.loadlevelList(parseInt($("#leveltype").val()));
                    $(".close").click();
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
            hotUtil.loading.close();
        });
    },
    del: function (dataId, type) {
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
                action: "DeleteLevel",
                levelId: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(levelHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功", "您已经永久删除了这条信息。", "success");
                        levelHelper.loadlevelList(type);

                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    dialog: function (dataId, type) {
        if (this.reset)
            this.reset.resetForm();
        levelHelper.validate(type);
        this.isAlly = type;
        var data = this.getModel(dataId, type);
        $("#modal-where-lable").text(type == 0 ? "成交订单数量累计达" : "盟友数量累计达");
        $(".spanunit").text(type == 0 ? "次" : "人");
        if (data != null) {
            $("#modal-title").text("编辑" + (type == 0 ? "盟友等级设置" : "盟主等级设置"));
            $("#levelid").val(dataId);
            $("#levelname").val(data.UL_LevelName);
            $("#levelmembernum").val(data.UL_MemberNum);
        }
        else {
            $("#modal-title").text("添加" + (type == 0 ? "盟友等级设置" : "盟主等级设置"));
            $("#signupForm input").val("");
        }
        $("#leveltype").val(type);
    },
    pageInit: function () {
        levelHelper.loadlevelList(1);
        levelHelper.loadlevelList(0);

    },
    validate: function (type) {
        var e = "<i class='fa fa-times-circle'></i> ";
        this.reset = $("#signupForm").validate({
            rules: {
                levelname: {
                    required: !0,
                    minlength: 2
                },
                levelmembernum: {
                    required: !0,
                    digits: true,
                }
            },
            messages: {
                levelname: {
                    required: e + "请输入" + (type == 0 ? "盟友" : "盟主") + "名称",
                    minlength: e + "必须两个字符以上"
                },
                levelmembernum: {
                    required: "请输入升级条件",
                    digits: "只能输入数字"
                }
            },
            submitHandler: function (form) {
                levelHelper.edit();
            }
        })
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
    levelHelper.pageInit();
});


