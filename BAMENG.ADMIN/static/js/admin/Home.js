/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

$(function () {

    var postData = {
        action: "GetHomeData"
    }
    hotUtil.ajaxCall("handler/HQ.ashx", postData, function (ret, err) {
        if (ret) {
            if (ret.status == 200) {
                if (ret.data != null) {
                    var todayData = ret.data[0];
                    var yesterdayData = ret.data[1];


                    $("#todayAllyCount").text(todayData.NewAllyCount);
                    $("#todayArticleCount").text(todayData.NewArticleCount);
                    $("#todayCustomerCount").text(todayData.NewCustomerCount);
                    $("#todayMessageCount").text(todayData.NewMessageCount);

                    $("#yesterdayAllyCount").text(yesterdayData.NewAllyCount);
                    $("#yesterdayCustomerCount").text(yesterdayData.NewCustomerCount);

                }
            }
        }
    });
})