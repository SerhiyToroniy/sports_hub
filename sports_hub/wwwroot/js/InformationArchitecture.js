let categories = [];
let conferences = [];
let teams = [];

let selectedCategory = null;
let selectedConference = null;
let currentConferences = null;
let currentTeams = null;

let maxCatId = 0;
let maxConfId = 0;
let maxTeamId = 0;

let to_post = {
    Categories: [],
    Conferences: [],
    Teams: [],
    CategoriesDeleted: [],
    ConferencesDeleted: [],
    TeamsDeleted: [],
    CategoriesEdited: [],
    ConferencesEdited: [],
    TeamsEdited: []
}

fetchData();

async function fetchData() {
    let response = await fetch('/data/categories');
    categories = await response.json();
    maxCatId = Math.max.apply(Math, categories.map(function (o) { return o.id }))
    categoryData();

    response = await fetch('/data/allconferences');
    conferences = await response.json();
    maxConfId = Math.max.apply(Math, conferences.map(function (o) { return o.id }))

    response = await fetch('/data/allteams');
    teams = await response.json();
    maxTeamId = Math.max.apply(Math, teams.map(function (o) { return o.id }))
}

function selectCategory(clickedBox) {

    function filterAndDraw(selected) {
        selectedCategory = categories[selected]
        currentConferences = Array.from(conferences).filter(conf => conf.categoryId == selectedCategory.id)
        clickedBox.style.color = '#D72130';
        if (currentConferences.length - 1 > selected)
            selected = currentConferences.length - 1;
        if (currentConferences.length != 0) {
            var images = clickedBox.parentElement.parentElement.getElementsByTagName("img");
            images[images.length - 1].style.visibility = 'visible';
            document.getElementById("first_line").style.height = `${selected * 57}px`;
        }
        else {
            document.getElementById("first_line").style.height = '0';
        }
        document.getElementById("add_conference").style.visibility = 'visible';
        document.getElementById("add_team").style.visibility = 'hidden';
    }

    let boxes = document.getElementById("categories").getElementsByClassName("column_item");

    let selected = 0;
    for (i = 0; i < boxes.length; i++) {
        var images = boxes[i].getElementsByTagName("img");
        images[images.length - 1].style.visibility = 'hidden';
        boxes[i].getElementsByTagName("div")[2].style.color = '#B2B2B2';

        if (clickedBox.textContent == boxes[i].getElementsByTagName("div")[2].textContent) {
            selected = i
        }
    }

    document.getElementById("second_line").style.height = '0';

    if (selectedCategory == null) {
        filterAndDraw(selected);
    }
    else if (selectedCategory.name == categories[selected].name) {
        selectedCategory = null;
        selectedConference = null;
        currentConferences = null;
        currentTeams = null;

        document.getElementById("first_line").style.height = '0';

        document.getElementById("add_conference").style.visibility = 'hidden';
        document.getElementById("add_team").style.visibility = 'hidden';
    }
    else {
        selectedConference = null;
        currentTeams = null;
        filterAndDraw(selected);
    }

    conferenceData();
    teamsData();
}

function selectConference(clickedBox) {
    function filterAndDraw(selected) {
        selectedConference = currentConferences[selected]
        currentTeams = Array.from(teams).filter(team => team.conferenceId == selectedConference.id)
        clickedBox.style.color = '#D72130';
        if (currentTeams.length - 1 > selected)
            selected = currentTeams.length - 1;
        if (currentTeams.length != 0) {
            var images = clickedBox.parentElement.parentElement.getElementsByTagName("img");
            images[images.length - 1].style.visibility = 'visible';
            document.getElementById("second_line").style.height = `${selected * 57}px`;
        }
        else {
            document.getElementById("second_line").style.height = '0';
        }
        document.getElementById("add_team").style.visibility = 'visible';
    }

    let boxes = document.getElementById("conferences").getElementsByClassName("column_item");
    let selected = 0;

    for (i = 0; i < boxes.length; i++) {
        var images = boxes[i].getElementsByTagName("img");
        images[images.length - 1].style.visibility = 'hidden';
        boxes[i].getElementsByTagName("div")[2].style.color = '#B2B2B2';

        if (clickedBox.textContent == boxes[i].getElementsByTagName("div")[2].textContent) {
            selected = i
        }
    }

    if (selectedConference == null) {
        filterAndDraw(selected);
    }
    else if (selectedConference.name == currentConferences[selected].name) {
        selectedConference = null;
        currentTeams = null;

        document.getElementById("second_line").style.height = '0';
        document.getElementById("add_team").style.visibility = 'hidden';
    }
    else {
        filterAndDraw(selected);
    }

    teamsData();
}

