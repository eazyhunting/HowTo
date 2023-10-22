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
  I'm registering my services in the program.cs as you might expect.
  `builder.Services.AddTransient<IVersionService, VersionService>();`
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
