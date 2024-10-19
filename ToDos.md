1. Add ORM to endpoint so it can save the external Business model to foodiesapidb.
2. Split that endpoint into service and repository.
3. Move business response model into services to fix object reference cycle issues.
4. AddUserLIkeBusinessEndpoint has two database operations happening, maybe the cause of the message "System.InvalidOperationException: A second operation was started on this context instance before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext"
5. Check the result of FindAsync in User's Delete call, FindAsync may never return non null results