function createBox(name, type, onclick) {
    var dragBox = document.createElement("div");
    dragBox.className = 'drag_box';

    var dragImg = document.createElement("img");
    dragImg.src = "/imagesAdminPage/InformationArchitecture/DragBox.svg"
    dragImg.className = 'drag_drop';
    dragBox.appendChild(dragImg);

    var textBox = document.createElement("div");
    textBox.className = 'name_box';
    textBox.textContent = name;
    if (onclick != null)
        textBox.onclick = function () { onclick(this) };

    var ellipsisImage = document.createElement("img");
    ellipsisImage.className = 'empty';
    ellipsisImage.src = "/imagesAdminPage/InformationArchitecture/Ellipsis.svg";

    var ellipsisButton = document.createElement("button");
    ellipsisButton.className = 'ellipsis_button'
    ellipsisButton.appendChild(ellipsisImage);
    ellipsisButton.onclick = function () { showDropDown(this) };

    var dropDownBox = document.createElement("div");
    dropDownBox.className = 'dropdown_holder';

    var redDots = document.createElement("img");
    redDots.src = "/imagesAdminPage/InformationArchitecture/EllipsisRed.svg";

    var dotHolder = document.createElement("div");
    dotHolder.className = 'dropdown_item';
    dotHolder.onclick = function () { hideDropDown(this) };
    dotHolder.appendChild(redDots);
    dropDownBox.appendChild(dotHolder);

    if (type != 'category') {
        var moveTo = document.createElement("div");
        moveTo.className = 'dropdown_item';
        moveTo.textContent = 'Move to';

        var triangle = document.createElement("img");
        triangle.src = "/imagesAdminPage/InformationArchitecture/Triangle.svg";
        triangle.className = "dropdown_triangle";

        moveTo.appendChild(triangle);
        dropDownBox.appendChild(moveTo);

        var categoryDropdown = document.createElement("div");
        categoryDropdown.className = 'dropdown_holder_for_move';
        moveTo.appendChild(categoryDropdown);

        moveTo.onmouseover = function () {
            moveTo.getElementsByClassName('dropdown_holder_for_move')[0].style.display = 'block';
        };
        moveTo.onmouseleave = function () {
            moveTo.getElementsByClassName('dropdown_holder_for_move')[0].style.display = 'none';
        };

        if (type == 'team') {
            categories.forEach(cat => {
                if (conferences.filter(conf => conf.categoryId == cat.id).length != 0) {
                    var moveItem = document.createElement("div");
                    moveItem.className = 'dropdown_item';
                    moveItem.textContent = cat.name;
                    moveItem.appendChild(triangle.cloneNode());

                    var moveSub = document.createElement("div");
                    moveSub.className = 'dropdown_holder_for_move';

                    conferences.filter(conf => conf.categoryId == cat.id).forEach(conf => {
                        if (conf.id != selectedConference.id) {
                            var moveItemSub = document.createElement("div");
                            moveItemSub.className = 'dropdown_item_conf';
                            moveItemSub.textContent = conf.name;
                            moveItemSub.onclick = function () { moveTeam(this) };
                            moveSub.appendChild(moveItemSub);
                        }
                    });
                    moveItem.onmouseover = function () {
                        moveItem.getElementsByClassName('dropdown_holder_for_move')[0].style.display = 'block';
                    };
                    moveItem.onmouseleave = function () {
                        moveItem.getElementsByClassName('dropdown_holder_for_move')[0].style.display = 'none';
                    };
                    moveItem.appendChild(moveSub);
                    categoryDropdown.appendChild(moveItem);
                }
            });
        }
        else if (type == 'subcategory') {

            categories.forEach(cat => {
                if (cat != selectedCategory) {
                    var moveItem = document.createElement("div");
                    moveItem.className = 'dropdown_item';
                    moveItem.textContent = cat.name;
                    moveItem.onclick = function () { moveConference(this) };

                    categoryDropdown.appendChild(moveItem);
                }
            });
        }
    }

    var hide = document.createElement("div");
    hide.className = 'dropdown_item';
    hide.textContent = 'Hide';
    dropDownBox.appendChild(hide);

    var deleteBox = document.createElement("div");
    deleteBox.className = 'dropdown_item';
    deleteBox.textContent = 'Delete';
    deleteBox.onclick = function () { openDeleteModal(this, type) };
    dropDownBox.appendChild(deleteBox);

    var edit = document.createElement("div");
    edit.className = 'dropdown_item';
    edit.textContent = 'Edit';
    edit.onclick = function () { openEditModal(this, type) };
    dropDownBox.appendChild(edit);

    var box = document.createElement("div");
    box.className = 'content_box';

    box.appendChild(dragBox);
    box.appendChild(textBox);
    box.appendChild(ellipsisButton);
    box.appendChild(dropDownBox);
    return box;
}

