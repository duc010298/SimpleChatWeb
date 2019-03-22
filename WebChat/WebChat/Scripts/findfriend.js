$('#btn-search').on('click', function () {
    var input = $('#input-search').val();
    $.ajax({
        url: location.origin + '/FindFriend/FindFriend',
        type: 'POST',
        data: {
            input: input
        }
    }).done(function (result) {
        $('#list-friend-container').html('');
        result.forEach(function (entry) {
            var avatar = entry.Avatar;
            if (avatar === null) {
                avatar = 'sample_avatar.png';
            }
            var status;
            if (entry.StatusRelation == 3) {
                status = 'Kết bạn';
            } else if (entry.StatusRelation == 0) {
                status = 'Đã chặn';
            } else if (entry.StatusRelation == 1) {
                status = 'Bạn bè';
            } else if (entry.StatusRelation == 2) {
                status = 'Đang gửi yêu cầu';
            }
            var html = '<div class="friend-container">' +
                '<input type="text" class="Birth" value="' + entry.Birth + '" hidden />' +
                '<input type="text" class="City" value="' + entry.City + '" hidden />' +
                '<input type="text" class="Description" value="' + entry.Description + '" hidden />' +
                '<input type="text" class="Gender" value="' + entry.Gender + '" hidden />' +
                '<input type="text" class="Id" value="' + entry.Id + '" hidden />' +
                '<input type="text" class="Email" value="' + entry.Email + '" hidden />' +
                '<input type="text" class="Phone" value="' + entry.Phone + '" hidden />' +
                '<img src="/UploadedFiles/Avatar/' + avatar + '" alt="">' +
                '<h4>' + entry.Name + '</h4>' +
                '<button type="button" class="btn btn-sm btn-primary action">' + status + '</button>' +
                '</div><hr>';
            $('#list-friend-container').append(html);
        });
        $('#input-search').val('');
        eventClick();
        eventAction();
    });
});

$('#input-search').on('keypress', function (e) {
    if (e.which == 13) {
        $('#btn-search').click();
    }
});

function eventClick() {
    $('.friend-container').off('click');
    $('.friend-container').on('click', function () {
        var avatar = $(this).children('img').attr('src');
        $('#right-content-avatar').attr('src', avatar);
        var name = $(this).children('h4').html();
        $('#right-content-name').html(name);
        $('#right-content-name-2').html(name);
        var birth = $(this).children('.Birth').val();
        $('#right-content-birth').html(birth);
        var gender = $(this).children('.Gender').val();
        $('#right-content-gender').html(gender);
        var city = $(this).children('.City').val();
        if (city == 'null') city = 'Không có';
        $('#right-content-city').html(city);
        var description = $(this).children('.Description').val();
        if (description == 'null') description = 'Không có';
        $('#right-content-des').html(description);
        var email = $(this).children('.Email').val();
        $('#right-content-email').html(email);
        var phone = $(this).children('.Phone').val();
        if (phone == 'null') phone = 'Không có';
        $('#right-content-phone').html(phone);
        $('#right-content').show();
    });
}


function eventAction() {
    $('.action').off('click');
    $('.action').on('click', function () {
        $(this).parent('.friend-container').children('.Id').val();
    });
}