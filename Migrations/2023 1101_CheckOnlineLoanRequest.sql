DROP TRIGGER IF EXISTS checking_fd_for_online_loan;

DELIMITER //

-- Before insert the data into the loan table for online loans, we are looking for 
-- the maximum existing FIXED DEPOSIT to check requirements.
CREATE TRIGGER checking_fd_for_online_loan 
BEFORE INSERT ON loan 
FOR EACH ROW
BEGIN
    DECLARE max_fd_amount DECIMAL(10, 2);

    -- Find the maximum FD amount for the customer
    SELECT MAX(amount) INTO max_fd_amount
    FROM fixed_deposit
    WHERE customer_id = NEW.customer_id;

    -- Check if the new loan request meets the necessary conditions
    IF NOT (
        NEW.is_online = 1 AND 
        NEW.total_amount <= max_fd_amount * 0.6 AND 
        max_fd_amount * 0.6 <= 500000
    ) THEN  -- Set the loan's approval status as auto-approved for online requests meeting criteria otherwise it won't add to the loan table.
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Online-loan does not meet requirements for automatic approval';
    END IF;
END;
//

DELIMITER ;
