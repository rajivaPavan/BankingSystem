create database bank_management_system;

use bank_management_system;

CREATE TABLE `customer` (
                            `id` int not null auto_increment,
                            `type` tinyint,
                            PRIMARY KEY (`id`)
);

CREATE TABLE `organization` (
                                `reg_no` varchar(20) NOT NULL,
                                `name` varchar(50) NOT NULL,
                                `customer_id` int NOT NULL,
                                `type` varchar(20) NOT NULL,
                                `company_email` varchar(255),
                                `address` varchar(255),
                                PRIMARY KEY (`reg_no`),
                                FOREIGN KEY (`customer_id`) REFERENCES `customer`(`id`)
);

CREATE TABLE `user` (
                        `user_id` int NOT NULL auto_increment,
                        `user_name` varchar(50) NOT NULL,
                        `password_hash` varchar(64) NOT NULL,
                        `user_type` tinyint NOT NULL,
                        `last_login_timestamp` timestamp,
                        PRIMARY KEY (`user_id`)
);

CREATE TABLE `individual` (
                              `individual_id` int NOT NULL auto_increment,
                              `NIC` varchar(12) NOT NULL,
                              `first_name` varchar(50) NOT NULL,
                              `last_name` varchar(50) NOT NULL,
                              `date_of_birth` date NOT NULL,
                              `customer_id` int,
                              `user_id` int,
                              `email` varchar(255),
                              `gender` boolean NOT NULL,
                              `mobile_number` varchar(20),
                              `home_number` varchar(20),
                              `address` varchar(255),
                              PRIMARY KEY (`individual_id`),
                              FOREIGN KEY (`customer_id`) REFERENCES `customer`(`id`),
                              FOREIGN KEY (`user_id`) REFERENCES `user`(`user_id`)
);

CREATE INDEX `individual_NIC` ON `individual` (`NIC`);

CREATE TABLE `guardian` (
                            `NIC` varchar(12),
                            `first_name` varchar(50),
                            `last_name` varchar(50),
                            `date_of_birth` date,
                            `gender` bool,
                            PRIMARY KEY (`NIC`),
                            FOREIGN KEY (`NIC`) REFERENCES `individual`(`NIC`)
);

CREATE TABLE `organization_individual` (
                                           `individual_id` int NOT NULL,
                                           `organization_reg_no` varchar(50) NOT NULL,
                                           `position` varchar(50),
                                           `work_email` varchar(255),
                                           `work_mobile_number` varchar(20),
                                           PRIMARY KEY (`individual_id`, `organization_reg_no`),
                                           FOREIGN KEY (`organization_reg_no`) REFERENCES `organization`(`reg_no`),
                                           FOREIGN KEY (`individual_id`) REFERENCES `individual`(`individual_id`)
);

CREATE TABLE `branch` (
                          `branch_id` int NOT NULL auto_increment,
                          `manager_employee_id` int,
                          `name` varchar(50),
                          `is_head_office` boolean,
                          PRIMARY KEY (`branch_id`)
);

CREATE TABLE `employee` (
                            `employee_id` int NOT NULL auto_increment,
                            `user_id` int NOT NULL,
                            `branch_id` int,
                            `first_name` varchar(50) NOT NULL,
                            `last_name` varchar(50) NOT NULL,
                            `date_of_birth` date NOT NULL,
                            `gender` boolean NOT NULL,
                            `NIC` varchar(12) NOT NULL,
                            `email` varchar(255) NOT NULL,
                            PRIMARY KEY (`employee_id`),
                            FOREIGN KEY (`branch_id`) REFERENCES `branch`(`branch_id`),
                            FOREIGN KEY (`user_id`) REFERENCES `user`(`user_id`)
);

ALTER TABLE `branch`
    ADD FOREIGN KEY (`manager_employee_id`) REFERENCES `employee`(`employee_id`);

CREATE TABLE `fd_plan` (
                           `fd_plan_id` int NOT NULL auto_increment,
                           `interest` float(2,2) NOT NULL,
                           `duration` tinyint NOT NULL,
                           PRIMARY KEY (`fd_plan_id`)
);

CREATE TABLE `savings_plan` (
                                `savings_plan_id` int NOT NULL auto_increment,
                                `name` varchar(50) NOT NULL,
                                `interest` float(2,2) NOT NULL,
                                `minimum` smallint NOT NULL,
                                `max_withdrawals` tinyint NOT NULL,
                                PRIMARY KEY (`savings_plan_id`)
);

