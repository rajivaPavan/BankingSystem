alter table savings_plan 
    add column age_lower_bound int not null default 0;

update savings_plan set age_lower_bound = 0 where savings_plan_id = 1;
update savings_plan set age_lower_bound = 13 where savings_plan_id = 2;
update savings_plan set age_lower_bound = 18 where savings_plan_id = 3;
update savings_plan set age_lower_bound = 60 where savings_plan_id = 4;

alter table savings_plan 
    add column age_upper_bound int not null default 0;

update savings_plan set age_upper_bound = 12 where savings_plan_id = 1;
update savings_plan set age_upper_bound = 17 where savings_plan_id = 2;
update savings_plan set age_upper_bound = 59 where savings_plan_id = 3;
update savings_plan set age_upper_bound = 200 where savings_plan_id = 4;
