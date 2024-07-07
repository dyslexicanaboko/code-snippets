# API Template

_Revision: 07/05/2024_

The idea behind this folder is so that when I have to start a new project, I have everything I need in one place. This is a ton of boiler plate code that I am tired of writing and rewriting.

## Included

- Library project
- WebApi project
- Unit Testing project

## Excluded

- Database

A database project has been excluded because this can be anything, not necessarily SQL Server. The only thing I can provide is a suggested folder structure and that's all.

### Library project

The library project is where 90% of the logic is held and it is broken down into the usual N-tier structure:

- DataAccess
  - Depends on `Dapper`
  - Insert, Select, Update, Delete verbs
- Entities
- Exceptions
- Mappers
- Models
- Services
  - Create, Get, Edit, Remove verbs
- Validation
  - Depends on `FluentValidation`

### Example pipeline

There is an example pipeline to get you started themed as "Task". This is meant to be generic and easy to understand for anyone. The bare minimum has been applied.

## Dependencies

There is built in bias and `NuGet` package dependencies. The bias is on how HTTP errors are handled, exceptions are raised, validation is performed, logging, Dependency Injection and more.

## Future

1. In the future a security token structure will be formalized and added in a way where it does not have to necessarily be used. I want to perfect what I have before making it part of the template.
2. Template generation - right now the idea is to copy this code verbatim and then to perform a manual rename of the namespaces, project names, solution file etc. I don't love that and it would be easier if I could just automatically replace everything in one swift motion. I am attempting to put some of this into Spot Welder as it's needed too.
