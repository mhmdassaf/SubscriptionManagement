using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionManagement.DAL.Migrations
{
    public partial class _04_calculate_remaining_days : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION calculate_remaining_days(subscription_id uuid)
            RETURNS integer AS $$
            DECLARE
                subscription_start date;
                subscription_end date;
                remaining_days integer;
            BEGIN
                -- Retrieve the start and end dates of the subscription using the provided ID
                SELECT
                    s.""StartDate"" date,
	                s.""EndDate"" date
                INTO
                    subscription_start,
                    subscription_end
                FROM
                    public.""Subscriptions"" s
                WHERE
                    s.""Id"" = subscription_id;

                -- Check if the subscription was found
                IF subscription_start IS NULL OR subscription_end IS NULL THEN
                    -- Handle the case where the subscription is not found or the dates are missing
                    RETURN NULL;
                END IF;

                -- Calculate the remaining days by subtracting the current date from the end date
                remaining_days := (subscription_end - current_date)::integer;

                -- Ensure the result is non-negative
                IF remaining_days < 0 THEN
                    remaining_days := 0;
                END IF;

                RETURN remaining_days;
            END;
            $$ LANGUAGE plpgsql;");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP FUNCTION IF EXISTS calculate_remaining_days");
		}
    }
}
