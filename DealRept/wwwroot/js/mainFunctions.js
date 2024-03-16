///////////////////////////////////////SHOW/HIDE PASSWORD////////////////////////////////////////////////////

function showPass() {
    const inputPassEls = document.querySelectorAll(`.inputPass`);

    inputPassEls.forEach(function (inputPassEl) {
        if (inputPassEl.type === `password`) {
            inputPassEl.type = `text`;
        } else {
            inputPassEl.type = `password`;
        }
    });
}

///////////////////////////////////////GET THE CURRENT YEAR FOR A FOOTER////////////////////////////////////////////////////

$('#year').text(new Date().getFullYear());

///////////////////////////////////////TOOLTIPS, SHINE BUTTONS AND SHADOWS ON TOUCH DEVICES////////////////////////////////////////////////////

//Function to check if device with touchscreen
function is_touch_enabled() {
    return ('ontouchstart' in window) ||
        (navigator.maxTouchPoints > 0) ||
        (navigator.msMaxTouchPoints > 0);
}

/*Tooltip init*/

if (!is_touch_enabled()) {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip({
        });
    });

    $(".shadowBox").hover(
        function () {
            $(this).addClass('shadow-lg');
        }, function () {
            $(this).removeClass('shadow-lg');
        }
    );

} else {
    $('.enabledTooltipOnTouch').tooltip({
    });

    const shineBlueButtons = document.querySelectorAll(`.scaleBlueButton`);
    shineBlueButtons.forEach((button) => {
        button.style.outline = `thin solid white`;
        button.style.boxShadow = `0 0 0.5rem rgba(0, 45, 119, 1)`;
    });

    const shineWarningButtons = document.querySelectorAll(`.btn-modal-warning`);
    shineWarningButtons.forEach((button) => {
        button.style.outline = `thin solid white`;
        button.style.boxShadow = `0 0 0.5rem rgba(255, 104, 37, 1)`;
        button.style.textDecoration = `none !important`;
    });

    const shineSuccessButtons = document.querySelectorAll(`.btn-modal-success`);
    shineSuccessButtons.forEach((button) => {
        button.style.outline = `thin solid white`;
        button.style.boxShadow = `0 0 0.5rem rgba(32, 201, 117, 1)`;
        button.style.textDecoration = `none !important`;
    });

    $(".shadowBox").addClass('shadow-sm');

    $(".notDisplayOnTouch").css('display', 'none');
}

//Tooltip responsive disabling
menuTooltip();

$(window).resize(menuTooltip);

function menuTooltip() {
    if ($(window).width() < 992) {
        $('.disabledTooltipOnMd').tooltip('disable');
    }
    else if (!is_touch_enabled()) {
        $('.disabledTooltipOnMd').tooltip('enable');
    }
};


///////////////////////////////////////POPOVERS///////////////////////////////////////////////////////////////
// Init Popover
$('[data-toggle="popover"]').popover();

$('.popover-dismiss').popover({
    trigger: 'focus'
})

////////////////////////////////////////////FILE INPUT////////////////////////////////////////////////////////
if (document.getElementById('the-file') !== null) {

    $('.custom-file-input').on('change', function () {  // when the value changes
        $(this).valid(); // trigger validation on this element
    });

    document.getElementById('the-file').addEventListener('hover', function (e) {
        (this).setAttrubute('title', '');
    });

    document.getElementById('the-file').addEventListener('change', function (e) {
        const file = document.getElementById("the-file").files[0]
        const nextSibling = e.target.nextElementSibling;
        if (file) {
            const fileName = document.getElementById("the-file").files[0].name;
            nextSibling.innerText = fileName;
        } else {
            nextSibling.innerText = ``;
        }
    });

    if ($(window).width() > 667) {
        $(".custom-file-input").tooltip({
            title: function () {
                const file = document.querySelector(".custom-file-input").files[0];
                if (file) {
                    const fileName = document.getElementById("the-file").files[0].name;
                    return fileName;
                }
                return ``;
            },
            placement: "top",
            trigger: `hover`
        });
    }

}

////////////////////////////////////////////CONTRACTS CHARTS////////////////////////////////////////////////////////
if (document.getElementById('contractsChartButton') !== null) {

    function chartsLoad() {

        const _monthesAgo = $('input[name=monthesAgo]:checked', '#chartDataForm').val();

        const chartViewLeft = document.getElementById(`chartViewLeft`);

        const chartViewRight = document.getElementById(`chartViewRight`);

        chartViewLeft.innerHTML = '&nbsp;';
        chartViewRight.innerHTML = '&nbsp;';
        $('#chartViewLeft').append('<div class="chart-container barChartContainer"><canvas id = "contractChartLeft"></canvas></div>');
        $('#chartViewRight').append('<div class="chart-container barChartContainer"><canvas id = "contractChartRight"></canvas></div>');

        $.ajax({
            type: `POST`,
            url: `/Contracts/GetChartData`,
            data: { MonthesAgo: _monthesAgo },
            contextType: `application/json; charset=utf-8`,
            dataType: `json`,
            success: onSuccessResult,
            error: onError
        });

        Chart.defaults.font.family = "'-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif";
        Chart.defaults.font.size = 14;
        Chart.defaults.font.weight = `normal`;
        Chart.defaults.color = `#002D70`;

        function onSuccessResult(data) {

            const _data = data;
            const _dataLabels = _data["labels"];
            const _chartDataTotalLeft = _data["totalContractsQuantity"];
            const _chartDataCurrentLeft = _data["currentContractQuantities"];
            const _chartDataTotalRight = _data["totalContractAmmounts"];
            const _chartStartDate = _data["startDate"];
            const _chartEndDate = _data["endDate"];
            const _chartDataCurrentRight = _data["currentContractAmmounts"];

            new Chart(`contractChartLeft`, {
                type: `bar`,
                data:
                {
                    labels: _dataLabels,
                    datasets: [
                        {
                            label: 'Total contract quantity',
                            data: _chartDataTotalLeft,
                            borderColor: '#002D70',
                            backgroundColor: 'rgba(0, 45, 112, 0.6)',
                            hoverBackgroundColor: 'rgba(0, 45, 112, 0.7)',
                            borderWidth: 1,
                            borderRadius: 5,
                            borderSkipped: false,
                        },
                        {
                            label: 'Current contract quantity',
                            data: _chartDataCurrentLeft,
                            borderColor: "#ff6825",
                            backgroundColor: 'rgba(255, 104, 37, 0.7)',
                            hoverBackgroundColor: 'rgba(255, 104, 37, 0.8)',
                            borderWidth: 1,
                            borderRadius: 5,
                            borderSkipped: false,
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            ticks: {
                                precision: 0
                            }
                        }
                    },
                    indexAxis: `y`,
                    plugins: {
                        title: {
                            display: true,
                            text: `Monthly quantity of Contracts (${_chartStartDate} - ${_chartEndDate})`,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            padding: {
                                top: 0
                            }
                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: true,
                            labels: {
                                padding: 10,
                                useBorderRadius: true,
                                boxHeight: 12,
                                boxWidth: 20,
                            }
                        }
                    }
                }

            });

            new Chart(`contractChartRight`, {

                type: `bar`,
                data: {
                    labels: _dataLabels,
                    datasets: [
                        {
                            label: `Total contract amount`,
                            backgroundColor: `rgba(0, 45, 112, 0.7)`,
                            hoverBackgroundColor: `rgba(0, 45, 112, 0.8)`,
                            borderColor: `#002D70`,
                            borderWidth: 1,
                            borderRadius: 5,
                            borderSkipped: false,
                            data: _chartDataTotalRight
                        },
                        {
                            label: `Current contract amount`,
                            borderColor: "#ff6825",
                            backgroundColor: 'rgba(255, 104, 37, 0.7)',
                            hoverBackgroundColor: 'rgba(255, 104, 37, 0.8)',
                            borderWidth: 1,
                            borderRadius: 5,
                            borderSkipped: false,
                            data: _chartDataCurrentRight
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            ticks: {
                                precision: 0
                            },
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: `Monthly Contract amount (UAH) (${_chartStartDate} - ${_chartEndDate})`,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            padding: {
                                top: 0
                            }
                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: true,
                            labels: {
                                padding: 10,
                                useBorderRadius: true,
                                boxHeight: 12,
                                boxWidth: 20,
                            }
                        }
                    }
                },

            });

        }

        function onError(err) {
            document.getElementById(`errorMessage`).innerHTML = `<div id="errorMessage" class="text-danger mb-2">
            Something went wrong. Try again, and if the problem persists see your system administrator.
        </div>`;
        }
    }

    window.onload = chartsLoad;

    $(function () {
        $(`#contractsChartButton`).click(chartsLoad);

    });
}


