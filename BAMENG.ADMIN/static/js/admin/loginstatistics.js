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
                text: title
            },
            tooltip: {
                trigger: "axis"
            },
            grid: {
                x: 30,
                x2: 40,
                y2: 24
            },
            calculable: !0,
            xAxis: [{
                type: "category",
                boundaryGap: !1,
                data: xAxisData,                
            }],
            yAxis: [{
                type: "value",
                axisLabel: {
                    formatter: "{value}"
                }
            }],
            series: [{
                name: "登录次数",
                type: "line",
                data: yData,
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

$(function () {
    chartsHelper.loadData(7);
});