# foodies_api
5-17-2025
Docker swarm doesnt need manual mounts set in docker-compose.
Docker doesn't automatically read from .env files.
Docker stack rm closes all services even if they're set to restart.
Docker-compose application service(foodies-api-db) should not have an environment PASSWORD_FILE variable. Swarm's Secrets handles the password to connect to db by using the 'secrets:' key.
5-18-2025
Making sure Ef migrations is pointed to foodies-api-db and using host port not port in container(to match env, variable to variable5-18-2025)