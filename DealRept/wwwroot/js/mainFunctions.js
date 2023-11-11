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

///////////////////////////////////////CARDS//////////////////////////////////////////////////////////////////
//Card Hover effect
$(document).ready(function () {
    // executes when HTML-Document is loaded and DOM is ready
    //$(".shadowBox").hover(
    //    function () {
    //        $(this).addClass('shadow-lg');
    //    }, function () {
    //        $(this).removeClass('shadow-lg');
    //    }
    //);
});

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

//development

//$.validator.methods.number = function (value, element) {
//    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)?(?:,\d+)?$/.test(value);
//}

//validate file size

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

////////////////////////////////////////DELETE RESTRICT TOOLTIPS////////////////////////////////
//$(".deleteRestrictTooltip").tooltip({
//    placement: "top",
//   // trigger: $(window).width() > 667 ? "hover" : "focus"
//});
////////////////////////////////////////////CITY & BANK DELETE MODAL////////////////////////////////
$('#modalDeleteEntity').on('show.bs.modal', function (event) {

    let button = $(event.relatedTarget); // Button that triggered the modal
    let entity = button.data(`entity`);
    let entityCode = button.data(`entitycode`);
    let entityCodeInBrackets = entityCode != undefined ? ` (${entityCode})` : "";
    let entityName = button.data(`entityname`);
    let entityId = button.data(`id`);
    let controller = button.data(`controller`);

    const modal = $(this);

    let cancelButton = modal.find(`#cancelButton`)[0];

    let deleteButton = modal.find(`#deleteButton`)[0];

    let deleteFunc = function (event) {
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

    deleteButton.addEventListener(`click`, deleteFunc);

    let reloadFunction = () => {
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
        cancelButton.addEventListener(`click`, reloadFunction);
    }

    function showSuccess() {
        modal.find(`#modalMessage`).html(`<span class="text-primary">${entity} - ${entityName}${entityCodeInBrackets} has been deleted.</span>`);
        document.activeElement.blur();
        deleteButton.remove();
        cancelButton.textContent = `Clear`;
        cancelButton.addEventListener(`click`, reloadFunction);
    }

    modal.on(`hidden.bs.modal`, function (event) {
        clearModal();
    });

    const clearModal = () => {
        document.activeElement.blur();
        deleteButton.removeEventListener(`click`, deleteFunc);
        cancelButton.removeEventListener(`click`, reloadFunction);
        button = undefined;
        entity = undefined;
        entityCode = undefined;
        entityCodeInBrackets = undefined;
        entityName = undefined;
        entityId = undefined;
        controller = undefined;
        deleteButton = undefined;
        cancelButton = undefined;
        deleteFunc = undefined;
        reloadFunction = undefined;
    }
})