////////////////////////////////////////////SUPPLIERS CHARTS////////////////////////////////////////////////////////

if (document.getElementById('supplierChartButton') !== null) {

    function chartsLoad() {

        const _isPastContractIncluded = $('input[name=isPastContractIncluded]:checked', '#chartDataForm').val();

        const chartViewLeft = document.getElementById(`chartViewLeft`);

        const chartViewRight = document.getElementById(`chartViewRight`);

        chartViewLeft.innerHTML = '&nbsp;';
        chartViewRight.innerHTML = '&nbsp;';
        $('#chartViewLeft').append('<div class="chart-container doughnutChartContainer"><canvas id = "suppliersChartLeft"></canvas></div>');
        $('#chartViewRight').append('<div class="chart-container doughnutChartContainer"><canvas id = "suppliersChartRight"></canvas></div>');

        $.ajax({
            type: `POST`,
            url: `/Suppliers/GetChartData`,
            data: { isPastContractIncluded: _isPastContractIncluded },
            contextType: `application/json; charset=utf-8`,
            dataType: `json`,
            success: onSuccessResult,
            error: onError
        });

        Chart.defaults.font.family = "'-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif";
        Chart.defaults.font.size = 14;
        Chart.defaults.font.weight = `normal`;
        Chart.defaults.color = `#002D70`;

        function onSuccessResult(data) {

            const _data = data;

            const _dataLabelsQuantity = _data["labelsQuantity"];
            const _chartDataLeft = _data["suppliersByContractQty"];

            const _dataLabelsAmount = _data["labelsAmount"];
            const _chartDataRight = _data["suppliersByContractAmt"];

            const _contractTypes = _data["isPastContractIncluded"] ? "current & past" : "current";


            new Chart(`suppliersChartLeft`, {
                type: `doughnut`,
                data:
                {
                    labels: _dataLabelsQuantity,
                    datasets: [
                        {
                            label: `Suppliers qty`,
                            data: _chartDataLeft,
                            backgroundColor: [
                                'rgb(255, 104, 37, 0.7)',
                                'rgb(0, 45, 112, 0.7)',
                                'rgb(32, 201, 117, 0.7)'
                            ],
                            hoverBackgroundColor: [
                                'rgb(255, 104, 37, 0.8)',
                                'rgb(0, 45, 112, 0.8)',
                                'rgb(32, 201, 117, 0.8)'
                            ],
                            borderColor:
                                [
                                    'rgb(255, 104, 37, 0.7)',
                                    'rgb(0, 45, 112, 0.7)',
                                    'rgb(32, 201, 117, 0.7)'
                                ],
                            borderSkipped: false,
                            borderWidth: 1,
                            hoverBorderWidth: 2,
                            cutout: 30,
                            offset: 5,
                            hoverOffset: 10,
                            borderRadius: 3,
                        }
                    ]
                },
                options: {
                    responsive: true,
                    layout: {
                        padding: {
                            bottom: 10
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: `Suppliers qty by their ${_contractTypes} contracts qty`,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            padding: {
                                top: 0,
                                bottom: 10
                            }

                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: false,
                        }
                    }
                }

            });

            new Chart(`suppliersChartRight`, {
                type: `doughnut`,
                data:
                {
                    labels: _dataLabelsAmount,
                    datasets: [
                        {
                            label: `Suppliers qty`,
                            data: _chartDataRight,
                            backgroundColor: [
                                'rgb(255, 104, 37, 0.7)',
                                'rgb(0, 45, 112, 0.7)',
                                'rgb(32, 201, 117, 0.7)'
                            ],
                            hoverBackgroundColor: [
                                'rgb(255, 104, 37, 0.8)',
                                'rgb(0, 45, 112, 0.8)',
                                'rgb(32, 201, 117, 0.8)'
                            ],
                            borderColor:
                                [
                                    'rgb(255, 104, 37, 0.7)',
                                    'rgb(0, 45, 112, 0.7)',
                                    'rgb(32, 201, 117, 0.7)'
                                ],
                            borderSkipped: false,
                            borderWidth: 1,
                            hoverBorderWidth: 2,
                            cutout: 30,
                            offset: 5,
                            hoverOffset: 10,
                            borderRadius: 3
                        }
                    ]
                },
                options: {
                    responsive: true,
                    layout: {
                        padding: {
                            bottom: 10
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            text: `Suppliers qty by their total ${_contractTypes} contracts amt`,
                            padding: {
                                top: 0,
                                bottom: 10
                            }

                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: false,
                        }
                    }
                }
            });
        }

        function onError(err) {
            document.getElementById(`errorMessage`).innerHTML = `<div id="errorMessage" class="text-danger mb-2">
            Something went wrong. Try again, and if the problem persists see your system administrator.
        </div>`;
        }
    }

    window.onload = chartsLoad;

    $(function () {
        $(`#supplierChartButton`).click(chartsLoad);

    });
}

////////////////////////////////////////////BRANCHES CHARTS////////////////////////////////////////////////////////