function moveConference(element) {
    var movableCategories = categories.filter(cat => cat != selectedCategory);
    var catIndex = Array.from(element.parentElement.getElementsByClassName('dropdown_item')).findIndex(item => item == element);

    var dropDowns = document.getElementById('conferences').getElementsByClassName('dropdown_holder_for_move');
    var selected = null;
    for (i = 0; i < dropDowns.length; i++) {
        var openDropdown = dropDowns[i];
        if (openDropdown.style.display == 'block') {
            selected = i;
            break;
        }
    }
    if (selected != null) {
        if (currentConferences[selected] == selectedConference) {
            currentTeams = null;
            selectedConference = null;
            document.getElementById("second_line").style.height = '0';
            document.getElementById("add_team").style.visibility = 'hidden';
        }
        var changedConf = currentConferences[selected];
        changedConf.categoryId = movableCategories[catIndex].id;
        currentConferences = currentConferences.filter(conf => conf.categoryId == selectedCategory.id);
        conferenceData();
        teamsData();
        redrawFirst();
        redrawSecond();
        if (to_post.Conferences.find(conf => conf.id == changedConf.id) == undefined) {
            if (to_post.ConferencesEdited.find(conf => conf.id == changedConf.id) == undefined) {
                to_post.ConferencesEdited.push(changedConf);
            }
        }
    }
}

function moveTeam(element) {
    var confIndex = Array.from(element.parentElement.getElementsByClassName('dropdown_item_conf'))
        .findIndex(item => item == element);
    var catIndex = Array.from(element.parentElement.parentElement.parentElement.getElementsByClassName('dropdown_item'))
        .findIndex(item => item == element.parentElement.parentElement);

    var notEmptyCats = categories.filter(cat => conferences.filter(conf => conf.categoryId == cat.id).length != 0);

    var dropDowns = document.getElementById('teams').getElementsByClassName('dropdown_holder');
    var selected = null;
    for (i = 0; i < dropDowns.length; i++) {
        var openDropdown = dropDowns[i];
        if (openDropdown.classList.contains('show')) {
            selected = i;
            break;
        }
    }
    if (selected != null) {
        var changedTeam = currentTeams[selected];
        changedTeam.conferenceId = conferences.filter(conf => conf.categoryId == notEmptyCats[catIndex].id
            && changedTeam.conferenceId != conf.id)[confIndex].id;
        currentTeams = currentTeams.filter(team => team.id != changedTeam.id);
        teamsData();
        redrawSecond();
        if (to_post.Teams.find(team => team.id == changedTeam.id) == undefined) {
            if (to_post.TeamsEdited.find(team => team.id == changedTeam.id) == undefined) {
                to_post.TeamsEdited.push(changedTeam);
            }
        }
    }
}

