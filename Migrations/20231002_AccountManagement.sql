# Create tables for Customer
create table if not exists customer
(
    CustomerId int auto_increment
        primary key,
    Type       varchar(50) not null # Enum - Individual or Organization
);

# Create tables for individual and guardian
create table if not exists individual
(
    IndividualId int auto_increment primary key,
    NIC         varchar(12),
    FirstName    varchar(50) not null,
    LastName     varchar(50) not null,
    DateOfBirth  DATETIME not null,
    CustomerId   int not null,
    UserId       int,
    Gender       varchar(10) not null, # Enum - Male, Female
    Address     varchar(100) not null,
    GuardianId  int,
    FOREIGN KEY (CustomerId) REFERENCES customer (CustomerId),
    FOREIGN KEY (UserId) REFERENCES user (UserId)
);

create table if not exists guardian
(
    GuardianId int auto_increment primary key,
    FirstName  varchar(50) not null,
    LastName   varchar(50) not null,
    NIC        varchar(12) not null,
    DateOfBirth DATETIME not null,
    Gender    varchar(10) not null, # Enum - Male, Female
    ChildId   int not null,
    FOREIGN KEY (ChildId) REFERENCES individual (IndividualId)
);

# Add foreign key to individual table for GuardianId
ALTER TABLE individual
ADD FOREIGN KEY (GuardianId) REFERENCES guardian (GuardianId);

# Create tables for organization
create table if not exists organization
(
    RegNo int auto_increment primary key,
    Name           varchar(50) not null,
    CustomerId     int not null,
    Type           varchar(50) not null, # Enum - Private, Public, NGO
    CompanyEmail   varchar(50) not null,
    Address        varchar(100) not null,
    MainOrganizationIndividual varchar(12) not null
);

create table if not exists organization_individual
(
    NIC            varchar(12) not null,
    OrganizationRegNo int not null,
    PRIMARY KEY (NIC, OrganizationRegNo),
    UserId         int not null,
    FirstName      varchar(50) not null,
    LastName       varchar(50) not null,
    Position       varchar(50) not null,
    Gender         varchar(10) not null, # Enum - Male, Female
    DateOfBirth    DATETIME not null,
    Address        varchar(100) not null,
    FOREIGN KEY (OrganizationRegNo) REFERENCES organization (RegNo),
    FOREIGN KEY (UserId) REFERENCES user (UserId)
);

# Add foreign key to main organization individual to organization table
ALTER TABLE organization 
ADD FOREIGN KEY (MainOrganizationIndividual) REFERENCES organization_individual (NIC);

# Add tables to handle phone numbers
create table if not exists phone_number
(
    PhoneNumberId int auto_increment primary key,
    PhoneNumber   varchar(12) not null,
    PhoneNumberType varchar(15) not null, # Enum - Mobile, Home, Office
    UserId    int not null,
    FOREIGN KEY (UserId) REFERENCES user (UserId)
);

create table if not exists individual_phone_number
(
    IndividualId int not null,
    PhoneNumberId int not null,
    PRIMARY KEY (IndividualId, PhoneNumberId),
    FOREIGN KEY (IndividualId) REFERENCES individual (IndividualId),
    FOREIGN KEY (PhoneNumberId) REFERENCES phone_number (PhoneNumberId)
);

# Fix: Date of Birth should be of Date type
ALTER TABLE individual
MODIFY COLUMN DateOfBirth DATE not null;
ALTER TABLE guardian
MODIFY COLUMN DateOfBirth DATE not null;
ALTER TABLE organization_individual
MODIFY COLUMN DateOfBirth DATE not null;



