$(document).ready(function () {
    $('a[href^="#"]').click(function () {
        elementClick = $(this).attr("href");
        destination = $(elementClick).offset().top;
        $('html').animate({ scrollTop: destination }, 700);
        return false;
    });
});


//create user
function showHiden(request)
{
    var area = document.getElementById("hidenVisibleAreaId");
    if (request == false)
    {
        area.style.display = "none";
    }
    else if (request == true)
    {
        area.style.display = "block";
    }
}

//menu
function selectNavElement(element)
{
    var selectedElem = document.getElementById("selectedId");
    var id = selectedElem.id;
    selectedElem.id = "";
    selectedElem.classList.remove("active");

    element.id = id;
    element.classList.add("active");
}


//sorting
var isReverceDirectionNumberSorting = false;
var isReverceDirectionNameSorting = false;
var sortByElement = 0

function sortBy(propery)
{
    sortByElement = sortPropery(propery);

    var area = document.getElementById("flightsListArea");

    var flights = area.children
    var sortedFlightd = [];

    for (var index1 = 0; index1 < flights.length; index1++)
    {
        sortedFlightd[index1] = flights[index1]
    }

    sortedFlightd.sort(comparer)
    area.innerHTML = "";

    for (var index2 = 0; index2  < sortedFlightd.length; index2 ++)
    {
        area.appendChild(sortedFlightd[index2]);
    }

    setRememberProperty(sortByElement)
}

function comparer(row1, row2)
{
    var direction = getRememberProperty(sortByElement);

    if (row1.children[sortByElement].innerText > row2.children[sortByElement].innerText) {
        return direction ? -1: 1;
    }
    else if (row1.children[sortByElement].innerText < row2.children[sortByElement].innerText) {
        return direction ? 1 : -1;
    }
    else {
        return 0;
    }
}

function sortPropery(properyName)
{
    if (properyName == "Number") return 0;
    else if (properyName == "Name") return 1;
}

function getRememberProperty(index)
{
    if (index == 0) return isReverceDirectionNumberSorting;
    else if (index == 1) return isReverceDirectionNameSorting;
}

function setRememberProperty(index) {
    if (index == 0) {
        isReverceDirectionNumberSorting = isReverceDirectionNumberSorting ? false : true;
    }
    else if (index == 1) {
        return isReverceDirectionNameSorting = isReverceDirectionNameSorting ? false : true;
    }
}


//flight request
function requestClosedSuccess(data)
{
    if (data[0].Error == null)
        deleteRequestFromList(data[0].Id)
}

function deleteRequestFromList(id) {
    var element = document.getElementById(id);

    if (element != null)
        element.remove();

    var message = document.getElementById("messageAreaId");
    message.innerHTML = "";
}

function requestDeleteSuccess(data)
{
    if (data[0].Error == null)
        deleteRequest(data[0].Id)
}

function deleteRequest(id) {
    var element = document.getElementById(id);

    if (element != null)
        element.remove();

    var deleteButton = document.getElementById("deleteButtonId");
    deleteButton.remove();
}


//add flight

function changeCityId(selected) {
    var targetId = selected.value;
    var ajaxHidenField = document.getElementById("targetId");
    ajaxHidenField.value = targetId;

    resetAllLists();
}

function loadAvailableAircrewMembers(data) {
    resetAllLists()

    for (var i = 0; i < data.length; i++) {
        var listId
        switch (data[i].Prosession) {
            case "Pilot":
                listId = "availablePilots";
                break;
            case "Aircraft Navigator":
                listId = "availableNavigators";
                break;
            case "Radio Operator":
                listId = "availableOperators";
                break;
            case "Flight Engineer":
                listId = "availableEngineers";
                break;
            case "Stewardess":
                listId = "availableStewardesses";
                break;
            default:
                break;
        }
        addToList(data[i], listId);
    }
}

