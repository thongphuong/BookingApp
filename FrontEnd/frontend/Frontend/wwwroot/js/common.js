/// <reference path="../assets/plugins/toastr/toastr.min.js" />
var TITLE_STATUS_SUCCESS = 1;
var TITLE_STATUS_WARNING = 2;
var TITLE_STATUS_DANGER = 3;
var TITLE_STATUS_INFO = 4;

var DEFAULT_LOCALE = 'vi';


$('.select2').select2({
    minimumResultsForSearch: -1
});

$(document).ready(function () {
    toastr.options = {
        'closeButton': true,
        'debug': false,
        'newestOnTop': false,
        'progressBar': false,
        'positionClass': 'toast-top-right',
        'preventDuplicates': false,
        'showDuration': '1000',
        'hideDuration': '1000',
        'timeOut': '5000',
        'extendedTimeOut': '1000',
        'showEasing': 'swing',
        'hideEasing': 'linear',
        'showMethod': 'fadeIn',
        'hideMethod': 'fadeOut',
    }
});

function showLoading() {
    $('#preloader').css('display', 'block');
}

function hideLoading() {
    setTimeout(function () {
        var keys = Object.keys(localStorage);
        var isOk = true;
        for (var i = 0; i < keys.length; i++) {
            if (keys[i].startsWith('loading'))
                isOk = false;
        }
        if (isOk)
            $('#preloader').css('display', 'none');
    }, 500);
}
var notify = {
    confirm: actionConfirm,
    noti: actionNoti,
}
function formatDatetimepicker(selector, xLocale) {
    var $selector;
    var locale = xLocale === null || xLocale.length === 0 ? DEFAULT_LOCALE : xLocale;
    if (selector instanceof jQuery) {
        $selector = selector;
    } else {
        $selector = $(selector);
    }

    $selector.datetimepicker({
        format: 'L',
        locale: locale
    });
}

function actionAlert(msg, title, status) {
    var $modal = $($('#modal-notify').html());
    $modal.find('#notify-body').html(msg);
    status = "panel-" + getStatusText(status);
    if (title === undefined || title.length > 0) {
        $modal.find('#notify-title').html(title);
        $modal.find('#notify-title').closest('.panel-heading').addClass(status);
    } else {
        $modal.find('#notify-title').closest('.panel-heading').remove();
    }
    $modal.modal({ keyboard: false, show: true });
    $modal.on('hidden.bs.modal', function (e) {
        $modal.remove();
    });
}
function actionConfirm(msg, title, status, fn) {
    var args = Array.prototype.slice.call(arguments);
    var params = [];
    for (let i = 4; i < args.length; ++i) {
        params.push(args[i]);
    }
    var $modal = $($('#modal-confirm').html());
    $modal.find('#notify-body').html(msg);
    status = "panel panel-" + getStatusText(status);
    $modal.find('#notify-title').html(title);
    $modal.find('#notify-title').closest('.panel-heading').addClass(status);
    //$modal.find('#notify-title').closest('.panel-heading').attr(status);
    $modal.modal({ keyboard: false, show: true });
    $modal.on('hidden.bs.modal', function (e) {
        $modal.remove();
    });
    $modal.on('click', 'button#confirm', function () {
        fn.apply(null, params);
        $modal.modal('hide');
    });
}
function actionNoti(msg, title, status) {
    var $modal = $($('#modal-noti').html());
    $modal.find('#notify-body').html(msg);
    status = "panel panel-" + getStatusText(status);
    $modal.find('#notify-title').html(title);
    $modal.find('#notify-title').closest('.panel-heading').addClass(status);
    //$modal.find('#notify-title').closest('.panel-heading').attr(status);
    $modal.modal({ keyboard: false, show: true });
    $modal.on('hidden.bs.modal', function (e) {
        $modal.remove();
    });
}

function getStatusText(status) {
    switch (status) {
        case TITLE_STATUS_SUCCESS:
            return 'success';
        case TITLE_STATUS_WARNING:
            return 'warning';
        case TITLE_STATUS_DANGER:
            return 'danger';
        case TITLE_STATUS_INFO:
            return 'info';
        default:
            return 'default';
    }
}

