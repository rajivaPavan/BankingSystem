-- add trigger to check if user_name is unique
drop trigger if exists user_name_unique;
create trigger user_name_unique
    before insert
    on bank_management_system.user
    for each row
begin
    if exists (select 1 from bank_management_system.user where user_name = new.user_name) then
        signal sqlstate '45000' set message_text = 'user_name must be unique';
    end if;
end;

-- function to check whether user has usertype
drop function if exists has_usertype;
create function has_usertype(p_user_name varchar(50), p_usertype tinyint) returns boolean
    deterministic
begin
    declare has_usertype boolean;
    set has_usertype = false;
    if exists (select 1
               from bank_management_system.user
               where user_name = p_user_name
                 and user_type = p_usertype) then
        set has_usertype = true;
    end if;
    return has_usertype;
end;

-- function to authenticate user from user_name and password hash
drop function if exists authenticate_user;
drop procedure if exists authenticate_user;
create procedure authenticate_user(in p_user_name varchar(50),
                                   in p_password_hash varchar(64),
                                   out o_user_type int)
begin
    select user_type
    into o_user_type
    from user
    where user_name = p_user_name
      and password_hash = p_password_hash;
    
    if o_user_type is null then
        SET o_user_type = -1;
    end if;
end;

drop procedure if exists register_individual_user;
create procedure register_individual_user(in p_user_name varchar(50),
                              in p_password_hash varchar(64),
                              in p_individual_id int) 
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
    
        UPDATE individual
        SET user_id = @user_id
        WHERE individual_id = p_individual_id;
    COMMIT;   
end;