if (document.getElementById('branchChartButton') !== null) {

    function chartsLoad() {

        const _isPastContractIncluded = $('input[name=isPastContractIncluded]:checked', '#chartDataForm').val();

        const chartViewLeft = document.getElementById(`chartViewLeft`);

        const chartViewRight = document.getElementById(`chartViewRight`);

        chartViewLeft.innerHTML = '&nbsp;';
        chartViewRight.innerHTML = '&nbsp;';
        $('#chartViewLeft').append('<div class="chart-container doughnutChartContainer"><canvas id = "branchChartLeft"></canvas></div>');
        $('#chartViewRight').append('<div class="chart-container doughnutChartContainer"><canvas id = "branchChartRight"></canvas></div>');

        $.ajax({
            type: `POST`,
            url: `/Branches/GetChartData`,
            data: { isPastContractIncluded: _isPastContractIncluded },
            contextType: `application/json; charset=utf-8`,
            dataType: `json`,
            success: onSuccessResult,
            error: onError
        });

        Chart.defaults.font.family = "'-apple-system', 'BlinkMacSystemFont', 'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif";
        Chart.defaults.font.size = 14;
        Chart.defaults.font.weight = `normal`;
        Chart.defaults.color = `#002D70`;

        function onSuccessResult(data) {

            const _data = data;

            const _dataLabelsQuantity = _data["labelsQuantity"];
            const _chartDataLeft = _data["branchesByContractQty"];

            const _dataLabelsAmount = _data["labelsAmount"];
            const _chartDataRight = _data["branchesByContractAmt"];

            const _contractTypes = _data["isPastContractIncluded"] ? "current & past" : "current";


            new Chart(`branchChartLeft`, {
                type: `polarArea`,
                data:
                {
                    labels: _dataLabelsQuantity,
                    datasets: [
                        {
                            label: `Branches qty`,
                            data: _chartDataLeft,
                            backgroundColor: [
                                'rgb(255, 104, 37, 0.7)',
                                'rgb(0, 45, 112, 0.7)',
                                'rgb(32, 201, 117, 0.7)'
                            ],
                            hoverBackgroundColor: [
                                'rgb(255, 104, 37, 0.8)',
                                'rgb(0, 45, 112, 0.8)',
                                'rgb(32, 201, 117, 0.8)'
                            ],
                            borderColor:
                                [
                                    'rgb(255, 104, 37, 0.7)',
                                    'rgb(0, 45, 112, 0.7)',
                                    'rgb(32, 201, 117, 0.7)'
                                ],
                            borderSkipped: false,
                            borderWidth: 1,
                            hoverBorderWidth: 2,
                            offset: 5,
                            hoverOffset: 10,
                            borderRadius: 3
                        }
                    ]
                },
                options: {
                    responsive: true,
                    layout: {
                        padding: {
                            bottom: 10
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: `Branches qty by their ${_contractTypes} contracts qty`,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            padding: {
                                top: 0,
                                bottom: 10
                            }

                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: false,
                        }
                    }
                }

            });

            new Chart(`branchChartRight`, {
                type: `polarArea`,
                data:
                {
                    labels: _dataLabelsAmount,
                    datasets: [
                        {
                            label: `Branches qty`,
                            data: _chartDataRight,
                            backgroundColor: [
                                'rgb(255, 104, 37, 0.7)',
                                'rgb(0, 45, 112, 0.7)',
                                'rgb(32, 201, 117, 0.7)'
                            ],
                            hoverBackgroundColor: [
                                'rgb(255, 104, 37, 0.8)',
                                'rgb(0, 45, 112, 0.8)',
                                'rgb(32, 201, 117, 0.8)'
                            ],
                            borderColor:
                                [
                                    'rgb(255, 104, 37, 0.7)',
                                    'rgb(0, 45, 112, 0.7)',
                                    'rgb(32, 201, 117, 0.7)'
                                ],
                            borderSkipped: false,
                            borderWidth: 1,
                            hoverBorderWidth: 2,
                            offset: 5,
                            hoverOffset: 10,
                            borderRadius: 3
                        }
                    ]
                },
                options: {
                    responsive: true,
                    layout: {
                        padding: {
                            bottom: 10
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            font: {
                                size: 16,
                                weight: `normal`,
                            },
                            text: `Branches qty by their total ${_contractTypes} contracts amt`,
                            padding: {
                                top: 0,
                                bottom: 10
                            }

                        },
                        tooltip: {
                            backgroundColor: `#f8f9fa`,
                            titleColor: `#495057`,
                            bodyColor: `#495057`,
                            borderColor: `#ced4da`,
                            borderWidth: 1,
                            font: {
                                size: 12,
                                weight: `normal`
                            }
                        },
                        legend: {
                            display: false,
                        }
                    },
                    scales: {
                        r: {
                            ticks: {
                                stepSize: 1
                            }
                        }
                    }
                }

            });
        }

        function onError(err) {
            document.getElementById(`errorMessage`).innerHTML = `<div id="errorMessage" class="text-danger mb-2">
            Something went wrong. Try again, and if the problem persists see your system administrator.
        </div>`;
        }
    }

    window.onload = chartsLoad;

    $(function () {
        $(`#branchChartButton`).click(chartsLoad);

    });
}



////////////////////////////////////////////VALIDATION////////////////////////////////////////////////////////

$.validator.addMethod('filesize', function (value, element, params) {

    if (element.files.length > 0) {
        const fileSize = element.files[0].size;
        const maxFileSize = params[1];
        return fileSize <= maxFileSize;
    }

    return true;
});

$.validator.unobtrusive.adapters.add('filesize', ['maxfilesize'], function (options) {
    const element = $(options.form).find('input#the-file')[0];

    options.rules['filesize'] = [element, parseInt(options.params['maxfilesize'])];
    options.messages['filesize'] = options.message;
});

//Add min property to ExperationDate on Conclusion Date change
if (document.getElementById(`expirationDate`)) {
    const expirationDate = document.getElementById(`expirationDate`);
    const conclusionDate = document.getElementById(`conclusionDate`);
    conclusionDate.addEventListener(`change`, (e) => {
        newDate = new Date(e.target.value);
        newDate.setDate(newDate.getDate() + 1);
        expirationDate.min = newDate.toISOString().slice(0, 10);
    });
}

var settings = {
    validClass: "is-valid",
    errorClass: "is-invalid",
    ignore: []
};
$.validator.setDefaults(settings);
$.validator.unobtrusive.options = settings;

//Validation add bs class
$(document).ready(function () {
    $("input.input-validation-error").addClass("is-invalid");

    $("textarea.input-validation-error").addClass("is-invalid");

    $("select.input-validation-error").addClass("is-invalid");

    $(".custom-file-input .input-validation-error").addClass("is-invalid");
});


