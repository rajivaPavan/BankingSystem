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



