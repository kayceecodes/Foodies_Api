1.X Add ORM to endpoint so it can save the external Business model to foodiesapidb.
2.X Split that endpoint into service and repository.
3. Move business response model into services to fix object reference cycle issues.
    - Moved business response model already, now I need to see if I 
4.X AddUserLIkeBusinessEndpoint has two database operations happening, maybe the cause of the message "System.InvalidOperationException: A second operation was started on this context instance before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext".
5.X Check the result of FindAsync in User's Delete call, FindAsync may never return non null results.
    - resolved: using findAsync does return null (but can be the wrong method in context) use FindFirst, Any, etc. instead
6. Navigation properties are returning null. Make sure endpoints are returning DTOs or Response objects instead.
(Note) - AddUserLikes, GetUserLikes (by id) and Delete UserLikes works.