function initDataTable(selector, url, customRows, length = 10, start = 0, nodata = 'No data available in table', callback) {
    if (customRows === null) customRows = [];
    customRows.unshift({
        "targets": 0,
        //"className": "text-center",
        "width": "1px",
        "sortable": false,
        "data": null,
        render: function (data, type, row, meta) {
            if (row[0].toString().trim() === "") {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
            return row[0];
        }
    });
    return $(selector).DataTable({
        //"scrollY": 300,
        "scrollX": true,
        "responsive": true,
        "searching": false,
        "aaSorting": [],
        "pageLength": length,
        "iDisplayStart": start,
        "columnDefs": customRows,
        "fnServerParams": getTblFilter,
        "bServerSide": true,
        "sAjaxSource": url,
        "bProcessing": true,
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "language": {
            "paginate": {
                "previous": "<i class='material-icons' style='font-size:24px;'>chevron_left</i>",
                "next": "<i class='material-icons' style='font-size:24px;'>chevron_right</i>"
            },
            "emptyTable": nodata,
        },
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "tablename", "value": "dashboard" });
            $.ajax({
                dataType: 'json',
                contentType: 'application/json',
                crossDomain: true,
                type: 'GET',
                headers: {
                    "x-api-key": "3EB76D87D97C427943957C555AB0B60847582D38CB1688ED86C59251206305E3",
                    "Authorization": getSessionToken()
                },
                url: sSource,
                data: aoData,
                success: function (result) {
                    fnCallback(result);
                },
                error: function (xhr, textStatus, error) {
                }
            }
            );
        },
        "drawCallback": function (oSettings) {
            scrollbar(oSettings.nScrollBody);
            $(oSettings.nScrollBody).css({ "white-space": "nowrap", "overflow": "hidden", "overflow-x": "auto" });
            if (callback && {}.toString.call(callback) === '[object Function]') {
                callback();
            }
        }
    });

}

function scrollbar(elm) {
    var mousePosition;
    var offset = [0, 0];
    var isDown = false;

    var div = elm;

    div.addEventListener('mousedown', function (e) {
        isDown = true;
        offset = [
            e.clientX,
            e.clientY
        ];
    }, true);

    div.addEventListener('mouseup', function () {
        isDown = false;
    }, true);

    div.addEventListener('mousemove', function (event) {
        if (event.target.tagName.toUpperCase() !== "INPUT") {
            event.preventDefault();
            if (isDown) {
                mousePosition = {
                    x: event.clientX,
                    y: event.clientY
                };
                var _sc = mousePosition.x - offset[0];
                if (_sc < 0) {
                    if (Math.abs(_sc) > 5) {
                        $(div).scrollLeft($(div).scrollLeft() + 20);
                    }
                }
                else {
                    if (Math.abs(_sc) > 5) {
                        $(div).scrollLeft($(div).scrollLeft() - 20);
                    }
                }
                offset = [
                    event.clientX,
                    event.clientY
                ];
            }
        }
    }, true);
}

function getTblFilter(oData) {
    var $filter = $('#frmFilter').find('.frmFilter');
    $.each($filter, function (i, o) {
        oData.push({ "name": $(o).attr('name'), "value": $(o).val() });
    });
    //console.log(oData);
}

function resetForm(ele, table) {
    var eles = $('.fastselect', ele);
    $('label#name-error').remove();
    $(".select2").val(null).trigger('change');
    $('select.frmFilter').val(null).trigger('change');
    $('select.frmFilter').not('.select2').empty();

    //console.log(eles);
    eles.each(function (i) {
        ///debugger;
        let e = $(this);
        let f = $(this).prop("multiple");
        if (f)
            e.prop('selectedIndex', -1);
        else
            e.prop('selectedIndex', 0);
        e.data('fastselect').destroy();
        if (e.hasClass('subselect')) {
            e.html("");
        }
        e.fastselect();
    });
    setTimeout(function () { $("#" + table).DataTable().draw(); }, 200);
}

function showValidate(id) {
    $("#" + id).closest('.form-line').addClass("error");
    $("#" + id).closest('.form-group').append(required_mgs);
}

function readURLC(input) {
    var fsize = 1024 * 1024 * 2;

    var validImageTypes = [
        "jpeg",
        "png",
        "jpg",
    ];

    if (input.files[0].size < fsize) {
        var imageFile = input.files[0];
        var fileType = imageFile.name.substr(imageFile.name.lastIndexOf(".") + 1);
        if ($.inArray(fileType.toLowerCase(), validImageTypes) < 0) {
            notify.alert("File not correct format (Allowed file : JPG, JPEG, PNG)", 'Warning', TITLE_STATUS_DANGER);
        }
        else {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(input).parent().children('img').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
                $(input).parent().children("i").removeClass("hidden");
            }
        }
    }
    else {
        notify.alert("Allowed file size exceeded. (Max. 2 MB)", 'Warning', TITLE_STATUS_DANGER);
    }
}

function showRemoveImage(cl) {
    $("." + cl).each(function (i, v) {
        if (($(v).attr("img-old") !== "/Content/img/no-image.jpg" && $(v).attr("img-old") !== "/Content/img/event-hightlight.png" && $(v).attr("img-old") !== "/Content/img/event-normal.png") || v.files.length > 0) {
            $(v).parent().children("i").removeClass("hidden");
        }
    });
}

