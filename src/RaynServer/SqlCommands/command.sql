-- rayn_db用のスクリプト
set character_set_database=utf8;
set character_set_server=utf8;

create table rayn_db.threads(
	ThreadId BINARY(16) not null PRIMARY key,
	OwnerId BINARY(16) not null,
	ThreadTitle text not null,
	BeginningDate datetime not null
);

-- insert into rayn_db.threads values (0xff00ff00ff00ff00ff00ff00ff00ff00, 0xff00ff00ff00ff00ff00ff00ff00ff00, 'title', '2019-10-04 15:25:07');


create table rayn_db.comments(
	Id int not null PRIMARY key auto_increment,
	ThreadId BINARY(16) not null,
	WrittenTime datetime not null,
	Message text
);

-- insert into rayn_db.comments (ThreadId, WrittenTime, Message) values (0x469EE82C6B76BF458267E2988D7C4436, '2021-04-01 00:00:00', 'めろんぱん');

show variables like "chara%";