﻿// All function and code javascript and jquery here
function scrollChatToBottom(isSlow) {
    if (isSlow) {
        $("#current-chat").animate({ scrollTop: $('#current-chat')[0].scrollHeight }, 'slow')
        return;
    }
    $('#current-chat').css("opacity", "0");
    $.when($('#current-chat').scrollTop($('#current-chat')[0].scrollHeight)).done(function () {
        $('#current-chat').css("opacity", "");
    });
}

function datetimeoffsetToDisplay(datetimeoffset) {
    var input = moment(datetimeoffset);
    var now = moment();
    if (input.format("DD-MM-YYYY") === now.format("DD-MM-YYYY")) {
        return input.format("HH:mm");
    }
    var duration = moment.duration(now.diff(input));
    if (duration.asDays() < 7) {
        var day;
        switch (input.day()) {
            case 0:
                day = "Thứ Hai ";
                break;
            case 1:
                day = "Thứ Ba ";
                break;
            case 2:
                day = "Thứ Tư ";
                break;
            case 3:
                day = "Thứ Năm ";
                break;
            case 4:
                day = "Thứ Sáu ";
                break;
            case 5:
                day = "Thứ Bảy ";
                break;
            case 6:
                day = "Chủ Nhật ";
                break;
        }
        return day + input.format("HH:mm");
    }
    return input.format("HH:mm") + " " + input.format("D") + " Tháng " + input.format("M") + ", " + input.format("YYYY");
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
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").append(html);
}

function insert_receive_message(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
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
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").prepend(html);
}

function insert_receive_message_top(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div>' +
        '<div class="msg_cotainer">' +
        Content +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '</div>';
    $("#current-chat").prepend(html);
}

function insert_send_message_img(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    var html =
        '<div class="d-flex justify-content-end">' +
        '<div class="msg_cotainer">' +
        '<img src="/UploadedFiles/Image/' + Content + '">' +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").append(html);
}

function insert_receive_message_img(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div>' +
        '<div class="msg_cotainer">' +
        '<img src="/UploadedFiles/Image/' + Content + '">' +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '</div>';
    $("#current-chat").append(html);
}

function insert_send_message_file(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    var html =
        '<div class="d-flex justify-content-end">' +
        '<div class="msg_cotainer">' +
        '<a href="/WebChat/DownloadFile?fileName=' + Content + '">' + '<i class="fas fa-download"></i> ' + Content + '</a>' +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div></div>';
    $("#current-chat").append(html);
}

function insert_receive_message_file(Avatar, Content, Message_status, Send_time) {
    var time = datetimeoffsetToDisplay(Send_time);
    if (Avatar == null) Avatar = 'sample_avatar.png';
    var html =
        '<div class="d-flex justify-content-start">' +
        '<div class="img_cont_msg">' +
        '<img src="/UploadedFiles/Avatar/' + Avatar + '">' +
        '</div>' +
        '<div class="msg_cotainer">' +
        '<a href="/WebChat/DownloadFile?fileName=' + Content + '">' + '<i class="fas fa-download"></i> ' + Content + '</a>' +
        '<span class="msg_time">' + time + '</span>' +
        '</div>' +
        '</div>';
    $("#current-chat").append(html);
}