function validateImage(cl) {
    var check = true;
    $("." + cl).each(function (i, v) {
        if ($(v).attr("img-old") !== "/Content/img/no-image.jpg" || v.files.length > 0) {
            check = false;
        }
    });

    if (check) {
        var required_mgs = '<label id="name-error" class="error">This field is required.</label>';
        $("." + cl).closest('.form-line').addClass("error");
        $("." + cl).closest('.form-group').append(required_mgs);
    }

    return check;
}

function removeImage(elm) {
    $(elm).addClass("hidden");
    if ($(elm).data("image") !== undefined && $(elm).data("image") !== "") {
        $(elm).parent().children("img").attr('src', $(elm).data("image"));
        $(elm).parent().children("input").attr('img-old', $(elm).data("image"));
    }
    else {
        $(elm).parent().children("img").attr('src', '/Content/img/no-image.jpg');
        $(elm).parent().children("input").attr('img-old', '/Content/img/no-image.jpg');
    }

    var idfile = $(elm).data("file");
    $("#" + idfile).val("");
}

function checkimagesize(img, corectRate, diff) {
    var result = false;
    $.each(img, function (i, v) {
        if (v.files.length > 0) {
            var img = new Image;
            img.src = $("#" + $(v).data('imageid')).attr("src");
            var rate = img.width / img.height;
            var _diff = Math.abs(rate - corectRate);
            if (_diff > diff) {
                result = true;
                return false;
            }
        }
    });
    return result;
}

function showvalidate(id, msg) {
    var _msg = `<label id="name-error" class="error">${msg}</label>`;
    $("#" + id).closest('.form-line').addClass("error");
    $("#" + id).closest('.form-group').append(_msg);
}

function debounce(func, wait) {
    var timeout;

    return function () {
        var context = this,
            args = arguments;

        var executeFunction = function () {
            func.apply(context, args);
        };

        clearTimeout(timeout);
        timeout = setTimeout(executeFunction, wait);
    };
};

function ValidateDateTimeFormat(value) {
    let isValidDate = Date.parse(value);
    if (isNaN(isValidDate)) {
        return false;
    }
    else {
        return true;
    }
};

$('input[type=number][max]:not([max=""])').on('input', function (ev) {
    var $this = $(this);
    var maxlength = $this.attr('max').length;
    var value = $this.val();
    if (value && value.length >= maxlength) {
        $this.val(value.substr(0, maxlength));
    }
});

$('input[type=number][max]:not([max=""])').on('keypress', function (ev) {
    return ev.charCode >= 48 && ev.charCode <= 57
});

function parseDate(str) {
    var mdy = str.split('-');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}

function datediff(first, second) {
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}

function GetUrlParam(param) {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get(param);

}

function UUID() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

function addUrlParameter(name, value) {
    const url = new URL(window.location.href);
    url.searchParams.set(name, value);
    window.history.replaceState(null, null, url); // or pushState
}

function validatePhone(selector, value) {
    var phoneno9x = /[0-9]{10}/;

    if (value.match(phoneno9x)) {
        return true;
    }
    else {
        var msg = `<label id="name-error" class="error">${localizationResources.lbl_phone_invalid}</label>`;
        if ($(selector).closest('.form-line').length > 0) {
            $(selector).closest('.form-line').addClass("error");
            $(selector).closest('.form-group').append(msg);
        }
        else {
            $(selector).parent().addClass("error");
            $(selector).parent().append(msg);
        }
        return false;
    }
}

function validateEmail(selector, value) {
    var re = /\S+@\S/;

    if (re.test(value))
        return true;
    else {
        var msg = `<label id="name-error" class="error">${localizationResources.lbl_email_invalid}</label>`;
        if ($(selector).closest('.form-line').length > 0) {
            $(selector).closest('.form-line').addClass("error");
            $(selector).closest('.form-group').append(msg);
        }
        else {
            $(selector).parent().addClass("error");
            $(selector).parent().append(msg);
        }
        return false;
    }
}

$("input[type='number']").on("keypress", function (e) {
    //var length = $(this).attr("max");
    //if (length > 0 && this.value.length > length)
    //    return false;
    return this.value = !!this.value && Math.abs(this.value) >= 0 ? Math.abs(this.value) : null;
})

function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop", "focusout"].forEach(function (event) {
        textbox.addEventListener(event, function (e) {
            if (inputFilter(this.value)) {
                // Accepted value
                if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                    this.setCustomValidity("");
                }
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                // Rejected value - restore the previous one
                this.reportValidity();
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                // Rejected value - nothing to restore
                this.value = "";
            }
        });
    });
}