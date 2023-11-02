drop procedure if exists calculate_savings_interest;
create procedure calculate_savings_interest()
BEGIN
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

    OPEN to_update;

    update_loop: LOOP
        FETCH to_update INTO v_account_no, v_balance, v_interest;
        IF done THEN
            LEAVE update_loop;
        END IF;
        INSERT INTO transactions (account_no, amount, transaction_type, time_stamp)
        VALUES (v_account_no, v_balance * v_interest / 12, 2, now());
        UPDATE savings_account
        SET next_calculation_on = DATE_ADD(next_calculation_on, INTERVAL 1 MONTH)
        WHERE account_no = v_account_no;
    END LOOP;
    CLOSE to_update;
END;