CREATE TABLE `loan_plan` (
                             `loan_plan_id` int NOT NULL auto_increment,
                             `name` varchar(50) NOT NULL,
                             `interest` float(2,2) NOT NULL,
                             PRIMARY KEY (`loan_plan_id`)
);

CREATE TABLE `bank_account` (
                                `account_no` varchar(16) NOT NULL,
                                `customer_id` int NOT NULL,
                                `branch_id` int NOT NULL,
                                `balance` decimal(10,2) NOT NULL,
                                `opening_date` date NOT NULL,
                                `closing_date` date,
                                `account_status` tinyint(1) NOT NULL,
                                `account_type` tinyint(1) NOT NULL,
                                PRIMARY KEY (`account_no`),
                                FOREIGN KEY (`customer_id`) REFERENCES `customer`(`id`),
                                FOREIGN KEY (`branch_id`) REFERENCES `branch`(`branch_id`)
);

CREATE TABLE `savings_account` (
                                   `account_no` varchar(16) NOT NULL,
                                   `savings_plan_id` int NOT NULL,
                                   `debit_card_number` varchar(16),
                                   `next_calculation_on` date,
                                   PRIMARY KEY (`account_no`),
                                   FOREIGN KEY (`account_no`) REFERENCES `bank_account`(`account_no`),
                                   FOREIGN KEY (`savings_plan_id`) REFERENCES `savings_plan`(`savings_plan_id`)
);

CREATE TABLE `fixed_deposit` (
                                 `fd_no` int NOT NULL auto_increment,
                                 `customer_id` int NOT NULL,
                                 `fd_plan_id` int NOT NULL,
                                 `savings_account_no` varchar(16) NOT NULL,
                                 `opening_date` timestamp,
                                 `amount` decimal(10,2),
                                 `next_calculation_on` timestamp,
                                 PRIMARY KEY (`fd_no`),
                                 FOREIGN KEY (`customer_id`) REFERENCES `customer`(`id`),
                                 FOREIGN KEY (`fd_plan_id`) REFERENCES `fd_plan`(`fd_plan_id`),
                                 FOREIGN KEY (`savings_account_no`) REFERENCES `savings_account`(`account_no`)
);

CREATE TABLE `debit_card` (
                              `card_number` varchar(16) NOT NULL,
                              `card_pin` char(4) NOT NULL,
                              `security_code` char(3) NOT NULL,
                              `date_of_expiry` date,
                              `name_in_card` varchar(50),
                              `savings_account_no` varchar(16),
                              `card_status` tinyint,
                              PRIMARY KEY (`card_number`, `card_pin`, `security_code`),
                              FOREIGN KEY (`savings_account_no`) REFERENCES `savings_account`(`account_no`)
);

ALTER TABLE `savings_account`
    ADD FOREIGN KEY (`debit_card_number`) REFERENCES `debit_card`(`card_number`);

CREATE TABLE `loan` (
                        `loan_id` int NOT NULL auto_increment,
                        `customer_id` int NOT NULL,
                        `total_amount` decimal(10,2) NOT NULL,
                        `duration` smallint NOT NULL,
                        `approval_status` int NOT NULL,
                        `loan_plan_id` int NOT NULL,
                        `is_online` bool NOT NULL,
                        `purpose` text,
                        PRIMARY KEY (`loan_id`),
                        FOREIGN KEY (`loan_plan_id`) REFERENCES `loan_plan`(`loan_plan_id`),
                            FOREIGN KEY(`customer_id`) REFERENCES `customer`(`id`)
);

CREATE TABLE `online_loan` (
                               `loan_id` int NOT NULL auto_increment,
                               `binded_fd_no` int NOT NULL,
                               PRIMARY KEY (`loan_id`),
                               FOREIGN KEY (`binded_fd_no`) REFERENCES `fixed_deposit`(`fd_no`)
);

CREATE TABLE `loan_installment` (
                                    `loan_id` int NOT NULL,
                                    `due_date` date NOT NULL,
                                    `is_paid` boolean DEFAULT false,
                                    PRIMARY KEY (`loan_id`, `due_date`),
                                    FOREIGN KEY (`loan_id`) REFERENCES `loan`(`loan_id`)
);

