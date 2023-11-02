-- view to get organizations and the organization individuals
drop view if exists organization_view_for_employee;
create view organization_view_for_employee as
select 
    reg_no,
    name,
    o.customer_id as customer_id,
    o.type as type,
    o.address as address,
    o.company_email as company_email,
    oi.individual_id as individual_id,
    oi.position as position,
    i.first_name as first_name,
    i.last_name as last_name,
    i.NIC as NIC
    from organization as o
join organization_individual as oi
    on oi.organization_reg_no = o.reg_no
join minimal_individual_view as i
    on i.individual_id = oi.individual_id;

 -- check organization exists
drop function if exists check_organization_exists;
create
    function check_organization_exists(p_reg_no int) returns int
    deterministic
BEGIN
    DECLARE count INT;
    SELECT COUNT(*) INTO count FROM organization_view_for_employee WHERE reg_no = p_reg_no;
    RETURN count;
END; 

drop function if exists check_individual_exists; 
create
    function check_individual_exists(p_nic varchar(12)) returns int
    deterministic
BEGIN
    DECLARE count INT;
    SELECT COUNT(*) INTO count FROM minimal_individual_view WHERE NIC = p_nic;
    RETURN count;
END;
