drop procedure if exists add_savings_account;
create procedure add_savings_account(IN p_account_no varchar(16), IN p_customer_id int,
                                                           IN p_branch_id int, IN p_balance decimal(10, 2),
                                                           IN p_opening_date date, IN p_savings_plan_id int)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;
    
    -- check if balance is greater than the minimum defined for the savings plan
    IF p_balance < (SELECT minimum FROM savings_plan WHERE savings_plan_id = p_savings_plan_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Initial deposit insufficient for the savings plan';
    END IF;

    -- Create the bank account
    INSERT INTO bank_account (account_no,
                              customer_id,
                              branch_id,
                              balance,
                              opening_date,
                              account_status,
                              account_type)
    VALUES (p_account_no,
            p_customer_id,
            p_branch_id,
            0,
            p_opening_date,
            0, -- 0 for active status
            1); -- 1 for savings account
            
    -- make the initial deposit
    INSERT INTO transactions (
                             account_no,
                             amount,
                             transaction_type,
                              time_stamp)
    VALUES (p_account_no,
            p_balance,
            0,
            current_timestamp); -- 0 for success

    -- Check 
    -- Create the savings account entry
    INSERT INTO savings_account (account_no,
                                 savings_plan_id,
                                 next_calculation_on)
    VALUES (p_account_no,
            p_savings_plan_id,
            p_opening_date + INTERVAL 1 MONTH);

    COMMIT;

END;


-- add trigger to handle transactions
drop trigger if exists transaction_trigger;
create trigger transaction_trigger
after insert on transactions
for each row
begin
    -- update the balance of the account
    -- sign of amount should be change depending on the transaction type
    declare new_amount double(10, 2);
    set new_amount = new.amount;
    if new.transaction_type = 1 or new.transaction_type = 3 then
        set new_amount = -1 * new.amount;
    end if;
    update bank_account
    set balance = balance + new_amount
    where account_no = new.account_no;
end;

