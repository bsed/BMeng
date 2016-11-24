/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />
/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
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
    initCharts: function (title,AxisData, total) {
        var e = echarts.init(document.getElementById("echarts-line-chart"));
        var a = {
            title: {
                text: ""
            },
            tooltip: {
                trigger: "axis"
            },
            legend: {
                data: ["客户信息总量", "有效客户信息量", "无效客户信息量"]
            },
            grid: {
                x: 40,
                x2: 40,
                y2: 24
            },
            calculable: !0,
            xAxis: [{
                type: "category",
                boundaryGap: !1,
                data: AxisData[0].xData
            }],
            yAxis: [{
                type: "value",
                axisLabel: {
                    formatter: "{value}"
                }
            }],
            series: [{
                name: "客户信息总量",
                type: "line",
                data: AxisData[0].yData,
                markPoint: {
                    data: [{
                        type: "max",
                        name: "最大值"
                    },
                    {
                        type: "min",
                        name: "最小值"
                    }]
                },
                markLine: {
                    data: [{
                        type: "average",
                        name: "平均值"
                    }]
                }
            },
            {
                name: "有效客户信息量",
                type: "line",
                data: AxisData[1].yData,
                markLine: {
                    data: [{
                        type: "average",
                        name: "平均值"
                    }]
                }
            },
            {
                name: "无效客户信息量",
                type: "line",
                data: AxisData[2].yData,
                markLine: {
                    data: [{
                        type: "average",
                        name: "平均值"
                    }]
                }
            }]
        };
        e.setOption(a);        

        var date = new Date();
        $(".login-time").text(date);        
        $(".login-total").text(total);
        $(".chart-info").show();
        $(window).resize(e.resize);
    },
    loadData: function (type) {
        var self = this;
        var param = {
            action: "CustomerStatistics",
            beginTime: $("#beginTime").val(),
            endTime: $("#endTime").val(),
            type: type
        }        
        hotUtil.loading.show();
        hotUtil.ajaxCall("/handler/HQ.ashx", param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    var total = 0;
                    $.each(ret.data, function (i, item) {
                        total += item.total
                    });
                    self.initCharts("", ret.data, total);
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