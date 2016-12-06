/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/// <reference path="shareConfig.js" />
/// <reference path="../jquery.min.js" />
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
                    var data = ret.data.ArticleBody;
                    
                    var reg = /<img.*?src="([^"]+)"/ig;
                    var m = data.match(new RegExp(reg));
                    if (m != null) {
                        for (var i = 0; i < m.length; i++) {
                            var result = new RegExp(reg).exec(m[i]);
                            if (result != null) {
                                var url = result[1];
                                data=data.replace(url, "images/none.png?v=" + i);
                            }
                        }
                    }
                    $("#articleInfo").html(data);
                    if (parseInt(articleInfoHelper.idt) == 4 || parseInt(articleInfoHelper.idt) == 3) {
                        $("#authorName").text(ret.data.AuthorName);
                    }
                    else {
                        _shareData.title = ret.data.ArticleTitle;
                        _shareData.desc = ret.data.ArticleIntro;
                        _shareData.img_url = ret.data.ArticleCover;
                    }
                    $(".bodyContent").show();
                    //setTimeout(function () {
                    //    var reg2 = /<img.*?data-bm-src="([^"]+)"/ig;
                    //    var m2 = data.match(new RegExp(reg2));
                    //    if (m2 != null) {
                    //        for (var i = 0; i < m2.length; i++) {
                    //            var u = "images/none.png?v=" + i;
                    //            var result2 = new RegExp(reg2).exec(m2[i]);
                    //            if (result2 != null) {
                    //                var url2 = result2[1];
                    //                data = data.replace(u, url2);
                    //                $("#articleInfo").html(data);
                    //            }
                    //        }
                    //    }
                    //}, 1000);


                    $(".scrollLoading").load(function () {
                        //图片默认隐藏  
                        $(this).hide();
                        //使用fadeIn特效  
                        $(this).stop().fadeIn("5000");
                    });
                    // 异步加载图片，实现逐屏加载图片
                    $(".scrollLoading").scrollLoading();

                }
                else
                    $.alert(ret.statusText);
            }
            self.hide();
        });
    }
}

$(function () {
    if (parseInt(articleInfoHelper.idt) == 4 || parseInt(articleInfoHelper.idt) == 3) {
        $("#spanAmount").hide();
        $("#spanMessage").show();
        _shareData.enable = false;
    }
    else
        enableShare();
    articleInfoHelper.load();
});