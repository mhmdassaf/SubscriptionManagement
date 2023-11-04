# SubscriptionManagement

Get Started:
 
1- Replace the current db connection string by your db connection
2- No need to apply the EF migration  manually since we added the auto-migrate feature on the application startup
2- 'get_active_subscriptions()', 'get_subscriptions_by_user', 'calculate_remaining_days' 
    postgresSQL functions added through EF migration (code-first approach) 
3- The Nuget package 'AssafTech.GenericRepository' was previously created by me to make it easier to work with data source.
   source code: https://github.com/mhmdassaf/AssafTech.GenericRepository 
4- DockerFile exists inside the solution.
5- 'Polly' Nuget package used to implementing a retry mechanism on 'GetActives' API.
6- The expectations are handled through the 'GlobalExceptionHandler' pipeline defined on the program class
7- 'AutoMapper' Nuget package used to make mapping between models, dtos..
8- 'Microsoft.AspNetCore.Identity' used for managing user authentication, authorization, and identity-related functionality.
9- "NLog" used as the logging framework