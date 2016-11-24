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
    initCharts: function (title,xAxisData, yData, total) {
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
                data: ["周一", "周二", "周三", "周四", "周五", "周六", "周日"]
            }],
            yAxis: [{
                type: "value",
                axisLabel: {
                    formatter: "{value} °C"
                }
            }],
            series: [{
                name: "客户信息总量",
                type: "line",
                data: [11, 11, 15, 13, 12, 13, 10],
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
                data: [1, -2, 2, 5, 3, 2, 0],
                markPoint: {
                    data: [{
                        name: "周最低",
                        value: -2,
                        xAxis: 1,
                        yAxis: -1.5
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
                name: "无效客户信息量",
                type: "line",
                data: [1, -2, 2, 5, 15, 2, 0],
                markPoint: {
                    data: [{
                        name: "周最低",
                        value: -2,
                        xAxis: 1,
                        yAxis: -1.5
                    }]
                },
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
    loadLoginData: function (type) {
        var self = this;
        var param = {
            action: "loginstatistics",
            beginTime: $("#beginTime").val(),
            endTime: $("#endTime").val(),
            type: type
        }        
        hotUtil.loading.show();
        hotUtil.ajaxCall("/handler/HQ.ashx", param, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {

                    self.initCharts("", ret.data.xData, ret.data.yData, ret.data.total);
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