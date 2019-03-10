function loadListFriend() {
    var page = $('#page').val();
    page++;
    $('#page').val(page);
    $.ajax({
        url: location.origin + '/Friend/GetFriend',
        type: 'POST',
        data: {
            page: page
        }
    }).done(function (result) {
        result.forEach(function (entry) {
            var avatar = entry.Avatar;
            avatar = avatar == null ? 'sample_avatar.png' : avatar;
            var status;
            if (entry.RelationshipStatus === 1) {
                status =
                    '<button type="button" class="btn btn-light dropdown-toggle" data-toggle="dropdown">' +
                    '<i class="fas fa-user"></i> Bạn bè' +
                    '</button>' +
                    '<div class="dropdown-menu">' +
                    '<a class="dropdown-item" href="/Friend/Unfriend">' +
                    '<i class="fas fa-user-minus"></i> Hủy kết bạn' +
                    '</a>' +
                    '</div>'
            } else {
                status =
                    '<button type="button" class="btn btn-light dropdown-toggle" data-toggle="dropdown">' +
                    '<i class="fas fa-user"></i> Đang yêu cầu' +
                    '</button>' +
                    '<div class="dropdown-menu">' +
                    '<a class="dropdown-item" href="/Friend/Unfriend/' + entry.Id + '">' +
                    '<i class="fas fa-user-minus"></i> Hủy yêu cầu' +
                    '</a>' +
                    '</div>'
            }
            var html = '<div class="col-12 col-sm-12 col-md-6 col-lg-4">' +
                '<div class="friend-container row">' +
                '<div class="col-3 col-md-3 col-sm-3 col-xs-3">' +
                '<img src="/UploadedFiles/Avatar/' + avatar + '" alt="">' +
                '</div >' +
                '<div class="col-9 col-md-9 col-sm-9 col-xs-9">' +
                '<div class="friend-content">' +
                '<h5>' + entry.Fullname + '</h5>' +
                '<div class="btn-group btn-group-sm">' +
                '<a class="btn btn-light start-chat" href="/WebChat/StartChat/' + entry.Id + '"><i class="fas fa-comments"></i></a>' +
                '<div class="btn-group">' +
                status +
                '</div></div></div></div></div></div>';
            $('#friend').append(html);
        });
    });
}

loadListFriend();
startChat();

