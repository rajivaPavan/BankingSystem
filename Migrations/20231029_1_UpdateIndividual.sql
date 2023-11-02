-- add is_organization_member to individual 
alter table individual add column is_organization_member boolean not null default false;

-- update is_organization_member to true for all individuals that are members of an organization
update individual set is_organization_member = true where individual.individual_id in (select individual_id from organization_individual);

-- For individuals above 18 years old
DROP VIEW IF EXISTS  individual_view_for_employee;

DROP VIEW IF EXISTS  minimal_individual_view;
CREATE VIEW minimal_individual_view AS
SELECT individual_id, customer_id, NIC, first_name, last_name, date_of_birth
FROM individual where timestampdiff(YEAR, date_of_birth, current_date()) > 18 and is_organization_member = false;


DROP VIEW IF EXISTS bank_accounts_view_for_employee;

DROP VIEW IF EXISTS minimal_bank_account_view;
CREATE VIEW minimal_bank_account_view AS
SELECT acc.account_no, acc.branch_id, acc.customer_id, acc.balance, acc.opening_date, acc.account_status, acc.account_type
FROM bank_account as acc;

DROP VIEW IF EXISTS  individual_bank_account_for_employee;

DROP VIEW IF EXISTS  minimal_individual_bank_account_view;
CREATE VIEW minimal_individual_bank_account_view AS
SELECT i.customer_id as customer_id, individual_id, NIC, first_name, last_name,
       date_of_birth,
       account_no, balance, opening_date,
       account_status, account_type
FROM minimal_individual_view AS i
         LEFT JOIN minimal_bank_account_view AS acc ON i.customer_id = acc.customer_id;


-- For individuals below 18 years of age
DROP VIEW IF EXISTS child_view_for_employee;

DROP VIEW IF EXISTS minimal_child_individual_view;
CREATE VIEW minimal_child_individual_view AS
SELECT individual_id, customer_id, NIC, first_name, last_name, date_of_birth
FROM individual where timestampdiff(YEAR, date_of_birth, current_date()) < 18;

DROP VIEW IF EXISTS child_and_guardian_view_for_employee;
DROP VIEW IF EXISTS minimal_child_and_guardian_view;
CREATE VIEW minimal_child_and_guardian_view AS
SELECT
    individual_id, customer_id, c.first_name as first_name,
    c.last_name as last_name,
    c.date_of_birth as date_of_birth,
    g.NIC as guardian_NIC, g.first_name as guardian_first_name,
    g.date_of_birth as guardian_date_of_birth
FROM minimal_child_individual_view AS c
         JOIN guardian as g ON g.NIC = c.NIC;

DROP VIEW IF EXISTS child_bank_account_for_employee;
DROP VIEW IF EXISTS minimal_child_bank_account_view;
CREATE VIEW minimal_child_bank_account_view AS
SELECT i.customer_id as customer_id, individual_id, guardian_NIC, first_name, last_name,
       date_of_birth,
       account_no, balance, opening_date,
       account_status, account_type
FROM minimal_child_and_guardian_view AS i
         JOIN minimal_bank_account_view AS acc ON i.customer_id = acc.customer_id;

drop function if exists check_individual_exists;
create
    function check_individual_exists(p_nic varchar(12)) returns int deterministic
BEGIN
    DECLARE count INT;
    SELECT COUNT(*) INTO count FROM minimal_individual_view WHERE NIC = p_nic;
    RETURN count;
END;