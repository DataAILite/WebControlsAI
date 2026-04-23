var selIdx = -1;
var isIE = false;

function AdjustScrollPosition(listID) {
    var selected = document.getElementById(listID + "_hfSelectedIndex").value;
    var scrollTop = document.getElementById(listID + "_hfScrollTop").value;
    var IsFocused = document.getElementById(listID + "_hfIsFocused").value;
    var list = document.getElementById(listID + "_InnerList");
    var outlist = document.getElementById(listID);

    if (list != void 0 && parseInt(selected) > -1) {

        var scrlDiff = list.scrollHeight - list.clientHeight;
        if (scrlDiff > 0) {
            var itemHeight = Math.round(list.scrollHeight / list.childNodes.length);
            var minVisibleIndex = Math.round(scrollTop / itemHeight);
            var maxVisibleItems = Math.round(outlist.clientHeight / itemHeight)-2;
            var maxVisibleIndex = minVisibleIndex + maxVisibleItems-1;
            if (selected > maxVisibleIndex) {
                //scrollTop = (selected - maxVisibleItems + 3) * itemHeight;
                scrollTop = (selected - maxVisibleItems) * itemHeight;
            }
            else if (selected < minVisibleIndex) {
                scrollTop = selected * itemHeight;
            }
            document.getElementById(listID + "_hfScrollTop").value = scrollTop;
            doScroll(listID, scrollTop);
        }
        if (IsFocused == "1") {
            list.children[parseInt(selected)].focus();
        }
    }
}
function checkPostBack(listID) {
    var hfPostBack = document.getElementById(listID + "_hfDoPostBack");
    var hfAdjustScroll = document.getElementById(listID + "_hfAdjustScroll");
    var selected = document.getElementById(listID + "_hfSelectedIndex").value;

    if (hfPostBack != void 0) {
         if (hfPostBack.value == "1") {
            hfPostBack.value = "0";
            AdjustScrollPosition(listID);
            var scrollTop = document.getElementById(listID + "_InnerList").scrollTop;
            __doPostBack(listID, selected + "~" + scrollTop);
         }
         else if (hfAdjustScroll.value == "1") {
             hfAdjustScroll.value = "0";
             AdjustScrollPosition(listID);
         }
    }
 }
function doScroll(listID, scrollTop) {
    var ul = document.getElementById(listID + "_InnerList")

    if (ul != void 0 && scrollTop != void 0)
        ul.scrollTop = scrollTop;
}
function OnItemFocus(listID) {
    var src = event.srcElement;
    if (src.tagName == "LI") {
        //OnItemClick(listID);
        //alert(src.innerText + " has focus.")
        //document.getElementById(listID + "_hfIsFocused").value = 1;
    }
}
function OnKeyPressed() {
    event.stopImmediatePropagation();
}

function OnItemNavigate(listID) {
    var e = event || window.event;
    var trgt = e.target || e.srcElement;
    var keyCode = e.keyCode;
    var hfSelectedIndex = document.getElementById(listID + "_hfSelectedIndex");
    var selectedIndex = parseInt(hfSelectedIndex.value);
    //var evt = new Event("Change", { bubbles: true });
    var evt = document.createEvent("HTMLEvents");
    evt.initEvent("change", true, true);

    if ((keyCode == 38 || keyCode == 40) && selectedIndex > -1) {
        e.preventDefault();
        e.stopPropagation();

        var list = document.getElementById(listID + "_InnerList");
        var outlist = document.getElementById(listID);
        var nItems = list.children.length;
        var autopostback = (list.getAttribute("AutoPostBack") == "true");
        var itemHeight = 0;
        var maxVisibleItems = nItems;
        var maxScrollTop = 0;

        if (list.scrollHeight > list.clientHeight) {
            itemHeight = Math.round(list.scrollHeight / nItems);
            maxVisibleItems = Math.round(outlist.clientHeight / itemHeight)-2;
            maxScrollTop = (parseInt(nItems) * itemHeight) - (parseInt(maxVisibleItems) * itemHeight);
        }
        switch (keyCode) {
            case 38: // up
                selectedIndex--;
                if (selectedIndex > -1) {
                     hfSelectedIndex.value = selectedIndex.toString();
                    list.children[selectedIndex + 1].className = "li";
                    list.children[selectedIndex].className = "selected"
                    list.children[selectedIndex].focus();
                    outlist.setAttribute("data-selectedindex", selectedIndex)
                    outlist.dispatchEvent(evt);
                    if (autopostback) {
                        __doPostBack(listID, selectedIndex + "~" + list.scrollTop);
                    }
                }
                break;
            case 40: // down
                selectedIndex++;
                if (selectedIndex < nItems) {
                    hfSelectedIndex.value = selectedIndex.toString();
                    list.children[selectedIndex - 1].className = "li";
                    list.children[selectedIndex].className = "selected"
                    list.children[selectedIndex].focus();
                    outlist.setAttribute("data-selectedindex", selectedIndex)
                    outlist.dispatchEvent(evt);
                    if (autopostback) {
                       __doPostBack(listID, selectedIndex + "~" + list.scrollTop);
                    }
                }
                break;
        }
    }
}
function OnDblClick(listID) {
    var e = event || window.event;
    var trgt = e.target || e.srcElement;
    var outlist = document.getElementById(listID);
    var evt = document.createEvent("HTMLEvents");
    evt.initEvent("dblclick", true, true);
    if (trgt.tagName == "LI") {
        outlist.dispatchEvent(evt);
    }

}
function OnItemClick(listID) {
    var e = event || window.event;
    var trgt = e.target || e.srcElement;
    var hfSelectedIndex = document.getElementById(listID + "_hfSelectedIndex");
    var selectedIndex = hfSelectedIndex.value;
    var hfScrollTop = document.getElementById(listID + "_hfScrollTop")
    var outlist = document.getElementById(listID);
    //var evt = new Event("Change", { bubbles: true });
    var evt = document.createEvent("HTMLEvents");
    evt.initEvent("change", true, true);

    if (trgt.tagName == "LI") {
        var ul = trgt.parentElement;
        var autopostback = (ul.getAttribute("AutoPostBack")=="true");
        var scrollTop = ul.scrollTop;
        var selected = trgt.getAttribute("index");
        ul.focus();
        trgt.focus();
        if (selected != selectedIndex) {
            if (selectedIndex != "" && selectedIndex != "-1")
                ul.children[selectedIndex].className = "li";
            trgt.className = "selected";
            hfSelectedIndex.value = selected;
            hfScrollTop.value = scrollTop;
            //var fnstr = "CallServer" + listID;
            //var fn = window[fnstr];
            //if (typeof fn == "function")
            //  fn(selected + "_" + listID);
            outlist.setAttribute("data-selectedindex", selected)
            outlist.dispatchEvent(evt);
            if (autopostback) {
                __doPostBack(listID, selected + "~" + scrollTop);
            }

        }
    }
}

