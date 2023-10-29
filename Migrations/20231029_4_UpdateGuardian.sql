alter table bank_management_system.guardian
    drop foreign key guardian_ibfk_1;

DROP VIEW IF EXISTS minimal_child_bank_account_view;
CREATE VIEW minimal_child_bank_account_view AS
SELECT i.customer_id as customer_id, individual_id, guardian_NIC, first_name, last_name,
       date_of_birth,
       account_no, balance, opening_date,
       account_status, account_type
FROM minimal_child_and_guardian_view AS i
         left JOIN minimal_bank_account_view AS acc ON i.customer_id = acc.customer_id;