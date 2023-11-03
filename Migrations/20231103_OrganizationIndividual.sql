drop procedure if exists add_organization_individual;
create procedure create_organization_individual(IN p_individual_id int, IN p_reg_no varchar(20),
                                                                   IN p_position varchar(50),
                                                                   IN p_work_email varchar(255),
                                                                   IN p_work_mobile_number varchar(20))
BEGIN
    INSERT INTO organization_individual(
        individual_id, organization_reg_no,
        position, work_email, work_mobile_number)
    VALUES (p_individual_id, p_reg_no, p_position, p_work_email, p_work_mobile_number);
END;

drop procedure if exists create_organization_with_individual;
drop procedure if exists add_organization_with_individual;
create procedure add_organization_with_individual(IN p_reg_no varchar(20),
                                                                           IN p_name varchar(50),
                                                                           IN p_address varchar(255),
                                                                           IN p_company_email varchar(255),
                                                                           IN p_type varchar(20), IN p_nic varchar(12),
                                                                           IN p_position varchar(50),
                                                                           IN p_work_email varchar(255),
                                                                           IN p_work_phone varchar(20),
                                                                           IN p_first_name varchar(50),
                                                                           IN p_last_name varchar(50),
                                                                           IN p_date_of_birth date,
                                                                           IN p_gender tinyint(1))
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

    CALL add_organization_individual(p_nic, p_first_name, p_last_name, p_date_of_birth, 
        @customer_id, p_work_email, p_gender, p_work_phone, p_reg_no, p_position);

    COMMIT;
END;

drop procedure if exists add_organization_individual;
CREATE PROCEDURE add_organization_individual(
    IN p_nic VARCHAR(12),
    IN p_first_name VARCHAR(50),
    IN p_last_name VARCHAR(50),
    IN p_date_of_birth DATE,
    IN p_customer_id INT,
    IN p_work_email VARCHAR(255),
    IN p_gender TINYINT(1),
    IN p_work_phone VARCHAR(20),
    IN p_reg_no VARCHAR(20),
    IN p_position VARCHAR(50)
)
BEGIN
    -- Start a new transaction for this procedure
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    START TRANSACTION;

    -- Call add_new_individual procedure
    CALL add_new_individual(
            p_nic, p_first_name, p_last_name, p_date_of_birth,
            p_customer_id, p_work_email, p_gender, p_work_phone,
            '0112323111', 'address', 1);

    -- Get the last inserted individual_id
    SET @individual_id = LAST_INSERT_ID();

    -- Call create_organization_individual procedure
    CALL create_organization_individual(
            @individual_id, p_reg_no, p_position, p_work_email, p_work_phone);

    COMMIT;
END;
