## Notes - Dated
*5-17-2025*
* Docker swarm doesnt need manual mounts set in docker-compose.
* Docker doesn't automatically read from .env files.
* Docker stack rm closes all services even if they're set to restart.
* Docker-compose application service(foodies-api-db) should not have an environment PASSWORD_FILE variable. Swarm's Secrets handles the password to connect to db by using the 'secrets:' key.

*5-18-2025*
* Making sure Ef migrations is pointed to foodies-api-db and using host port not port in container(to match env, variable to variable5-18-2025)
* To update(ef migrations) db to a db container make sure your connection string is set to the host's port (if it's not the standard port# for db's) and not to the container's port.
* You can set up EF migrations command to use a temporary connection string for a database container
* You can also set another derived DbContext class to use another connection string

*5-21-2025*
* DbContext classes
* Placeholders were given in my docker compose's environment variables, they weren't referencing anything nor was docker automatically reading from the .env file.
    * Ex: DB_NAME={DB_NAME}
* After adding another dbcontext derived class in the directory of dbcontext classes you have to run 'dotnet ef migrations add -c \<name of dbcontext class\>' to add it.

*5-22-2025*
* Docker containers can hold jwt key secrets within the app's container. Make the env variable in docker-compose's app service then reference it anywhere in the app ie Environment.GetVariable("JWT_SECRET_FILE").

*7-11-2025*
* HttpOnly cookies are only accessed by the backend. The browser handles it when it comes to the client making calls, so with every call the browser is giving the backend server the token for Authorization. 

*7-30-2025*
* When logging out, removing the cookie from the client isn't sufficient. Especially if it's an HttpOnly cookie (HttpOnly: true, in header) since Javascript cannot touch nor delete the HttpOnly cookies from the client. Also, since you can't remove the cookie, the client will still send the cookie with each request. So it's important to remove the cookie from the backend.
* When logging in, it's import to set 'Path' to "/" under the CookieOptions object. So that the browser uses that cookie for every endpoint. By default it may choose the route that is declaring the CookieOptions, i.e. "/api/login", which is incorrect. So remove the cookie with the same exact CookieOptions properties.