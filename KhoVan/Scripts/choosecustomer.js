function choosecustomer() {

    try {
        var url = "/CustomerC/ChooseCustomer";
        var h = 400;
        var w = 600;
        showtc(url, w, h);
    } catch (e) {
        alert(e);
    }

}
function choosecustomerurl(url) {

    try {
        var h = 400;
        var w = 600;
        showtc(url, w, h);
    } catch (e) {
        alert(e);
    }

}
function setSearchResult(returnValue) {
    try {
        $('#CustomerID').combobox('setValues', [returnValue, returnValue]);
        $("#" + CustomerTextID).blur();


        tcboxclose();
    } catch (e) {
        alert(e);
    }

}
