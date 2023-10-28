DELIMITER //
CREATE PROCEDURE add_savings_account(
    IN p_account_no INT,
    IN p_customer_id INT,
    IN p_branch_id INT,
    IN p_balance DECIMAL(10, 2),
    IN p_opening_date DATE,
    IN p_savings_plan_id INT)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;

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
            p_balance,
            p_opening_date,
            0, -- 0 for active status
            1); -- 1 for savings account

    -- Create the savings account entry
    INSERT INTO savings_account (account_no,
                                 savings_plan_id,
                                 next_calculation_on)
    VALUES (p_account_no,
            p_savings_plan_id,
            p_opening_date + INTERVAL 1 MONTH);
    
    COMMIT;

END //
DELIMITER ;