////////////////////////////////////////////SELECT2////////////////////////////////////////////////////////
$(document).ready(function () {

    //Init select2
    $('#supplierLCodeSelect').select2({
        dropdownCssClass: 'text-primary',
        minimumInputLength: '3',
        selectionCssClass: 'text-primary',
        placeholder: `Choose a Supplier`
    });

    $('#branchCodeSelect').select2({
        dropdownCssClass: 'text-primary',
        minimumInputLength: '3',
        selectionCssClass: 'text-primary',
        placeholder: `Choose a Branch`
    });

    $('#contractStatusSelect').select2({
        dropdownCssClass: 'text-primary',
        selectionCssClass: 'text-primary'
    });

    $('#supplierSpecializationsSelect').select2({
        dropdownCssClass: 'text-primary',
        minimumInputLength: '3',
        selectionCssClass: 'text-primary',
        placeholder: `Choose a Specialization`
    });

    $('#citySelect').select2({
        dropdownCssClass: 'text-primary',
        minimumInputLength: '3',
        selectionCssClass: 'text-primary',
        placeholder: `Choose a City`,
    });

    $('#bankSelect').select2({
        dropdownCssClass: 'text-primary',
        minimumInputLength: '3',
        selectionCssClass: 'text-primary',
        placeholder: `Choose a Bank`
    });


    // trigger validation on this element
    $('select').on('change', function () {  // when the value changes
        $(this).valid(); // trigger validation on this element
    });

    //select2 tooltips init
    $('.select2-selection__rendered').hover(function () {
        $(this).prop('title', '');
    });


    if (!is_touch_enabled()) {

        $("#supplierLCodeSelect + .select2-container").tooltip({
            title: function () {
                let bsTitle = $(`#supplierLCodeSelect + .select2-container .select2-selection__rendered`).prop(`innerText`);
                let placeholderText = $(`#supplierLCodeSelect + .select2-container .select2-selection__placeholder`).prop(`innerText`)
                if (bsTitle === placeholderText) {
                    bsTitle = ``;
                }
                return bsTitle;
            },
            placement: "auto",
            trigger: `hover`
        });

        $("#branchCodeSelect + .select2-container").tooltip({
            title: function () {
                let bsTitle = $(`#branchCodeSelect +.select2-container  .select2-selection__rendered`).prop(`innerText`);
                let placeholderText = $(`#branchCodeSelect +.select2-container .select2-selection__placeholder`).prop(`innerText`)
                if (bsTitle === placeholderText) {
                    bsTitle = ``;
                }
                return bsTitle;
            },
            placement: "auto",
            trigger: `hover`
        });

        $("#contractStatusSelect + .select2-container").tooltip({
            title: "<b>Warning:</b> if you change the Status it won`t be possible to edit the Contract later!",
            placement: "auto",
            trigger: `hover`,
            html: true
        });

        $("#supplierSpecializationsSelect + .select2-container").tooltip({
            title: function () {
                let bsTitle = $(`#supplierSpecializationsSelect +.select2-container  .select2-selection__rendered`).prop(`innerText`);
                let placeholderText = $(`#supplierSpecializationsSelect +.select2-container .select2-selection__placeholder`).prop(`innerText`)
                if (bsTitle === placeholderText) {
                    bsTitle = ``;
                }
                return bsTitle;
            },
            placement: "auto",
            trigger: `hover`
        });

        $("#citySelect + .select2-container").tooltip({
            title: function () {
                let bsTitle = $(`#citySelect +.select2-container  .select2-selection__rendered`).prop(`innerText`);
                let placeholderText = $(`#citySelect +.select2-container .select2-selection__placeholder`).prop(`innerText`)
                if (bsTitle === placeholderText) {
                    bsTitle = ``;
                }
                return bsTitle;
            },
            placement: "auto",
            trigger: `hover`
        });

        $("#bankSelect + .select2-container").tooltip({
            title: function () {
                let bsTitle = $(`#bankSelect +.select2-container  .select2-selection__rendered`).prop(`innerText`);
                let placeholderText = $(`#bankSelect +.select2-container .select2-selection__placeholder`).prop(`innerText`)
                if (bsTitle === placeholderText) {
                    bsTitle = ``;
                }
                return bsTitle;
            },
            placement: "auto",
            trigger: `hover`
        });
    }
});

////////////////////////////////////////////CITY & BANK DELETE MODAL////////////////////////////////
$('#modalDeleteEntity').on('show.bs.modal', function (event) {

    const button = $(event.relatedTarget); // Button that triggered the modal
    const entity = button.data(`entity`);
    const entityCode = button.data(`entitycode`);
    const entityCodeInBrackets = entityCode != undefined ? ` (${entityCode})` : "";
    const entityName = button.data(`entityname`);
    const entityId = button.data(`id`);
    const controller = button.data(`controller`);

    const modal = $(this);

    const cancelButton = modal.find(`#cancelButton`)[0];

    const deleteButton = modal.find(`#deleteButton`)[0];

    const deleteFunc = function (event) {
        $.ajax({
            type: `POST`,
            url: `/${controller}/Delete`,
            data: { id: `${entityId}` },
            contextType: `application/json; charset=utf-8`,
            dataType: `json`,
            success: onSuccessResult,
            error: onError
        });
    };

    $(deleteButton).off(`click`).on(`click`, deleteFunc);

    const reloadFunction = () => {
        window.location.href = `/${controller}/index`;
    }

    modal.find(`#modalEntity`).text(entity);

    modal.find(`#modalMessage`).html(`<span class="text-primary">Are you sure you want to delete a ${entity} - ${entityName}${entityCodeInBrackets}?</span>`);

    function onSuccessResult(data) {
        if (data === true) {
            showSuccess();
        } else {
            showError();
        }
    }

    function onError() {
        showError();
    }

    function showError() {
        modal.find(`#modalMessage`).html(`<span class="text-warning"><svg width = "24" height = "24" fill = "#ff6825" viewBox = "0 0 1000 1000"
                                                             xmlns="http://www.w3.org/2000/svg">
                                                            <path d="M 500 0C 224 0 0 224 0 500C 0 776 224 1000 500 1000C 776 1000 1000 776 1000 500C 1000 224 776 0 500 0C 500 0 500 0 500 0M 500 25C 762 25 975 238 975 500C 975 762 762 975 500 975C 238 975 25 762 25 500C 25 238 238 25 500 25C 500 25 500 25 500 25M 525 263C 552 263 570 270 579 284C 588 297 588 312 588 325C 588 352 581 408 575 458C 569 508 562 552 562 552C 562 552 563 550 563 550C 563 550 563 559 558 568C 553 577 542 588 525 588C 525 588 475 588 475 588C 458 588 447 577 442 568C 437 559 438 550 438 550C 438 550 438 552 438 552C 438 552 431 508 425 458C 419 408 413 352 413 325C 413 312 412 297 421 284C 430 270 448 263 475 263C 475 263 525 263 525 263M 442 298C 438 303 438 313 438 325C 438 348 444 405 450 455C 456 505 462 548 462 548C 462 548 463 549 463 549C 463 549 463 550 463 550C 463 550 463 554 464 557C 466 560 467 563 475 563C 475 563 525 563 525 563C 533 563 534 560 536 557C 537 554 538 550 538 550C 538 550 538 549 538 549C 538 549 538 548 538 548C 538 548 544 505 550 455C 556 405 563 348 563 325C 563 313 562 303 558 298C 555 292 548 288 525 288C 525 288 475 288 475 288C 452 288 445 292 442 298C 442 298 442 298 442 298M 562 638C 578 655 588 677 588 700C 587 748 548 788 500 788C 452 788 413 748 413 700C 413 677 422 655 438 638C 455 622 477 612 500 613C 523 612 545 622 562 638C 562 638 562 638 562 638M 456 656C 444 668 438 683 438 700C 438 735 465 763 500 763C 535 763 562 735 563 700C 563 683 556 668 544 656C 532 644 517 637 500 638C 483 637 468 644 456 656C 456 656 456 656 456 656"/>
                                                        </svg> Delete failed. Try again, and if the problem persists see your system administrator.</span>`);
        document.activeElement.blur();
        $(cancelButton).off(`click`).on(`click`, reloadFunction);
    }

    function showSuccess() {
        modal.find(`#modalMessage`).html(`<span class="text-primary">${entity} - ${entityName}${entityCodeInBrackets} has been deleted.</span>`);
        document.activeElement.blur();
        deleteButton.remove();
        cancelButton.textContent = `Clear`;
        $(cancelButton).off(`click`).on(`click`, reloadFunction);
    }

    modal.on(`hidden.bs.modal`, function (event) {
        //clearModal();
    });
})

