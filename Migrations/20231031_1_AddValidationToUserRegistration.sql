drop procedure if exists register_individual_user;
create procedure register_individual_user(IN p_user_name varchar(50),
                                          IN p_password_hash varchar(64), IN p_individual_id int)
begin
    -- transaction to add record to user table and update individual table
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    -- check if individual already has a user account
    IF EXISTS (SELECT * FROM individual WHERE individual_id = p_individual_id AND user_id IS NOT NULL) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Individual already has a user account';
    END IF;

    START TRANSACTION;
    INSERT INTO user (user_name, password_hash, user_type)
    VALUES (p_user_name, p_password_hash, 0);

    SET @user_id = LAST_INSERT_ID();

    UPDATE individual
    SET user_id = @user_id
    WHERE individual_id = p_individual_id;
    COMMIT;
end;

drop procedure if exists register_banker_user;
create procedure register_banker_user(IN p_user_name varchar(50), IN p_password_hash varchar(64),
                                      IN p_employee_id int)
begin
    -- transaction to add record to user table and update individual table
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    -- check if employee already has a user account
    IF EXISTS (SELECT * FROM employee WHERE employee_id = p_employee_id AND user_id IS NOT NULL) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Employee already has a user account';
    END IF;

    START TRANSACTION;
    INSERT INTO user (user_name, password_hash, user_type)
    VALUES (p_user_name, p_password_hash, 1);

    SET @user_id = LAST_INSERT_ID();

    UPDATE employee
    SET user_id = @user_id
    WHERE employee_id = p_employee_id;
    COMMIT;
end;

-- make user_id of employee nullable
alter table employee
    modify user_id int;