/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author lgh
**/

var chartsHelper = {
    /**
    *
    * 初始化统计数据
    * @param xAxisData 统计日期数组
    * @param yData 统计数数据数组
    * @param total 总数
    *
    **/
    initCharts: function (title, AxisData) {
        var e = echarts.init(document.getElementById("echarts-line-chart"));
        var u = {
            title: {
                text: "现金券使用金额统计图",
                subtext: "共使用现金券金额：" + AxisData.total + "元",
                x: "center"
            },
            tooltip: {
                trigger: "item",
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: "vertical",
                x: "left",
                data: AxisData.xData
            },
            calculable: !0,
            series: [{
                name: "使用金额",
                type: "pie",
                radius: "55%",
                center: ["50%", "60%"],
                data: AxisData.yData
            }]
        };
        e.setOption(u);

        var date = new Date();
        //$(".login-time").text(date);
        //$(".login-total").text(total);
        $(".chart-info").show();
        $(window).resize(e.resize);
    },
    loadData: function (type) {
        var self = this;
        var param = {
            action: "couponstatisticspie",
            beginTime: $("#beginTime").val(),
            endTime: $("#endTime").val(),
            type: type
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall("/handler/HQ.ashx", param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {                    
                    self.initCharts("", ret.data);
                }
            }
            hotUtil.loading.close();
        });
    },
    tab: function (type) {
        $("#beginTime,#endTime,#createTimePick").val("");
        this.loadData(type);
    }
}
$(function () {
    chartsHelper.loadData(7);
});