function createLine() {
    var line = document.createElement("img");
    line.src = "/imagesAdminPage/InformationArchitecture/HorizontalLine.svg"
    line.className = 'hor_line';
    return line;
}

function categoryData() {
    var categs = document.getElementById("categories");
    $(categs).empty();
    categories.forEach(cat => {
        var line = createLine();

        var box = createBox(cat.name, 'category', selectCategory);

        var entire = document.createElement("div");
        entire.className = "column_item";
        entire.appendChild(box);
        entire.appendChild(line);

        categs.appendChild(entire);
    });
}

function conferenceData() {
    var confs = document.getElementById("conferences");
    $(confs).empty();
    if (currentConferences != null) {
        currentConferences.forEach(conf => {
            var line = createLine();
            var line2 = line.cloneNode()
            line.style.visibility = 'visible';

            var box = createBox(conf.name, 'subcategory', selectConference);

            var entire = document.createElement("div");
            entire.className = "column_item";
            entire.appendChild(line);
            entire.appendChild(box);
            entire.appendChild(line2);

            confs.appendChild(entire);
        })
    }
}

function teamsData() {
    var teamsContainer = document.getElementById("teams");
    $(teamsContainer).empty();
    if (currentTeams != null) {
        currentTeams.forEach(team => {
            var line = createLine();
            line.style.visibility = 'visible';

            var box = createBox(team.name, 'team', null);

            var entire = document.createElement("div");
            entire.className = "column_item";
            entire.appendChild(line);
            entire.appendChild(box);

            teamsContainer.appendChild(entire);
        })
    }
}

function openModal(name) {
    document.getElementById("modalTitle").innerText = 'Add new ' + name;
    document.getElementById("add_edit_button").innerText = 'Add';
    document.getElementById("add_edit_button").onclick = function () { addNew() };
    document.getElementById("modal").style.display = 'block';
}

function openEditModal(element, type) {
    var where = null;
    var collection = null;
    switch (type) {
        case 'category':
            where = 'categories';
            collection = categories;
            break;
        case 'subcategory':
            where = 'conferences';
            collection = currentConferences;
            break;
        case 'team':
            where = 'teams';
            collection = currentTeams;
            break;
    }
    if (where != null) {
        var dropDowns = document.getElementById(where).getElementsByClassName('dropdown_holder');
        var selected = null;
        for (i = 0; i < dropDowns.length; i++) {
            var openDropdown = dropDowns[i];
            if (openDropdown.classList.contains('show')) {
                selected = i;
                break;
            }
        }
        document.getElementById("edit_id").value = collection[selected].id;
        document.getElementById("modalTitle").innerText = 'Edit this ' + type;
        document.getElementById("modal_input").value = collection[selected].name;
        document.getElementById("add_edit_button").innerText = 'Edit';
        document.getElementById("add_edit_button").onclick = function () { editOne() };
        document.getElementById("modal").style.display = 'block';
    }
    else {
        alert('something wrong');
    }
}

