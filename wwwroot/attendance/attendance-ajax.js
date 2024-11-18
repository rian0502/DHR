$(document).ready(function () {
    $('#loadAttendanceUser').click(function () {
        var periodId = $('.select2bs4').val();
        var csrfToken = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/Attendance/GetAttendance',
            type: 'POST',
            data: {
                periodId: periodId
            },
            headers: {
                'RequestVerificationToken': csrfToken
            },
            success: function (response) {
                var tableBody = $('#example3 tbody');
                tableBody.empty();
                response.data.forEach(function (attendance) {
                    var row = `<tr>
                        <td>${attendance.Date}</td>
                        <td>${attendance.Day}</td>
                        <td>${attendance.In}</td>
                        <td>${attendance.Out}</td>
                        <td>${attendance.Status}</td>
                        <td>${attendance.Notes}</td>
                    </tr>`;
                    tableBody.append(row);
                });
            },
            error: function () {
                alert("Failed to load data.");
            }
        });
    });
});
