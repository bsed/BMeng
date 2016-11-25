/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/// <reference path="shareConfig.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved. 
**/

var articleInfoHelper = {
    idt: hotUtil.getQuery("idt"),
    show: function () {
        $("#loadBox").show();
    },
    hide: function () {
        $("#loadBox").hide();
    },
    load: function () {
        var self = this;
        var postData = {
            auth: hotUtil.auth(),
            articleId: hotUtil.getQuery("articleId")
        }
        self.show();
        hotUtil.ajaxCall("/handler/articleinfo.ashx", postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    document.title = ret.data.ArticleTitle;
                    $(".demos-title").text(ret.data.ArticleTitle);
                    $("#articleTime").text(ret.data.PublishTime);
                    $("#articleAmount").text(ret.data.BrowseAmount);
                    $("#articleInfo").html(ret.data.ArticleBody);
                    if (parseInt(articleInfoHelper.idt) == 4 || parseInt(articleInfoHelper.idt) == 3) {
                        $("#authorName").text(ret.data.AuthorName);
                    }
                    else {
                        _shareData.title = ret.data.ArticleTitle;
                        _shareData.desc = ret.data.ArticleIntro;
                        _shareData.img_url = ret.data.ArticleCover;
                    }
                    $(".bodyContent").show();
                }
                else
                    $.alert(ret.statusText);
            }
            self.hide();
        });
    }
}

$(function () {
    _shareData.enable = false;
    if (parseInt(articleInfoHelper.idt) == 4 || parseInt(articleInfoHelper.idt) == 3) {
        $("#spanAmount").hide();
        $("#spanMessage").show();
    }
    else
        enableShare();
    articleInfoHelper.load();
});