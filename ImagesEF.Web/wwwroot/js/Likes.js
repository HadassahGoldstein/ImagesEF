$(() => {
    setInterval(() => {   
        const id = $("#id").val();        
        $.get("/Home/CurrentLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })                
    }, 500)
    $("#like-btn").on('click', function () {
        const id = $("#id").val();
        $.post("/Home/AddLike", { id }, function (likes) {
        })
        $("#like-btn").prop('disabled', true);
    })
})