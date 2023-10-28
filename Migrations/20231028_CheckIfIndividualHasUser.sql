create
    definer = root@localhost function check_individual_exists_has_user_account(p_nic varchar(12),
                                                                               p_bankAccount varchar(16),
                                                                               p_business_reg_no varchar(20),
                                                                               is_individual tinyint(1))
    returns tinyint(1)
    deterministic
BEGIN
    DECLARE result_count INT;
    DECLARE result_user_id INT;
    SET result_count = 0;
    SET result_user_id = NULL;

    IF is_individual = 1 THEN
        SELECT user_id, COUNT(individual_id) INTO result_user_id, result_count
        FROM (SELECT customer_id FROM bank_account AS b WHERE b.account_no = p_bankAccount) AS ba
                 JOIN customer AS c ON ba.customer_id = c.id
                 JOIN individual AS i ON c.id = i.customer_id
        WHERE i.nic = p_nic;
    ELSE
        SELECT i.user_id, COUNT(i.individual_id) INTO result_user_id, result_count
        FROM organization_individual AS oi
                 JOIN individual AS i ON oi.individual_id = i.individual_id
        WHERE oi.organization_reg_no = p_business_reg_no AND i.nic = p_nic;
    END IF;

    IF result_count = 1 AND result_user_id IS NOT NULL THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;
END;

