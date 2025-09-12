namespace foodies_api.Utils;

public static class ContainerSecrets
{

    public async static Task<Secrets> ReadSecrets(IConfiguration configuration)
    {
        var secrets = new Secrets();

        var dbConnFile = configuration["DB_CONNECTION_FILE"];
        if (string.IsNullOrEmpty(dbConnFile) || !File.Exists(dbConnFile))
            throw new Exception("DB_CONNECTION_FILE is not set or file not found");

        secrets.ConnectionString = (await File.ReadAllTextAsync(dbConnFile)).Trim();

        var jwtKeySecretFile = configuration["JWT_KEY_SECRET_FILE"];
        if (string.IsNullOrEmpty(jwtKeySecretFile) || !File.Exists(jwtKeySecretFile))
            throw new Exception("JWT_KEY_SECRET_FILE is not set or file not found");

        secrets.JwtKey = (await File.ReadAllTextAsync(jwtKeySecretFile)).Trim();

        return secrets;
    }

    public static void ValidateAndReadSecret(string filePath, Action<string> setSecret)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new InvalidOperationException("File path is empty has not been set!");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Secret file was not found at {filePath}!");

        var content = File.ReadAllText(filePath).Trim();
        if (string.IsNullOrEmpty(content))
            throw new InvalidOperationException($"Secret file is empty: {filePath}");
            
        setSecret(content);
    }
}

public class Secrets
{
    public string JwtKey = "";
    public string ConnectionString = "";
}