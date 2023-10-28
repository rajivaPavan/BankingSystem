DELIMITER //
Drop procedure if exists add_new_individual;
CREATE PROCEDURE add_new_individual(
    IN pNIC VARCHAR(12), -- NIC of the individual or guardian
    IN pFirstName VARCHAR(50), -- name of the individual
    IN pLastName VARCHAR(50), -- name of the individual
    IN pDateOfBirth DATE,  -- date of birth of the individual
    IN pCustomerId INT, -- customer id of the organization individual
    IN pEmail VARCHAR(255), -- email of the individual or guardian
    IN pGender BOOL, --  gender of the individual
    IN pMobileNumber VARCHAR(20), -- mobile number of the individual or guardian
    IN pHomeNumber VARCHAR(20), -- home number of the individual or guardian
    IN pAddress VARCHAR(255) -- address of the individual
)
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
        NIC, first_name, last_name, date_of_birth, customer_id, email, gender, mobile_number, home_number, address
    ) VALUES (
                 pNIC, pFirstName, pLastName, pDateOfBirth, @customerId, pEmail, pGender, pMobileNumber, pHomeNumber, pAddress
             );

    COMMIT;
END//

DELIMITER ;