function editOne() {
    if (document.getElementById("modal_input").value == '') {
        alert('Please enter valid name');
    }
    else {
        switch (document.getElementById("modalTitle").textContent.split(' ')[2]) {
            case 'category':
                categories.find(cat => cat.id == document.getElementById("edit_id").value).name
                    = document.getElementById("modal_input").value;
                categoryData();
                conferenceData();
                redrawFirst();
                redrawSecond();
                if (to_post.Categories.find(cat => cat.id == document.getElementById("edit_id").value) == undefined) {
                    if (to_post.CategoriesEdited.find(cat => cat.id == document.getElementById("edit_id").value) == undefined) {
                        to_post.CategoriesEdited.push(categories.find(cat => cat.id == document.getElementById("edit_id").value));
                    }
                }
                break;
            case 'subcategory':
                conferences.find(conf => conf.id == document.getElementById("edit_id").value).name
                    = document.getElementById("modal_input").value;
                conferenceData();
                redrawSecond();
                if (to_post.Conferences.find(conf => conf.id == document.getElementById("edit_id").value) == undefined) {
                    if (to_post.ConferencesEdited.find(conf => conf.id == document.getElementById("edit_id").value) == undefined) {
                        to_post.ConferencesEdited.push(conferences.find(conf => conf.id == document.getElementById("edit_id").value));
                    }
                }
                break;
            case 'team':
                teams.find(team => team.id == document.getElementById("edit_id").value).name
                    = document.getElementById("modal_input").value;
                teamsData();
                if (to_post.Teams.find(team => team.id == document.getElementById("edit_id").value) == undefined) {
                    if (to_post.TeamsEdited.find(team => team.id == document.getElementById("edit_id").value) == undefined) {
                        to_post.TeamsEdited.push(teams.find(team => team.id == document.getElementById("edit_id").value));
                    }
                }
                break;
        }
        closeModal();
    }
}

function closeModal() {
    document.getElementById("modal_input").value = '';
    document.getElementById("modal").style.display = 'none';
}

function openDeleteModal(element, type) {
    var where = null;
    var collection = null;
    switch (type) {
        case 'category':
            where = 'categories';
            collection = categories;
            break;
        case 'subcategory':
            where = 'conferences';
            collection = currentConferences;
            break;
        case 'team':
            where = 'teams';
            collection = currentTeams;
            break;
    }
    if (where != null) {
        var dropDowns = document.getElementById(where).getElementsByClassName('dropdown_holder');
        var selected = null;
        for (i = 0; i < dropDowns.length; i++) {
            var openDropdown = dropDowns[i];
            if (openDropdown.classList.contains('show')) {
                selected = i;
                break;
            }
        }
        document.getElementById("to_delete_input").value = collection[selected].id;
        document.getElementById("delete_modal_title").innerText = 'You are about to delete this ' + type + '!';
        document.getElementById("delete_modal").style.display = 'block';
    }
    else {
        alert('something wrong');
    }

}

function closeDeleteModal() {
    document.getElementById("delete_modal").style.display = 'none';
}

function addNew() {

    if (document.getElementById("modal_input").value == '') {
        alert("Enter valid name");
    }
    else {
        if (document.getElementById("modalTitle").innerText.split(' ')[2] == 'category') {
            maxCatId += 1;
            let to_add = {
                id: categories.length != 0 ? maxCatId : 1,
                name: document.getElementById("modal_input").value,
                order: categories.length != 0 ? Math.max.apply(Math, categories.map(function (o) { return o.order })) + 1 : 1
            }
            categories.push(to_add);
            to_post.Categories.push(to_add);
            categoryData();
            redrawFirst();
        }
        else if (document.getElementById("modalTitle").innerText.split(' ')[2] == 'subcategory') {
            if (selectedCategory == null) {
                alert("Choose a category first to add subcategory");
            }
            else {
                maxConfId += 1;
                let to_add = {
                    id: conferences.length != 0 ? maxConfId : 1,
                    name: document.getElementById("modal_input").value,
                    order: currentConferences.length != 0 ? Math.max.apply(Math, currentConferences.map(function (o) { return o.order })) + 1 : 1,
                    categoryId: selectedCategory.id
                };
                currentConferences.push(to_add);
                conferences.push(to_add);
                to_post.Conferences.push(to_add);
                conferenceData();
                redrawFirst();
                redrawSecond();
            }
        }
        else if (document.getElementById("modalTitle").innerText.split(' ')[2] == 'team') {
            if (selectedCategory == null) {
                alert("Choose a category first to add subcategory");
            }
            else if (selectedConference == null) {
                alert("Choose a conference to add a team");
            }
            else {
                maxTeamId += 1;
                let to_add = {
                    id: teams.length != 0 ? maxTeamId : 1,
                    name: document.getElementById("modal_input").value,
                    order: currentTeams.length != 0 ? Math.max.apply(Math, currentTeams.map(function (o) { return o.order })) + 1 : 1,
                    conferenceId: selectedConference.id
                };
                currentTeams.push(to_add);
                teams.push(to_add);
                to_post.Teams.push(to_add);
                teamsData();
                redrawSecond();
            }
        }
        closeModal();
    }
}

