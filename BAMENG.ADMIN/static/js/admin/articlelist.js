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

var articleHelper = {
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
            action: "GetArticleList",
            pageIndex: page,
            pageSize: 20,
            key: $("#keyword").val(),
            searchType: 5,
            startTime: "",
            endTime: "",
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
                            var tempHtml = parseInt(self.type) == 0 ? $("#templist2").html() : $("#templist").html();
                            tempHtml = tempHtml.replace("{NO}", i + 1);
                            tempHtml = tempHtml.replace("{ArticleTitle}", item.ArticleTitle);
                            if (!hotUtil.isNullOrEmpty(item.ArticleCover))
                                tempHtml = tempHtml.replace("{ArticleCover}", item.ArticleCover);
                            else
                                tempHtml = tempHtml.replace("{ArticleCover}", "/static/img/bg.png");
                            tempHtml = tempHtml.replace(/{ArticleId}/gm, item.ArticleId);
                            tempHtml = tempHtml.replace("{PublishTime}", item.PublishTime);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            tempHtml = tempHtml.replace("{SendTargetId}", item.SendTargetId == 0 ? "所有人" : item.SendTargetId == 1 ? "盟主" : "盟友");
                            tempHtml = tempHtml.replace("{EnableTop}", item.EnableTop == 1 ? "<span style='color:red;'>已置顶</span>" : "未置顶")
                            if (item.ArticleStatus==1)
                                tempHtml = tempHtml.replace("{EnablePublish}", item.EnablePublish == 1 ? "<span style='color:red;'>已发布</span>" : "未发布")
                            else
                                tempHtml = tempHtml.replace("{EnablePublish}", item.ArticleStatus == 0 ? "审核中" : "审核失败")
                            tempHtml = tempHtml.replace("{publishText}", item.EnablePublish == 1 ? "撤回" : "发布");
                            tempHtml = tempHtml.replace("{topText}", item.EnableTop == 1 ? "取消置顶" : "设置置顶");
                            tempHtml = tempHtml.replace("{publish}", item.EnablePublish);
                            tempHtml = tempHtml.replace("{top}", item.EnableTop);

                            tempHtml = tempHtml.replace("{ShopName}", item.ShopName);
                            tempHtml = tempHtml.replace("{SendTarget}", item.SendTargetId == 1 ? "盟主" : item.SendTargetId == 0 ? "所有人" : "盟友");
                            tempHtml = tempHtml.replace("{ArticleStatus}", item.ArticleStatus == 1 ? "审核通过" : item.ArticleStatus == 0 ? "申请中" : "审核失败");

                            tempHtml = tempHtml.replace(/{display}/gm, item.ArticleStatus == 0 ? "" : "display:none");


                            listhtml += tempHtml;
                        });
                        if (parseInt(self.type) != 0)
                            $("#listMode").html(listhtml);
                        else
                            $("#listMode2").html(listhtml);
                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total, 7);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                articleHelper.loadList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    search: function () {
        articleHelper.loadList(1);
    },
    searchAll: function () {
        $("#keyword").val("");
        articleHelper.loadList(1);
    },
    getModel: function (dataId) {
        var model = null;
        if (this.loaclData != null && this.loaclData.length > 0) {
            $.each(this.loaclData, function (i, item) {
                if (item.ArticleId == dataId) {
                    model = item;
                    return false;
                }
            });
        }
        return model;
    },
    edit: function (dataId) {
        var data = this.getModel(dataId);
        if (data != null)
            hotUtil.newTab("admin/articleedit.html?articleid=" + dataId, "编辑资讯[" + data.ArticleTitle + "]");
        else
            hotUtil.newTab("admin/articleedit.html?articleid=" + dataId, "添加资讯");
    },
    audit: function (dataId) {
        var data = this.getModel(dataId);
        hotUtil.newTab("admin/articleedit.html?audit=1&articleid=" + dataId, "资讯详情[" + data.ArticleTitle + "]");
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
                action: "UpdateArticleCode",
                articleId: dataId,
                type: 3,
                active: 0
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(articleHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("删除成功！", "您已经永久删除了这条信息。", "success");
                        articleHelper.loadList(articleHelper.pageIndex);
                    }
                    else {
                        swal(ret.statusText, "", "warning");
                    }
                }
                hotUtil.loading.close();
            });
        });
    },
    enablePublish: function (dataId, active) {
        var param = {
            action: "UpdateArticleCode",
            articleId: dataId,
            type: 2, //1修改置顶，2修改发布，3，删除
            active: active == 1 ? 0 : 1
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(articleHelper.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("设置成功！", "", "success");
                    $("#enablePublish_" + dataId).attr('data-original-title', active == 1 ? "撤回" : "发布");
                    articleHelper.loadList(articleHelper.pageIndex);
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
            hotUtil.loading.close();
        });
    },
    enableTop: function (dataId, active) {
        var param = {
            action: "UpdateArticleCode",
            articleId: dataId,
            type: 1,
            active: active == 1 ? 0 : 1
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(articleHelper.ajaxUrl, param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    swal("设置成功！", "", "success");
                    $("#enableTop_" + dataId).attr('data-original-title', active == 0 ? "取消置顶" : "设置置顶");
                    articleHelper.loadList(articleHelper.pageIndex);
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
            hotUtil.loading.close();
        });
    },
    updateStatus: function (dataId, code) {
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
                articleId: dataId,
                type: 4,
                active: code,
                remark: inputValue
            }
            hotUtil.loading.show();
            hotUtil.ajaxCall(articleHelper.ajaxUrl, param, function (ret, err) {
                if (ret) {
                    if (ret.status == 200) {
                        swal("操作成功！", "", "success");
                        articleHelper.loadList(articleHelper.pageIndex);
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


$(function () {

    if (parseInt(articleHelper.type) == 0) {
        $("#btnAddArticle,#articleSuccess").hide();
        $("#applyArticle").show();
    }

    articleHelper.loadList(articleHelper.pageIndex);
});


