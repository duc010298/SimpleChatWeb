use master;
drop database WebChat;

create database WebChat;
use WebChat;

create table app_role (
	role_id bigint primary key,
	role_name varchar(30) not null unique
);

create table app_user (
	app_user_id uniqueidentifier primary key default newid(),
	username varchar(150) unique not null,
	encrypted_password varchar(150) not null,
);

create table user_role(
	app_user_id uniqueidentifier foreign key references app_user(app_user_id),
	role_id bigint foreign key references app_role(role_id),
	primary key (app_user_id, role_id)
);

-- Tách customer thành 2 bảnh User_role và customer để phòng trường hợp sau này làm đăng nhập
-- cho admin và có thêm bảng Admin_Detail

create table customer(
	app_user_id uniqueidentifier primary key foreign key references app_user(app_user_id),
	avatar varchar(max),
	fullname nvarchar(200) not null,
	status_online bit not null,
	last_online datetimeoffset not null,
	email varchar(254) not null,
	phone varchar(10),
	gender bit not null,
	birth date not null,
	city nvarchar(200),
	customer_description nvarchar(max),
	last_change_password datetimeoffset not null,
);

create table relationship(
	relationship_id uniqueidentifier primary key default newid(),
	cus1_id uniqueidentifier foreign key references customer(app_user_id) not null,
	cus2_id uniqueidentifier foreign key references customer(app_user_id) not null,
	relationship_status int not null,
	--status = 0: Đang chặn
	--status = 1: Đang kết bạn
	--status = 2: Đang gửi yêu cầu
	unique(cus1_id, cus2_id)
);

create table message (
	id uniqueidentifier primary key default newid(),
	cus_send_id uniqueidentifier foreign key references customer(app_user_id) not null,
	cus_receive_id uniqueidentifier foreign key references customer(app_user_id) not null,
	message nvarchar(max) not null,
	send_time datetimeoffset not null,
	message_status int not null
	--status = 0: Đã gửi
	--status = 1: Đã nhận nhưng chưa xem
	--status = 2: Đã xem
);

create table notify(
	notify_id uniqueidentifier primary key default newid(),
	cus_id uniqueidentifier foreign key references customer(app_user_id) not null,
	date_create datetimeoffset not null,
	notify_content nvarchar(max) not null
);