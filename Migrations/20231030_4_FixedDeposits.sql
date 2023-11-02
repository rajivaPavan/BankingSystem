-- create procedure to add fixed deposit from savings_account_no, fd_plan, amount
drop procedure if exists add_fixed_deposit;
create procedure add_fixed_deposit(
    IN p_savings_account_no varchar(16), 
    IN p_fd_plan_id int, 
    IN p_amount int)
begin
    DECLARE v_customer_id int;
    DECLARE today date;
    SELECT customer_id INTO v_customer_id FROM bank_account 
        WHERE account_no = p_savings_account_no AND account_type = 1;
    IF v_customer_id IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid savings account number';
    END IF;
    SET today = CURDATE();
    INSERT INTO fixed_deposit(customer_id,fd_plan_id, savings_account_no, opening_date, amount, next_calculation_on)
        VALUES (v_customer_id, p_fd_plan_id, p_savings_account_no, today, p_amount, DATE_ADD(today, INTERVAL 1 MONTH));
end;

drop view if exists fd_view;
create view fd_view as
select fd.customer_id as customer_id, fd.fd_no, fd.savings_account_no, fd.next_calculation_on, fd.opening_date, fd.amount, plan.duration, plan.interest
from fixed_deposit as fd
         join fd_plan as plan on fd.fd_plan_id = plan.fd_plan_id;

-- create procedure to calculate interest for fixed deposit
drop procedure if exists calculate_fd_interests;
create procedure calculate_fd_interests()
begin
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_account_no varchar(16);
    DECLARE v_fd_amount double(10,2);
    DECLARE v_interest double(2,2);
    DECLARE v_fd_no int;
    DECLARE to_update CURSOR FOR
        SELECT fd.savings_account_no, fd.amount, fd.interest, fd.fd_no
        FROM fd_view as fd
        WHERE fd.next_calculation_on = CURDATE() 
          AND DATE_ADD(fd.opening_date, 
              INTERVAL fd.duration MONTH) >= CURDATE();

    -- declare NOT FOUND handler
    DECLARE CONTINUE HANDLER
        FOR NOT FOUND SET done = 1;

    OPEN to_update;

    update_loop: LOOP
        FETCH to_update INTO v_account_no, v_fd_amount, v_interest, v_fd_no;
        IF done THEN
            LEAVE update_loop;
        END IF;
        INSERT INTO transactions (account_no, amount, transaction_type, time_stamp)
        VALUES (v_account_no, v_fd_amount * v_interest / 12, 2, now());
        UPDATE fixed_deposit
        SET next_calculation_on = DATE_ADD(next_calculation_on, INTERVAL 1 MONTH)
        WHERE fd_no = v_fd_no;
    END LOOP;
    CLOSE to_update;
end;


drop event if exists calculate_fd_interests_event;
create event calculate_fd_interests_event
    on schedule every 1 day
    do call calculate_fd_interests();

