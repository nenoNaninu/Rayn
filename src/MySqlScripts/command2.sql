create table rayn_db.accounts(
    UserId binary(16) not null primary key,
    Email text not null,
    LinkToGoogle boolean not null
);

create table rayn_db.google_accounts(
    UserId binary(16) not null primary key,
    Identifier text not null ,
    Email text not null
);

alter table rayn_db.threads add(
    AuthorID binary(16) null,
    CreatedDate datetime not null,
    DateOffset time not null
);
