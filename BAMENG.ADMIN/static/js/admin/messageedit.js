/// <reference path="../plugins/switchery/switchery.js" />
/// <reference path="../plugins/summernote/summernote.min.js" />
/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
**/

var messageHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    dataId: hotUtil.getQuery("messageid"),
    type: hotUtil.getQuery("type"),
    identity: 0,
    loadShop: function () {
        var param = {
            action: "GetMessageShopList"
        }
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    var html = "";
                    if (ret.data.useridentity != 2) {
                        $.each(ret.data.list, function (i, item) {
                            html += "<option value='" + item.ShopID + "'>" + item.ShopName + "</option>";
                        });
                        $("#sendtarget").html(html);
                        if (!hotUtil.isNullOrEmpty(html))
                            $("#div_sendtarget").show();
                    }
                    messageHelper.identity = ret.data.useridentity;
                }
            }
        });
    },
    loadData: function () {
        var self = this;
        var postData = {
            action: "GetMessageInfo",
            messageid: this.dataId,
            type: this.type
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200 && ret.data) {
                    $("#messagetitle").val(ret.data.Title);

                    $("#sendbelongShop").setChecked(ret.data.IsSendBelongShopId == 1);
                    $("#issend").setChecked(ret.data.IsSend == 1);

                    if (ret.data.IsSendBelongShopId == 1 && ret.data.IsSend == 1) {
                        $("#issendshopSelect").hide();
                        $("#issendshopText").show();
                    }
                    if (ret.data.IsSend == 1) {
                        $("#issendSelect").hide();
                        $("#issendText").show();
                        $("#sendtarget").attr("disabled", "disabled");
                    }
                    if (!hotUtil.isNullOrEmpty(ret.data.SendTargetIds)) {
                        $("#sendtarget").val(ret.data.SendTargetIds.split(","));
                    }
                    messageHelper.setEditContent(ret.data.MessageBody);
                }
            }
            self.initCheck();
            hotUtil.loading.close();
        });
    },
    edit: function () {
        var sendtarget = $("#sendtarget").val();
        if (messageHelper.identity != 2) {
            if (!$("#sendbelongShop").attr("checked")) {
                if (sendtarget == null) {
                    swal("请选择发送对象", "", "warning");
                    return false;
                }
            }
            else {
                if (sendtarget == null)
                    sendtarget = "";
            }
        }
        else
            sendtarget = "";
        var self = this;
        var postData = {
            action: "EditMessage",
            messageid: messageHelper.dataId,
            issend: $("#issend").attr("checked") ? 1 : 0,
            sendbelongshop: $("#sendbelongShop").attr("checked") ? 1 : 0,
            title: hotUtil.encode($("#messagetitle").val()),
            sendtarget: sendtarget.toString(),
            content: hotUtil.encode(messageHelper.getEditContent()),
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(messageHelper.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (messageHelper.dataId == 0) {
                        swal({
                            title: "提交成功",
                            text: "即将更新...",
                            timer: 1500,
                            showConfirmButton: false
                        }, function () {
                            window.location.reload();
                        });
                    }
                    else {
                        swal("提交成功", "", "success");
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    getEditContent: function () {
        return $(".summernote").code();
    },
    setEditContent: function (content) {
        $(".summernote").code(content)
    },
    initCheck: function () {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html);
        });
    }
};



$(document).ready(function () {

    if (hotUtil.GetCookie("SHOP_INDENTITY") == "1") {
        $("#div_sendbelongShop").show();
    }

    $("#sendbelongShop,#issend").change(function () {
        if ($(this).attr("checked"))
            $(this).setChecked(false);
        else
            $(this).setChecked(true);
    });

    $(".summernote").summernote({
        lang: "zh-CN",
        onImageUpload: function (files, editor, $editable) {
            var formData = new FormData();
            formData.append('file', files[0]);
            var uploadUrl = '/handler/UploadFileEidt.ashx?uploadtype=1&userid=bameng/article/img';
            hotUtil.loading.show();
            $.ajax({
                url: uploadUrl,//后台文件上传接口        
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    var obj = eval('(' + data + ')');
                    editor.insertImage($editable, obj.fileUrl);
                    hotUtil.loading.close();
                }
            });
        }
    });


    if (parseInt(messageHelper.type) == 2) {
        $(".btn-submit").hide();
        $("#div_sendtarget,#div_issend,#div_sendbelongShop").hide();
    }
    else
        messageHelper.loadShop();
    messageHelper.loadData();
    var e = "<i class='fa fa-times-circle'></i> ";
    $("#signupForm").validate({
        rules: {
            messagetitle: {
                required: !0,
                minlength: 2
            },
            articleIntro: "required"
        },
        messages: {
            messagetitle: {
                required: e + "请输入名称",
                minlength: e + "联系人必须两个字符以上"
            },
            articleIntro: e + "请输入您的手机号码"
        },
        submitHandler: function (form) {
            messageHelper.edit();
        }
    });
});

