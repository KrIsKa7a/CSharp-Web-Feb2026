$(document).ready(function () {
    $(".buy-ticket-btn").on("click", function () {
        const cinemaId = $(this).attr("data-cinema-id");
        const cinemaName = $(this).attr("data-cinema-name");
        const movieId = $(this).attr("data-movie-id");
        const movieName = $(this).attr("data-movie-name");

        if (!cinemaId || !movieId) {
            Swal.fire("Error!", "Missing Cinema ID or Movie ID.", "error");
            return;
        }

        $("#cinemaId").val(cinemaId);
        $("#movieId").val(movieId);
        $("#cinemaNamePlaceholder").text(cinemaName);

        const showTimesSelect = $("#showtime");
        showTimesSelect.find("option")
            .remove() // Clear existing options
            .append('<option value="">Select Showtime...</option>');

        $.ajax({
            url: `/api/MovieApi/GetShowTimes?movieId=${movieId}&cinemaId=${cinemaId}`,
            method: "GET",
            success: function (response) {
                for (const showtimeDto of response) {
                    showTimesSelect.append(new Option(showtimeDto.showtime, showtimeDto.id));
                }
            },
            error: function (xhr) {
                let errorMessage = "An error occurred while purchasing tickets.";
                console.error("Raw Response:", xhr.responseText); // Log the raw response

                try {
                    if (xhr.responseJSON) {
                        errorMessage = xhr.responseJSON.title || xhr.responseJSON.message || errorMessage;
                    } else if (xhr.responseText) {
                        errorMessage = xhr.responseText; // Use response as-is if it's plain text
                    }
                } catch (e) {
                    console.error("Error processing response:", e);
                }

                Swal.fire("Error!", errorMessage, "error");
            }

        });

        $("#buyTicketModalLabel")
            .text(`Buy Ticket - ${cinemaName} `)
            .append($("<br>"))
            .append($("<small>").addClass("text-muted").text(movieName));

        $("#buyTicketModal").modal("show");
    });

    $("#buyTicketButton").on("click", function () {
        const requestData = {
            cinemaId: $("#cinemaId").val().trim(),
            movieId: $("#movieId").val().trim(),
            quantity: parseInt($("#quantity").val(), 10),
            projectionId: $("#showtime").find(":selected").val()
        };

        if (!requestData.quantity || requestData.quantity < 1) {
            $("#errorMessage").text("Please enter a valid ticket quantity.").removeClass("d-none");
            return;
        }

        $.ajax({
            url: "/api/TicketApi/BuyTicket",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(requestData),
            success: function (response) {
                Swal.fire("Success!", "Your ticket has been purchased successfully!", "success");
                $("#buyTicketModal").modal("hide");
            },
            error: function (xhr) {
                let errorMessage = "An error occurred while purchasing tickets.";
                console.error("Raw Response:", xhr.responseText); // Log the raw response

                try {
                    if (xhr.responseJSON) {
                        errorMessage = xhr.responseJSON.title || xhr.responseJSON.message || errorMessage;
                    } else if (xhr.responseText) {
                        errorMessage = xhr.responseText; // Use response as-is if it's plain text
                    }
                } catch (e) {
                    console.error("Error processing response:", e);
                }

                Swal.fire("Error!", errorMessage, "error");
            }
        });
    });
});
