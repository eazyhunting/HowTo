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
- Constructor Injection
  ```
  public VersionController(IVersionService versionService) { 
    _versionService = versionService;
  }
  ```
- RESTful Routes
  By default the route would have simply been /version. In order to follow proper REST conventions I added:
  `[HttpPost("compare")]`
  This results in the API route being `/Version/compare`
  
- Proper Response
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

- Async/Await
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
