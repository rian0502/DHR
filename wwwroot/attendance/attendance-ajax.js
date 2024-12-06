$(document).ready(function () {
    function formatDecimal(time) {
        return time === null || time === undefined ? '-' : time.toFixed(2);
    }

    function noteLateInformation(late, note, code, lateCount) {
        if (note && note.trim() !== '') {
            return note;
        }
        if (lateCount > 3 && code === '-') {
            return `<span class="badge bg-danger">Not eligible for meal allowance due to being late ${lateCount} times</span>`;
        }
        if (late > 0) {
            return `<span class="badge bg-warning">Late ${late} Hour</span>`;
        }
        return '-';
    }

    function statusAttendance(code) {
        switch (code) {
            case '1': return `<span class="badge bg-primary">Entered</span>`;
            case 'A': return `<span class="badge bg-danger">Alpha</span>`;
            case 'P': return `<span class="badge bg-info">Business Trip</span>`;
            case 'D': return `<span class="badge bg-warning">Dispensation</span>`;
            case 'L': return `<span class="badge bg-secondary">Holiday</span>`;
            case 'S': return `<span class="badge bg-warning">Sick</span>`;
            case 'C': return `<span class="badge bg-warning">Leave</span>`;
            case 'I': return `<span class="badge bg-info">Permission</span>`;
            case 'CB': return `<span class="badge bg-warning">Collective Leave</span>`;
            case 'M': return `<span class="badge bg-warning">Maternity Leave</span>`;
            case 'N': return `<span class="badge bg-secondary">National Holiday</span>`;
            case '-': return `<span class="badge bg-primary">Entered</span>`;
            default: return `<span class="badge bg-dark">Unknown Status</span>`;
        }
    }

    function formatDateWithDay(dateString) {
        const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        const dateObj = new Date(dateString);
        const day = days[dateObj.getDay()];
        const formattedDate = dateObj.toLocaleDateString();
        return `${day}, ${formattedDate}`;
    }

    const table = $('#attendanceRow').DataTable({
        ajax: {
            url: '/Attendance/GetAttendance',
            type: 'POST',
            data: function () {
                return {
                    periodId: $('.select2bs4').val(),
                    year: $('#year-period').val(),
                };
            },
            headers: {
                'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val()
            },
            beforeSend: function () {
                Swal.fire({
                    title: 'Loading...',
                    html: '<strong>Loading data, please wait !.</strong>',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            complete: function () {
                Swal.close();
            },
            dataSrc: function (response) {
                let totalDays = 0;
                let totalLeave = 0;
                let totalMeal = 0;
                let totalNational = 0;
                let lateCount = 0;
                const tableRows = [];

                response.attendance.result.forEach(function (attendance, index) {
                    const isMealEligible = attendance.mealAllowance === 1 && attendance.code !== '-';
                    if (attendance.late > 0) {
                        lateCount++;
                    }
                    tableRows.push({
                        no: index + 1,
                        date: formatDateWithDay(attendance.date),
                        checkIn: formatDecimal(attendance.checkIn),
                        checkOut: formatDecimal(attendance.checkOut),
                        status: statusAttendance(attendance.code),
                        note: noteLateInformation(attendance.late, attendance.note, attendance.code, lateCount)
                    });

                    if (attendance.code === 'L') totalLeave++;
                    if (isMealEligible || attendance.code === 'P') totalMeal++;
                    if (attendance.code === 'N') totalNational++;
                    totalDays++;
                });

                $('#total-days').text(totalDays);
                $('#work-days').text(totalDays - totalLeave - totalNational);
                $('#meal-days').text(totalMeal);
                $('#meal-total').text(totalMeal * 85000);

                return tableRows;
            }
        },
        columns: [
            { data: 'no', className: 'text-center' },
            { data: 'date'},
            { data: 'checkIn' },
            { data: 'checkOut' },
            { data: 'status', className: 'text-center' },
            { data: 'note' }
        ],
        paging: false,
        lengthChange: true,
        searching: false,
        ordering: true,
        info: true,
        autoWidth: false,
        responsive: true
    });

    $('#loadAttendanceUser').click(function () {
        table.ajax.reload();
    });
});