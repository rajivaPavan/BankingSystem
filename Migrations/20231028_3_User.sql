create index user_name_index
    on bank_management_system.user (user_name);

-- add trigger to check if user_name is unique
create trigger user_name_unique
    before insert on bank_management_system.user
    for each row
begin
    if exists (select 1 from bank_management_system.user where user_name = new.user_name) then
        signal sqlstate '45000' set message_text = 'user_name must be unique';
    end if;
end;

-- function to check whether user has usertype
create function has_usertype (p_user_name varchar(50), p_usertype tinyint) returns boolean
    deterministic
begin
    declare has_usertype boolean;
    set has_usertype = false;
    if exists (select 1 from bank_management_system.user 
        where user_name = p_user_name 
          and user_type = p_usertype) then
        set has_usertype = true;
    end if;
    return has_usertype;
end;

-- function to authenticate user from user_name and password hash
create function authenticate_user (p_user_name varchar(50), p_password_hash varchar(64)) returns boolean
    deterministic
begin
    declare authenticated boolean;
    set authenticated = false;
    if exists (select 1 from bank_management_system.user 
        where user_name = p_user_name 
          and password_hash = p_password_hash) then
        set authenticated = true;
    end if;
    return authenticated;
end;


