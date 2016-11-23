﻿/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/
var _shareData = {
    title: "test",
    desc: "desc",
    img_url: "http://bmadmin.fancat.cn/static/img/mz@3x.png",
    link: "http://www.baidu.com",
    enable: true
};

//获取分享数据对象
function getShareData() {
    if (/(android)/i.test(navigator.userAgent)) {
        android.sendShare(_shareData.title, _shareData.desc, _shareData.link, _shareData.img_url);
        return;
    }
    if (_shareData.enable)
        return _shareData.title + '^' + _shareData.desc + '^' + _shareData.link + '^' + _shareData.img_url;
    else
        return "";
}

//是否启用分享
function enableShare() {
    if (/(android)/i.test(navigator.userAgent)) {
        android.enableShare(_shareData.enable);
    }
}
