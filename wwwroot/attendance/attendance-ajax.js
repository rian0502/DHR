$(document).ready(function () {
    function formatDecimal(time) {
        if (time === null || time === undefined) return '-';
        return time.toFixed(2);
    }
    function statusAttendance(code) {
        switch (code) {
            case '1':
                return `<span class="badge bg-primary">Entered</span>`; // Masuk
            case 'A':
                return `<span class="badge bg-danger">Alpha</span>`; // Alpha
            case 'P':
                return `<span class="badge bg-info">Business Trip</span>`; // Perjalanan Dinas
            case 'D':
                return `<span class="badge bg-warning">Dispensation</span>`; // Dispensasi
            case 'L':
                return `<span class="badge bg-secondary">Holiday</span>`; // Hari Libur
            case 'S':
                return `<span class="badge bg-warning">Sick</span>`; // Sakit
            case 'C':
                return `<span class="badge bg-warning">Leave</span>`; // Cuti
            case 'I':
                return `<span class="badge bg-info">Permission</span>`; // Ijin
            case 'CB':
                return `<span class="badge bg-warning">Collective Leave</span>`; // Cuti Bersama
            case 'M':
                return `<span class="badge bg-warning">Maternity Leave</span>`; // Cuti Melahirkan
            case 'N':
                return `<span class="badge bg-secondary">National Holiday</span>`; // Libur Nasional
            default:
                return `<span class="badge bg-dark">Unknown Status</span>`; // Default
        }
    }
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
                var tableBody = $('#attendanceRow tbody');
                tableBody.empty();
                var totalDays = 0;
                var totalLeave = 0;
                var totalMeal = 0;
                var totalNational = 0;
                response.attendance.result.forEach(function (attendance) {
                    var row = `<tr>
                        <td>${new Date(attendance.date).toLocaleDateString()}</td>
                        <td>${formatDecimal(attendance.checkIn)}</td>
                        <td>${formatDecimal(attendance.checkOut)}</td>
                        <td>${statusAttendance(attendance.code)}</td>
                        <td>${attendance.note}</td>
                    </tr>`;
                    tableBody.append(row);
                    if (attendance.code == 'L') {
                        totalLeave++;
                    }
                    if (attendance.mealAllowance == 1) {
                        totalMeal++;
                    }
                    if (attendance.code == 'N') {
                        totalNational++;
                    }
                    totalDays++;

                });
                $('#total-days').text(totalDays);
                $('#work-days').text(totalDays - totalLeave - totalNational);
                $('#meal-days').text(totalMeal);
                $('#meal-total').text(totalMeal * 85000);
            },
            error: function () {
                alert("Failed to load data.");
            }
        });
    });
});
