// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


//var klient = document.getElementById("klient");
//var rentBtn = document.querySelectorAll(".rentBtn");
//var returnBtn = document.querySelectorAll(".returnBtn");

//rentBtn.forEach(x => {
//    x.addEventListener("click", () => {
//        var klientIndex = klient.options[klient.selectedIndex].value;
//        var link = x.getAttribute("href");
//        var myArr = link.split("");
//        myArr.splice(-1, 1, klientIndex)
//        link = myArr.join("");
//        x.href = link;
//    });
//})

//rentBtn.forEach(x => {
//    x.addEventListener("click", (e) => {
//        e.preventDefault();
//        $.post("/RentedMovies/Rent",
//            {
//                CusId: klient.options[klient.selectedIndex].value,
//                TvShowId: x.id
//            });
//        x.style.display = "none";
//        returnBtn.forEach(y => {
//            if (x.id == y.id) {
//                y.style.display = "block";
//            }
//        })
//    });
//})
//returnBtn.forEach(y => {
//    y.addEventListener("click", (e) => {
//        e.preventDefault();
//        $.post("/RentedMovies/Return",
//        {
//            CusId: klient.options[klient.selectedIndex].value,
//            TvShowId: y.id
//        });
//        y.style.display = "none";
//        rentBtn.forEach(x => {
//            if (y.id == x.id) {
//                x.style.display = "block";
//            }
//        })
//    })
//})



//-------------------------------------------------------------------

function rent(id) {
    $.post("/RentedMovies/Rent",
        {
            CusId: document.getElementById("klient").value,
            TvShowId: id
        });

    var rentId = "#rent_" + id;
    $(rentId).hide();
    var retId = "#ret_" + id;
    $(retId).show();
}
function ret(id) {
    $.post("/RentedMovies/Return",
        {
            CusId: document.getElementById("klient").value,
            TvShowId: id
        });

    var rentId = "#rent_" + id;
    $(rentId).show();
    var retId = "#ret_" + id;
    $(retId).hide();
}
//---------------------------------------------dole za excel-ot
let tr = document.querySelectorAll('tr');
tr.forEach(e => {
    e.classList.add('myrow')
})
var myrow = $('.myrow')

function fnExcelReport() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    let last = myrow.find('th:last-child, td:last-child').remove()
    let first = myrow.find("td:first-child");

    for (var j = 0; j < myrow.length; j++) {
        tab_text = tab_text + myrow[j].innerHTML + "</tr>";
        myrow[j].append(last[j]);
    }
   
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table

    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}
//-----------------------------------------------------------------------------------

function mySearch() {
    let input = document.getElementById("myInput");
    let filter = input.value.toLowerCase();
    let myRow = document.querySelectorAll('.myRow');
    let title1 = document.querySelectorAll('.title1');
    let title2 = document.querySelectorAll('.title2');
    for (let i = 0; i < myRow.length; i++) {
        let titleText1 = title1[i].innerText;
        let titleText2 = title2[i].innerText;
        if (titleText1.toLowerCase().indexOf(filter) > -1 || titleText2.toLowerCase().indexOf(filter) > -1 ) {
                myRow[i].style.display = "";
            } else {
                myRow[i].style.display = "none";
            }
        }  
}
