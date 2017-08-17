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
    cid: hotUtil.getQuery("cid", 0),
    loadList: function (type) {
        var self = this;
        var postData = {
            action: "GetAssertList",
            cid: self.cid
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist").html();
                            tempHtml = tempHtml.replace("{NO}", i + 1);
                            tempHtml = tempHtml.replace("{AssertContent}", item.AssertContent);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);
                    }
                }
            }
            hotUtil.loading.close();
        });
    }
};
$(function () {
    levelHelper.loadList();
});