function deleteOne() {
    switch (document.getElementById("delete_modal_title").textContent.split(' ')[6]) {
        case 'category!':
            var deletedCategory = categories.find(cat => cat.id == document.getElementById("to_delete_input").value);
            categories = categories.filter(cat => cat.id != deletedCategory.id);
            var teams_to_del = conferences.filter(conf => conf.categoryId == deletedCategory.id);
            for (i = 0; i < teams_to_del.length; i++) {
                teams = teams.filter(team => team.conferenceId != teams_to_del[i].id);
            }
            conferences = conferences.filter(conf => conf.categoryId != deletedCategory.id);

            if (selectedCategory != null) {
                if (selectedCategory.id == deletedCategory.id) {
                    selectedCategory = null;
                    currentConferences = null;
                    selectedConference = null;
                    currentTeams = null;

                    document.getElementById("add_team").style.visibility = 'hidden';
                    document.getElementById("add_conference").style.visibility = 'hidden';
                    document.getElementById("first_line").style.height = '0';
                    document.getElementById("second_line").style.height = '0';
                }
            }
            categoryData();
            conferenceData();
            teamsData();
            redrawFirst();
            redrawSecond();
            if (to_post.Categories.find(cat => cat.id == deletedCategory.id) != undefined) {
                to_post.Categories = to_post.Categories.filter(cat => cat.id != deletedCategory.id);
                var teams_to_remove = to_post.Conferences.filter(conf => conf.categoryId == deletedCategory.id);
                for (i = 0; i < teams_to_remove.length; i++) {
                    to_post.Teams = to_post.Teams.filter(team => team.conferenceId != teams_to_remove[i].id);
                }
                to_post.Conferences = to_post.Conferences.filter(conf => conf.categoryId != deletedCategory.id);
            }
            else {
                to_post.CategoriesEdited = to_post.CategoriesEdited.filter(cat => cat.id != deletedCategory.id);
                to_post.CategoriesDeleted.push(deletedCategory);
            }
            break;
        case 'subcategory!':
            var deletedSubcategory = conferences.find(conf => conf.id == document.getElementById("to_delete_input").value);
            conferences = conferences.filter(conf => conf.id != deletedSubcategory.id);
            currentConferences = currentConferences.filter(conf => conf.id != deletedSubcategory.id);
            teams = teams.filter(team => team.conferenceId != deletedSubcategory.id);
            if (selectedConference != null) {
                if (selectedConference.id == deletedSubcategory.id) {
                    selectedConference = null;
                    currentTeams = null;

                    document.getElementById("second_line").style.height = '0';
                    document.getElementById("add_team").style.visibility = 'hidden';
                }
            }
            conferenceData();
            teamsData();
            redrawFirst();
            redrawSecond();
            if (to_post.Conferences.find(conf => conf.id == deletedSubcategory.id) != undefined) {
                to_post.Conferences = to_post.Conferences.filter(conf => conf.id != deletedSubcategory.id);
                to_post.Teams = to_post.Teams.filter(team => team.conferenceId != deletedSubcategory.id);
            }
            else {
                to_post.ConferencesEdited = to_post.ConferencesEdited.filter(conf => conf.id != deletedSubcategory.id);
                to_post.ConferencesDeleted.push(deletedSubcategory);
            }
            break;
        case 'team!':
            var deletedTeam = teams.find(team => team.id == document.getElementById("to_delete_input").value);
            teams = teams.filter(team => team.id != deletedTeam.id);
            currentTeams = currentTeams.filter(team => team.id != deletedTeam.id);
            teamsData();
            redrawSecond();
            if (to_post.Teams.find(team => team.id == deletedTeam.id) != undefined) {
                to_post.Teams = to_post.Teams.filter(team => team.id != deletedTeam.id);
            }
            else {
                to_post.TeamsEdited = to_post.TeamsEdited.filter(team => team.id != deletedTeam.id);
                to_post.TeamsDeleted.push(deletedTeam);
            }
            break;
    }
    closeDeleteModal();
}

