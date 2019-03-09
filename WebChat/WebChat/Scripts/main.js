// All function and code javascript and jquery here
function scrollChatToBottom(isSlow) {
    $('#elementToScrollTo').remove();
    $('#current-chat').append('<span id="elementToScrollTo"></span>');
    if (isSlow) {
        $("#current-chat").animate({ scrollTop: $("#elementToScrollTo").position().top }, 'slow')
        return;
    }
    $('#current-chat').css("opacity", "0");
    $.when($("#current-chat").animate({ scrollTop: $("#elementToScrollTo").position().top }, 1)).done(function () {
        $('#current-chat').css("opacity", "");
    });
}

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

function insert_send_message(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    var html =
        '<div class="d-flex justify-content-end">' +
        '<div class="msg_cotainer">' +
        Content +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '<div class="img_cont_msg">' +
        '<img src="UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").append(html);
}

function insert_receive_message(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="UploadedFiles/Avatar/' + Avatar + '">' +
        '</div>' +
        '<div class="msg_cotainer">' +
        Content +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '</div>';
    $("#current-chat").append(html);
}

function insert_send_message_top(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    var html =
        '<div class="d-flex justify-content-end">' +
        '<div class="msg_cotainer">' +
        Content +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '<div class="img_cont_msg">' +
        '<img src="UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").prepend(html);
}

function insert_receive_message_top(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="UploadedFiles/Avatar/' + Avatar + '">' +
        '</div>' +
        '<div class="msg_cotainer">' +
        Content +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '</div>';
    $("#current-chat").prepend(html);
}

function loadChatContent() {
    $(".current-friend-container").on('click', function () {
        $('#chat-page').val('1');
        var page = $('#chat-page').val();
        $('#current-chat').html("");
        $(".current-friend-container").removeClass('active');
        $(this).addClass('active');
        var friendId = $(this).children('.friendId').val();
        $('#current-friend-id').val(friendId);
        $('#spinner-container').show();
        $.ajax({
            url: 'WebChat/GetChatContent',
            type: 'POST',
            data: {
                guid: friendId,
                page: page
            }
        }).done(function (result) {
            $('#spinner-container').hide();
            var avatarFriend = result.AvatarFriend;
            if (avatarFriend !== null) {
                $('#chat-title-avatar').attr('src', avatarFriend);
            }
            var avatarCurrent = result.AvatarCurrent;
            if (avatarCurrent === null) {
                avatarCurrent = 'sample_avatar.png';
            }
            if (result.Status_online) {
                $('#chat-title-isOnline').show();
                $('#chat-title-last-online').html("Online");
            } else {
                $('#chat-title-isOnline').hide();
                $('#chat-title-last-online').html(datetimeoffsetToDisplay(result.Last_online));
            }
            $('#chat-title-name').html(result.Fullname);
            var messages = result.Messages;
            messages.forEach(function (entry) {
                if (entry.IsSend) {
                    insert_send_message(avatarCurrent, entry.Content, entry.Message_status, entry.Send_time);
                } else {
                    insert_receive_message(avatarFriend, entry.Content, entry.Message_status, entry.Send_time);
                }
            });
            scrollChatToBottom(false);
        });
    });
}

function loadCurrentFriend() {
    $.ajax({
        url: 'WebChat/GetCurrentFriend',
        type: 'POST',
    }).done(function (result) {
        result.forEach(function (entry) {
            var avatar = 'sample_avatar.png';
            if (entry.Avatar != null) {
                avatar = entry.Avatar;
            }
            var html = '<div class="current-friend-container">' +
                '<input class="friendId" value="' + entry.FriendId + '" hidden>' +
                '<div class="curent-friend-image">' +
                '<img src="UploadedFiles/Avatar/' + avatar + '" alt="">';
            if (entry.Status_online) {
                html += '<span class="curent-friend-image-icon online"></span>';
            }
            html += '</div>' +
                '<div class="curent-friend-content">' +
                '<div class="current-friend-name">' + entry.FriendName + '</div>' +
                '<div class="current-friend-last-message">' + entry.LastMessage + '</div>' +
                '<div class="current-friend-last-message-time">' + entry.LastSendTime + '</div>' +
                '</div></div>';
            $('#currentf-friend').append(html);
        });
        $(".current-friend-last-message-time").each(function () {
            var datetimeoffset = $(this).html();
            $(this).html(datetimeoffsetToDisplay(datetimeoffset));
        });
        loadChatContent();
        $(".current-friend-container").first().click();
    });
}

loadCurrentFriend();

$('#current-chat').scroll(function () {
    var pos = $('#current-chat').scrollTop();
    if (pos == 0) {
        $('#scroll-to-current-pos').remove();
        $('<span id="scroll-to-current-pos"></span>').insertBefore('.d-flex:first');
        var friendId = $('#current-friend-id').val();
        var page = $('#chat-page').val();
        page++;
        $('#chat-page').val(page);
        $.ajax({
            url: 'WebChat/GetChatContent',
            type: 'POST',
            data: {
                guid: friendId,
                page: page
            }
        }).done(function (result) {
            $('#spinner-container').hide();
            var avatarFriend = result.AvatarFriend;
            if (avatarFriend !== null) {
                $('#chat-title-avatar').attr('src', avatarFriend);
            }
            var avatarCurrent = result.AvatarCurrent;
            if (avatarCurrent === null) {
                avatarCurrent = 'sample_avatar.png';
            }
            if (result.Status_online) {
                $('#chat-title-isOnline').show();
                $('#chat-title-last-online').html("Online");
            } else {
                $('#chat-title-isOnline').hide();
                $('#chat-title-last-online').html(datetimeoffsetToDisplay(result.Last_online));
            }
            $('#chat-title-name').html(result.Fullname);
            /* --------------------------- */
            var messages = result.Messages;
            messages.forEach(function (entry) {
                if (entry.IsSend) {
                    insert_send_message_top(avatarCurrent, entry.Content, entry.Message_status, entry.Send_time);
                } else {
                    insert_receive_message_top(avatarFriend, entry.Content, entry.Message_status, entry.Send_time);
                }
                $("#current-chat").animate({ scrollTop: $("#scroll-to-current-pos").position().top }, 0);
            });
        });
    }
});