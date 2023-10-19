$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getSessionToken());
            xhr.setRequestHeader('Accept-Language', 'vi-VN');
            xhr.setRequestHeader('x-api-key', API_KEY);
            showLoading();
            $('button').prop('disabled', true);
            $('input:not([type="search"])').prop('disabled', true);

        },
        complete: function (xhr, status, error) {
            if (xhr.status == 0)
                toastr.error(localizationResources.errorServer, localizationResources.lbl_error);
            else if (xhr.status == 401)
                toastr.warning(localizationResources.error401, localizationResources.lbl_warning);
            else if (xhr.status == 400)
                toastr.error(localizationResources.error400, localizationResources.lbl_error);
            $('button').prop('disabled', false);
            $('input:not([type="search"])').prop('disabled', false);

        },
        timeout: 5000000
    });

    $(document).ajaxStop(function () {
        hideLoading();
    });
})
function getSessionToken() {
    if (localStorage['SessionToken'] != undefined)
        return 'Bearer ' + localStorage['SessionToken'];
    return null;
}

function GetUrl(host, controller, action, method, params, callbackSuccess, callbackError) {
    $.ajax({
        type: method,
        url: host + controller + action + (params == null ? "" : "?" + new URLSearchParams(params).toString()),
        contentType: false,
        processData: false,
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            if (window[callbackError] != undefined)
                window[callbackError](request, status, error);
        }
    });
}

function GetUrl1(host, controller, action, method, params, callbackSuccess, callbackError) {
    var rs;
    $.ajax({
        type: method,
        url: host + controller + action + (params == null ? "" : "?" + new URLSearchParams(params).toString()),
        contentType: false,
        processData: false,
        async: false,
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                rs = window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            if (window[callbackError] != undefined)
                rs = window[callbackError](request, status, error);
        }
    });
    return rs;
};

function PostBody(host, controller, action, method, body, callbackSuccess, callbackError) {
    $.ajax({
        type: method,
        url: host + controller + action,
        contentType: "application/json; charset=utf-8",
        data: (method == 'GET' ? data : JSON.stringify(body)),
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            console.log(error);
            if (window[callbackError] != undefined)
                window[callbackError](request, status, error);
        }
    });
}

function PostFormData(host, controller, action, method, formdata, callbackSuccess, callbackError) {
    $.ajax({
        type: method,
        url: host + controller + action,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            if (window[callbackError] != undefined)
                window[callbackError](request, status, error);
        }
    });
}

function msgError(respon, status, error) {
    if (respon.status == 404) //Not found
        toastr.error(localizationResources[respon.responseText], localizationResources.lbl_error);
    if (respon.status == 409) //Conflict
        toastr.error(localizationResources[respon.responseText], localizationResources.lbl_error);
    if (respon.status == 403) // Forbiden
        toastr.error(localizationResources[respon.responseText], localizationResources.lbl_error);
}

function callApi_multipleselect(selector, placeholder, host, controller, action, multiple, param) {
    $(selector).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        multiple: multiple,
        closeOnSelect: true,
        allowClear: true,
        ajax: {
            delay: 250,
            headers: {
                "Authorization": "Bearer " + localStorage['SessionToken'],
                "x-api-key": API_KEY,
            },
            url: function () {
                return CombineUrl(host, controller, action, param);
            },
            dataType: 'json',
            data: function (params) {
                var query = {
                    q: params.term,
                    type: 'public'
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        var obj = {
                            id: item.key,
                            text: item.value,
                        }
                        $.each(item.list_attr, function (index, value) {
                            obj[value.key] = value.value
                        });
                        return obj;
                    })
                };
            },
            cache: false
        }
    }});
}

function callApi_multipleselectPaging(selector, placeholder, host, controller, action, multiple, param) {
    $(selector).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        multiple: multiple,
        closeOnSelect: true,
        allowClear: true,
        ajax: {
            delay: 250,
            headers: {
                "Authorization": "Bearer " + localStorage['SessionToken'],
                "x-api-key": API_KEY,
            },
            url: function () {
                return CombineUrl(host, controller, action, param);
            },
            dataType: 'json',
            data: function (params) {
                var query = {
                    q: params.term,
                    type: 'public',
                    page: params.page || 1
                }
                return query;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (item) {
                        var obj = {
                            id: item.key,
                            text: item.value,
                        }
                        $.each(item.list_attr, function (index, value) {
                            obj[value.key] = value.value
                        });
                        return obj;
                    }),
                    pagination: {
                        more: (params.page * 100) < 1000
                    }
                };
            },
            cache: false
        }
    });
}


function CombineUrl(host, controler, action, param) {
    var obj = "?";
    //if (param != undefined && param != null)
    for (var key in param) {
        if (param.hasOwnProperty(key)) {
            obj += key + "=" + $(param[key]).val();
        }
    }
    return host + controler + action + obj;

}