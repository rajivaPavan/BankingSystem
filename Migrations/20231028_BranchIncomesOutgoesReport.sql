-- For view all the incomes (account deposits/fixed deposits/transfer from another branch account) in a Branch for a particular time period.
DROP VIEW IF EXISTS  income_report_view_for_employees;
CREATE VIEW income_report_view_for_employees AS
SELECT fd.savings_account_no AS 'account_number', ba.branch_id, fd.opening_date, fd.amount   -- FOR fixed deposits amounts
FROM fixed_deposit AS fd
JOIN bank_account AS ba ON fd.savings_account_no = ba.account_no
UNION ALL
SELECT tr.account_no, ba.branch_id, tr.time_stamp, tr.amount         -- FOR deposits of each acc. 
FROM transactions AS tr
JOIN bank_account AS ba ON tr.account_no = ba.account_no
WHERE tr.transaction_type = '0'   -- ****** PLEASE CHANGE THIS ACCORDING TO THE CONVENTION YOU USE ******
UNION ALL
SELECT tr.from_account_no, ba2.branch_id, tr.time_stamp, tr.amount   -- FOR getting money from another branch acc.  
FROM transfer tr
JOIN bank_account ba ON ba.account_no = tr.from_account_no
JOIN bank_account ba2 ON ba2.account_no = tr.to_account_no
WHERE ba.branch_id != ba2.branch_id;


-- For view all the outgoes (account withdraws/transfer to another branch account) in a Branch for a particular time period.
DROP VIEW IF EXISTS  outgo_report_view_for_employees;

CREATE VIEW outgo_report_view_for_employees AS
SELECT tr.account_no, ba.branch_id, tr.time_stamp, tr.amount    -- FOR withdraw of each acc.
FROM transactions AS tr
JOIN bank_account AS ba ON tr.account_no = ba.account_no
WHERE tr.transaction_type = '1'  -- ****** PLEASE CHANGE THIS ACCORDING TO THE CONVENTION YOU USE ******
UNION ALL
SELECT tr.to_account_no, ba.branch_id, tr.time_stamp, tr.amount     -- FOR transfer to another branch acc.
FROM transfer tr
JOIN bank_account ba ON ba.account_no = tr.from_account_no
JOIN bank_account ba2 ON ba2.account_no = tr.to_account_no
WHERE ba.branch_id != ba2.branch_id;