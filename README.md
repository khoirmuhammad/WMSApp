# PROJECT STRUCTURES
Please refer to project solution and look at the project structure. Here I simply explain you about custom folders created by me
## Constants
It will configure the constant name insted we type it manually. Therefore we able to minimalize error type in our code.
## Contracts
Basically we able to name it as our preference such as Abstarctions/Interfaces/Contracts. It will be used to store Interface files we need when development apps
## CustomModels
This models are relate to table (database) and migrations will be stored in Models folder otherwise will be stored here. 
## Data
It will store application data context in application
## Extensions
It will store extension class we needed. Example, rather than type service dependency injection on by one in startup.cs, I prefer create single class and register all custom Dependency Injection here. Then call the service extension in startup.cs
## Helpers
It will be create helper class that might be used by othe class. so intead we create function/method on each class, we simply create helper class
## Repositories
Since we implement repository pattern to create data access, we need to create this folder.
