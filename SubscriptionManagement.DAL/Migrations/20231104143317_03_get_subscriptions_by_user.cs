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
	                StartDate timestamp with time zone,
	                EndDate timestamp with time zone
                ) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT
                        s.""Id"",
		                s.""UserId"",
		                s.""SubscriptionType"",
		                s.""StartDate"",
		                s.""EndDate""
                    FROM
                        public.""Subscriptions"" s
                    WHERE
                        s.""UserId"" = user_id;
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
