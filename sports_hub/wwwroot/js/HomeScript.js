function changeActive(number) {
    var prevP = document.getElementsByClassName('mini-news-number selected');
    prevP[0].className = "mini-news-number";

    var prevDiv = document.getElementsByClassName('mini-news selected');
    prevDiv[0].className = "mini-news";

    var newPSelected = document.getElementById('MNS' + number);
    newPSelected.className += ' selected';

    var nextDiv = document.getElementById('MN' + number);
    nextDiv.className += ' selected';
}

function showConference(obj, leftBarIndex) {
    if (obj.parentElement.children[1].children.length == 0) {
        $("<div id='wait-for-conection'></div>").appendTo(obj.parentElement.children[1]);
        $.ajax({
            type: "GET",
            url: "/data/Conferences",
            data: { CategoryName: obj.id },
            dataType: "json",
            success: function (msg) {
                console.log(msg);
                var items = [];
                $.each(msg, function (key, val) {
                    items.push("<li class='conference'> <a id='" + val.name + "' class='nav-link conference' onmouseover='showTeams(this)' href='#'>" + val.name + "</a><div class='team'></div></li>");
                });

                $("<ul/>", {
                    "class": "conference",
                    html: items.join("")
                }).appendTo(obj.parentElement.children[1]);
                obj.parentElement.children[1].children[0].remove();
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    }


    var curentActive = document.getElementsByClassName('categories active')[0];
    curentActive.classList.remove("active");
    if (document.getElementsByClassName('conference show').length != 0) {
        curentActive.parentElement.children[1].classList.remove("show");
    }
    if (document.getElementsByClassName('team show').length != 0) {
        document.getElementsByClassName('team show')[0].classList.remove("show");
    }
    if (document.getElementsByClassName('nav-link conference active').length != 0) {
        document.getElementsByClassName('nav-link conference active')[0].classList.remove("active");
    }
    obj.className = 'nav-link categories active';
    obj.parentElement.children[1].className = 'conference show';
    document.body.style.overflow = 'hidden';
    var addBlur = document.getElementById('blur');
    addBlur.className = 'blur';
}

function hideLeftBar(leftBarIndex) {
    var obj = document.getElementsByClassName('categories active')[0];
    obj.classList.remove("active");
    obj.parentElement.children[1].classList.remove("show");
    if (document.getElementsByClassName('nav-link conference active').length != 0) {
        document.getElementsByClassName('nav-link conference active')[0].classList.remove("active");
    }
    document.getElementsByClassName('categories')[leftBarIndex].classList.add("active");
    document.body.style.overflow = "scroll";
    var removeBlur = document.getElementById('blur');
    removeBlur.className = '';
}

function showTeams(obj) {
    if (obj.className != 'nav-link conference active') {
        if (obj.parentElement.children[1].children.length == 0) {
            $("<div id='wait-for-conection'></div>").appendTo(obj.parentElement.children[1]);
            $.ajax({
                type: "GET",
                url: "/Data/Teams",
                data: { ConferenceName: obj.id },
                dataType: "json",
                success: function (msg) {
                    var items = [];
                    $.each(msg, function (key, val) {
                        items.push("<li class='team'> <a id='" + val.name + "' class='nav-link team' href='/TeamPortal/" + obj.parentElement.parentElement.parentElement.parentElement.children[0].id + "/" + obj.id + "/" + val.name + "'>" + val.name + "</a></li>");
                    });

                    $("<ul/>", {
                        "class": "team",
                        html: items.join("")
                    }).appendTo(obj.parentElement.children[1]);
                    obj.parentElement.children[1].children[0].remove();
                },
                error: function (req, status, error) {
                    console.log(msg);
                }
            });
        }

        if (document.getElementsByClassName('conference active').length != 0) {
            document.getElementsByClassName('conference active')[0].classList.remove("active");
            if (document.getElementsByClassName('team show').length != 0) {
                document.getElementsByClassName('team show')[0].classList.remove("show");
            }
        }

        obj.className = 'nav-link conference active';
        obj.parentElement.children[1].className = 'team show';
    }
    else {
        obj.classList.remove("active");
        obj.parentElement.children[1].classList.remove("show");
    }
}




const timeChangeForArticle = 3000;

var intervalID = setInterval(changeToNext, timeChangeForArticle);

let firstLoad = true;

function changeArticle(number) {
    if (firstLoad) {
        let main_image = document.getElementsByClassName('part');
        for (var i = 0; i < main_image.length; i++) {
            main_image[i].addEventListener("mouseenter", function () {
                clearInterval(intervalID);
            });
            main_image[i].addEventListener("mouseleave", function () {
                intervalID = setInterval(changeToNext, timeChangeForArticle);
            });
        }
        firstLoad = false;
    }
    let index = parseInt(number);
    let nums = document.getElementsByClassName('mini-news-number');
    for (var i = 0; i < nums.length; i++) {
        nums[i].style.color = "#B1B1B1";
        if (i == index) {
            continue;
        }
    }
    let mains = document.getElementsByClassName('part');
    for (var i = 0; i < mains.length; i++) {
        mains[i].style.display = "none";
        if (i == index) {
            continue;
        }
    }
    document.getElementsByClassName('part')[index - 1].style.display = "block";
    document.getElementById(number).style.color = "#D72130";
    var nextDiv = document.getElementById('MN' + number);
    nextDiv.className += ' selected';
}


function articleClick(number) {
    location.href = "/article/getarticle?id=" + number;
}

function changeToNext() {
    let current = 0;
    let nums = document.getElementsByClassName('mini-news-number');
    for (var i = 0; i < nums.length; i++) {
        if (nums[i].style.color == "rgb(215, 33, 48)") {
            current = i + 1;
            break;
        }
    }
    if (current < nums.length) {
        changeArticle(`${current + 1}`);
    }
    else {
        changeArticle('1');
    }
}

function changeToPrevious() {
    let current = 0;
    let nums = document.getElementsByClassName('mini-news-number');
    for (var i = 0; i < nums.length; i++) {
        if (nums[i].style.color == "rgb(215, 33, 48)") {
            current = i + 1;
            break;
        }
    }
    if (current > 1) {
        changeArticle(`${current - 1}`);
    } else {
        changeArticle(`${nums.length}`);
    }
}

let searchTimer = null;

async function searchWithInterval(value) {
    clearTimeout(searchTimer);
    if (value != '') {

        let waitElement = document.createElement("div")
        waitElement.className = "searchResult";
        waitElement.innerText = "Please wait..."
        waitElement.style.textAlign = "center";

        document.getElementById("searchResults").style.display = 'block';
        document.getElementById("searchResults").replaceChildren(waitElement);

        searchTimer = setTimeout(searchArticles, 2000, value);
    }
    else {
        document.getElementById("searchResults").style.display = 'none';
    }

}

async function searchArticles(value) {
    let response = await fetch('/article/findarticles?toFind=' + value);
    let data = await response.json();
    let searchResults = document.getElementById("searchResults");

    let newChildren = [];
    Array.from(data).forEach(item => new function () {
        let newElement = document.createElement("a");
        newElement.href = "/article/getarticle?id=" + item.id;
        let title = document.createElement("div");
        title.className = "path";

        if (item.discriminatorValue == "Article") {
            let category = document.createElement("span");
            category.innerText = item.team.conference.category.name + ' > ';
            title.appendChild(category);

            let conference = document.createElement("span");
            conference.innerText = item.team.conference.name + ' > ';
            title.appendChild(conference);

            let team = document.createElement("span");
            team.innerText = item.team.name;
            team.style.color = '#D72130';
            title.appendChild(team);
        }
        else if (item.discriminatorValue == "ArticleLifestyle"){
            let lifestyle = document.createElement("span");
            lifestyle.innerText = 'Lifestyle';
            lifestyle.style.color = '#D72130';
            title.appendChild(lifestyle);
        }
        else if (item.discriminatorValue == "ArticleDealbook"){
            let dealbook = document.createElement("span");
            dealbook.innerText = 'Dealbook';
            dealbook.style.color = '#D72130';
            title.appendChild(dealbook);
        }

        let caption = document.createElement("div");
        caption.innerText = item.caption;
        newElement.appendChild(title);
        newElement.appendChild(caption);
        newElement.className = "searchResult";

        newChildren.push(newElement);

    });
    if (newChildren.length > 0) {
        searchResults.style.display = 'block';
        searchResults.replaceChildren(...newChildren);
    }
    else {
        let noResultElement = document.createElement("div")
        noResultElement.className = "searchResult";
        noResultElement.innerText = "No results."
        noResultElement.style.textAlign = "center";

        document.getElementById("searchResults").replaceChildren(noResultElement);
    }
}

window.addEventListener("click", function (event) {
    if (!event.target.matches('.form-inline header') && !event.target.matches('#searchResults')) {
        document.getElementById("searchResults").style.display = 'none';
    }
});