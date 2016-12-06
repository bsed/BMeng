/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/
var _shareData = {
    title: document.title,
    desc: "",
    img_url: "http://" + window.location.host + "/app/images/appShareLogo.png",
    link: window.location.href,
    enable: true
};

//获取分享数据对象
function getShareData() {
    if (/(bmandroid)/i.test(navigator.userAgent)) {
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
    if (/(bmandroid)/i.test(navigator.userAgent)) {
        android.enableShare(_shareData.enable);
    }
}
