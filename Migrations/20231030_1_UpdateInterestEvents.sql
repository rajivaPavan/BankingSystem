drop event if exists calculate_savings_interest;
create event calculate_savings_interest on schedule
    every 1 DAY
        starts CURDATE() + INTERVAL 1 DAY
    enable
    do
    CALL calculate_savings_interest();

