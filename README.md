# HowTo 

The goal of this solution is to illustrate some of the techniques I might use when developing a .NET solution.

# Background

The solution uses .NET Core 6 and at the time of this writting consists of 4 projects. 

- HowTo.API - A simple .NET Core Web API
- HowTo.Services - A service layer for my business processes
- HowTo.Shared - This is a shared layer that consists of models that are shared between my API and Services
- HowTo.Tests - An xUnit project that exercises my code to ensure nothing is glaringly broken

# Points of Interest

- Configure Dependency Injection:
  So we know that a standard way of registering your dependencies would be to do so directly in the program.cs file as such:
  `builder.Services.AddTransient<IVersionService, VersionService>();`

  While this is great for small projects, as your solution grows you will get an extremely squirrely program.cs. You may also introduce dependencies where you don't really want them such as if you were to register your data dependencies in your controller. Remember your controllers should not access your data stores directly, they should only do so through services. Additionally, when you have hundreds of services you will have to look through a long list of what could very well be unorganized list of service registrations. For this I employ the ServiceCollection Extension Pattern.

  What we do is create a static class as you would for any extension methods. Then extend the IServiceCollection interface and register your services in this method.
  ```
  public static class DependencyInjectionExtensions
  {
      public static void RegisterDependencies(this IServiceCollection services)
      {
          services.AddTransient<IVersionService, VersionService>();
      }
  }
  ```
  Now in your program.cs you can simply call `builder.Services.RegisterDependencies();`

  This may not seem very helpful here but you can use this as a starting point to organize your dependancies. You can create several extension methods for specify types of registrations. Maybe one for `Scoped` & `Singleton` services versus `Transient` services. You might possibly consider seperating them by feature/function say `RegisterUserDependencies` or `RegisterCachingDependencies`. You could do something like `RegisterDataDependencies` but DON'T!!! Remember, your controller shouldnt' have access to your data dependencies.

  At the risk of getting ahead of myself I'll offer you this bit of advice. Move your extension methods into the layer they belong to. In this way your services layer becomes responsible for registering it's own dependencies. You can do this with your data layer as well. So now we've truly isolated and decoupled our layers. If I add a new service to my business layer I only need to edit the code in the services project. I don't need to touch the code in the API layer. 
  
- Constructor Injection:
  ```
  public VersionController(IVersionService versionService) { 
    _versionService = versionService;
  }
  ```
- RESTful Routes:
  By default the route would have simply been /version. In order to follow proper REST conventions I added:
  `[HttpPost("compare")]`
  This results in the API route being `/Version/compare`
  
- Proper Response:
  I could have simple returned the model back from the API but I wanted to provide information about if the request was success or not. A simple way to do this is to return an `IActionResult` that contains your model
  ```
   [HttpPost("compare")]
   public async Task<IActionResult> Compare(VersionComparisonModel comparisonModel)
   {
       try
       {
           var response = await _versionService.Compare(comparisonModel);
           return Ok(response);
       } catch (Exception ex)
       {
           return BadRequest(ex.Message);
       }   
   }
   ```

- Async/Await:
  It is important to use Asyncronous programming so that you do not block the calling thread. This is done by marking your method with the `async` keyword and return a `Task`. You also need to use the `await` keyword at least once in       your method.

    ```
   [HttpPost("compare")]
   public async Task<IActionResult> Compare(VersionComparisonModel comparisonModel)
   {
       try
       {
           var response = await _versionService.Compare(comparisonModel);
           return Ok(response);
       } catch (Exception ex)
       {
           return BadRequest(ex.Message);
       }   
   }
   ```
- IVersionService:  All services should be based on interfaces. This encourages decoupling and provides greater testability for mocking.
- Async: I have painstaking chosen to make the CompareAsync method asynchronous. I didn't need to because I'm not truly doing anything that requires it. At any rate I've marked it as `async` and return a `Task<VersionComparisonResultModel>`. The trick to returning something asynchronous when it really isn't to wrap it in `Task.Run()` as seen below:
  ```
  return await Task.Run(() =>
  {
      return Task.FromResult(new VersionComparisonResultModel(comparisonModel)
      {
          Result = enumValue
      });
  });
  ```
- FivePartVersion: I've chosen to create a domain object to handle some business logic. This could cause some areas of debate for some folks. Here's my reasoning.
    1. I was recently asked to compare version numbers in a code test :)
    2. While .NET provides a very useful `Version` class it only handles 4 part versions. (And the code test wanted 5 parts)
    3. It gives me the opportunity to do some things with implementing system interfaces such as `IComparable`
    4. It allows me to illustrate when, in Domain Driven Design, you would want to have a business object implement logic. Besides if all your business objects contain no business logic that you are actually using an antipattern called the Anemic Domain Model. That said it's important to distinguish between Models and Business Objects. 
  
- Shared Dependencies:
  My shared project consists of things I wouldn't mind sharing between the Controllers and my Services. Note that I haven't put my Domain objects in shared because I would not want my Controllers doing anything fancy outside of request/response.

- Tests: I'm using xUnit here. Implementing your basic Green scenarios using a data driven `[Theory]`. Of Course it's important to ensure things fail when they're supposed to so I use a `[Fact]` and assert that the appropriate exception was thrown.