CREATE TABLE `transfer` (
                            `transfer_id` int NOT NULL auto_increment,
                            `from_account_no` varchar(16) NOT NULL,
                            `to_account_no` varchar(16) NOT NULL,
                            `amount` decimal(10,2) NOT NULL,
                            `time_stamp` timestamp NOT NULL,
                            `reference` varchar(100) NOT NULL,
                            PRIMARY KEY (`transfer_id`),
                            FOREIGN KEY (`to_account_no`) REFERENCES `bank_account`(`account_no`),
                            FOREIGN KEY (`from_account_no`) REFERENCES `bank_account`(`account_no`)
);

CREATE TABLE `transactions` (
                                `transaction_id` int NOT NULL auto_increment,
                                `account_no` varchar(16) NOT NULL,
                                `amount` decimal(10,2) NOT NULL,
                                `transaction_type` tinyint NOT NULL,
                                `time_stamp` timestamp NOT NULL,
                                PRIMARY KEY (`transaction_id`),
                                FOREIGN KEY (`account_no`) REFERENCES `bank_account`(`account_no`)
);


-- Missing foreign key
ALTER TABLE `online_loan`
    ADD FOREIGN KEY (`loan_id`) REFERENCES `loan` (`loan_id`);

-- Constraints
ALTER TABLE `customer`
    ADD CONSTRAINT `customer_type_check` CHECK (`type` IN (0, 1));

ALTER TABLE `individual`
    ADD CONSTRAINT `individual_email_check` CHECK (`email` REGEXP '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$');


ALTER TABLE `organization_individual`
    ADD CONSTRAINT `work_email_check` CHECK (`work_email` REGEXP '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$');

ALTER TABLE `employee`
    ADD CONSTRAINT `employee_email_check` CHECK (`email` REGEXP '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$');

ALTER TABLE `fd_plan`
    ADD CONSTRAINT `fd_plan_interest_check` CHECK (`interest` >= 0 AND `interest` <= 100);

ALTER TABLE `fd_plan`
    ADD CONSTRAINT `fd_plan_duration_check` CHECK (`duration` >= 0);


ALTER TABLE `savings_plan`
    ADD CONSTRAINT `savings_plan_interest_check` CHECK (`interest` >= 0 AND `interest` <= 100),
    ADD CONSTRAINT `savings_plan_min_check` CHECK (`minimum` >= 0),
    ADD CONSTRAINT `savings_plan_max_withdrawals_check` CHECK (`max_withdrawals` >= 0);

ALTER TABLE `loan_plan`
    ADD CONSTRAINT `loan_plan_interest_check` CHECK (`interest` >= 0 AND `interest` <= 100);


ALTER TABLE `bank_account`
    ADD CONSTRAINT `bank_account_account_type_check` CHECK (`account_type` IN (0, 1)),
    ADD CONSTRAINT `bank_account_account_status_check` CHECK (`account_status` IN (0, 1, 2, 3)),
    ADD CONSTRAINT `bank_account_balance_check` CHECK (`balance` >= 0),
    ADD CONSTRAINT `bank_account_opening_date_check` CHECK (`opening_date` <= closing_date),
    ADD CONSTRAINT `bank_account_closing_date_check` CHECK (`closing_date` IS NULL OR `closing_date` >= opening_date);

ALTER TABLE `fixed_deposit`
    ADD CONSTRAINT `fixed_deposit_amount_check` CHECK (`amount` >= 0),
    ADD CONSTRAINT `fixed_deposit_opening_date_check` CHECK (`opening_date` <= next_calculation_on),
    ADD CONSTRAINT `fixed_deposit_next_calculation_on_check` CHECK (`next_calculation_on` >= opening_date);

ALTER TABLE `debit_card`
    ADD CONSTRAINT `debit_card_card_status_check` CHECK (`card_status` IN (0, 1, 2, 3));

ALTER TABLE `loan`
    ADD CONSTRAINT `loan_total_amount_check` CHECK (`total_amount` >= 0),
    ADD CONSTRAINT `loan_duration_check` CHECK (`duration` >= 0),
    ADD CONSTRAINT `loan_approval_status_check` CHECK (`approval_status` IN (0, 1, 2));

ALTER TABLE `transfer`
    ADD CONSTRAINT `transfer_amount_check` CHECK (`amount` >= 0);

ALTER TABLE `transactions`
    ADD CONSTRAINT `transactions_amount_check` CHECK (`amount` >= 0),
    ADD CONSTRAINT `transactions_transaction_type_check` CHECK (`transaction_type` IN (0, 1, 2, 3, 4));
    
