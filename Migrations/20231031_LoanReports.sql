USE bank_management_system;

-- This function is used to calculate the total arrears for a loan.
DELIMITER //
DROP FUNCTION IF EXISTS CalculateMissingAmount;
CREATE FUNCTION CalculateMissingAmount(
-- function getting these parameters as inputs.
    total_amount DECIMAL(10, 2),
    duration SMALLINT,
    interest FLOAT(2, 2),
    no_of_missing_installments INT
) 
RETURNS DECIMAL(10, 2)  
BEGIN
    DECLARE missing DECIMAL(10, 2);
    SET missing = (((total_amount * interest)+ total_amount) / duration ) * no_of_missing_installments;
    RETURN missing;  -- returning calculated arrears amoun
END//

DELIMITER ;



-- creating a view for late loan installment report with having enough information.
DROP VIEW IF EXISTS loan_installment_report;
CREATE VIEW loan_installment_report AS
SELECT 
    ind.customer_id, 
    ind.NIC, 
    CONCAT(ind.first_name, ' ', ind.last_name) AS Name, -- combine first and last names
    l.loan_id,
    MIN(li.due_date) AS last_unpaid_date, -- getting last paid date (minimum date) for calculate the installments and for display
    SUM(CASE WHEN li.is_paid = 0 THEN 1 ELSE 0 END) AS no_of_missing_installments, -- counting how many installments having to pay
    lp.interest,
    l.total_amount,
    CalculateMissingAmount(l.total_amount, l.duration, lp.interest, SUM(CASE WHEN li.is_paid = 0 THEN 1 ELSE 0 END)) AS missing_amount
FROM individual AS ind
JOIN loan AS l ON l.customer_id = ind.customer_id
JOIN loan_plan AS lp ON l.loan_plan_id = lp.loan_plan_id
RIGHT JOIN loan_installment AS li ON l.loan_id = li.loan_id
GROUP BY ind.customer_id, ind.NIC, l.loan_id;



