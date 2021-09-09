create table rayn_db.account(
	UserId binary(16) not null primary key,
	Email text not null,
	LinkToGoogle boolean not null,
);

create table rayn_db.google_account(
	Identifier text not null primary key,
	UserId binary(16) not null ,
	Email text not null,
);

alter table rayn_db.threads add(
	AuthorID binary(16) not null,
	CreatedDate datetime not null,
	DateOffset time not null,
);