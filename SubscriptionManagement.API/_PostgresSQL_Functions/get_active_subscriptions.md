CREATE OR REPLACE FUNCTION get_active_subscriptions() RETURNS TABLE(
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
        public."Subscriptions"
    WHERE
        StartDate <= current_date
        AND EndDate >= current_date;
END;
$$ LANGUAGE plpgsql;