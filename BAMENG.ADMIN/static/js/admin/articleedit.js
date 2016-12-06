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




var articleHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    picDir: "bameng/article/img",
    dataId: hotUtil.getQuery("articleid"),
    audit: hotUtil.getQuery("audit"),
    loadData: function () {
        var self = this;
        var postData = {
            action: "GetArticleInfo",
            articleid: this.dataId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200 && ret.data) {
                    $("#articleTitle").val(ret.data.ArticleTitle);
                    $("#articleIntro").val(ret.data.ArticleIntro);
                    $("#articleTop").setChecked(ret.data.EnableTop == 1);
                    $("#articlePublish").setChecked(ret.data.EnablePublish == 1);
                    $("#txtcover").val(ret.data.ArticleCover);
                    $("#articleShopName").val(ret.data.ShopProv + "/" + ret.data.ShopCity + "/" + ret.data.ShopName);


                    $("input[name='radioInline']").each(function (i) {
                        if (i == ret.data.SendTargetId) {
                            $(this).attr("data-check", "true");
                            $(this).attr("checked", "checked");
                            return false;
                        }
                    });


                    articleHelper.setEditContent(ret.data.ArticleBody);

                    if (parseInt(self.audit) == 1 && ret.data.ArticleStatus == 0) {
                        $(".btn-yes,.btn-no").show();
                    }

                }
            }
            self.initCheck();
            hotUtil.loading.close();
        });
    },
    edit: function () {
        var TargetId = 0;
        $("input[name='radioInline']").each(function (i, v) {
            if ($(this).attr("data-check") == "true")
                TargetId = $(this).val();
        });
        var self = this;

        this.upload(function () {
            var postData = {
                action: "EditArticle",
                articleid: articleHelper.dataId,
                top: $("#articleTop").attr("checked") ? 1 : 0,
                publish: $("#articlePublish").attr("checked") ? 1 : 0,
                title: hotUtil.encode($("#articleTitle").val()),
                intro: hotUtil.encode($("#articleIntro").val()),
                content: hotUtil.encode(articleHelper.getEditContent()),
                targetid: TargetId,
                cover: $("#txtcover").val()
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(articleHelper.ajaxUrl, postData, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        if (articleHelper.dataId == 0) {
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
    upload: function (callback) {
        if (!hotUtil.isNullOrEmpty($("#uploadfile").val())) {
            if ($.trim(hotUtil.isNullOrEmpty(this.getEditContent())).length == 0) {
                swal("资讯内容不能为空", "", "warning")
            }
            else {
                hotUtil.loading.show();
                hotUtil.uploadImg("uploadfile", this.picDir, function (url) {
                    hotUtil.loading.close();
                    if (url) {
                        $("#txtcover").val(url);
                        callback();
                    }
                    else
                        swal("图片上传失败", "请检查图片格式是否正确", "warning");
                }, null, 1);
            }
        }
        else {
            if (!hotUtil.isNullOrEmpty($("#txtcover").val())) {
                if ($.trim(hotUtil.isNullOrEmpty(this.getEditContent())).length == 0) {
                    swal("资讯内容不能为空", "", "warning")
                }
                else
                    callback();
            }
            else
                swal("请上传封面", "", "warning");
        }
    },
    updateStatus: function (code) {
        swal({
            title: code == 1 ? "您确定要同意吗？" : "您确定要拒绝吗？",
            text: code == 1 ? "" : "请输入拒绝理由",
            type: code == 1 ? "warning" : "input",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            closeOnConfirm: false,
            inputPlaceholder: "理由"
        }, function (inputValue) {
            if (inputValue) {
                var param = {
                    action: "UpdateArticleCode",
                    articleId: articleHelper.dataId,
                    type: 4,
                    active: code,
                    remark: inputValue
                }
                hotUtil.loading.show();
                hotUtil.ajaxCall(articleHelper.ajaxUrl, param, function (ret, err) {
                    if (ret) {
                        if (ret.status == 200) {
                            $(".btn-yes,.btn-no").hide();
                            swal("操作成功", "", "success");
                        }
                        else {
                            swal(ret.statusText, "", "warning");
                        }
                    }
                    hotUtil.loading.close();
                });
            }
        });
    }

};



$(document).ready(function () {
    $("#articleTop,#articlePublish").change(function () {
        if ($(this).attr("checked"))
            $(this).setChecked(false);
        else
            $(this).setChecked(true);
    });

    $("input[name='radioInline']").change(function () {
        $("input[name='radioInline']").removeAttr("data-check");
        if (!$(this).attr("data-check")) {
            $(this).attr("data-check", "true");
        }
    });

    $('input[type="file"]').prettyFile();
    var smnote = $(".summernote").summernote({
        lang: "zh-CN",
        onImageUpload: function (files, editor, $editable) {
            var formData = new FormData();
            formData.append('file', files[0]);
            var uploadUrl = '/handler/UploadFileEidt.ashx?uploadtype=1&userid=' + articleHelper.picDir;
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
                    setTimeout(function () {
                        hotUtil.loading.close();
                    }, 2000);
                }
            });
        }
    });

    articleHelper.loadData();

    if (parseInt(articleHelper.audit) == 1) {
        $(".btn-submit").hide();
        $("#shopnameshow").show();
    }
    var e = "<i class='fa fa-times-circle'></i> ";
    $("#signupForm").validate({
        rules: {
            articleTitle: {
                required: !0,
                maxlength: 30
            },
            articleIntro: {
                required: !0,
                maxlength: 150
            }
        },
        messages: {
            articleTitle: {
                required: e + "请输入名称",
                maxlength: e + "最长30个字符"
            },
            articleIntro: {
                required: e + "请输入资讯简介",
                maxlength: e + "最长150个字符"
            }
        },
        submitHandler: function (form) {
            articleHelper.edit();
        }
    });
});

