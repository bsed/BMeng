/// <reference path="../plugins/prettyfile/bootstrap-prettyfile.js" />
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

var focusHelper = {
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
            action: "GetFocusPicList",
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
                            tempHtml = tempHtml.replace("{Description}", item.Description);
                            tempHtml = tempHtml.replace(/{LinkUrl}/g, item.LinkUrl);
                            tempHtml = tempHtml.replace("{Sort}", item.Sort);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            if (!hotUtil.isNullOrEmpty(item.PicUrl))
                                tempHtml = tempHtml.replace("{PicUrl}", item.PicUrl);
                            else
                                tempHtml = tempHtml.replace("{PicUrl}", "/static/img/bg.png");

                            tempHtml = tempHtml.replace("{IsEnable}", item.IsEnable == 1 ? "<span style='color:red;'>已启用</span>" : "已禁用")

                            tempHtml = tempHtml.replace("{ActiveText}", item.IsEnable == 1 ? "禁用" : "启用");


                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init(function (p) {
                            goTo(p, function (page) {
                                focusHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        focusHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        focusHelper.loadList(1);
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
        var self = this;
        this.upload(function () {
            var postData = hotUtil.serializeForm("#signupForm .form-control");
            postData.action = "EditFocusPic";
            postData.type = self.type;
            postData.focusenable = $("#focusenable").attr("checked") ? 1 : 0;
            hotUtil.loading.show();
            hotUtil.ajaxCall(self.ajaxUrl, postData, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        focusHelper.loadList(focusHelper.pageIndex);                        
                        $(".close").click();
                        swal({
                            title: ret.statusText,
                            text: "即将更新...",
                            timer: 1500,
                            showConfirmButton: false
                        }, function () {
                            window.location.reload();
                        });
                    }
                    else
                        swal(ret.statusText, "", "warning");
                }
                hotUtil.loading.close();
            });
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
                action: "DeleteFocusPic",
                focusid: dataId
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(focusHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功", "您已经永久删除了这条信息。", "success");
                        focusHelper.loadList(focusHelper.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    updateActive: function (dataId, obj) {
        $(obj).text("禁用");
        var param = {
            action: "SetFocusEnable",
            focusid: dataId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("设置成功", "", "success");
                    focusHelper.loadList(focusHelper.pageIndex);
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
            $("#modal-title").text("编辑轮播图");
            $("#focusid").val(dataId);
            $("#focustitle").val(data.Title);
            $("#focuspicurl").val(data.PicUrl);
            $("#focuslinkurl").val(data.LinkUrl);
            $("#focusdescription").val(data.Description);
            $("#focussort").val(data.Sort);
            $("#focusenable").setChecked(data.IsEnable == 1);
        }
        else {
            $("#modal-title").text("添加轮播图");
            $("#signupForm input").val("");
        }

    },
    pageInit: function () {
        focusHelper.loadList(focusHelper.pageIndex);
        focusHelper.validate();
        this.initCheck();
    },
    validate: function () {
        var e = "<i class='fa fa-times-circle'></i> ";
        this.reset = $("#signupForm").validate({
            rules: {
                focustitle: "required",
                focuspicurl: "required",
                focussort: "required",
                focusdescription: "required"
            },
            messages: {
                focustitle: e + "请输入标题",
                focuspicurl: e + "请上传图片",
                focussort: e + "请输入排序号",
                focusdescription: e + "请输入说明",

            },
            submitHandler: function (form) {
                focusHelper.edit();
            }
        })
    },
    upload: function (callback) {
        if (!hotUtil.isNullOrEmpty($("#uploadfile").val())) {
            hotUtil.loading.show();
            hotUtil.uploadImg("uploadfile", this.picDir, function (url) {
                hotUtil.loading.close();
                if (url) {
                    $("#focuspicurl").val(url);                    
                    callback();
                }
                else
                    swal("图片上传失败", "请检查图片格式是否正确", "warning");
            }, {
                tbw: 640,
                tbh: 320
            }, 1);
        }
        else {
            if (!hotUtil.isNullOrEmpty($("#focuspicurl").val()))
                callback();
            else
                swal("请上传焦点图", "", "warning");
        }
    },
    initCheck: function () {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html);
        });
    }
};

$(function () {
    focusHelper.pageInit();

    $('input[type="file"]').prettyFile();

    $("#focusenable").change(function () {
        if ($(this).attr("checked"))
            $(this).setChecked(false);
        else
            $(this).setChecked(true);
    });
});


