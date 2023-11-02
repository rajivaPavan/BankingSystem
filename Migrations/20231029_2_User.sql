drop procedure if exists authenticate_user;
create
    definer = root@localhost procedure authenticate_user(IN p_user_name varchar(50), IN p_password_hash varchar(64),
                                                         OUT o_user_type int, OUT o_user_id int)
begin
    select user_id,user_type
    into o_user_id, o_user_type
    from user
    where user_name = p_user_name
      and password_hash = p_password_hash;

    if o_user_type is null then
        SET o_user_type = -1;
        SET o_user_id = -1;
    end if;
end;