function selectAll(form) {
    var pilits = document.getElementById("flightPilots");
    selectAllOptions(pilits);
    var navigators = document.getElementById("flightNavigators");
    selectAllOptions(navigators);
    var operators = document.getElementById("flightOperators");
    selectAllOptions(operators);
    var engineer = document.getElementById("flightEngineers");
    selectAllOptions(engineer);
    var stewardesses = document.getElementById("flightStewardesses");
    selectAllOptions(stewardesses);

    form.submit();
}

function selectAllOptions(list) {
    for (var i = 0; i < list.options.length; i++) {
        list.options[i].selected = true;
    }
}

function swapOptions(from, to) {
    var list1 = document.getElementById(from);
    var list2 = document.getElementById(to);

    var is_selected = [];
    for (var i = 0; i < list1.options.length; ++i) {
        is_selected[i] = list1.options[i].selected;
        if (is_selected[i]) {
            var option = new Option(list1.options[i].text, list1.options[i].value, true, true);
            list2.options[list2.options.length] = option;
        }
    }

    i = list1.options.length;
    while (i--) {
        if (is_selected[i]) {
            list1.remove(i);
        }
    }
}

function addToList(data, listId) {
    var list = document.getElementById(listId);
    var newOption = new Option(data.FullName, data.Id, true, true);
    list.options[list.options.length] = newOption;
}

function resetAllLists() {
    var list = document.getElementById("flightPilots");
    list.options.length = 0;
    list = document.getElementById("availablePilots");
    list.options.length = 0;
    list = document.getElementById("flightNavigators");
    list.options.length = 0;
    list = document.getElementById("availableNavigators");
    list.options.length = 0;
    list = document.getElementById("flightOperators");
    list.options.length = 0;
    list = document.getElementById("availableOperators");
    list.options.length = 0;
    list = document.getElementById("flightEngineers");
    list.options.length = 0;
    list = document.getElementById("availableEngineers");
    list.options.length = 0;
    list = document.getElementById("flightStewardesses");
    list.options.length = 0;
    list = document.getElementById("availableStewardesses");
    list.options.length = 0;
}


// flight delete 
function postFlightDeleting(data)
{
    var infoArea = document.getElementById("informationArea");
    infoArea.innerHTML = "";

    var infoDiv = document.createElement("div");
    infoDiv.className = "text-center";

    var infoSpan = document.createElement("span");
    infoSpan.className = "h5"

    infoDiv.appendChild(infoSpan);
    infoArea.appendChild(infoDiv);

    if (data[0].Sucsses == true)
    {
        var flight = document.getElementById(data[0].Id);
        flight.remove();

        infoSpan.innerHTML = data[0].Message;

        var listArea = document.getElementById("flightsListArea");
        if (listArea.children.length == 0)
        {
            flightsListEmpty()
        }
    }
    else
    {
        infoSpan.innerHTML = data[0].Message
    }
}

function flightsListEmpty() {
    var listArea = document.getElementById("flightsListArea");
    listArea.innerHTML = "";

    var infoDiv1 = document.createElement("div");
    infoDiv1.className = "row";

    var infoDiv2 = document.createElement("div");
    infoDiv2.className = "col-md-12 text-center";

    var infoSpan = document.createElement("span");
    infoSpan.innerHTML = "Flights are not found";

    infoDiv2.appendChild(infoSpan);
    infoDiv1.appendChild(infoDiv2);
    listArea.appendChild(infoDiv1);

}


//Delete user
function deleUser(data)
{
    var answerArea = document.getElementById("deleteOperationAnswerId");
    answerArea.innerHTML = "";

    if (data.Sucsses == true)
    {
        var removedElement = document.getElementById(data.Id);
        removedElement.remove();
    }

    var borderDiv = document.createElement("div");
    borderDiv.classList = "content-elements-border content-element-background";

    var containerDiv = document.createElement("div");
    containerDiv.classList = "area-element-margin table-element-margin text-center";

    var messageSpan = document.createElement("span");
    messageSpan.innerHTML = data.StatusMessage;

    containerDiv.appendChild(messageSpan);
    borderDiv.appendChild(containerDiv);
    answerArea.appendChild(borderDiv);
}
