-- change type of fixed_deposit alter table fixed_deposit opening_date to date
alter table fixed_deposit
    modify opening_date date null;

alter table fixed_deposit
    modify next_calculation_on date null;