////////////////////////////////////////////PRINT TO PDF////////////////////////////////
if (document.getElementById(`printPDF`) !== null) {
    $(`#printPDF`).click(function () {
        const button = document.getElementById(`printPDF`);
        const _entity = button.dataset.entity;
        const _entityIdentity = button.dataset.entityIdentity;
        const _controller = button.dataset.controller;
        const _contractType = button.dataset.contractType;;
        const _entityId = button.dataset.entityId;

        $.ajax({
            type: `POST`,
            url: `/${_controller}/GetPDFData`,
            data: {
                id: _entityId,
                contractType: _contractType
            },
            contextType: `application/json; charset=utf-8`,
            success: onSuccessResult,
            error: onError
        });

        function onSuccessResult(data) {

            if (data !== null) {

                const divContents = data;

                const printWindow = window.open(' ', '', `height=800,width=800`);

                printWindow.document.write(`<html><head><title>${_entity} ${_entityIdentity}</title>`);
                printWindow.document.write(`<style>
* {
    box-sizing: border-box;
}

.infoColumnLeft{
    flex: 40%;
    width: 40%;
    padding: 0.2rem;
}

.infoColumnRight {
    flex: 60%;
    width: 60%;
    padding: 0.2rem;
    word-wrap: break-word;
    text-align: justify;
}

.infoRow{
    display:flex;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif !important;
    color:black;
}

.infoContainer {
    margin:1rem;
}

.infoLabel {
    font-size: 1.5rem;
    text-transform:uppercase;
}

.infoData {
    font-size: 1.5rem;
}

.infoWarning {
    color: #ff6825;
}

.notPrint {
    display: none;
}

.infoTitle{
    font-size:2rem;
    text-decoration:underline;
    margin:1.5rem 0;
    text-align:center;
}

.infoSubTitle{
    font-size:1.5rem;
    padding: 0.8rem 0.2rem 0.2rem 0.2rem;
    text-align:center;
    text-transform:uppercase;
    flex: 100%;
    width: 100%;
}
</style>`);
                printWindow.document.write(`</head><body>`);
                printWindow.document.write(divContents);
                printWindow.document.write(`</body></html>`);
                printWindow.document.close();
                printWindow.print();
                setTimeout(function () {
                    printWindow.close();
                }, 1000);
            } else {
                onError();
            }
        }
        function onError() {
            console.log(`Error`);
        }
    });
}

////////////////////////////////////////////SHOW CONTRACT EVENTS////////////////////////////////

