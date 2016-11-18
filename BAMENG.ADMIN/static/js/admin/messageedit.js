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
    audit: hotUtil.getQuery("audit"),
    loadShop: function () {
        var param = {
            action: "GetMessageShopList"
        }
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    var html = "";
                    $.each(ret.data, function (i, item) {
                        html += "<option value='" + item.ShopID + "'>" + item.ShopName + "</option>";
                    });
                    $("#sendtarget").html(html);
                    if (!hotUtil.isNullOrEmpty(html))
                        $("#div_sendtarget").show();
                }
            }
        });
    },
    loadData: function () {
        var self = this;
        var postData = {
            action: "GetMessageInfo",
            messageid: this.dataId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200 && ret.data) {
                    $("#messagetitle").val(ret.data.Title);

                    $("#sendbelongShop").setChecked(ret.data.IsSendBelongShopId == 1);
                    $("#issend").setChecked(ret.data.IsSend == 1);


                    messageHelper.setEditContent(ret.data.MessageBody);

                    //if (parseInt(self.audit) == 1 && ret.data.ArticleStatus == 0) {
                    //    $(".btn-yes,.btn-no").show();
                    //}

                }
            }
            self.initCheck();
            hotUtil.loading.close();
        });
    },
    edit: function () {
        var sendtarget =$("#sendtarget").val();                
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
                    swal("提交成功", "", "success");
                    if (messageHelper.dataId == 0) {
                        $("#signupForm")[0].reset();
                        messageHelper.setEditContent("");
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
    },
    updateStatus: function (code) {
        swal({
            title: code == 1 ? "您确定要同意吗？" : "您确定要拒绝吗？",
            text: code == 1 ? "同意后将无法恢复，请谨慎操作！" : "请输入拒绝理由",
            type: code == 1 ? "warning" : "input",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: code == 1 ? "同意" : "拒绝",
            cancelButtonText: "我再想想",
            closeOnConfirm: false,
            inputPlaceholder: "理由"
        }, function (inputValue) {
            var param = {
                action: "UpdateArticleCode",
                articleId: messageHelper.dataId,
                type: 4,
                active: code,
                remark: inputValue
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(messageHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        $(".btn-yes,.btn-no").hide();
                        swal("操作成功！", "", "success");
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    }
};



$(document).ready(function () {
    $("#sendbelongShop,#issend").change(function () {
        if ($(this).attr("checked"))
            $(this).setChecked(false);
        else
            $(this).setChecked(true);
    });

    $(".summernote").summernote({ lang: "zh-CN" });

    messageHelper.loadShop();

    messageHelper.loadData();

    if (parseInt(messageHelper.audit) == 1) {
        $(".btn-submit").hide();
    }
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

