drop procedure if exists add_new_individual;
create procedure add_new_individual(IN pNIC varchar(12), IN pFirstName varchar(50),
                                                          IN pLastName varchar(50), IN pDateOfBirth date,
                                                          IN pCustomerId int, IN pEmail varchar(255),
                                                          IN pGender tinyint(1), IN pMobileNumber varchar(20),
                                                          IN pHomeNumber varchar(20), IN pAddress varchar(255),
                                                          IN pIsOrganizationMember BOOL)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;

    -- if pCustomerId is null, then insert a new record into the Customer table for individual
    IF pCustomerId IS NULL THEN
        INSERT INTO customer (type) VALUES (0);
        SET @customerId = LAST_INSERT_ID();
    ELSE
        -- this means that the customer is already in the database (in the case of organization individuals)
        SET @customerId = pCustomerId;
    END IF;

    -- Insert a new record into the Individual table
    INSERT INTO individual (
        NIC, first_name, last_name, date_of_birth, customer_id, email, gender, mobile_number, home_number, address, is_organization_member
    ) VALUES (
                 pNIC, pFirstName, pLastName, pDateOfBirth, @customerId, pEmail, pGender, pMobileNumber, pHomeNumber, pAddress, pIsOrganizationMember
             );

    COMMIT;
END;


drop procedure if exists add_organization_individual;
create procedure if not exists add_organization_individual(
    p_individual_id int,
    p_reg_no varchar(20),
    p_position varchar(50),
    p_work_email varchar(255),
    p_work_mobile_number varchar(20)
)
BEGIN
    INSERT INTO organization_individual(
        individual_id, organization_reg_no,
        position, work_email, work_mobile_number)
    VALUES (p_individual_id, p_reg_no, p_position, p_work_email, p_work_mobile_number);
END;


-- create organization with an individual
drop procedure if exists create_organization_with_individual;
create procedure create_organization_with_individual(
    -- organization details
    p_reg_no varchar(20),
    p_name varchar(50),
    p_address varchar(255),
    p_company_email varchar(255),
    p_type varchar(20),
    -- organization individual details
    p_nic varchar(12),
    p_position varchar(50),
    p_work_email varchar(255),
    p_work_phone varchar(20),
    p_first_name varchar(50),
    p_last_name varchar(50),
    p_date_of_birth date,
    p_gender bool
)
BEGIN
    -- start transaction
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;
    -- CHECK IF ORGANIZATION EXISTS the raise error and rollback
    IF check_organization_exists(p_reg_no) > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Organization already exists';
    END IF;

    -- create customer for the organization
    INSERT INTO customer(type) VALUES (1);
    SET @customer_id = LAST_INSERT_ID();

    -- create organization
    INSERT INTO organization(reg_no, name, address, company_email, type, customer_id)
    VALUES (p_reg_no, p_name, p_address, p_company_email, p_type, @customer_id);

    -- call add individual procedure
    CALL add_new_individual(
            p_nic, p_first_name, p_last_name, p_date_of_birth,
            @customer_id, '' , p_gender, '', '', '', 1);

    -- add organization individual
    SET @individual_id = LAST_INSERT_ID();
    CALL add_organization_individual(
            @individual_id, p_reg_no,
            p_position, p_work_email,
            p_work_phone);

    COMMIT;
END;
