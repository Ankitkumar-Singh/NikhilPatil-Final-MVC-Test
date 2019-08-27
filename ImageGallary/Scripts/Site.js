// Open the Modal
function openModal(imageId) {
    document.getElementById("myModal").style.display = "block";

    if (imageId != null && imageId.id != "") {
        $.ajax({
            url: "/User/Comments",
            dataType: "HTML",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{ 'imageId': " + imageId.id + "}",
            success: function (result) {
                $('#comments').html(result);
            },
            error: function (response) {
                alert(response.statusText);
            }
        });

        $.ajax({
            url: "/User/Tags",
            dataType: "HTML",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{ 'imageId': " + imageId.id + "}",
            success: function (result1) {
                $('#tags').html(result1);
            },
            error: function (response) {
                alert(response.statusText);
            }
        });
    }    


}

// Close the Modal
function closeModal() {
    document.getElementById("myModal").style.display = "none";
}

var slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    //if (imageId != null && imageId.id != "") {
    //    $.ajax({
    //        url: "/User/Comments",
    //        dataType: "HTML",
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        data: "{ 'imageId': " + imageId.id + "}",
    //        success: function (result) {
    //            $('#comments').html(result);
    //        },
    //        error: function (response) {
    //            alert(response.statusText);
    //        }
    //    });
    //}  
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    //dots[slideIndex - 1].className += " active";
    //captionText.innerHTML = dots[slideIndex - 1].alt;
}