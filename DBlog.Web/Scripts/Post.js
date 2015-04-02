

$(function () {
    $('#editForm').validate({       
        rules: {
            Title: {
                required: true
            },
            Slug: {
                required: true,
                remote: "/api/Slug/Exist/?slug=" + $("input[name='Slug']").val()
            },

        }
    });
     


    $("input[name='Title']").change(function () {
        var title = $(this).val();
        if (title != "") {
            $.ajax({
                url: "/api/Slug/Trans/?name="+title,
                type: "GET",                
                async: false,
                success: function (data) {
                    $("input[name='Slug']").val(data);
                }
            })
        }
    });

    $('form').validate({
        rules: {
            Slug: {
                required: true,
                remote: "/api/Slug/Exist/?slug=" + $("input[name='Slug']").val(),
            }
        }
    });

    //$("input[name='Slug']").change(function () {
    //    var slug = $(this).val();
    //    if (slug != ""){
    //        $.ajax({
    //            url: "/api/Slug/Exist?slug=" + slug,
    //            type: "GET",
    //            async: false,
    //            success: function (data) {
    //                if (data)
    //                {
                        
    //                }
    //            }
    //        })
    //    }
    //});

});