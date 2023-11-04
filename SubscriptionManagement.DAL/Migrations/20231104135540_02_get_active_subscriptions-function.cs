using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionManagement.DAL.Migrations
{
    public partial class _02_get_active_subscriptionsfunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION get_active_subscriptions() RETURNS TABLE(
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
                    StartDate <= current_date
                    AND EndDate >= current_date;
            END;
            $$ LANGUAGE plpgsql;");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_active_subscriptions");
		}
    }
}
