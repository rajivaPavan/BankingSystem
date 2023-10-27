alter table bank_account
    modify account_status tinyint not null;

alter table bank_account
    modify account_type tinyint not null;

alter table employee
    modify gender tinyint(1) not null;

alter table fd_plan
    modify interest double(2, 2) not null;

alter table guardian
    modify gender tinyint(1) null;

alter table individual
    modify gender tinyint(1) not null;

alter table loan_plan
    modify interest double(2, 2) not null;

alter table savings_plan
    modify interest double(2, 2) not null;