function redrawFirst() {
    if (selectedCategory != null) {
        let boxes = document.getElementById("categories").getElementsByClassName("column_item");
        let selected = 0;

        for (i = 0; i < boxes.length; i++) {
            if (selectedCategory.name == boxes[i].getElementsByTagName("div")[2].textContent) {
                selected = i
            }
        }
        boxes[selected].getElementsByTagName('div')[2].style.color = '#D72130';
        if (currentConferences.length != 0) {
            var images = boxes[selected].getElementsByTagName('img');
            images[images.length - 1].style.visibility = 'visible';
        }
        if (currentConferences.length - 1 > selected)
            selected = currentConferences.length - 1;
        if (currentConferences.length != 0) {
            document.getElementById("first_line").style.height = `${selected * 57}px`;
        }
        else {
            var images = boxes[selected].getElementsByTagName('img');
            images[images.length - 1].style.visibility = 'hidden';
            document.getElementById("first_line").style.height = '0';
        }
    }
}

function redrawSecond() {
    if (selectedConference != null) {
        let boxes = document.getElementById("conferences").getElementsByClassName("column_item");
        let selected = 0;

        for (i = 0; i < boxes.length; i++) {
            if (selectedConference.name == boxes[i].getElementsByTagName("div")[2].textContent) {
                selected = i
            }
        }
        boxes[selected].getElementsByTagName('div')[2].style.color = '#D72130';
        if (currentTeams.length != 0) {
            var images = boxes[selected].getElementsByTagName('img');
            images[images.length - 1].style.visibility = 'visible';
        }
        if (currentTeams.length - 1 > selected)
            selected = currentTeams.length - 1;
        if (currentTeams.length != 0) {
            document.getElementById("second_line").style.height = `${selected * 57}px`;
        }
        else {
            var images = boxes[selected].getElementsByTagName('img');
            images[images.length - 1].style.visibility = 'hidden';
            document.getElementById("second_line").style.height = '0';
        }
    }
}

function showDropDown(button) {
    closeDropDowns();
    button.parentElement.getElementsByClassName("dropdown_holder")[0].classList.add("show");
}
function hideDropDown(button) {
    button.parentElement.parentElement.getElementsByClassName("dropdown_holder")[0].classList.remove("show");
}

function closeDropDowns() {
    var dropDowns = document.getElementsByClassName('dropdown_holder');

    for (i = 0; i < dropDowns.length; i++) {
        var openDropdown = dropDowns[i];
        if (openDropdown.classList.contains('show')) {
            openDropdown.classList.remove('show');
        }
    }
}

window.addEventListener("click", function (event) {
    if (!event.target.matches('.dropdown_item') && !event.target.matches('.ellipsis_button') && !event.target.matches('.empty')) {
        closeDropDowns();
    }
});

var overlayTimeout = null;

function saveChanges() {
    $.ajax({
        type: 'POST',
        data: JSON.stringify(to_post),
        contentType: "application/json",
        url: '/data/savechanges'
    });
    to_post = {
        Categories: [],
        Conferences: [],
        Teams: [],
        CategoriesDeleted: [],
        ConferencesDeleted: [],
        TeamsDeleted: [],
        CategoriesEdited: [],
        ConferencesEdited: [],
        TeamsEdited: []
    }
    $('#save_alert').show()
    overlayTimeout = setTimeout(function () {
        $('#save_alert').hide();
    }, 5000);
}

function closeConfirmation() {
    $('#save_alert').hide()
    if (overlayTimeout != null) {
        clearTimeout(overlayTimeout);
    }
}