function loadChatContent() {
    $(document).on('click', '.current-friend-container', function () {
        var startChatFriendId = $('#start-chat').val();
        var friendId;
        if (startChatFriendId == null) {
            $(".current-friend-container").removeClass('active');
            $(this).addClass('active');
            friendId = $(this).children('.friendId').val();
        } else {
            friendId = startChatFriendId;
            $('#start-chat').remove();
        }
        var childContent = $(this).children('.curent-friend-content');
        childContent.children('.current-friend-name').removeClass('current-friend-name-bold');
        childContent.children('.current-friend-last-message').removeClass('current-friend-last-message-bold');
        childContent.children('.current-friend-last-message-time').removeClass('current-friend-last-message-time-bold');
        $('#chat-page').val('1');
        var page = $('#chat-page').val();
        $('#current-chat').off('scroll');
        $('#current-chat').html("");
        $('#current-friend-id').val(friendId);
        $('#spinner-container').show();
        $.ajax({
            url: location.origin + '/WebChat/GetChatContent',
            type: 'POST',
            data: {
                guid: friendId,
                page: page
            }
        }).done(function (result) {
            $('#spinner-container').hide();
            var avatarFriend = result.AvatarFriend;
            if (avatarFriend !== null) {
                $('#chat-title-avatar').attr('src', '/UploadedFiles/Avatar/' + avatarFriend);
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
            loadScrollEvent();
        });
    });
}

function loadCurrentFriend(isFirstClick) {
    $.ajax({
        url: location.origin + '/WebChat/GetCurrentFriend',
        type: 'POST',
    }).done(function (result) {
        $('#currentf-friend').html("");
        result.forEach(function (entry) {
            var avatar = 'sample_avatar.png';
            if (entry.Avatar != null) {
                avatar = entry.Avatar;
            }
            var friendName = entry.FriendName;
            var lastMessage = entry.LastMessage;
            var lastSendTime = entry.LastSendTime;
            if (entry.IsSend) {
                lastMessage = 'Bạn: ' + lastMessage;
            }
            if (entry.MessageStatus === 0 || entry.MessageStatus === 1) {
                friendName = '<div class="current-friend-name current-friend-name-bold">' + friendName + '</div>';
                lastMessage = '<div class="current-friend-last-message current-friend-last-message-bold">' + lastMessage + '</div>';
                lastSendTime = '<div class="current-friend-last-message-time current-friend-last-message-time-bold">' + lastSendTime + '</div>';
            } else {
                friendName = '<div class="current-friend-name">' + friendName + '</div>';
                lastMessage = '<div class="current-friend-last-message">' + lastMessage + '</div>';
                lastSendTime = '<div class="current-friend-last-message-time">' + lastSendTime + '</div>';
            }

            var html = '<div class="current-friend-container">' +
                '<input class="friendId" value="' + entry.FriendId + '" hidden>' +
                '<div class="curent-friend-image">' +
                '<img src="/UploadedFiles/Avatar/' + avatar + '" alt="">';
            if (entry.Status_online) {
                html += '<span class="curent-friend-image-icon online"></span>';
            }
            html += '</div>' +
                '<div class="curent-friend-content">' +
                friendName +
                lastMessage +
                lastSendTime +
                '</div></div>';
            $('#currentf-friend').append(html);
        });
        $(".current-friend-last-message-time").each(function () {
            var datetimeoffset = $(this).html();
            $(this).html(datetimeoffsetToDisplay(datetimeoffset));
        });
        if (isFirstClick) $(".current-friend-container").first().click();
    });
}

function loadScrollEvent() {
    $('#current-chat').on('scroll', function () {
        var pos = $('#current-chat').scrollTop();
        if (pos == 0) {
            console.log('a');
            $('#scroll-to-current-pos').remove();
            $('<span id="scroll-to-current-pos"></span>').insertBefore('.d-flex:first');
            var friendId = $('#current-friend-id').val();
            var page = $('#chat-page').val();
            page++;
            $('#chat-page').val(page);
            $.ajax({
                url: location.origin + '/WebChat/GetChatContent',
                type: 'POST',
                data: {
                    guid: friendId,
                    page: page
                }
            }).done(function (result) {
                $('#spinner-container').hide();
                var avatarFriend = result.AvatarFriend;
                if (avatarFriend !== null) {
                    $('#chat-title-avatar').attr('src', '/UploadedFiles/Avatar/' + avatarFriend);
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
                });
                $("#current-chat").animate({ scrollTop: $("#scroll-to-current-pos").position().top }, 0);
            });
        }
    });
}


loadCurrentFriend(true);
loadChatContent();
loadScrollEvent();

$('#input-search').on('focus', function () {
    $('.current-friend-last-message').hide();
    $('.current-friend-last-message-time').hide();
    $('.current-friend .online').hide();
    $('.current-friend').css('top', '149px');
    $('.curent-friend-image').css('width', '28px');
    $('.curent-friend-image').css('height', '28px');
    $('.curent-friend-image img').css('width', '28px');
    $('.curent-friend-image img').css('height', '28px');
    $('.search-with').show();
});

