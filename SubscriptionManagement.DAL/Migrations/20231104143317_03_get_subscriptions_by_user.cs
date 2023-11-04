using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionManagement.DAL.Migrations
{
    public partial class _03_get_subscriptions_by_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION get_subscriptions_by_user(user_id text)
                RETURNS TABLE (
                    Id uuid,
	                UserId text,
	                SubscriptionType text,
	                StartDate date,
	                EndDate date
                ) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT
                        Id,
		                UserId,
		                SubscriptionType,
		                StartDate,
		                EndDate
                    FROM
                        public.""Subscriptions""
                    WHERE
                        UserId = user_id;
                END;
                $$ LANGUAGE plpgsql;
                ");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_subscriptions_by_user");
		}
    }
}
