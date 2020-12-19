function fixnumber(e) {
    var s = "";
    var v = $(e).val();
    for (var i = v.length - 1; i >= 0; i--) {
        if (isNaN(v[i]) == false) {
            s = v[i] + s;
        }
    }
    if (s == "")
        s = "0";

    $(e).val(s);

}

$(document).ready(function () {
    $(".txtquantity").attr("maxlength", "5");
    $(".txtquantity").attr("type", "number");
    $(".txtquantity").attr({ "min": 0 });

    $(".txtquantity").keydown(function () {

        setTimeout(fixnumber, 100, $(this));
    })
    //if (isvalidbrowser() == false)
    //    location = "/QR/NotSupport";
})
function fixleftdiv() {
    $(".leftdiv").removeClass("clear");
    var arr = $(".leftdiv").get();
    var w = 0;
    if (arr.length == 0) return;
    var p = $(arr[0]).parent();
    var wt = $(p).width();

    for (var i = 0; i < arr.length; i++) {
        if ($(arr[i]).height() < 10) {
            $(arr[i]).hide();
            continue;
        }
        w = w + $(arr[i]).width() + 20;
        if (w > wt) {
            $(arr[i]).addClass("clear");
            w = $(arr[i]).width() + 20;
        }
    }
    if ($(window).width() < 600)
        $(".leftdiv").addClass("clear");


}

function addparamater(url, paramater) {
    for (var i = 0; i < paramater.length; ++i) {
        if (url.indexOf("?") < 0)
            url = url + "?" + paramater[i].name + "=" + paramater[i].value;
        else
            url = url + "&" + paramater[i].name + "=" + paramater[i].value;


    }
    return url;
}

function msieversion() {

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
    {
        return true;
    }
    else  // If another browser, return 0
    {
        return false;
    }

    return false;
}
function commentorder(orderid) {
    var url = '/home/commentorder?id=' + orderid;
    showtc(url, 600, 300);

}
function logorders(orderid) {
    var url = '/admin/logorders?id=' + orderid;
    showtc(url, 600, 400);

}
function formatnumber(e) {

    e = e + "";
    while (e.indexOf(",") >= 0)
        e = e.replace(",", "");

    var len = (e + "").length;
    var result = "";
    var c = 0;
    for (var i = len - 1; i >= 0; i--) {

        result = e[i] + result;
        if (c == 2 && i > 0 && i < len - 1)
            result = "," + result;
        c = (c + 1) % 3;
    }
    return result;
}

function isvalidbrowser() {
    var valid = false;
  // Opera 8.0+
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    if (isOpera == true)
        valid = true;

    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';
    if (isFirefox == true)
        valid = true;

    // Safari 3.0+ "[object HTMLElementConstructor]" 
    var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || safari.pushNotification);
    if (isSafari == true)
        valid = true;
    
    // Chrome 1+
    var isChrome = !!window.chrome && !!window.chrome.webstore;
    if (isChrome == true) {
        valid = true;

    }
    valid = true;
    return valid;

}
function checkdulicaterow(ob) {
    var arr = new Array();
    var trung = false;
    var dstrung = "";

    var lines = ob.split('\n');
    for (var i = 0; i < lines.length; i++) {
        var e = lines[i].trim().split('\t')[0];
        for (var j = 0; j < arr.length; j++) {
            if (e == arr[j]) {
                if (dstrung != "")
                    dstrung += ", " + e;
                else
                    dstrung = e;

                trung = true;
                break;
            }
        }
        if (trung == true) {

        }
        arr[arr.length] = e;
    }
    if (trung == true) {
       
        alert(dstrung + " bị trùng");
        return false;
    }
    return true;
}
