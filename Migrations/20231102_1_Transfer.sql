drop procedure if exists add_new_transfer;
CREATE PROCEDURE add_new_transfer(
    IN pSenderAccountNo VARCHAR(16), -- Account number of the sender account
    IN pReceiverAccountNo VARCHAR(16), -- Account number of the receiver account
    IN pTransferAmount double(10, 2), -- Transfer amount
    IN pReference VARCHAR(100) -- Transfer reference (optional)
)
BEGIN
    DECLARE plan_id INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;


    -- Validate the sender and receiver account numbers
    IF NOT EXISTS (SELECT 1
                   FROM bank_account
                   WHERE account_no = pSenderAccountNo) OR NOT EXISTS (SELECT 1
                                                                       FROM bank_account
                                                                       WHERE account_no = pReceiverAccountNo) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid sender or receiver account number';
    END IF;

    -- Validate the transfer amount
    IF pTransferAmount < 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Transfer amount must be positive';
    END IF;

    -- Get the savings account plan for the account
    SELECT savings_plan_id INTO plan_id FROM savings_account WHERE account_no = pSenderAccountNo;

    -- Validate the transaction amount against the minimum balance requirement for the savings account plan
    IF pTransferAmount > (SELECT minimum
                          FROM savings_plan
                          WHERE savings_plan_id = plan_id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Withdrawal amount exceeds minimum balance requirement';
    END IF;

    -- Insert a new record into the transfers table
    INSERT INTO transfer (from_account_no, to_account_no, amount, reference, time_stamp)
    VALUES (pSenderAccountNo, pReceiverAccountNo, pTransferAmount, pReference, NOW());
    COMMIT;
END;

-- trigger to update balance 
drop trigger if exists transfer_trigger;
CREATE TRIGGER transfer_trigger
    AFTER INSERT
    ON transfer
    FOR EACH ROW
BEGIN
    -- Update the sender account balance
    UPDATE bank_account
    SET balance = balance - NEW.amount
    WHERE account_no = NEW.from_account_no;

    -- Update the receiver account balance
    UPDATE bank_account
    SET balance = balance + NEW.amount
    WHERE account_no = NEW.to_account_no;
END;
