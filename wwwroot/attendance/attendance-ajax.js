$(document).ready(function () {
    function formatDecimal(time) {
        return time === null || time === undefined ? '-' : time.toFixed(2);
    }
    function noteLateInformation(late, note, code, lateCount) {
        let lang = document.cookie
            .split('; ')
            .find(row => row.startsWith('lang='))
            ?.split('=')[1];
        if (lang !== 'id' && lang !== 'en') {
            lang = 'en';
        }
        const translations = {
            en: {
                lateMessage: 'Late',
                lateHour: 'Hour',
                lateCountMessage: `Not eligible for allowance due to being late {count} times`,
                default: '-'
            },
            id: {
                lateMessage: 'Terlambat',
                lateHour: 'Jam',
                lateCountMessage: `Tidak mendapat tunjangan karena terlambat {count} kali`,
                default: '-'
            }
        };
        if (note && note.trim() !== '') {
            return note;
        }
        if (lateCount > 3 && code === '-') {
            return `<span class="badge bg-danger">${translations[lang].lateCountMessage.replace("{count}", lateCount)}</span>`;
        }
        if (late > 0) {
            return `<span class="badge bg-warning">${translations[lang].lateMessage} ${late} ${translations[lang].lateHour}</span>`;
        }
        return translations[lang].default;
    }
    function statusAttendance(code) {
        let lang = document.cookie
            .split('; ')
            .find(row => row.startsWith('lang='))
            ?.split('=')[1];
        if (lang !== 'id' && lang !== 'en') {
            lang = 'en';
        }
        const translations = {
            en: {
                '1': 'Entered',
                'A': 'Alpha',
                'P': 'Business Trip',
                'D': 'Dispensation',
                'L': 'Holiday',
                'S': 'Sick',
                'C': 'Leave',
                'I': 'Permission',
                'CB': 'Collective Leave',
                'M': 'Maternity Leave',
                'N': 'National Holiday',
                '-': 'Late',
                default: 'Unknown Status'
            },
            id: {
                '1': 'Masuk',
                'A': 'Alpha',
                'P': 'Perjalanan Dinas',
                'D': 'Dispensasi',
                'L': 'Libur',
                'S': 'Sakit',
                'C': 'Cuti',
                'I': 'Izin',
                'CB': 'Cuti Bersama',
                'M': 'Cuti Melahirkan',
                'N': 'Libur Nasional',
                '-': 'Terlambat',
                default: 'Tidak Diketahui'
            }
        };
        const text = translations[lang][code] || translations[lang].default;
        return `<span class="badge bg-${getBadgeColor(code)}">${text}</span>`;
    }

    function getBadgeColor(code) {
        switch (code) {
            case '1': return 'primary';
            case 'A': return 'danger';
            case 'P': return 'info';
            case 'D': return 'warning';
            case 'L': return 'secondary';
            case 'S': return 'warning';
            case 'C': return 'warning';
            case 'I': return 'info';
            case 'CB': return 'warning';
            case 'M': return 'warning';
            case 'N': return 'secondary';
            case '-': return 'danger';
            default: return 'dark';
        }
    }


    function formatDateWithDay(dateString) {
        let lang = document.cookie
            .split('; ')
            .find(row => row.startsWith('lang='))
            ?.split('=')[1];

        if (lang !== 'id' && lang !== 'en') {
            lang = 'en';
        }
        const daysFormatter = new Intl.DateTimeFormat(lang, { weekday: 'long' });
        const days = daysFormatter.formatToParts(new Date(dateString))
            .find(part => part.type === 'weekday')?.value;
        const formattedDate = new Date(dateString).toLocaleDateString(lang);

        return `${days}, ${formattedDate}`;
    }


    const table = $('#attendanceRow').DataTable({
        ajax: {
            url: '/Attendance/GetAttendanceData',
            type: 'POST',
            data: function () {
                return {
                    periodId: $('.select2bs4').val(),
                };
            },
            headers: {
                'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val()
            },
            beforeSend: function () {
                Swal.fire({
                    title: 'Loading...',
                    html: '<strong>Loading data, please wait!</strong>',
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
                let totalAlpha = 0;
                let totalLeave = 0;
                let timeOff = 0;
                let totalMeal = 0;
                let totalNational = 0;
                let lateCount = 0;
                let totalMealAmount = 0;
                const tableRows = [];

                response.attendance.forEach(function (attendance, index) {
                    const isMealEligible = attendance.mealAllowance === 1 && attendance.code !== '-' || attendance.code === 'P';
                    if (attendance.late > 0) {
                        lateCount++;
                    }

                    if (attendance.code === 'S' || attendance.code === 'M' || attendance.code === 'C' || attendance.code === 'I') {
                        timeOff++;
                    }

                    if (attendance.code === 'A') {
                        totalAlpha++;
                    }

                    if (isMealEligible) {
                        totalMeal++;
                        totalMealAmount += attendance.benefitAmount || 0;
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
                    if (attendance.code === 'N') totalNational++;
                    totalDays++;
                });

                const workDays = totalDays - totalLeave - totalNational;
                const mealAllowancePerDay = totalMeal > 0 ? (totalMealAmount / totalMeal) : 0;

                $('#total-days').text(totalDays);
                $('#work-days').text(workDays);
                $('#meal-days').text(totalMeal);
                $('#meal-allowance-days').text(mealAllowancePerDay.toLocaleString('id-ID'));
                $('#meal-total').text(totalMealAmount.toLocaleString('id-ID'));

                $('#time-off').text(timeOff);
                $('#late-days').text(lateCount);
                $('#alpha-days').text(totalAlpha);

                return tableRows;
            }
        },
        columns: [
            {data: 'no', className: 'text-center'},
            {data: 'date'},
            {data: 'checkIn'},
            {data: 'checkOut'},
            {data: 'status', className: 'text-center'},
            {data: 'note'}
        ],
        paging: false,
        lengthChange: true,
        searching: false,
        ordering: true,
        info: true,
        autoWidth: false,
        responsive: true,
        fixedHeader: true
    });

    $('#loadAttendanceUser').click(function () {
        table.ajax.reload();
    });
});