$('#input-search').on('focusout', function () {
    setTimeout(function () {
        $('.current-friend-last-message').show();
        $('.current-friend-last-message-time').show();
        $('.current-friend .online').show();
        $('.current-friend').css('top', '115px');
        $('.curent-friend-image').css('width', '60px');
        $('.curent-friend-image').css('height', '60px');
        $('.curent-friend-image img').css('width', '57px');
        $('.curent-friend-image img').css('height', '57px');
        $('.search-with').hide();
        loadCurrentFriend(false);
        $('#input-search').val('');
    }, 100);
});

var timeoutInput = null;
$('#input-search').on('keydown', function (e) {
    clearTimeout(timeoutInput);
    timeoutInput = setTimeout(function () {
        var input = $('#input-search').val();
        $('#search-with').html('Tìm kiếm tin nhắn cho "' + input + '"');
        $.ajax({
            url: location.origin + '/WebChat/GetCurrentFriendSearch',
            type: 'POST',
            data: {
                input: input
            }
        }).done(function (result) {
            $('#currentf-friend').html("");
            result.forEach(function (entry) {
                var avatar = 'sample_avatar.png';
                if (entry.Avatar != null) {
                    avatar = entry.Avatar;
                }
                var friendName = entry.FriendName;
                var lastMessage = entry.LastMessage;
                var lastSendTime = entry.LastSendTime;
                if (entry.IsSend) {
                    lastMessage = 'Bạn: ' + lastMessage;
                }
                if (entry.MessageStatus === 0 || entry.MessageStatus === 1) {
                    friendName = '<div class="current-friend-name current-friend-name-bold">' + friendName + '</div>';
                    lastMessage = '<div class="current-friend-last-message current-friend-last-message-bold">' + lastMessage + '</div>';
                    lastSendTime = '<div class="current-friend-last-message-time current-friend-last-message-time-bold">' + lastSendTime + '</div>';
                } else {
                    friendName = '<div class="current-friend-name">' + friendName + '</div>';
                    lastMessage = '<div class="current-friend-last-message">' + lastMessage + '</div>';
                    lastSendTime = '<div class="current-friend-last-message-time">' + lastSendTime + '</div>';
                }

                var html = '<div class="current-friend-container">' +
                    '<input class="friendId" value="' + entry.FriendId + '" hidden>' +
                    '<div class="curent-friend-image">' +
                    '<img src="/UploadedFiles/Avatar/' + avatar + '" alt="">';
                if (entry.Status_online) {
                    html += '<span class="curent-friend-image-icon online"></span>';
                }
                html += '</div>' +
                    '<div class="curent-friend-content">' +
                    friendName +
                    lastMessage +
                    lastSendTime +
                    '</div></div>';
                $('#currentf-friend').append(html);
            });
            $(".current-friend-last-message-time").each(function () {
                var datetimeoffset = $(this).html();
                $(this).html(datetimeoffsetToDisplay(datetimeoffset));
            });
            $('.current-friend-last-message').hide();
            $('.current-friend-last-message-time').hide();
            $('.current-friend .online').hide();
            $('.current-friend').css('top', '149px');
            $('.curent-friend-image').css('width', '28px');
            $('.curent-friend-image').css('height', '28px');
            $('.curent-friend-image img').css('width', '28px');
            $('.curent-friend-image img').css('height', '28px');
        });
    }, 500);
});

$(function () { //This section will run whenever we call Chat.cshtml page
    var objHub = $.connection.chatHub;

    loadClientMethods(objHub);

    $.connection.hub.start().done(function () {
        loadEvents(objHub)
    });
});

function loadEvents(objHub) {
    $('#input-message').on('click', function () {
        var currentFriendId = $('#current-friend-id').val();
        $.ajax({
            type: "POST",
            url: location.origin + '/WebChat/MakeAllRead',
            data: {
                id: currentFriendId
            },
        });
        loadCurrentFriend(false);
    });

    $("#input-message").keypress(function (e) {
        if (e.which == 13) {
            var currentAvatar = $('.justify-content-end').first().children('.img_cont_msg').first().children('img').first().attr('src');
            currentAvatar = currentAvatar.split('/')[currentAvatar.split('/').length - 1];
            var currentFriendId = $('#current-friend-id').val();
            var msg = $("#input-message").val().trim();
            $("#input-message").val('');
            if (msg.length > 0) {
                insert_send_message(currentAvatar, msg, 2, new Date());
                scrollChatToBottom(true);
                objHub.server.sendMessageToUser(currentFriendId, msg);
            }
            setTimeout(function () { loadCurrentFriend(false); }, 1500);
        }
    });
}

