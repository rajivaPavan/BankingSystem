# Database Documentation

## Introduction

This document provides technical documentation for the database of the "Bank A" Banking System. It contains information about the database schema, data structures, and other technical details to assist developers, database-administrators, and other technical stakeholders in understanding and maintaining the database.

## 1. Database Overview

Provide a high-level description of the "Bank A" Banking System database, its purpose, and its role within the organization.

## 2. Database Schema

Explain the structure and organization of the database schema, including its components and relationships between them.

## 3. Enums

### Gender

- **Description**: Self-explanatory.
- **Values**:
  - `Male`: 0 
  - `Female`: 1 

### UserType

- **Description**: Types of users in the system.
- **Values**:
  - `Customer`: 0 
  - `Employee`: 1 
  - `Manager`: 2
  - `Admin`: 3

### CustomerType
- **Description**: Types of customers in the system.
- **Values**:
  - `Individual`: 0 
  - `Organization`: 1

### BankAccountType
- **Description**: Types of bank accounts.
- **Values**:
  - `Current`: 0 
  - `Savings`: 1 

### TransactionType
- **Description**: Types of transactions.
- **Values**:
  - `Deposit`: 0 
  - `Withdrawal`: 1 
  - `Interest`: 2
  - `Fee`: 3
  - `Other`: 4

### LoanApprovalStatus
- **Description**: Status of a loan application.
- **Values**:
  - `Pending`: 0 
  - `Approved`: 1 
  - `Rejected`: 2

### AccountStatus/CardStatus
- **Description**: Status of a bank account.
- **Values**:
  - `Active`: 0 
  - `Inactive`: 1 
  - `Frozen`: 3
  - `Closed`: 2

## 4. Tables

### Table 1

- **Description**: Description of Table 1.
- **Columns**:
  - `Column1`: Description of Column1.
  - `Column2`: Description of Column2.
  - ...

(Repeat this section for each table)

## 5. Views

### minimal_bank_account_view
- **Purpose:** This view provides a minimal representation of bank accounts, including account number, branch ID, customer ID, balance, opening date, account status, and account type.

### minimal_child_and_guardian_view
- **Purpose:** This view combines child and guardian information, including individual ID, customer ID, first name, last name, date of birth, guardian's NIC, guardian's first name, and guardian's date of birth.

### minimal_child_bank_account_view
- **Purpose:** This view combines child, guardian, and bank account information, including customer ID, individual ID, guardian's NIC, first name, last name, date of birth, account number, balance, opening date, account status, and account type.

### minimal_child_individual_view
- **Purpose:** This view represents individual children with limited information, including individual ID, customer ID, NIC, first name, last name, and date of birth. It filters individuals under 18 years old.

### minimal_individual_bank_account_view
- **Purpose:** This view combines individual and bank account information, including customer ID, individual ID, NIC, first name, last name, date of birth, account number, balance, opening date, account status, and account type.

### minimal_individual_view
- **Purpose:** This view represents individual customers who are not organization members and are over 18 years old. It includes individual ID, customer ID, NIC, first name, last name, and date of birth.

### organization_view_for_employee
- **Purpose:** This view provides an overview of organizations, including registration number, name, customer ID, type, address, company email, individual ID, position, first name, last name, and NIC of employees within the organization.


## 6. Stored Procedures and Triggers

### `add_new_individual`
- Adds a new individual to the system.

### `add_organization_individual`
- Adds an individual to an organization.

### `add_savings_account`
- Creates a new savings account, considering initial deposit requirements.

### `authenticate_user`
- Authenticates a user and retrieves user type and ID.

### `calculate_savings_interest`
- Calculates and records interest for active savings accounts.

### `check_individual_exists`
- Checks if an individual with a specific NIC exists.

### `check_organization_exists`
- Checks if an organization with a specific registration number exists.

### `create_organization_with_individual`
- Creates a new organization with an associated individual.

### `has_usertype`
- Checks if a user has a specified user type.

### `individual_exists_has_user_account`
- Checks if an individual with an associated bank account has a user account.

### `register_individual_user`
- Registers an individual user and associates them with a user account.

### `register_banker_user`
- Registers a banker user by adding a record to the user table and updating the employee table with the associated user ID. It allows the creation of user accounts for employees who are designated as bankers(employees/managers).

## 7. Indexes

### `user_name_index`
- **Purpose:** This index is created on the `user_name` column in the `user` table of the `bank_management_system` database. It is used to optimize and speed up queries that involve searching for users by their usernames.

## 8. Triggers

### `user_name_unique`
- **Purpose:** This trigger is designed to ensure the uniqueness of the `user_name` in the `user` table of the `bank_management_system` database. It runs before an insert operation and checks if a user with the same username already exists. If a duplicate `user_name` is detected, it raises an error.
- **Trigger Type:** Before Insert
- **Affected Table:** `user`
- **Event:** For Each Row
- **Error Handling:** If a duplicate `user_name` is found, the trigger signals a SQLSTATE '45000' error with the message 'user_name must be unique.'

### `check_bank_account_exists`
- **Purpose:** This trigger checks for the existence of a bank account before inserting a new record into the `bank_account` table. It ensures that a bank account with the same `customer_id`, `branch_id`, and `account_type` does not already exist. If a duplicate is detected, it raises an error.
- **Trigger Type:** Before Insert
- **Affected Table:** `bank_account`
- **Event:** For Each Row
- **Error Handling:** If a duplicate bank account is found, the trigger signals a SQLSTATE '45000' error with the message 'Bank account already exists.'

### `check_individual_exists_trigger`
- **Purpose:** This trigger is designed to prevent the insertion of duplicate individual records in the `individual` table. It checks for the existence of an individual with the same `customer_id` and `is_organization_member` set to false. If a duplicate is detected, it raises an error.
- **Trigger Type:** Before Insert
- **Affected Table:** `individual`
- **Event:** For Each Row
- **Error Handling:** If a duplicate individual is found, the trigger signals a SQLSTATE '45000' error with the message 'Individual already exists.'

## 9. Events

### `calculate_savings_interest`
- **Purpose:** This event is scheduled to run every day to calculate and record interest for active savings accounts. It calls the `calculate_savings_interest()` stored procedure, which performs interest calculations and inserts them into the transactions table.
- **Schedule:** The event is scheduled to run every day (`every 1 DAY`) and starts one day from the current date (`starts CURDATE() + INTERVAL 1 DAY`).
- **Enabled:** The event is enabled, meaning it will execute according to the defined schedule.

This event automates the process of calculating savings account interest on a daily basis, ensuring timely and accurate interest calculations for active accounts.

---

