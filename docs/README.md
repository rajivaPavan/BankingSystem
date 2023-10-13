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
Provide details about any views created in the database, including their purpose and parameters.

## 6. Stored Procedures and Triggers

Provide details about any stored procedures, triggers, or functions used in the database, including their purpose and parameters.

## 7. Indexes

List and describe any indexes created on the tables for performance optimization.

## 8. Security and Access Control

Explain who has access to the database and detail the permissions and access control mechanisms in place.


## 9. Backup and Recovery

Document the backup and recovery procedures for the database to ensure data safety.


## 10. Sample Queries

Provide sample SQL queries that demonstrate how to interact with the database.

---