function loadClientMethods(objHub) {
    objHub.client.getMessages = function (userId, message) {
        loadCurrentFriend(false);
        var currentFriendId = $('#current-friend-id').val();
        if (currentFriendId === userId) {
            var avatar = $('#chat-title-avatar').attr('src');
            avatar = avatar.split('/')[avatar.split('/').length - 1];
            var messageStatus = 1;
            var sendTime = new Date();
            insert_receive_message(avatar, message, messageStatus, sendTime);
            scrollChatToBottom(true);
        }
        var messageAudio = document.getElementById("message-audio");
        messageAudio.play();
    }

    objHub.client.getMessagesImage = function (userId, message) {
        loadCurrentFriend(false);
        var currentFriendId = $('#current-friend-id').val();
        if (currentFriendId === userId) {
            var avatar = $('#chat-title-avatar').attr('src');
            avatar = avatar.split('/')[avatar.split('/').length - 1];
            var messageStatus = 1;
            var sendTime = new Date();
            insert_receive_message_img(avatar, message, messageStatus, sendTime);
            scrollChatToBottom(true);
        }
        var messageAudio = document.getElementById("message-audio");
        messageAudio.play();
    }

    objHub.client.getMessagesFile = function (userId, message) {
        loadCurrentFriend(false);
        var currentFriendId = $('#current-friend-id').val();
        if (currentFriendId === userId) {
            var avatar = $('#chat-title-avatar').attr('src');
            avatar = avatar.split('/')[avatar.split('/').length - 1];
            var messageStatus = 1;
            var sendTime = new Date();
            insert_receive_message_file(avatar, message, messageStatus, sendTime);
            scrollChatToBottom(true);
        }
        var messageAudio = document.getElementById("message-audio");
        messageAudio.play();
    }
}

$('#send-button-image').on('click', function () {
    $('#input-send-img').trigger('click');
});


$('#input-send-img').on('change', function () {
    var currentFriendId = $('#current-friend-id').val();
    var file_data = $('#input-send-img').prop('files')[0];
    if (file_data.size > 10000000) {
        alert("File quá lớn");
        return;
    }
    var form_data = new FormData();
    form_data.append('file', file_data);
    $.ajax({
        url: location.origin + '/WebChat/SendImageMessage/' + currentFriendId,
        type: 'POST',
        dataType: 'text',
        cache: false,
        contentType: false,
        processData: false,
        data: form_data
    }).done(function (result) {
        var currentAvatar = $('.justify-content-end').first().children('.img_cont_msg').first().children('img').first().attr('src');
        currentAvatar = currentAvatar.split('/')[currentAvatar.split('/').length - 1];
        insert_send_message_img(currentAvatar, result, 1, new Date());
        scrollChatToBottom(true);
        $('#input-send-img').val('');
    });
});

$('#send-button-file').on('click', function () {
    $('#input-send-file').trigger('click');
});


$('#input-send-file').on('change', function () {
    var currentFriendId = $('#current-friend-id').val();
    var file_data = $('#input-send-file').prop('files')[0];
    if (file_data.size > 10000000) {
        alert("File quá lớn");
        return;
    }
    var form_data = new FormData();
    form_data.append('file', file_data);
    $.ajax({
        url: location.origin + '/WebChat/SendFileMessage/' + currentFriendId,
        type: 'POST',
        dataType: 'text',
        cache: false,
        contentType: false,
        processData: false,
        data: form_data
    }).done(function (result) {
        console.log(result);
        var currentAvatar = $('.justify-content-end').first().children('.img_cont_msg').first().children('img').first().attr('src');
        currentAvatar = currentAvatar.split('/')[currentAvatar.split('/').length - 1];
        insert_send_message_file(currentAvatar, result, 1, new Date());
        scrollChatToBottom(true);
        $('#input-send-file').val('');
    });
});