if (document.getElementById(`contractCalendar`) !== null) {
    const calendarEl = document.getElementById('contractCalendar')
    const contractId = calendarEl.dataset.id;
    const startEventTerm = calendarEl.dataset.startTerm;
    const endEventTerm = calendarEl.dataset.endTerm;
    const modalErrorMessage = document.getElementById(`errorMessage`);
    const expirationDate = calendarEl.dataset.expirationDate;
    const conclusionDate = calendarEl.dataset.conclusionDate;
    const contractStatus = calendarEl.dataset.contractStatus;

    //Show error modal
    function showErrorModal(message) {
        modalErrorMessage.textContent = message;
        $(`#errorModal`).modal(`show`);
    }

    //get all contract events
    $.ajax({
        type: `POST`,
        data: { id: contractId, contractStatus: contractStatus },
        url: `/Contracts/GetEvents`,
        contextType: `application/json; charset=utf-8`,
        success: onSuccessResultGetEvents,
        error: () => showErrorModal(`Something went wrong. Try again, and if the problem persists see your system administrator.`)
    });
    function onSuccessResultGetEvents(data) {
        if (data) {
            addContractDatesToEvents(data);

            //Details modal
            const detailsTitle = document.getElementById(`detailsEventTitle`);
            const detailsStart = document.getElementById(`detailsEventStart`);
            const detailsEnd = document.getElementById(`detailsEventEnd`);
            const detailsDescription = document.getElementById(`detailsEventDescription`);
            const detailsAllDay = document.getElementById(`detailsAllDay`);
            const detailsEditBtn = document.getElementById(`detailsEditBtn`);
            const detailsDeleteBtn = document.getElementById(`detailsDeleteBtn`);

            //Delete modal
            const deleteDeleteBtn = document.getElementById(`deleteDeleteBtn`);
            const deleteEventTitle = document.getElementById(`deleteEventTitle`);

            //Create-Edit modal
            const modalTitle = document.getElementById(`eventModalTitle`);
            const modalStart = document.getElementById(`startDate`);
            const modalEnd = document.getElementById(`endDate`);
            const modalDescription = document.getElementById(`description`);
            const modalAllDay = document.getElementById(`allDay`);
            const modalStartDateAllDay = document.getElementById(`startDateAllDay`);
            const modalEndDateAllDay = document.getElementById(`endDateAllDay`);
            const modalStartEndContainer = document.getElementById(`startEndContainer`);
            const modalStartDateAllDayContainer = document.getElementById(`startDateAllDayContainer`);
            const modalHeaderEventFunction = document.getElementById(`eventFunction`);
            const eventInputEnd = new Event('input');

            //Get date string
            function getDateAny(date, dateKind) {
                const day = ("0" + date.getDate()).slice(-2).toString();
                const year = date.getFullYear().toString();
                const month = ("0" + (date.getMonth() + 1)).slice(-2).toString();
                const hours = ("0" + date.getHours()).slice(-2).toString();
                const minutes = ("0" + date.getMinutes()).slice(-2).toString();
                switch (dateKind) {
                    case `dateTimeT`: return (`${year}-${month}-${day}T${hours}:${minutes}`); break;
                    case `date`: return (`${year}-${month}-${day}`); break;
                    case `dateDisplay`: return (`${day}-${month}-${year}`); break;
                    case `dateTime`: return (`${year}-${month}-${day} ${hours}:${minutes}`); break;
                    case `dateTimeDisplay`: return (`${day}-${month}-${year} ${hours}:${minutes}`); break;
                    case `dateTimeTZeroTime`: return (`${year}-${month}-${day}T00:00`); break;
                    default: throw new Error(`Invalid dateKind parameter.`); break;
                }
            };

            //Add minutes to date string
            function addMinutes(date, minutes) {
                return new Date(date.getTime() + minutes * 60000);
            }

            //Validation settings
            function validateForm() {
                const validator = $(`#eventForm`).validate({
                    validClass: "is-valid",
                    errorClass: "is-invalid",
                    ignore: [],
                    rules: {
                        startDate: {
                            required: true,
                            max: getDateAny(addMinutes(new Date(expirationDate), 1380), `dateTimeT`)
                        },
                        endDate:
                        {
                            required: true,
                            max: getDateAny(addMinutes(new Date(expirationDate), 1440), `dateTimeT`)
                        },
                        startDateAllDay: {
                            required: true,
                        },
                        endDateAllDay: {
                            required: true,
                        },
                        eventModalTitle: {
                            required: true,
                            minlength: 2,
                            maxlength: 300
                        },
                        description: {
                            required: true,
                            minlength: 2,
                            maxlength: 500
                        }
                    },
                    messages: {
                        eventModalTitle: {
                            required: "Please enter a value for Title.",
                            minlength: "Allowed length: no less than {0} characters.",
                            maxlength: "Allowed length: no more than {0} characters."
                        },
                        description: {
                            required: "Please enter a value for Description.",
                            minlength: "Allowed length: no less than {0} characters.",
                            maxlength: "Allowed length: no more than {0} characters."
                        },
                        startDate: {
                            required: "Please enter a value for Start.",
                            max: `Please enter a value less than or equal to ${getDateAny(addMinutes(new Date(expirationDate), 1380), `dateTime`)}.`
                        },
                        endDate: {
                            required: "Please enter a value for End.",
                            max: `Please enter a value less than or equal to ${getDateAny(addMinutes(new Date(expirationDate), 1440), `dateTime`)}.`
                        },
                        startDateAllDay: {
                            required: "Please enter a value for Start.",
                        },
                        endDateAllDay: {
                            required: "Please enter a value for End.",
                        }
                    }
                });
                return validator;
            }

            //Set min attribute to the end and start dates when start dates change
            modalStart.addEventListener(`input`, () => {
                const newDateObj = modalStart.value ? addMinutes(new Date(modalStart.value), 30)
                    : new Date();
                $.validator.addMethod("endMin", $.validator.methods.min,
                    $.validator.format(`Please enter a value greater than or equal to ${getDateAny(newDateObj, `dateTime`)}.`));
                $.validator.addClassRules("endEvent", { endMin: getDateAny(newDateObj, `dateTimeT`) });
                modalEnd.min = getDateAny(newDateObj, `dateTimeT`);

                $.validator.addMethod("startMin", $.validator.methods.min,
                    $.validator.format(`Please enter a value greater than or equal to ${getDateAny(new Date(), `dateTime`)}.`));
                $.validator.addClassRules("startEvent", { startMin: getDateAny(new Date(), `dateTimeT`) });
                modalStart.min = getDateAny(new Date(), `dateTimeT`);
            });

            modalStartDateAllDay.addEventListener(`input`, () => {
                const newDateObj = modalStartDateAllDay.value ?
                    addMinutes(new Date(modalStartDateAllDay.value), 1440)
                    : new Date();
                modalEndDateAllDay.min = getDateAny(newDateObj, `date`);
            });

            //Switch date input 
            function switchAllDay(event) {
                if (modalAllDay.checked) {
                    modalStartDateAllDayContainer.classList.add(`show`);
                    modalStartEndContainer.classList.remove(`show`);
                    if (event.type === `change`) {
                        modalStartDateAllDay.value = getDateAny(new Date(modalStart.value), `date`);
                        modalEndDateAllDay.value = getDateAny(new Date(modalEnd.value), `date`);
                        modalStartDateAllDay.dispatchEvent(eventInputEnd);
                    }
                    modalStartDateAllDay.disabled = false;
                    modalEndDateAllDay.disabled = false;
                    modalStart.disabled = true;
                    modalEnd.disabled = true;
                } else {
                    modalStartEndContainer.classList.add(`show`);
                    modalStartDateAllDayContainer.classList.remove(`show`);
                    if (event.type === `change`) {
                        modalStart.value = getDateAny(new Date(modalStartDateAllDay.value), `dateTimeTZeroTime`);
                        modalEnd.value = getDateAny(new Date(modalEndDateAllDay.value), `dateTimeTZeroTime`);
                        modalStart.dispatchEvent(eventInputEnd);
                    }
                    modalStart.disabled = false;
                    modalEnd.disabled = false;
                    modalStartDateAllDay.disabled = true;
                    modalEndDateAllDay.disabled = true;;
                }
            }
            modalAllDay.addEventListener(`change`, switchAllDay);

            //Add conclusion and expiration dates of contract
            function addContractDatesToEvents(data) {
                data.push({
                    allDay: true, start: conclusionDate, end: addMinutes(new Date(conclusionDate), 1440),
                    display: 'background', title: `Conclusion date`, description: `Conclusion date`, backgroundColor: `#20c975`,
                    color: `#002d77`
                });
                data.push({
                    allDay: true, start: expirationDate, end: addMinutes(new Date(expirationDate), 1440),
                    display: 'background', title: `Expiration date`, description: `Expiration date`, backgroundColor: `#20c975`,
                    color: `#002d77`
                });
            }

            //Create/edit event function
            function createEditEvent(infoObject, isEdit) {
                if (!isEdit) {
                    if ((new Date(expirationDate)) < new Date(getDateAny(new Date(), `dateTimeTZeroTime`))) {
                        showErrorModal(`This contract has expired; you can\'t add or edit its events!`);
                        return;
                    }

                    if (infoObject.start < new Date(getDateAny(new Date(), `dateTimeTZeroTime`)) ||
                        infoObject.start > addMinutes(new Date(expirationDate), 1380)) {
                        showErrorModal(`Selected dates are not available; select dates from today till ${getDateAny(new Date(expirationDate), `dateDisplay`)}.`);
                        return;
                    }
                }

                modalHeaderEventFunction.textContent = isEdit ? `Edit` : `Create`;
                modalStartDateAllDay.value = getDateAny(infoObject.start, `date`);
                modalEndDateAllDay.value = getDateAny(infoObject.end, `date`);
                modalStart.value = getDateAny(infoObject.start, `dateTimeT`);
                modalEnd.value = getDateAny(infoObject.end, `dateTimeT`);

                if (isEdit) {
                    modalDescription.value = infoObject.description;
                    modalTitle.value = infoObject.title;
                }

                if (infoObject.allDay) {
                    modalAllDay.checked = true;
                } else {
                    modalAllDay.checked = false;
                }

                switchAllDay(new Event(`select`));

                const validator = validateForm();

                $('#modalEventCreate').on('show.bs.modal', function (event) {
                    modalStart.dispatchEvent(eventInputEnd);
                    modalStartDateAllDay.dispatchEvent(eventInputEnd);

                    //functions when submiting edit and create

                    function submitCreate(event) {
                        event.preventDefault();

                        if ($(`#eventForm`).valid()) {
                            const newEventStart =
                                modalAllDay.checked ? modalStartDateAllDay.value
                                    : modalStart.value;
                            const newEventEnd = modalAllDay.checked ? modalEndDateAllDay.value
                                : modalEnd.value;
                            $.ajax({
                                type: `POST`,
                                data: {
                                    contractID: contractId,
                                    title: modalTitle.value,
                                    end: newEventEnd,
                                    start: newEventStart,
                                    description: modalDescription.value,
                                    allDay: modalAllDay.checked
                                },
                                url: `/Contracts/CreateEvent`,
                                contextType: `application/json; charset=utf-8`,
                                success: onSuccessResultCreateEdit,
                                error: (error) => {
                                    $(`#modalEventCreate`).modal(`hide`);
                                    showErrorModal(`Unable to save changes. Try again, and if the problem persists see your system administrator.`);
                                }
                            });
                        }
                    }

                    function submitEdit(event) {
                        event.preventDefault();

                        if ($(`#eventForm`).valid()) {
                            const newEventStart =
                                modalAllDay.checked ? modalStartDateAllDay.value
                                    : modalStart.value;
                            const newEventEnd = modalAllDay.checked ? modalEndDateAllDay.value
                                : modalEnd.value;
                            $.ajax({
                                type: `POST`,
                                data: {
                                    eventID: infoObject.id,
                                    title: modalTitle.value,
                                    end: newEventEnd,
                                    start: newEventStart,
                                    description: modalDescription.value,
                                    allDay: modalAllDay.checked
                                },
                                url: `/Contracts/EditEvent`,
                                contextType: `application/json; charset=utf-8`,
                                success: onSuccessResultCreateEdit,
                                error: (error) => {
                                    $(`#modalEventCreate`).modal(`hide`);
                                    showErrorModal(`Unable to save changes. Try again, and if the problem persists see your system administrator.`);
                                }
                            });
                        }
                    }

                    function onSuccessResultCreateEdit(data) {
                        const newEventStart =
                            modalAllDay.checked ? modalStartDateAllDay.value
                                : modalStart.value;
                        if (data) {
                            $.ajax({
                                type: `POST`,
                                data: { id: contractId, contractStatus: contractStatus },
                                url: `/Contracts/GetEvents`,
                                contextType: `application/json; charset=utf-8`,
                                success: (data) => {
                                    addContractDatesToEvents(data);
                                    calendar.setOption('events', data);
                                    calendar.gotoDate(newEventStart);
                                    $(`#modalEventCreate`).modal(`hide`);
                                },
                                error: (error) => {
                                    $(`#modalEventCreate`).modal(`hide`);
                                    showErrorModal(`Unable to save changes.
                                            Try again, and if the problem persists
                                            see your system administrator.`);
                                }
                            });
                        } else {
                            {
                                $(`#modalEventCreate`).modal(`hide`);
                                showErrorModal(`Unable to save changes.
                                        Try again, and if the problem persists
                                        see your system administrator.`);
                            }
                        }
                    }

                    isEdit ? $(`#eventForm`).off(`submit`).on(`submit`, submitEdit)
                        : $(`#eventForm`).off(`submit`).on(`submit`, submitCreate);
                });
                $(`#modalEventCreate`).modal();
                $('#modalEventCreate').on('hidden.bs.modal', function (event) {
                    modalStartDateAllDay.disabled = false;
                    modalEndDateAllDay.disabled = false;
                    modalStart.disabled = false;
                    modalEnd.disabled = false;
                    validator.resetForm();
                    $(`#eventForm`)[0].reset();
                });
            }

            //Options for titleFormat in the views
            const optionsDay = {
                year: "numeric",
                month: "short",
                day: "numeric"
            };
            const optionsMonth = {
                year: "numeric",
                month: "long",
            };
            const optionsWeek = {
                year: "numeric",
                month: "short",
                day: "numeric"
            };
            const optionsWeekHeader = {
                month: "numeric",
                day: "numeric",
                weekday: "short",
            };

            //Enable bootstrap tooltips
            function enableBsTooltips(el, title) {
                if (title) {
                    $(el).tooltip({
                        placement: `top`,
                        trigger: `hover`,
                        container: `body`,
                        title: title
                    });
                } else {
                    $(el).tooltip({
                        placement: `top`,
                        trigger: `hover`,
                        container: `body`
                    });
                }
            }

            //Hide bootstrap tooltips
            function hideBsTooltips(el) {
                $(el).tooltip(`hide`);
            }

            const calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: `dayGridMonth`,
                firstDay: 1,
                themeSystem: 'bootstrap',
                headerToolbar: {
                    start: contractStatus === `Current` ? 'today prev,next addCustomButton' : 'today prev,next',
                    center: 'title',
                    end: 'dayGridMonth dayGridWeek timeGridDay'
                },
                dayMaxEventRows: 3,
                eventMaxStack: 3,
                closeHint: ``,
                viewDidMount: function (view, el) {
                    if (!is_touch_enabled()) {
                        enableBsTooltips(`[title]`, ``);
                    }
                },
                viewWillUnmount: function (view, el) {
                    if (!is_touch_enabled()) {
                        hideBsTooltips(`[title]`);
                    }
                },
                dayHeaderDidMount: function (view, el) {
                    if (!is_touch_enabled()) {
                        enableBsTooltips(`[title]`, ``);
                    }
                },
                dayHeaderWillUnmount: function () {
                    if (!is_touch_enabled()) {
                        hideBsTooltips(`[title]`);
                    }
                },
                moreLinkDidMount: function () {
                    if (!is_touch_enabled()) {
                        enableBsTooltips(`[title]`, ``);
                    }
                },
                moreLinkWillUnmount: function () {
                    if (!is_touch_enabled()) {
                        hideBsTooltips(`[title]`);
                    }
                },
                validRange: {
                    start: startEventTerm,
                    end: endEventTerm
                },
                views: {
                    timeGridDay: {
                        titleFormat: function (date) {
                            return new Intl.DateTimeFormat("en-GB", optionsDay).format(date.date.marker);
                        },
                        buttonText: `Day`,
                    },
                    dayGridMonth: {
                        titleFormat: function (date) {
                            return new Intl.DateTimeFormat("en-GB", optionsMonth).format(date.date.marker);
                        },
                        eventDidMount: function (info) {
                            if (!is_touch_enabled()) {
                                enableBsTooltips(info.el, info.event.extendedProps.description);
                            }
                        },
                        buttonText: `Month`
                    },
                    dayGridWeek: {
                        titleFormat: function (date) {
                            return `${new Intl.DateTimeFormat("en-GB", optionsWeek).format(date.start.marker)}
                            - ${new Intl.DateTimeFormat("en-GB", optionsWeek).format(date.end.marker)}`;
                        },
                        dayHeaderFormat: function (date) {
                            return new Intl.DateTimeFormat("en-GB", optionsWeekHeader).format(date.date.marker);
                        },
                        buttonText: `Week`,
                    },
                },
                buttonText: {
                    today: 'Current',
                },
                buttonHints: {
                    next: `Next`,
                    today: `Current`,
                    prev: `Previous`
                },
                customButtons: {
                    addCustomButton: {
                        text: `Add`,
                        hint: `Add an event`,
                        click: function () {
                            createEditEvent({
                                start: new Date(), end: addMinutes(new Date(), 1440), allDay: true
                            }, false)
                        }
                    }
                },
                navLinks: `true`,
                navLinkHint:``,
                eventColor: `#002d77`,
                events: data,
                timeZone: `local`,
                eventClick: function (eventClickInfo) {
                    if (eventClickInfo.el.parentElement.parentElement.classList.contains(`fc-popover-body`)) {
                        const closeEl = $(`span.fc-popover-close`);
                        closeEl.trigger(`click`);
                    }
                    detailsDescription.textContent = eventClickInfo.event.extendedProps.description;
                    (eventClickInfo.event.extendedProps.description.length > 150)
                        ? detailsDescription.classList.add(`scrollDiv`)
                        : detailsDescription.classList.remove(`scrollDiv`);

                    detailsTitle.textContent = eventClickInfo.event.title;
                    detailsAllDay.textContent = eventClickInfo.event.allDay ? `Yes` : `No`;

                    const dateFormat = eventClickInfo.event.allDay ? `dateDisplay` : `dateTimeDisplay`;
                    detailsEnd.textContent = getDateAny(new Date(eventClickInfo.event.end), dateFormat);
                    detailsStart.textContent = getDateAny(new Date(eventClickInfo.event.start), dateFormat);

                    if (new Date(eventClickInfo.event.start) < new Date()
                        || contractStatus != "Current") {
                        if (detailsEditBtn) {
                            detailsEditBtn.hidden = true;
                        }
                        if (detailsDeleteBtn) {
                            detailsDeleteBtn.hidden = true;
                        }
                    }

                    $('#modalEventInfo').on('show.bs.modal', function (event) {
                        hideBsTooltips(`[title]`);
                    });

                    $('#modalEventInfo').on('hidden.bs.modal', function (event) {
                        if (detailsEditBtn) {
                            detailsEditBtn.hidden = false;
                        }
                        if (detailsDeleteBtn) {
                            detailsDeleteBtn.hidden = false;
                        }
                    });

                    $(`#modalEventInfo`).modal(`show`);

                    function switchToEditModal() {
                        const eventInfo = {
                            title: eventClickInfo.event.title,
                            description: eventClickInfo.event.extendedProps.description,
                            end: eventClickInfo.event.end,
                            start: eventClickInfo.event.start,
                            id: eventClickInfo.event.id,
                            allDay: eventClickInfo.event.allDay
                        };
                        $(`#modalEventInfo`).modal(`hide`);
                        createEditEvent(eventInfo, true);
                    }

                    function switchToDeleteModal() {
                        const eventStart = eventClickInfo.event.start;
                        $(`#modalEventInfo`).modal(`hide`);

                        deleteEventTitle.textContent = eventClickInfo.event.title;

                        $(`#modalDeleteEvent`).on('show.bs.modal', function (event) {

                            function deleteEvent() {
                                $.ajax({
                                    type: `POST`,
                                    data: {
                                        id: eventClickInfo.event.id,
                                    },
                                    url: `/Contracts/DeleteEvent`,
                                    contextType: `application/json; charset=utf-8`,
                                    success: onSuccessResultDelete,
                                    error: (error) => {
                                        $(`#modalEventDelete`).modal(`hide`);
                                        showErrorModal(`Unable to save changes. Try again, and if the problem persists see your system administrator.`);
                                    }
                                });

                                function onSuccessResultDelete(data) {
                                    if (data) {
                                        $.ajax({
                                            type: `POST`,
                                            data: { id: contractId, contractStatus: contractStatus },
                                            url: `/Contracts/GetEvents`,
                                            contextType: `application/json; charset=utf-8`,
                                            success: (data) => {
                                                addContractDatesToEvents(data);
                                                calendar.setOption('events', data);
                                                calendar.gotoDate(eventStart);
                                                $(`#modalDeleteEvent`).modal(`hide`);
                                            },
                                            error: (error) => {
                                                $(`#modalDeleteEvent`).modal(`hide`);
                                                showErrorModal(`Unable to save changes.
                                            Try again, and if the problem persists
                                            see your system administrator.`);
                                            }
                                        });
                                    } else {
                                        $(`#modalDeleteEvent`).modal(`hide`);
                                        showErrorModal(`Unable to save changes.
                                        Try again, and if the problem persists
                                        see your system administrator.`);
                                    }
                                }
                            }
                            $(deleteDeleteBtn).off(`click`).on(`click`, deleteEvent);
                        });

                        $(`#modalDeleteEvent`).modal(`show`);
                    }

                    if (detailsEditBtn) {
                        $(detailsEditBtn).off(`click`).on(`click`, switchToEditModal);
                    }

                    if (detailsDeleteBtn) {
                        $(detailsDeleteBtn).off(`click`).on(`click`, switchToDeleteModal);
                    }
                },
                select: function (selectionInfo) { createEditEvent(selectionInfo, false); },
                selectable: contractStatus === "Current" ? true : false,
            });
            calendar.render();
        }else {
            showErrorModal(`Something went wrong. Try again, and if the problem persists see your system administrator.`);
        }
    }
}






