$(function () {
    $.ajax({
        url: "/api/Tags/Clouds",
        type: "GET",
        async: false,
        success: function (data) {
            $('#tags').jQCloud(data, {
                width: 300,
                height: 200
            });
        }
    });
    
})