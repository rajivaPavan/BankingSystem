-- Add trigger to check if bank account with same customer id and branch id exists

create trigger check_bank_account_exists
before insert on bank_account
for each row
begin
    if exists (select * from bank_account where customer_id = new.customer_id and branch_id = new.branch_id) then
        signal sqlstate '45000' set message_text = 'Bank account already exists';
    end if;
end;

create trigger check_individual_exists_trigger
before insert on individual
for each row
begin
    if exists (select * from individual where customer_id = new.customer_id and is_organization_member=false) then
        signal sqlstate '45000' set message_text = 'Individual already exists';
    end if;
end;
