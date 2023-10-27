ALTER TABLE bank_management_system.savings_account
    DROP FOREIGN KEY savings_account_ibfk_1;

ALTER TABLE bank_management_system.savings_account
    ADD CONSTRAINT savings_account_ibfk_1
        FOREIGN KEY (account_no)
            REFERENCES bank_management_system.bank_account (account_no)
            ON DELETE CASCADE;