DELIMITER //

DROP TRIGGER IF EXISTS after_a_loan_insert;
CREATE TRIGGER after_a_loan_insert
AFTER INSERT ON loan
FOR EACH ROW
BEGIN
    -- only add installments dates having is_paid = 0 if loan is approved. otherwise store it as in original state 
    -- in loan table but there is no any update for loan_installment table
    DECLARE i INT DEFAULT 1;
    IF NEW.approval_status = 1 THEN  
        WHILE i <= NEW.duration DO
            INSERT INTO loan_installment (loan_id, due_date, is_paid)
            VALUES (NEW.loan_id, DATE_ADD(CURDATE(), INTERVAL i * 30 DAY), 0);
            SET i = i + 1;
        END WHILE;
    END IF;
END//

DELIMITER ;
