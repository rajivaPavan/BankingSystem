-- For individuals above 18 years old
DROP VIEW IF EXISTS  individual_view_for_employee;
CREATE VIEW individual_view_for_employee AS
SELECT individual_id, customer_id, NIC, first_name, last_name, date_of_birth,
       email, gender, mobile_number, home_number, address
FROM individual where timestampdiff(YEAR, date_of_birth, current_date()) > 18;

DROP VIEW IF EXISTS bank_accounts_view_for_employee;
CREATE VIEW bank_accounts_view_for_employee AS
SELECT acc.account_no, acc.customer_id, acc.balance, acc.opening_date, acc.account_status, acc.account_type  
FROM bank_account as acc;

DROP VIEW IF EXISTS  individual_bank_account_for_employee;
CREATE VIEW individual_bank_account_for_employee AS
SELECT i.customer_id as customer_id, individual_id, NIC, first_name, last_name, 
       date_of_birth, email, 
       gender, mobile_number, 
       home_number, address, 
       account_no, balance, opening_date, 
       account_status, account_type
FROM individual_view_for_employee AS i
         JOIN bank_accounts_view_for_employee AS acc ON i.customer_id = acc.customer_id;


-- For individuals below 18 years of age
DROP VIEW IF EXISTS child_view_for_employee;
CREATE VIEW child_view_for_employee AS 
SELECT individual_id, customer_id, NIC, first_name, last_name, date_of_birth,
       email, gender, mobile_number, home_number, address
FROM individual where timestampdiff(YEAR, date_of_birth, current_date()) < 18;

DROP VIEW IF EXISTS child_and_guardian_view_for_employee;
CREATE VIEW child_and_guardian_view_for_employee AS
SELECT 
    individual_id, customer_id, c.first_name as first_name, 
    c.last_name as last_name, 
    c.date_of_birth as date_of_birth,
    c.gender as gender,
    g.NIC as guardian_NIC, g.first_name as guardian_first_name, 
    g.date_of_birth as guardian_date_of_birth,
    email, mobile_number, home_number, address
    FROM child_view_for_employee AS c 
JOIN guardian as g ON g.NIC = c.NIC;

DROP VIEW IF EXISTS child_bank_account_for_employee;
CREATE VIEW child_bank_account_for_employee AS
SELECT i.customer_id as customer_id, individual_id, guardian_NIC, first_name, last_name,
       date_of_birth, email,
       gender, mobile_number,
       home_number, address,
       account_no, balance, opening_date,
       account_status, account_type
FROM child_and_guardian_view_for_employee AS i
         JOIN bank_accounts_view_for_employee AS acc ON i.customer_id = acc.customer_id;




