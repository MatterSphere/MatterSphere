var interval;

$(document).ready(function () {

    interval = setInterval(function () {
        $.ajax({
            type: "GET",
            url: "/state",
            success: function (data) {

                if (data == "NotInProgress") {
                    $("#syncActionNTUsers").show();
                    $("#syncActionNTUserNames").show();                    
                    $("#loadProcess").hide();
                    $("#errorInfo").hide();
                    $("#successInfo").hide();
                };

                if (data == "InProgress") {
                    $("#syncActionNTUsers").hide();
                    $("#syncActionNTUserNames").hide();                   
                    $("#loadProcess").show();
                    $("#errorInfo").hide();
                    $("#successInfo").hide();
                };

                if (data.includes("Success:")) {
                    $("#syncActionNTUsers").show();
                    $("#syncActionNTUserNames").show();                   
                    $("#loadProcess").hide();
                    $("#successInfo").html("");
                    $("#successInfo").append(data);
                    $("#successInfo").show();
                    $("#errorInfo").hide();
                };

                if (data.includes("Error:")) {
                    $("#errorInfo").html("");
                    $("#errorInfo").append(data);
                    $("#errorInfo").show();
                    $("#syncActionNTUsers").show();
                    $("#syncActionNTUserNames").show();                   
                    $("#loadProcess").hide();
                    $("#successInfo").hide();
                };

            },
            error: function (xhr, textStatus, errorThrown) {
                $("#errorInfo").html("");
                $("#errorInfo").append("Unhandled Error: " + errorThrown);
                $("#errorInfo").show();
                $("#syncActionNTUsers").show();
                $("#syncActionNTUserNames").show();               
                $("#loadProcess").hide();
                $("#successInfo").hide();
            }

        }
        );
    }, 1000);

    $("#syncActionNTUsers").click(function () {
        $.ajax({
            type: "POST",
            url: "/syncntusers"
        });
    });
    $("#syncActionNTUserNames").click(function () {
        $.ajax({
            type: "POST",
            url: "/syncusernames"
        });
    });
});
