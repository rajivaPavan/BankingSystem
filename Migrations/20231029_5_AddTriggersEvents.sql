-- Add trigger to check if bank account with same customer id and branch id exists

drop trigger check_bank_account_exists;
create trigger check_bank_account_exists
before insert on bank_account
for each row
begin
    if exists (select * from bank_account where customer_id = new.customer_id and branch_id = new.branch_id and account_type=new.account_type) then
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



DROP PROCEDURE IF EXISTS calculate_savings_interest;
CREATE PROCEDURE calculate_savings_interest()
    BEGIN
        DECLARE today DATE;
        DECLARE done INT DEFAULT FALSE;
        DECLARE v_account_no varchar(16);
        DECLARE v_balance double(10,2);
        DECLARE v_interest double(2,2);
        DECLARE to_update CURSOR FOR
            SELECT a.account_no, a.balance, sp.interest FROM savings_account as sa
                                                                 LEFT JOIN bank_account as a
                                                                           ON sa.account_no = a.account_no
                                                                 LEFT JOIN savings_plan as sp
                                                                           ON sa.savings_plan_id = sp.savings_plan_id
            WHERE closing_date IS NULL AND account_type = 1 AND account_status = 0;

        -- declare NOT FOUND handler
        DECLARE CONTINUE HANDLER
            FOR NOT FOUND SET done = 1;

        SET today = NOW();
        
        OPEN to_update;
        
        update_loop: LOOP
            FETCH to_update INTO v_account_no, v_balance, v_interest;
            IF done THEN
                LEAVE update_loop;
            END IF;
            INSERT INTO transactions (account_no, amount, transaction_type, time_stamp)
                VALUES (v_account_no, v_balance * v_interest, 2, today);
        END LOOP;
        CLOSE to_update;
    END;

CALL calculate_savings_interest();

DROP EVENT IF EXISTS calculate_savings_interest;
-- schedule interest calculation
CREATE EVENT calculate_savings_interest
    ON SCHEDULE EVERY '1' day 
        STARTS CURRENT_DATE
    DO
        CALL calculate_savings_interest();

-- EVENTS ON
SET GLOBAL event_scheduler = ON;

SHOW VARIABLES LIKE 'event_scheduler';

SHOW EVENTS;
