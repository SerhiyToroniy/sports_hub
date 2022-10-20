function openTab(evt, tabname) {
    var i, tabcontent, profileTabs;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    document.getElementById(tabname).style.display = "block";
}

function closeMessage() {
    document.getElementsByClassName("notification")[0].style.display = "none";
}

function appear(elm, i, step, speed) {
    var t_o;
    //initial opacity
    i = i || 0;
    //opacity increment
    step = step || 5;
    //time waited between two opacity increments in msec
    speed = speed || 50;

    t_o = setInterval(function () {
        //get opacity in decimals
        var opacity = i / 100;
        //set the next opacity step
        i = i + step;
        if (opacity > 1 || opacity < 0) {
            clearInterval(t_o);
            //if 1-opaque or 0-transparent, stop
            return;
        }
        //modern browsers
        elm.style.opacity = opacity;
        //older IE
        elm.style.filter = 'alpha(opacity=' + opacity * 100 + ')';
    }, speed);
}