
function OnMsgButtonClick(msgID) {
    var e = event || window.event;
    var trgt = e.target || e.srcElement;
    var retString = "button clicked~" + trgt.dataset.buttonresult;

    __doPostBack(msgID, retString);
    //var msgbox = document.getElementById(msgID + "_MsgBox");

    //msgbox.focus();
    //trgt.focus();

    
}

function CloseMessage(msgID) {
    var divMsgBackGround = document.getElementById(msgID + "_divmsgBackground");
    //divMsgBackGround.style("display") = "none";
    __doPostBack(msgID, "close");
}