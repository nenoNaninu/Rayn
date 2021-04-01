create table rayn_db.threads(
	ThreadId BINARY(16) not null PRIMARY key,
	OwnerId BINARY(16) not null,
	ThreadTitle text not null,
	BeginningDate datetime not null
);


create table rayn_db.comments(
	Id int not null PRIMARY key auto_increment,
	ThreadId BINARY(16) not null,
	WrittenTime datetime not null,
	Message text
);