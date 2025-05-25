using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using FirebaseAdmin.Auth;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var uid = decodedToken.Uid;

                // Attach UID to HttpContext for controllers to use
                context.Items["UserUID"] = uid;
            }
            catch
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Firebase token.");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing Firebase token.");
            return;
        }

        await _next(context);
    }
}
