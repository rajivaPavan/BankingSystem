create procedure register_banker_user(IN p_user_name varchar(50),
    IN p_password_hash varchar(64), IN p_employee_id int)
begin
    -- transaction to add record to user table and update individual table
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;
    INSERT INTO user (user_name, password_hash, user_type)
    VALUES (p_user_name, p_password_hash, 0);

    SET @user_id = LAST_INSERT_ID();

    UPDATE employee
    SET user_id = @user_id
    WHERE employee_id = p_employee_id;
    COMMIT;
end;

