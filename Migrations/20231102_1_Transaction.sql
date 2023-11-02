drop procedure if exists add_new_transaction;
CREATE PROCEDURE add_new_transaction(
    IN pAccountNo VARCHAR(16), -- Account number of the sender account
    IN pTransactionType TINYINT, -- Transaction type (0: deposit, 1: withdraw, 2: interest, 3: fee, 4: other)
    IN pAmount double(10, 2), -- Transaction amount
    IN pReference VARCHAR(100) -- Transaction reference (optional)
)
BEGIN
    DECLARE plan_id INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;

-- Validate the account number
    IF NOT EXISTS (SELECT 1
                   FROM bank_account
                   WHERE account_no = pAccountNo) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid account number';
    END IF;

-- Validate the transaction type
    IF pTransactionType NOT IN (0, 1, 2, 3, 4) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid transaction type';
    END IF;

-- Validate the transaction amount
    IF pAmount < 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Transaction amount must be positive';
    END IF;

    -- Get the savings account plan for the account
    SELECT savings_plan_id INTO plan_id FROM savings_account WHERE account_no = pAccountNo;

    -- Validate the transaction amount against the minimum balance requirement for the savings account plan
    IF pTransactionType = 1 AND pAmount > (SELECT minimum
                                           FROM savings_plan
                                           WHERE savings_plan_id = plan_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Withdrawal amount exceeds minimum balance requirement';
    END IF;

-- Validate that the transaction type is valid for the account type
    IF pTransactionType IN (2, 3) AND (SELECT account_type
                                       FROM bank_account
                                       WHERE account_no = pAccountNo) = 1 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid transaction type for account type';
    END IF;


-- Insert a new record into the transactions table
    INSERT INTO transactions (account_no, transaction_type, amount, time_stamp)
    VALUES (pAccountNo, pTransactionType, pAmount, NOW());
    COMMIT;
END;

-- add trigger to handle transactions
drop trigger if exists transaction_trigger;
create trigger transaction_trigger
    after insert
    on transactions
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