function OnItemBlur(listID) {
    var src = event.srcElement;
    if (src.tagName == "LI") {
        //alert(src.innerText + " has lost focus.")
        document.getElementById(listID + "_hfIsFocused").value = 0;
    }
}
function ReceiveServerData(retValue) {

}
function drag(ev) {
    var clientRect = ev.target.getBoundingClientRect();
    var offsetX = 0;
    var offsetY = 0;
    if (clientRect.x != void 0) {
        offsetX = parseInt((ev.clientX - clientRect.left), 10);
        offsetY = parseInt((ev.clientY - clientRect.top), 10);
        isIE = false;
    }
    else {
        offsetX = parseInt((ev.clientX - clientRect.left + 4), 10);
        offsetY = parseInt((ev.clientY - clientRect.top - 6), 10);
        isIE = true;
    }
    ev.dataTransfer.dropEffect = 'copy';
    ev.dataTransfer.setData("text", ev.target.id + "," + offsetX + "," + offsetY);
    //if (!isIE)
    //    ev.dataTransfer.setDragImage(ev.target, 0, 0);

    return false;

}
function reindexList(list) {
    var li
    for (var i = 0; i < list.children.length; i++) {
        li = list.children[i];
        li.setAttribute("index", i);
    }
}
function allowDrop(ev) {
    ev.preventDefault();
}
function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    var params = data.split(",");
    var id = params[0];
    var idParts = id.split("_");
    var trgt = ev.currentTarget;
    var list = trgt.parentNode;
    var listID = idParts[0];
    var outList = document.getElementById(listID);
    var itemToDrop = document.getElementById(id);
    var targId = trgt.id;
    var hfSelectedIndex = document.getElementById(listID + "_hfSelectedIndex");
    var hfScrollTop = document.getElementById(listID + "_hfScrollTop")
    var scrollTop;
    if (trgt.tagName == "LI" && id != targId) {
        var autopostback = (list.getAttribute("AutoPostBack") == "true");
        var evt = document.createEvent("HTMLEvents");
        var targIndex = trgt.getAttribute("index")
        var selected = itemToDrop.getAttribute("index");
        var up = (targIndex < selected);
        var down = (targIndex > selected)
        var diff = (targIndex-selected)
        evt.initEvent("change", true, true);

        if (targIndex == 0) {
            list.insertBefore(itemToDrop, trgt); //before target
        }
        else if (targIndex == list.children.length - 1) {
            list.insertBefore(itemToDrop, trgt.nextSibling); //after target
        }
        else if (diff==1) {
            
                list.insertBefore(itemToDrop, trgt.nextSibling); //after target
        }
        else {
            list.insertBefore(itemToDrop, trgt); //before target
        }
        //if (targIndex < selected) { // dragged up
        //    list.insertBefore(itemToDrop, trgt); //before target
        //}
        //else { // dragged down
        //    list.insertBefore(itemToDrop, trgt.nextSibling); //after target
        //}

        reindexList(list);
        itemToDrop = document.getElementById(id);
        selected = itemToDrop.getAttribute("index");
        scrollTop = list.scrollTop;
        hfSelectedIndex.value = selected;
        outList.setAttribute("data-selectedindex", selected);
        outList.dispatchEvent(evt);

        if (autopostback) {
            __doPostBack(listID, selected + "~" + scrollTop);
        }

    }
    ev.stopPropagation();
}
