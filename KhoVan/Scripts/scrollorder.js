
function gotooldpos() {
    var l = location + "";
    var sr = "scrolltop=";

    var pos = l.indexOf(sr);
    if (pos < 0) return;
    var p = l.substring(pos + sr.length, l.length);
    $('html,body').animate({
        scrollTop: p
    });
}
function getlinkscroll(e) {
    e = e + "";

    var sr = "&scrolltop=";
    var vt = e.indexOf(sr);
    if (vt < 0)
        return e + "&scrolltop=" + $(window).scrollTop();
    else {
        e = e.substring(0, vt);
        return e + "&scrolltop=" + $(window).scrollTop();
    }
}
$(document).ready(function () {
    setTimeout(gotooldpos, 100);
})