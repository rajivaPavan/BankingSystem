INSERT INTO `user`
VALUES (1, 'user1', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (2, 'user2', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (3, 'user3', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (4, 'user4', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (5, 'user5', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (6, 'user6', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (7, 'user7', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00'),
       (8, 'user8', '0B14D501A594442A01C6859541BCB3E8164D183D32937B851835442F69D5C94E', 0, '2017-07-22 18:30:00');


INSERT INTO `customer`
VALUES (11, 0),
       (12, 0),
       (13, 0),
       (14, 0),
       (15, 1),
       (16, 1);


INSERT INTO `individual`
VALUES (1, '123456789012', 'John', 'Doe', '1990-05-15', 11, 1, 'john@example.com', 0, '0772323111', '0112323111', 'abc'),
       (2, '123456781012', 'Mary', 'Doe', '1990-05-15', 12, 2, 'mary@example.com', 1, '0772323111', '0112323111', 'abc'),
       (3, '123456789023', 'Alice', 'Smith', '1990-05-15', 13, 3, 'alice@example.com', 1, '0772323111', '0112323111',
        'abc'),
       (4, '123456789123', 'Bob', 'Johnson', '1990-05-15', 14, 4, 'bob@example.com', 0, '0772323111', '0112323111',
        'abc');

INSERT INTO `organization` (`reg_no`, `name`, `customer_id`, `type`, `company_email`, `address`) VALUES ('1234567890', 'Organization 1', 15, 'Type 1', 'company1@example.com', 'Address 1');
INSERT INTO `organization` (`reg_no`, `name`, `customer_id`, `type`, `company_email`, `address`) VALUES ('0987654321', 'Organization 2', 16, 'Type 2', 'company2@example.com', 'Address 2');


INSERT INTO `guardian` (`NIC`, `first_name`, `last_name`, `date_of_birth`, `gender`) VALUES ('123456789012', 'Guardian 1', 'Doe', '1970-01-01', 1);
INSERT INTO `guardian` (`NIC`, `first_name`, `last_name`, `date_of_birth`, `gender`) VALUES ('123456781012', 'Guardian 2', 'Smith', '1975-05-05', 0);

INSERT INTO `organization_individual` (`individual_id`, `organization_reg_no`, `position`, `work_email`, `work_mobile_number`) VALUES (4, '1234567890', 'Position 1', 'work1@example.com', '1234567890');


INSERT INTO `branch` (`manager_employee_id`, `name`, `is_head_office`) VALUES (NULL, 'Branch 1', 1);
INSERT INTO `branch` (`manager_employee_id`, `name`, `is_head_office`) VALUES (NULL, 'Branch 2', 0);



INSERT INTO `employee` (`user_id`, `branch_id`, `first_name`, `last_name`, `date_of_birth`, `gender`, `NIC`, `email`) VALUES (5, 1, 'Employee 1', 'Doe', '1990-01-01', 1, '123456789012', 'employee1@example.com');
INSERT INTO `employee` (`user_id`, `branch_id`, `first_name`, `last_name`, `date_of_birth`, `gender`, `NIC`, `email`) VALUES (6, 2, 'Employee 2', 'Smith', '1995-05-05', 0, '987654321098', 'employee2@example.com');
INSERT INTO `employee` (`user_id`, `branch_id`, `first_name`, `last_name`, `date_of_birth`, `gender`, `NIC`, `email`) VALUES (7, 1, 'Employee 3', 'Johnson', '1960-02-02', 1, '123456789013', 'employee3@example.com');
INSERT INTO `employee` (`user_id`, `branch_id`, `first_name`, `last_name`, `date_of_birth`, `gender`, `NIC`, `email`) VALUES (8, 1, 'Employee 4', 'Brown', '1965-03-03', 0, '987654321099', 'employee4@example.com');


UPDATE `branch` SET `manager_employee_id` = 1 WHERE `name` = 'Branch 1' and `is_head_office`=1;
UPDATE `branch` SET `manager_employee_id` = 1 WHERE `name` = 'Branch 2' and `is_head_office`=0;


INSERT INTO `fd_plan` (`interest`, `duration`) VALUES (0.13, 6);
INSERT INTO `fd_plan` (`interest`, `duration`) VALUES (0.14, 12);
INSERT INTO `fd_plan` (`interest`, `duration`) VALUES (0.14, 12);
INSERT INTO `fd_plan` (`interest`, `duration`) VALUES (0.15, 36);

INSERT INTO `savings_plan` (`name`, `interest`, `minimum`, `max_withdrawals`) VALUES ('Savings Plan 1', 0.03, 1000, 5);
INSERT INTO `savings_plan` (`name`, `interest`, `minimum`, `max_withdrawals`) VALUES ('Savings Plan 2', 0.04, 5000, 3);
INSERT INTO `savings_plan` (`name`, `interest`, `minimum`, `max_withdrawals`) VALUES ('Savings Plan 3', 0.05, 2000, 4);
INSERT INTO `savings_plan` (`name`, `interest`, `minimum`, `max_withdrawals`) VALUES ('Savings Plan 4', 0.06, 6000, 2);

INSERT INTO `loan_plan` (`name`, `interest`) VALUES ('Loan Plan 1', 0.08);
INSERT INTO `loan_plan` (`name`, `interest`) VALUES ('Loan Plan 2', 0.1);
INSERT INTO `loan_plan` (`name`, `interest`) VALUES ('Loan Plan 3', 0.12);
INSERT INTO `loan_plan` (`name`, `interest`) VALUES ('Loan Plan 4', 0.15);

INSERT INTO `loan` (`customer_id`,`total_amount`, `duration`, `approval_status`, `loan_plan_id`, `is_online`, `purpose`) VALUES (11,3000.00, 12, 2, 1, false, 'Car Loan');
INSERT INTO `loan` (`customer_id`,`total_amount`, `duration`, `approval_status`, `loan_plan_id`, `is_online`, `purpose`) VALUES (12,7500.00, 36, 0, 2, true, 'Education Loan');
INSERT INTO `loan` (`customer_id`,`total_amount`, `duration`, `approval_status`, `loan_plan_id`, `is_online`, `purpose`) VALUES (13,10000.00, 48, 1, 3, false, 'Business Loan');
INSERT INTO `loan` (`customer_id`,`total_amount`, `duration`, `approval_status`, `loan_plan_id`, `is_online`, `purpose`) VALUES (14,15000.00, 60, 2, 4, true, 'Personal Loan');

INSERT INTO `bank_account` (`account_no`, `customer_id`, `branch_id`, `balance`, `opening_date`, `closing_date`, `account_status`, `account_type`) VALUES ('ACCT001', 11, 1, 5000.00, '2023-10-18', NULL, 1, 0);
INSERT INTO `bank_account` (`account_no`, `customer_id`, `branch_id`, `balance`, `opening_date`, `closing_date`, `account_status`, `account_type`) VALUES ('ACCT002', 12, 1, 7000.00, '2023-10-18', NULL, 1, 0);
INSERT INTO `bank_account` (`account_no`, `customer_id`, `branch_id`, `balance`, `opening_date`, `closing_date`, `account_status`, `account_type`) VALUES ('ACCT003', 13, 2, 3000.00, '2023-10-18', NULL, 1, 1);
INSERT INTO `bank_account` (`account_no`, `customer_id`, `branch_id`, `balance`, `opening_date`, `closing_date`, `account_status`, `account_type`) VALUES ('ACCT004', 14, 2, 10000.00, '2023-10-18', NULL, 1, 0);
INSERT INTO `bank_account` (`account_no`, `customer_id`, `branch_id`, `balance`, `opening_date`, `closing_date`, `account_status`, `account_type`) VALUES ('ACCT005', 15, 1, 1500.00, '2023-10-18', NULL, 1, 1);


INSERT INTO `savings_account` (`account_no`, `savings_plan_id`, `debit_card_number`, `next_calculation_on`) VALUES ('ACCT002', 2, NUll, '2023-12-18');
INSERT INTO `savings_account` (`account_no`, `savings_plan_id`, `debit_card_number`, `next_calculation_on`) VALUES ('ACCT003', 3, NUll, '2023-12-20');
INSERT INTO `savings_account` (`account_no`, `savings_plan_id`, `debit_card_number`, `next_calculation_on`) VALUES ('ACCT004', 4, NUll, '2023-12-22');
INSERT INTO `savings_account` (`account_no`, `savings_plan_id`, `debit_card_number`, `next_calculation_on`) VALUES ('ACCT005', 1, NUll, '2023-12-25');

INSERT INTO `debit_card` (`card_number`, `card_pin`, `security_code`, `date_of_expiry`, `name_in_card`, `savings_account_no`, `card_status`) VALUES ('2345678923456789', '1234', '123', '2024-12-31', 'John Doe', 'ACCT002', 1);
INSERT INTO `debit_card` (`card_number`, `card_pin`, `security_code`, `date_of_expiry`, `name_in_card`, `savings_account_no`, `card_status`) VALUES ('3456789034567890', '5678', '456', '2025-06-30', 'Jane Smith', 'ACCT003', 1);
INSERT INTO `debit_card` (`card_number`, `card_pin`, `security_code`, `date_of_expiry`, `name_in_card`, `savings_account_no`, `card_status`) VALUES ('5678901256789012', '9012', '789', '2024-09-30', 'Bob Johnson', 'ACCT005', 1);

UPDATE `savings_account` SET `debit_card_number` = '2345678923456789' WHERE `account_no` = 'ACCT002';
UPDATE `savings_account` SET `debit_card_number` = '3456789034567890' WHERE `account_no` = 'ACCT003';
UPDATE `savings_account` SET `debit_card_number` = '5678901256789012' WHERE `account_no` = 'ACCT005';

INSERT INTO `fixed_deposit` (`customer_id`, `fd_plan_id`, `savings_account_no`, `opening_date`, `amount`, `next_calculation_on`) VALUES (11, 1, 'ACCT002', '2023-10-18 09:00:00', 5000.00, '2023-12-15 09:00:00');
INSERT INTO `fixed_deposit` (`customer_id`, `fd_plan_id`, `savings_account_no`, `opening_date`, `amount`, `next_calculation_on`) VALUES (12, 2, 'ACCT003', '2023-10-18 09:00:00', 7000.00, '2023-12-18 09:00:00');
INSERT INTO `fixed_deposit` (`customer_id`, `fd_plan_id`, `savings_account_no`, `opening_date`, `amount`, `next_calculation_on`) VALUES (13, 2, 'ACCT004', '2023-10-18 09:00:00', 3000.00, '2023-12-20 09:00:00');
INSERT INTO `fixed_deposit` (`customer_id`, `fd_plan_id`, `savings_account_no`, `opening_date`, `amount`, `next_calculation_on`) VALUES (14, 3, 'ACCT005', '2023-10-18 09:00:00', 10000.00, '2023-12-22 09:00:00');

INSERT INTO `online_loan` (`binded_fd_no`) VALUES (1);
INSERT INTO `online_loan` (`binded_fd_no`) VALUES (2);

INSERT INTO `loan_installment` (`loan_id`, `due_date`, `is_paid`) VALUES (1, '2023-11-18', false);
INSERT INTO `loan_installment` (`loan_id`, `due_date`, `is_paid`) VALUES (2, '2023-11-10', false);

INSERT INTO `transfer` (`from_account_no`, `to_account_no`, `amount`, `time_stamp`, `reference`) VALUES ('ACCT001', 'ACCT002', 1000.00, '2023-10-18 09:00:00', 'Transfer for Bill Payment');
INSERT INTO `transfer` (`from_account_no`, `to_account_no`, `amount`, `time_stamp`, `reference`) VALUES ('ACCT003', 'ACCT004', 500.00, '2023-10-19 09:00:00', 'Transfer for Groceries');
INSERT INTO `transfer` (`from_account_no`, `to_account_no`, `amount`, `time_stamp`, `reference`) VALUES ('ACCT002', 'ACCT003', 2000.00, '2023-10-20 09:00:00', 'Transfer for Rent');
INSERT INTO `transfer` (`from_account_no`, `to_account_no`, `amount`, `time_stamp`, `reference`) VALUES ('ACCT004', 'ACCT001', 3000.00, '2023-10-21 09:00:00', 'Transfer for Loan Repayment');
INSERT INTO `transfer` (`from_account_no`, `to_account_no`, `amount`, `time_stamp`, `reference`) VALUES ('ACCT005', 'ACCT002', 800.00, '2023-10-22 09:00:00', 'Transfer for Entertainment');

INSERT INTO `transactions` (`account_no`, `amount`, `transaction_type`, `time_stamp`) VALUES ('ACCT001', 500.00, 1, '2023-10-18 10:00:00');
INSERT INTO `transactions` (`account_no`, `amount`, `transaction_type`, `time_stamp`) VALUES ('ACCT002', 700.00, 1, '2023-10-18 11:00:00');
INSERT INTO `transactions` (`account_no`, `amount`, `transaction_type`, `time_stamp`) VALUES ('ACCT003', 300.00, 1, '2023-10-18 12:00:00');
INSERT INTO `transactions` (`account_no`, `amount`, `transaction_type`, `time_stamp`) VALUES ('ACCT004', 1000.00, 1, '2023-10-18 13:00:00');
INSERT INTO `transactions` (`account_no`, `amount`, `transaction_type`, `time_stamp`) VALUES ('ACCT005', 150.00, 1, '2023-10-18 14:00:00');
