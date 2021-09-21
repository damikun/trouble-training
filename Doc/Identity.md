

# Table of contents

- [Identity and security overview](#identity-and-security-overview)
    - [Authenticatioton vs Authorization](#authenticatioton-vs-authorization)
    - [Tokens vs Cookie and Sessions](#tokens-vs-cookie-and-sessions)
        - [Cookies](#cookies)
            - [What is origin?](#what-is-origin)
            - [Another cookie definitions:](#another-cookie-definitions)
        - [Sessions](#sessions)
        - [Tokens](#tokens)
            - [JWT Tokens](#jwt-tokens)
    - [Identity protocols](#identity-protocols)
        - [OAuth](#oauth)
            - [Token exchange flows](#token-exchange-flows)
        - [OpenID Connect](#openid-connect)
    - [Backend for Frontend pattern (BFF)](#backend-for-frontend-pattern-bff)
        - [Security reasons](#security-reasons)
        - [Architectural reasons](#architectural-reasons)
            - [BFF vs API gatway](#bff-vs-api-gatway)
        - [BBF cookies termination and token isolation](#bbf-cookies-termination-and-token-isolation)
- [Implementing indentity in .NetCore](#implementing-indentity-in-netcore)
    - [Duende IndentityServer](#duende-indentityserver)
    - [Demo Microservices overview](#demo-microservices-overview)
        - [IdentityServer project configuration](#identityserver-project-configuration)
            - [Nuget Packages:](#nuget-packages)
            - [ConfigureServices](#configureservices)
                - [AddCorsConfiguration](#addcorsconfiguration)
                - [AddAppIdentityDbContext](#addappidentitydbcontext)
                - [AddIdentityServer](#addidentityserver)
                - [Data Stores](#data-stores)
                - [User object:](#user-object)
            - [Dummy initial data](#dummy-initial-data)
            - [Setup migrations](#setup-migrations)
                - [Creating migrations](#creating-migrations)
                - [Apply existing migrations](#apply-existing-migrations)
            - [Running identiyserver project](#running-identiyserver-project)
        - [BFF project configuration](#bff-project-configuration)
            - [Nuget Packages:](#nuget-packages)
            - [ConfigureServices](#configureservices)
                - [AddIdentityConfiguration](#addidentityconfiguration)
                - [AddBff configuration](#addbff-configuration)
        - [API project configuration](#api-project-configuration)
            - [Nuget Packages:](#nuget-packages)
            - [ConfigureServices](#configureservices)
    - [Running microservices](#running-microservices)
                - [Idnetityserver](#idnetityserver)
                - [BFF](#bff)
                - [APIServer](#apiserver)
        - [Idnetity server UI interface:](#idnetity-server-ui-interface)

## Identity and security overview

![Idnetity server with BFF flow](./Assets/identity_server_sketch_flow.png "Idnetity server with BFF flow")

When it comes to security. There are many tutorials out there and also notes and comments why use and dont use each approach. It is hard to understand all different options and security issues they have. 

Also you have different clinets (Mobile/Web/App) and each presents different security requiremns for managing, storing and processing data as tokens, cookies or sessions.

At the end your App can scale and not all options are able to be deployed as multy instances nodes without aditional changes in app and infrastructure.

This means you realy need to make decesions at the beginning and ask question like: *what are your clinets?* and *what are your requirements?* and *what can be in future added to system?* and *how it is possible to handle with selected approach?* etc.. etc..

### Authenticatioton vs Authorization

 - `Authenticatioton` - Process of verifying who a user is (identity)
 - `Authorization` -  Process of verifying what user have access to (permissions check to access the specific resource)

### Tokens vs Cookie and Sessions

Tokens,Cookies and Sessions can be interpreted as resources or tools that are used by various protocols/standards/patterns (OAuth,OpenID, BFF) to handle identity related tasks.

It is important to understand basic because at the end they are used in combination regarding to enviroment, used clients and security level.

At the end you can wrap JWT token inside Cookie and use that with session data for `Authorization` and `Authenticatioton`.


#### Cookies

Cookies can be interpreted as small blocks of data created by server written once to response and always being automaicaly resended with aditional request by Web-Browser until thier lifetime expire.

They can be used for Authentication/Authorization or for tracking and marketing reasons. And many more...

*IndentityServer cookies example:*
![IdnetityServer Cookies in web browsers](./Assets/cookies_webBrowser.png "IdnetityServer Cookies in web browsers")

There are 3 main cookies properties:
- `HttpOnly` - An http-only cookie cannot be accessed by client-side APIs, such as JavaScript. Browser will not allow you to acces this cookie from frontend code.
- `Secure` A secure cookie can only be transmitted over an encrypted connection HTTPS.
- `SameSite` 3 option values `Strict`, `Lax` or `None` this tells browser to what domain and oringing can be cookie sended.

##### What is origin?

Origin is usually ip and port or domain name and subdomain name.

```
// This are different origins since subdomain are different
https://developer.mozilla.org
https://mozilla.org

// This are also different origins since port number is different
https://localhost:5001
https://localhost:7001
```
 
##### Another cookie definitions:
- `Session cookie` - Created only for browser session (in-memory) and is deleted/lost after close.
- `Third-party cookie` - Normally, a cookie's domain attribute will match the domain that is shown in the web browser's address bar. as `first-party cookie`. The `Third-party cookie` does not match current domain and are used as  `Tracking cookie` to track user activity.

#### Sessions

Session is used to temporarily store the information on the server to be used across multiple pages of the website. It is commonly connected with cookie which is used to identify the session stored on server but does not contain data.

![Session diagram](./Assets/session_diagram.png "Session diagram")


#### Tokens

Tokens are pieces of data  that allow application systems to perform the authorization and authentication process. Usualy they are encoded as base64 strings.

There are multiple types of tokens:
- `access token` - Wraps user claims and sign it by secreat. it use JWT Tokens.
- `refresh token` - Are used to *"Refresh"* and obtain new `access token` after its lifetime expire
- `id token` - json encoded data about user profile info
- etc, etc...

##### JWT Tokens

JSON Web Token is an open standard that defines way how to securely transmit information between parties as a JSON object. 

They are used for `Authorization` and `Information Exchange` because they provide security sign proof that information wrapped inside are valid and was written by trusted source.

You can easily write any data inside token, sign that data and than use it by clients to access the server resources. Server can validate if token was signed and is still valid. 


**Basic JWT token flow example:**

</br>

![Auth diagram JWT token](./Assets/jwt_token_auth_process_basic.png "Auth diagram JWT token")

**JWT content:**

![JWT token](./Assets/jwt_token.png "JWT Token")

**JWT consist of 3 parts:**

- `Header` - Holds information as type of the token (JWT) and the signing algorithm being used, such as HMAC SHA256 or RSA.
    </br>
    ```json
    {
    "alg": "HS256",
    "typ": "JWT"
    }
    ```
- `Payload` - Securly signed data (claims)
    </br>
    ```json
    {
    "sub": "1234567890",
    "name": "John Doe",
    "admin": true
    }
    ```
- `Signature` - Encoded header, the encoded payload, a secret and siged by algorithm specified in the header.
    </br>
    ```js
    HMACSHA256(
    base64UrlEncode(header) + "." +
    base64UrlEncode(payload),
    secret
    ```

    You can read more anitiona linfo about JWT tokens under [official documentation](https://jwt.io/introduction).

    </br>
    
    The most common used token for `Authorizing` acces to APIs is `Bearer` token.

    </br>

    

### Identity protocols

There are multiple protocols/specifications available that allows you to manage your identity or authorization process.

This was needed to standardize authentication and autorization between services and clients. This allows us to use different global idetity/auth providers Like Facebook, Google (externally/intenally) and also standardied way how the flow is implemented.

![Oauth and OpenId logo](./Assets/openid_oauth_logo.jpg "Oauth and OpenId logo")


This demo focuse on the most used `OAuth` and `OpenID Connect` protocols.

Both protocols uses by default JWT token to encript and sign some sensitiv data or validate that request was sended from trusted source. It is allso possible to use cookies on front and let backend to handle session and token autorization for you.

You can also watch and learn differents from this talks:
- [OAuth 2.0 and OpenID Connect - [Nate Barbettini]](https://www.youtube.com/watch?v=996OiexHze0)
- [Introduction to OAuth 2.0 and OpenID Connect - [Philippe De Ryck]](https://www.youtube.com/watch?v=GyCL8AJUhww)

#### OAuth
Is used primarly to authorize acces of som App to specific resource. This is done without providing your pasword to external sides.

![Oauth slack grant promt example](./Assets/oAuth_slack_promt_example.png "Oauth slack grant promt example")

If you’ve ever signed up to a new application and agreed to let it automatically access your contacts, calendar etc.. then you’ve used *OAuth 2.0*. This protocol does not provide information about endpoint user just provide token to access specific resources. You can read more about [OAuth under this Docs.](https://auth0.com/docs/authorization/protocols/protocol-oauth2) 

![Oauth is authorization not authentication](./Assets/oauth_is_authorization.png "Oauth is authorization not authentication")


OAuth generally provides clients a *"secure delegated access"* to specific resource. Imagine you are google user and some app wanna access your calendar data. This is displayed in this example:

`OAuth` flow example:
![Oauth flow explain](./Assets/oauth_flow_explain.png "Oauth flow explain")

In above example app as Slack, Jira etc.. will just get authorization to get acces to specific resource (in example calendar) but not to user itself so profile data as username, email are not transferend and keeps protected.

If you wanna lear nore about OAuth you can watch folowing talks:
- [Introduction to OAuth 2.0 Flow - [Philippe De Ryck]](https://youtu.be/GyCL8AJUhww?t=655)

##### Token exchange flows

There are several ways how can be `grant token` passed to client. Option depens on what kind of client is requesting acces and how much is this client trusted.

- Authorization Code Flow
- Authorization Code Flow with PKCE
- Implicit Flow

![Oauth grant flow](./Assets/oauth_grant_flowchart.png "Oauth grant flow")

#### OpenID Connect

OpenID is a protocol used for for decentralized authentication

One login used by multiple internal/external applications. If you’ve used your Google or Facebook etc. to sign-in to exteranl web or app, then you’ve used *OpenID Connect`*.

OpenID Connect is built on the *OAuth 2.0.* (OAuth is underlying protocol and OpenId is identity layer on it) and additionaly it uses jwt token called `id_token` which encapsulates the identity claims in JSON format. You can find more info about OpenId [under this specification](https://openid.net/connect/).

`id_token` example:


```json
{
"iss": "http://server.example.com",
"sub": "248289761001",
"aud": "s6BhdRkqt3",
"nonce": "n-0S6_WzA2Mj",
"exp": 1311281970,
"iat": 1311280970,
"name": "Dalibor Kundrat",
"given_name": "Dalibor",
"family_name": "Kundrat",
"gender": "male",
"birthdate": "0000-10-31",
"email": "d.kundrat@example.com",
"picture": "http://example.com/somepicture_of_dalibor.jpg"
}
```
</br>

`OpenId` flow example:

![OpenId Flow](./Assets/openid_flow.png "OpenId Flow")

</br>

There are multiple flows how to obtain token in `OpenId`. You can read more about [under this article](https://darutk.medium.com/diagrams-of-all-the-openid-connect-flows-6968e3990660).

Each `OpenId` server by specification provides multiple endpoints to interact with it:

- `/authorize` - Interact with the resource owner and obtain an authorization grant
- `/token`- Obtain an access and/or ID token by presenting an authorization grant (code) or refresh token
- `/revoke` - Revoke an access or refresh token.
- `/logout` - End the session associated with the given ID token
- `/userinfo` - Return claims about the authenticated end user.
- `/keys` -Return public keys used to sign responses.
- `/.well-known/oauth-authorization-server`	Return OAuth 2.0 metadata related to the specified authorization server.
- `/.well-known/openid-configuration`	Return OpenID Connect metadata related to the specified authorization server.

### Backend for Frontend pattern (BFF)

BBF is backend consumed by specific frontend aplication.

Since endpoint APIs can have multiple clients with different requirements BFF can provide client specific backend mediator and behave as proxy to forward, and merge multiple request to various services APIs.

![Backend for frontend - BFF example](./Assets/BFF_example.png "Backend for frontend - BFF example")

> &#10240;
>Ok we have cookies, tokens and sessions. We used them by various authentication/authorization protocols (OpenId, OAuth etc..) **and what the hack is BFF good for**?
>
> Answer is:
>- Security reasons
>- Architecture reasons 
> &#10240;

#### Security reasons

In recent years it has been common to implement OpenID Connect for SPAs in Javascript (React, Angular, Vue...) and this is no longer recommended:
- Using access tokens in the browser has more security risks than using secure cookies.
- An SPA is a public client and it cannot hold a secret because, such a secret would be part of the JavaScript and can be accessible to anyone inspecting the source code.
- Recent browser changes in preventig tracking may result in dropping `third party cookies`
- It is not possible to store something in the browser safely over a long time because it can be stole by various attacks.

Due to the mentioned issues outlined above, the best security recommendation for an SPA is to **avoid keeping tokens in the browser** and creates lightweight backend to help with this process called Backend for Frontend pattern (BFF).

By this you can keep using `acces_tokens` to authorize access to all your APIs but keep clients for example depending on type (browser, mobile) to use cookies or tokens how they needs from security reasons.

BFF can be:
- `stateful` - keeps tokens in a storage and use session to manage that.
- `stateless` - stores the tokens in encrypted HTTP-only, same-site cookies

#### Architectural reasons

When you desin your application you have various options how to acces APIs from clients (Web/Mobile/External).

1) A single API gateway providing a single API for all clients
2) A single API gateway provides an API for each kind of client
3) A per-client API gateway providing each client with an API. **This is the BFF pattern**

##### BFF vs API gatway

While an `API Gateway` **is a single entri point** into to the system for all clients, a `BFF` is only responsible for a **single type of client**.

![Backend for frontend vs API gateway](./Assets/BFF_vs_api_gateway.png "Backend for frontend vs API gateway")

#### BBF cookies termination and token isolation

So as was mentiond in text the most important is:
- Avoid keeping tokens in the browser. (“No tokens in the browser” Policy)
- Storing tokens on the server-side and using encrypted/signed HTTP-only cookies 
</br>

Recommended BFF pattern to secure SPA frontends:

![Backend for frontend  (BFF) cookie and tokens flow + proxy](./Assets/bbf_flow_cookies_tokens.png "Backend for frontend  (BFF) cookie and tokens flow + proxy")

- Using this, all communication from the SPA Frontend to the Authorization Server will now go through the BFF, and tokens will not reach the SPA.
- The BFF now issues session cookies. This are par of request to APIs and are exchanged for an access token on proxy level.
- Client side cookies gets terminated by BFF proxy.

## Implementing indentity in .NetCore


NetCore host application has builded in support for [Authentication and Authorization]((https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-5.0).). You can setup bacis Token, Cookie and Session Auth withou big afford and much to know about it.

To successfully implement an Identity Server covered by BFF, it is better to use some libraries that provide you with these integrations (do not try to build them yourself). These external implementations usually use parts of the Microsoft Auth base to keep the code flexible and connect to the existing code base.

> &#10240;
>**There is no reason to study and spend time to implement custom auth** between this layers and is recomendted to use some maintained library.
> &#10240;

One of this libs available with full identity and auth handling is [*IndentityServer*](duendesoftware).

### Duende IndentityServer

![Duende logo](./Assets/duende_logo.png "Duende logo")


**Duende identity server** is framework which implements `OpenId Connect` and `OAuth2` protocols under the `.NetCore` enviroment. It also provides aditional tooling as BFF pattern integration or softly prepared UI interface that you can rewrite to your needs.


> &#10240;
>Duende identity server is licensed version of previous *IdentityServer v4* that was by authors moved under new company *Duende* with [corresponding licensing](https://duendesoftware.com/products/isv).
>
>**It is not free** for production **only for development and testing**!
> &#10240;

This is identity flow architecture for this demo:

![Idnetity server with BFF flow](./Assets/identity_server_sketch_flow.png "Idnetity server with BFF flow")
</br>

**IdentityServer** will help you to define how your clinets *(Web,Mobile,External)* access the protected APIs and how user identity *(Name,Email,Profileinfo)* is managed, stored and accesed in centalised way for all internal and external services that needs to comunicate and use your system.

> &#10240;
>**Note** The Identity Server is not primarily designed for user management, but can be connected to the ASPNet User Store.
> &#10240;

IdentityServer helps you to:
- Integrate witn NetCore user identity
- Integrate IdentityServer `OpenId` and `Oauth` to NetCore App
- Issue tokens
- Secure WebAPIs and protect your resources
- Store identity data to Entity Framework (EF Integration)
- Provide tooling for BFF pattern 
- Secure Frontend comunication over revers proxy
- Manage identiy clients/resources
etc...

You can read more under official documentation:
- [Officail Duende documentaion](https://docs.duendesoftware.com/identityserver/v5)
- [Old IdnetityServer v4 documentation](https://identityserver4.readthedocs.io/en/latest/)

</br>

### Demo Microservices overview

This is the simplified microservice architecture used in demo:

![Identity server and BFF microservices](./Assets/bbf_microservices.png "Identity server and BFF microservices")

>This picture hiddes infrastructure, logging tracing and external DB storages or services like scheduler etc.

Architecture contains 3 separate `.net` projects:
 - `IdentityServer` - Providing, storing and managing Identity and authorization.
- `BBF` - Backend for frontend include React aplication frontend served from static page middleware
- `API microservice` - API server with aplication logic.

</br>

#### IdentityServer project configuration
</br>

![Identity server and BFF microservices - IndetityServer Configuration](./Assets/identity_server_microservices_IdnetityServer_configuration.png "Identity server and BFF microservices - IndetityServer Configuration")

Project is allready fully configured and this is just quickly explanation of the most important steps and guides you over the structure. 

Besic configuration as logging or Telemetry are part of previous tutorials and this projects use equal approach:
- [Configure Logging](./Logging.md)
- [Configure Tracing](./OpenTelemetry.md)

</br>

##### Nuget Packages:

This packages are required to perform identity on API microservice. Other packages are hidden and not displayed in this list.

1) Main package
- `dotnet add package Duende.IdentityServer --version 5.2.2`

2) Extensions NetCore Identity
- `dotnet add package Duende.IdentityServer.AspNetIdentity --version 5.2.2`

3) Extensions Entity Framework
- `dotnet add package Duende.IdentityServer.EntityFramework --version 5.2.2`

4) Extensions Entity Framework .Netcore
- `dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 5.0.9`

5) MVC runtime compilation for Razor Pages
- `dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 5.0.9`

6) Entity framework
- `dotnet add package Microsoft.EntityFrameworkCore --version 5.0.9`

7) Enity Framework migration tools
- `dotnet add package Microsoft.EntityFrameworkCore.Tools --version 5.0.9`

</br>

##### ConfigureServices
`Startup.cs` contains following content:

```c#
    public void ConfigureServices(IServiceCollection services)
    {   
        // See more under AddCorsConfiguration implementation
        services.AddCorsConfiguration(Environment);

        services.AddControllersWithViews().AddRazorRuntimeCompilation();

        // See more under AddAppIdentityDbContext implementation
        services.AddAppIdentityDbContext(Configuration,Environment);

        // See more under AddIdentityServer implementation
        services.AddIdentityServer(Configuration,Environment);

        services.AddHealthChecks();

        services.AddMvc();

         // See more under AddTelemerty implementation
        services.AddTelemerty(Configuration,Environment);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

        if (Environment.IsDevelopment()){
            app.UseDeveloperExceptionPage();

            ServiceExtension.InitializeDbTestData(app);
        }

        app.UseElasticApm(Configuration, new IDiagnosticsSubscriber [0]);

        app.UseCookiePolicy();

        app.UseCors("cors_policy");

        app.UseHealthChecks("/health");

        app.UseStaticFiles();

        app.UseRouting(); 
        
        app.UseIdentityServer();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{Action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
```

</br>

###### AddCorsConfiguration

CORS is Cross Origin Resource Sharing. It is a W3C standard that allows a server to make cross-domain calls from the specified domains. Demo apply this requirements and define `allowed_origins ` array and does not allow reading data from another origins.

If you forgot what is origin return back to [origin definiton](#what-is-origin?).

We must use UseCors before the UseMvc call then only the CORS middleware will execute before any other endpoints. in `Program.cs`.

> &#10240;
>**NOTE:** This must be adjusted for production enviroment with valid production origins.
> &#10240;

`./Src/IdentityServer/API/Configuration/AddCorsConfig.cs`
<pre style="max-height: 400px; overflow-y:scroll !important">
<code>
 
public static partial class ServiceExtension {
    public static IServiceCollection AddCorsConfiguration(
    this IServiceCollection serviceCollection,
    IWebHostEnvironment Environment) {

        string[] allowed_origins = null;

        if(Environment.IsDevelopment()){
            allowed_origins = new string[]{ 
                "https://localhost:5001",
                "https://localhost:5015",
                "https://localhost:5021"
            };
        }else{
                // Add your production origins hire
            allowed_origins = new string[]{ 
                "https://localhost:5001"
            };
        }
        
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy("cors_policy", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                //------------------------------------
                policy.WithOrigins(allowed_origins);
                //policy.AllowAnyOrigin()
                //------------------------------------
                policy.AllowCredentials();
                policy.SetPreflightMaxAge(TimeSpan.FromSeconds(10000));
            });
        });

        // This is IdentityServer part
        serviceCollection.AddSingleton<ICorsPolicyService>((container) =>
        {
            var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

            return new DefaultCorsPolicyService(logger) 
            {
                    AllowedOrigins = allowed_origins
                //AllowAll = true
            };
        });

        return serviceCollection;

    }
}
</code>
</pre>

</br>

###### AddAppIdentityDbContext

`./Src/IdentityServer/API/Configuration/AddDbContext.cs`
<pre style="max-height: 400px; overflow-y:scroll !important">
<code>

public static IServiceCollection AddAppIdentityDbContext(
    this IServiceCollection serviceCollection,
    IConfiguration Configuration, IWebHostEnvironment Environment) {

    serviceCollection.AddDbContext<AppIdnetityDbContext>(option => {

        option.UseNpgsql(Configuration["ConnectionStrings:AppDBConnection"], opt => {
            opt.EnableRetryOnFailure();
        });

        if (Environment.IsDevelopment()) {
            option.EnableDetailedErrors();
            option.EnableSensitiveDataLogging();
        }

    }, ServiceLifetime.Transient);


    return serviceCollection;

}
</code>
</pre>
</br>

###### AddIdentityServer

 `./Src/IdentityServer/API/Configuration/AddIdentityServer.cs`
<pre style="max-height: 400px; overflow-y:scroll !important">
<code>

 public static IServiceCollection AddIdentityServer(
    this IServiceCollection serviceCollection,
    IConfiguration Configuration,
    IWebHostEnvironment Environment) {

    // app user 
    serviceCollection.AddIdentity<ApplicationUser, IdentityRole>(options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    }).AddEntityFrameworkStores<AppIdnetityDbContext>()
    .AddDefaultTokenProviders();

    var identityServerBuilder = serviceCollection.AddIdentityServer(options => options.KeyManagement.Enabled = true);
    
    if (Environment.IsDevelopment()) {
        identityServerBuilder.AddDeveloperSigningCredential();
    }

    // codes, tokens, consents
    identityServerBuilder.AddOperationalStore<AppPersistedGrantDbContext>(options =>
        options.ConfigureDbContext = builder =>
            builder.UseNpgsql(Configuration["ConnectionStrings:AppDBConnection"]));
            
    // clients, resources
    identityServerBuilder.AddConfigurationStore<AppConfigurationDbContext>(options =>
        options.ConfigureDbContext = builder =>
            builder.UseNpgsql(Configuration["ConnectionStrings:AppDBConnection"]));

    identityServerBuilder.AddAspNetIdentity<ApplicationUser>();
    // serviceCollection.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
    
    return serviceCollection;
    }
}
</code>
</pre>
###### Data Stores
IdentityServer requires to define data stores. This stores can be presised in Database or just loaded in-memory from hardcoded configuration.

```c#
// codes, tokens, consents
identityServerBuilder.AddOperationalStore(options =>
    options.ConfigureDbContext = builder =>
        builder.UseNpgsql(Configuration["ConnectionStrings:AppDBConnection"]));
        
identityServerBuilder.AddConfigurationStore(options =>
    options.ConfigureDbContext = builder =>
        builder.UseNpgsql(Configuration["ConnectionStrings:AppDBConnection"]));

```

This Demo use EntityFramework implementation of:
- `Operational data` stores *tokens*, *codes*, etc...
- `Configuration data` stores *Client*, *Resources*, *ApiScopes* etc...

To read more about StoreConfiguration please fllow offical documentation: [Using EntityFramework Core for configuration and operational data](https://docs.duendesoftware.com/identityserver/v5/quickstarts/4_ef/)

Configuration store presist:
 - Clients - Applications that can request tokens from your IdentityServer.
 ```
    new Client
    {
        ClientId = "spa",
        ClientSecrets = { new Secret("secret".Sha256()) },
        
        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

        RedirectUris = { "https://localhost:5015/signin-oidc" },
        
        FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
        BackChannelLogoutUri = "https://localhost:5015/bff/backchannel",
        
        PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },

        AllowOfflineAccess = true,
        AllowedScopes = { "openid", "profile", "api" }
    }
 ```

###### User object:
With *NetCore* whe normally use its Identity to define `User` object. This is part of `Microsoft Identity`. IdentityServer extends this and build aditional layers around and is able to manage `User` related tasks as emit claims into tokens.

class `ApplicationUser` can than contains any aditional user data.
```
serviceCollection.AddIdentity<ApplicationUser, IdentityRole>(options => {
    // options cfg.
    }).AddEntityFrameworkStores<AppIdnetityDbContext>()
    .AddDefaultTokenProviders();

serviceCollection.AddIdentityServer().AddAspNetIdentity<ApplicationUser>();
```
</br>

##### Dummy initial data

All dummy test data are available uder class `Src/IdentityServer/API/Config.cs`. This data are on initial startup loaded and written to database. After that only data from DB are used.

> &#10240;
>**NOTE:** This are the test data in real app you probably wanna create custom interface with UI to manage all this users by yourself.
> &#10240;

Identity resources example:
```
return new[] {
    new IdentityResources.OpenId(),
    new IdentityResources.Profile(),
    new IdentityResources.Email(),
    new IdentityResource
    {
        Name = "role",
        UserClaims = new List<string> {"role"}
    }
};
```

Api resource example:
```
new[]
{
    new ApiResource{
        Name = "api",
        DisplayName = "API #1",
        Description = "Allow the application to access API",
        Scopes = new List<string> {"api.read", "api.write"},
        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
        UserClaims = new List<string> {"role"}
    }
};
```

Api scope example:
```
new ApiScope[]{
    new ApiScope("api", new[] { "name" }),
    
};
```
</br>

##### Setup migrations

To proces migrations ensure you have installed `dotnet-tools`:

```
dotnet tool install --global dotnet-ef
```

Also make sure your database exist. Demo use separate database under main server called `IdenityDB` with this parameters:

- Hostname: `localhost`
- Port: `6543`
- Database: `IdentityDB`
- Username: `postgres`
- Password: `postgres`

> &#10240;
>**NOTE:** Please change username and password for production enviroment.
> &#10240;

Connestion string shoud be awailable under `appsettings.json`

```  
"ConnectionStrings": {
  "AppDBConnection": "Host=localhost;Port=6543;Database=IndetityDB;Username=postgres;Password=postgres",
}
```

PostgreSQL adding another database:

![PostgreSQL database identity setup](./Assets/db_setup_identity.png "PostgreSQL database identity setup")

###### Creating migrations

Migrations for demo are pre-created and sits in folder: `Src/IdentityServer/Persistence/Migrations`

They was created using following command:

```
dotnet ef migrations add Init_ConfigurationDbContext -c AppConfigurationDbContext
dotnet ef migrations add Init_PersistedGrantDbContext -c AppPersistedGrantDbContext
dotnet ef migrations add Init_PersistedGrantDbContext -c AppIdnetityDbContext
```

###### Apply existing migrations

To fill your empty database with new migrations please follow this steps:

Go to dirrectory: `Src/IdentityServer/Persistence` and run following commands:
```
 dotnet ef database update --context AppConfigurationDbContext
 dotnet ef database update --context AppPersistedGrantDbContext
 dotnet ef database update --context AppIdnetityDbContext
```
</br>

##### Running identiyserver project

Navigae to `Src/IdentityServer/API` and run `dotnet watch run` ensure the database and elasticsearch are running. (if not proper exception will be thrown that connection is not established).

Demo identityServer runs on `https://localhost:5001`

You can display openid configuration detail under url:

`https://localhost:5001/.well-known/openid-configuration`

```json
   ],
   "response_modes_supported":[
      "form_post",
      "query",
      "fragment"
   ],
   "token_endpoint_auth_methods_supported":[
      "client_secret_basic",
      "client_secret_post"
   ],
   "id_token_signing_alg_values_supported":[
      "RS256"
   ],
   "subject_types_supported":[
      "public"
   ],
   "code_challenge_methods_supported":[
      "plain",
      "S256"
   ],
   "request_parameter_supported":true,
   "request_object_signing_alg_values_supported":[
      "RS256",
      "RS384",
      "RS512",
      "PS256",
      "PS384",
      "PS512",
      "ES256",
      "ES384",
      "ES512",
      "HS256",
      "HS384",
      "HS512"
   ],
   "authorization_response_iss_parameter_supported":true
}
```

</br>

#### BFF project configuration

![Identity server and BFF microservices - BFF Configuration](./Assets/identity_server_microservices_IdnetityServer_configuration_bffservice.png "Identity server and BFF microservices - BFF Configuration")

##### Nuget Packages:

This packages are required to perform identity on API microservice. Other packages are hidden and not displayed in this list.

1) BFF main package
- `dotnet add package Duende.BFF --version 1.0.0-rc.3`

2) SPA extension middleware
- `dotnet add package Microsoft.AspNetCore.SpaServices.Extensions --version 5.0.9`

3) Extension .NetCore OpenId Connect 
- `ddotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect --version 5.0.9`

</br>

##### ConfigureServices
`Startup.cs` contains following content:

```c#
public void ConfigureServices(IServiceCollection services){

    services.AddControllersWithViews();

    // In production, the React files will be served from this directory
    services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "ClientApp/build";
    });

    // Add BFF services to DI - also add server-side session management
    services.AddBff(options =>
    {
        options.AntiForgeryHeaderValue = "1";
        options.AntiForgeryHeaderName = "X-CSRF";
        options.ManagementBasePath = "/system";
    })
    .AddServerSideSessions();

    services.AddIdentityConfiguration();

    services.AddHealthChecks();

    services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
    
    services.AddTelemerty(Configuration,Environment);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment()){
        app.UseDeveloperExceptionPage();
    }else{
        app.UseExceptionHandler("/Error");

        app.UseHsts();
    }

    app.UseElasticApm(Configuration, new IDiagnosticsSubscriber [0]);

    app.UseHealthChecks("/health");

    app.UseHttpsRedirection();

    app.UseDefaultFiles();

    app.UseStaticFiles();

    app.UseSpaStaticFiles();

    app.UseAuthentication();

    app.UseRouting();

    app.UseBff();

    app.UseAuthorization(); // adds authorization for local and remote API endpoints

    app.UseEndpoints(endpoints =>{
        // local APIs
        endpoints.MapControllers()
            .RequireAuthorization()
            .AsBffApiEndpoint();
            
        
        endpoints.MapBffManagementEndpoints();   // login, logout, user, backchannel logout...
        
        endpoints.MapRemoteBffApiEndpoint("/graphql", "https://localhost:5022/graphql",true)
            .WithOptionalUserAccessToken()
            
            .AllowAnonymous();
    
    });

    app.UseSpa(spa =>{
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment()){
            spa.UseReactDevelopmentServer(npmScript: "start");
        }
    });
}
```

</br>


###### AddIdentityConfiguration

This method define that authentication mechanism for frontend will use cookie with with `OpenId Connect` also defining all its parameters.

> &#10240;
>**NOTE** This configuration contains dummy test data as secrets. Dont use it in production!
> &#10240;

`./Src/APIServer/API/Configuration/AddIdentityConfiguration.cs`
<pre style="max-height: 400px; overflow-y:scroll !important">
<code>
 
 public static partial class ServiceExtension {
    public static IServiceCollection AddIdentityConfiguration(
    this IServiceCollection serviceCollection) {

        // cookie options
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultScheme = "cookie";
            options.DefaultChallengeScheme = "oidc";
            options.DefaultSignOutScheme = "oidc";
        })
        .AddCookie("cookie", options =>
        {
            // set session lifetime
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            
            // sliding or absolute
            options.SlidingExpiration = false;
            
            // host prefixed cookie name
            options.Cookie.Name = "__SPA_FF";
            
            // strict SameSite handling
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001";
            
            // confidential client using code flow + PKCE
            options.ClientId = "spa";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.ResponseMode = "query";

            options.MapInboundClaims = false;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

            // request scopes + refresh tokens
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("api");
            options.Scope.Add("offline_access");
        });

        return serviceCollection;
    }
}
</code>
</pre>

</br>

###### AddBff configuration
</br>

**Cross-Site Request Forgery (CSRF)** is protection mechanism that BFF require to include from called in header. This means your frontend app must set this values with all request. The value or Header name cam be adjusted as on example:

Example fetch function from Frontend React app to BFF

```
return fetch(`${BASE_SERVER_URL}/${GQL_ENDPOINT}`, {
    credentials: "include",
    method: "POST",
    mode: 'cors',
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      'X-CSRF': '1' //  <---- THIS IS CSRF HEADER
    },

    body: JSON.stringify({
      id: operation.id, 
      query: operation.text,
      variables,
      operationName: operation.name,
    }),
  })
    .then((response) => {
      return response.json();
    })
    .then((json) => {
      return json;
    }).catch((ex)=>console.log(ex));
}
```

**ManagementBasePath** defines frontend endpoints used to comunicate with identityserver. Frontend client use this to perform identity tasks as Login or logout as example: `https://frontendurl/system/login` or  `https://frontendurl/system/logout` etc..

```
services.AddBff(options =>
{
    options.AntiForgeryHeaderValue = "1";
    options.AntiForgeryHeaderName = "X-CSRF";
    options.ManagementBasePath = "/system";
})
.AddServerSideSessions();
```
</br>

#### API project configuration

![Identity server and BFF microservices - API Configuration](./Assets/identity_server_microservices_IdnetityServer_configuration_apiservice.png "Identity server and BFF microservices - API Configuration")
</br>

##### Nuget Packages:

This packages are required to perform identity on API microservice. Other packages are hidden and not displayed in this list.


1) Extension .NetCore JWT Bearer
- `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 5.0.9`

</br>

##### ConfigureServices

There is no much from API protected  project to be configured. This API is protecteed using JWT token. `Startup.cs` require to add following configuration to API project.

From code you can use standard `[Authorize]` attribute to perform all auth checks.

> &#10240;
>**NOTE:** This configuration display only identity relevant parts:
> &#10240;

```c#
public void ConfigureServices(IServiceCollection services){

    services.AddAuthentication("token")
        .AddJwtBearer("token", options =>
        {
            options.Authority = "https://localhost:5001";
            options.MapInboundClaims = false;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidTypes = new[] { "at+jwt" },
                
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });

    services.AddAuthorization(options =>{ });

    // And  Other services related to API ...
    // etc etc ..
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // And  Other middleware related to API ...
    // etc etc ..

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
    });

    app.UseAuthentication();
    app.UseAuthorization();

    // And  Other middleware related to API ...
    // etc etc ..
}
```

</br>

### Running microservices

You can run all services from their work dirrectory or pack them to docker container.

> &#10240;
>**NOTE:** Please ensure all DBs are created and has initial migration done.
> &#10240;

###### Idnetityserver

Navigate to: `Src/Idnetityserver/API/`
Run: `dotnet watch run`

###### BFF

Navigate to: `Src/BFF/API/`
Run: `dotnet watch run`

###### APIServer

Navigate to: `Src/APIServer/API/`
Run: `dotnet watch run`

</br>

#### Idnetity server UI interface:


Endpoint `https://localhost:5001/Account/Login`

![Endpoint user login](./Assets/endpoint_login.PNG "Endpoint user login")

</br>

Endpoint `https://localhost:5001/Account/Logout`

![Endpoint user logout](./Assets/endpoint_logout.PNG "Endpoint user logout")

</br>

Endpoint `https://localhost:5001/diagnostics`

![Endpoint OpenId user diagnostic and info](./Assets/endpoint_user_info_openid.PNG "Endpoint OpenId user diagnostic and info")

</br>

Endpoint `https://localhost:5001/diagnostics`

![Allowed devices - revocation grands](./Assets/endpoint_revocation_grants.PNG "Allowed devices")

#### Login and Logout integration to UI

Frontend App in this demo was built with React and uses the POST fetch function to call the BFF, which makes a request to the GraphQL server. This is the standard flow for communication between client and API. GraphQL API is accessible to anonymous users. If you are not authorised, a concrete error will be returned for the data you requested, but in some cases it will simply return `null`. This is up to the backend SW engineer on how he implemented this error and auth handling.

To perform loging and logout from clinet this works bit differently. BFF maps special endpoint for this and expose it on this urls:

- `https://your_app_url:port/system/login`
- `https://your_app_url:port/system/logout`

##### Login

At the top level of the frontend query, you normally request the user data as a `me` query. This is called when the app is first rendered.

```tsx
// UserProvider.tsx
query UserProviderQuery {
    me {
        id
        name
        sessionId
    }
}
```

This function returns the user data or `null` if the user is not authenticated, and you can use this to redirect the user to the correct component in your routing.

```tsx

if (!userStore?.user?.me) {
    return (
      <Routes>
        <Route path="/*" element={<Login/>} />
      </Routes>
    );
  } else {
    return (
      <Routes>
        <PrivateRoute  path={"/*"} element={<Settings />} />
        // Another routes
      </Routes>
    );
  }

```

Then your `<Login/>` component can navigate you to the BFF endpoint, which is called: `https://your_app_url:port/system/login`. This navigation must happenend out of React router!

```tsx
// This is the simole login redirect
export default function Login() {

      useEffect(() => {
        window.location.href = LOGIN_ENDPOINT;
      }, [])

      return <></>
  }
```

After the app encounters this BFF, the request is automatically forwarded to the IdentityServer with all the contextual data of the request. You perform the login and the server redirects you back to the post-login URL of the app.

#### Logout

When it comes to logging out, it's a little more complicated. The user usually clicks on an Logout button, which in turn redirects to a `<Logout/>` component:

```tsx
// example logout 
export default function Logout() {

  const store = useUserStore();
    
      useEffect(() => {
        if(store?.user?.me?.sessionId){
          window.location.href = `${LOGOUT_ENDPOINT}?sid=${store?.user?.me?.sessionId}`;
        }else{
          window.location.href = LOGOUT_ENDPOINT;
        }

      }, [])

      return <></>
  }
}
```
</br>

As you can see in the code, this request needs to provide `sid`, which stands for SessionId. 

```
`${LOGOUT_ENDPOINT}?sid=${store?.user?.me?.sessionId}
```
The question is how to get this information? You can see it btw in the secured cookie, but you can not read the secured cookie, so those are some of your options:

- Use a custom cookie sessionid middelware and manually write that data into the context.
- You can call the user endpoint (of the idnetity server) from the frontend and get the data from there.
- You can put the data in jwt token claims and provide it in a graphql schema for querying.

This demo uses the sessionId provided by the GraphQL server from the token claim. So each token contains this `sid` value and these tokens are only used on the backend side behind the proxy.

If you do not provide the correct `sid` identity, the server will respond with an exception:

```sh
Exception: Invalid Session Id
Duende.Bff.DefaultLogoutService.ProcessRequestAsync(HttpContext context) in DefaultLogoutService.cs, line 49

Stack Query Cookies Headers Routing
Exception: Invalid Session Id
Duende.Bff.DefaultLogoutService.ProcessRequestAsync(HttpContext context) in DefaultLogoutService.cs
Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)
Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
Duende.Bff.Endpoints.BffMiddleware.Invoke(HttpContext context) in BffMiddleware.cs
Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Builder.Extensions.MapWhenMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)
```
