$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function datetimeoffsetToDisplay(datetimeoffset) {
    var start = moment(datetimeoffset);
    var end = moment();
    var duration = moment.duration(end.diff(start));
    var result;
    var time = duration.asSeconds();
    if (time <= 60) {
        result = Math.round(time) + ' giây';
    } else {
        time = duration.asMinutes();
        if (time <= 60) {
            result = Math.round(time) + ' phút';
        } else {
            time = duration.asHours();
            if (time <= 24) {
                result = Math.round(time) + ' giờ';
            } else {
                time = duration.asDays();
                if (time <= 7) {
                    result = Math.round(time) + ' ngày';
                } else {
                    result = start.format("DD-MM-YYYY");
                }
            }
        }
    }
    return result;
}

function notify(title, message) {
    $("#notifyModal .modal-title").html(title);
    $("#notifyModal .modal-body").html(message);
    $("#notifyModal").modal({ backdrop: "static" });
}

function covertDateTime() {
    var datetimeoffset = $('#password').html();
    var time = datetimeoffsetToDisplay(datetimeoffset);
    $('#password').html('Thay đổi lần cuối ' + time);
}

covertDateTime();

$('#uploadAvatar').on('click', function () {
    $('#avatar').trigger('click');
});

$('#avatar').on('change', function () {
    var file_data = $('#avatar').prop('files')[0];
    if (file_data.size > 10000000) {
        alert("File quá lớn");
        return;
    }
    var form_data = new FormData();
    form_data.append('file', file_data);
    $.ajax({
        url: location.origin + '/User/ChangeAvatar',
        type: 'POST',
        dataType: 'text',
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        error: function () {
            notify('Lỗi', 'Không thể xử lí dữ liệu');
        }
    }).done(function (result) {
        notify('Thông báo', result);
        $('.btn-danger').on('click', function () {
            location.reload();
        });
    });
});

function eventAcceptSave() {
    $('#BtnAcceptSave').on('click', function () {
        var whatChange = $('#whatChange').val();
        switch (whatChange) {
            case username:
                var newUsername = $('#attr-input').val();
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeUsername',
                    data: {
                        newUsername: newUsername
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
            case fullname:
                var newFullname = $('#attr-input').val();
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeFullname',
                    data: {
                        newFullname: newFullname
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
            case birth:
                var newBirth = $('#attr-input').val();
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeBirth',
                    data: {
                        newBirth: newBirth
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
            case gender:
                var newGender = $('#attr-input').val();
                if (newGender === "Nam") {
                    newGender = 1;
                } else {
                    newGender = 0;
                }
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeGender',
                    data: {
                        gender: gender
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
            case city:
                var newCity = $('#attr-input').val();
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeCity',
                    data: {
                        newCity: newCity
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
            case description:
                var newDes = $('#attr-input').val();
                $.ajax({
                    type: 'POST',
                    url: location.origin + '/User/ChangeDescription',
                    data: {
                        newDes: newDes
                    },
                    error: function () {
                        notify('Lỗi', 'Không thể xử lí dữ liệu');
                    }
                }).done(function (result) {
                    notify('Thông báo', result);
                    $('.btn-danger').on('click', function () {
                        location.reload();
                    });
                });
                return;
        }
    });
}

$('#changeUsername').on('click', function () {
    $('#spec-input').hide();
    $('#normal-input').show();
    $('#whatChange').val('username');
    $('#modal-title').html('Thay đổi tên đăng nhập');
    $('#attr').html('Tên đăng nhập');
    $('#attr-input').attr('type', 'text');
    $('#attr-input').attr('placeholder', 'Nhập tên đăng nhập mới');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changeFullName').on('click', function () {
    $('#spec-input').hide();
    $('#normal-input').show();
    $('#whatChange').val('fullname');
    $('#modal-title').html('Thay đổi họ và tên');
    $('#attr').html('Họ và tên:');
    $('#attr-input').attr('type', 'text');
    $('#attr-input').attr('placeholder', 'Nhập họ và tên mới');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changeDateOfBirth').on('click', function () {
    $('#spec-input').hide();
    $('#normal-input').show();
    $('#whatChange').val('birth');
    $('#modal-title').html('Thay đổi ngày sinh');
    $('#attr').html('Ngày sinh:');
    $('#attr-input').attr('type', 'text');
    $('#attr-input').attr('placeholder', 'dd/MM/yyyy');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changeGender').on('click', function () {
    $('#normal-input').hide();
    $('#spec-input').show();
    $('#whatChange').val('gender');
    $('#modal-title').html('Thay đổi giới tính');
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changeCity').on('click', function () {
    $('#spec-input').hide();
    $('#normal-input').show();
    $('#whatChange').val('city');
    $('#modal-title').html('Thay đổi thành phố');
    $('#attr').html('Thành phố:');
    $('#attr-input').attr('type', 'text');
    $('#attr-input').attr('placeholder', 'Nhập thành phố mới');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changeDescription').on('click', function () {
    //TODO change to use Textarea here
    $('#spec-input').hide();
    $('#normal-input').show();
    $('#whatChange').val('description');
    $('#modal-title').html('Thay đổi mô tả');
    $('#attr').html('Mô tả');
    $('#attr-input').attr('type', 'text');
    $('#attr-input').attr('placeholder', 'Nhập mô tả ngắn');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    eventAcceptSave();
});

$('#changePassword').on('click', function () {
    $('#spec-input').hide();
    $('#normal-input').show();
    var oldPassword;
    var newPassword;
    $('#whatChange').val('password');
    $('#modal-title').html('Thay đổi mật khẩu');
    $('#attr').html('Mật khẩu cũ');
    $('#attr-input').attr('placeholder', 'Nhập mật khẩu cũ');
    $('#attr-input').attr('type', 'password');
    $('#attr-input').val("");
    $("#acceptSaveNotify").modal({ backdrop: "static" });
    $('#BtnAcceptSave').off('click');
    $('#BtnAcceptSave').on('click', function () {
        oldPassword = $('#attr-input').val();
        $('#attr-input').val("");
        $('#modal-title').html('Thay đổi mật khẩu');
        $('#attr').html('Mật khẩu mới');
        $('#attr-input').attr('placeholder', 'Nhập mật khẩu mới');
        setTimeout(function () {
            $("#acceptSaveNotify").modal({ backdrop: "static" });
        }, 500);
        $('#BtnAcceptSave').off('click');
        $('#BtnAcceptSave').on('click', function () {
            newPassword = $('#attr-input').val();
            $.ajax({
                type: 'POST',
                url: location.origin + '/User/ChangePassword',
                data: {
                    oldPassword: oldPassword,
                    newPassword: newPassword
                },
                error: function () {
                    notify('Lỗi', 'Không thể xử lí dữ liệu');
                }
            }).done(function (result) {
                notify('Thông báo', result);
                $('.btn-danger').on('click', function () {
                    location.reload();
                });
            });
        });
    });
});