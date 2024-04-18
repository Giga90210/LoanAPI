# LoanApi

This is a simple API with three entities:

- Accountant
- User
- Loan

## Services

There are 3 services, one for each entity. Each service has an interface that it implements, these interfaces are located in the Application layer. The services that implement the interfaces are located in the Infrastructure layer.

## AccountantService

- BlockUser(int userId)
  to block users
- UnblockUser(int userId) to unblock user
- Login([FromBody]Accountant loginModel)
  to log in and get token
- Register([FromBody]Accountant registerModel)
  to register into the database
- GetAccountantById(int id) for getting accountants by name

## UserService

- GetUserById(int id) to get user by id
- GetUsers() to get all users
- login([FromBody]User loginModel) to log in and get token
- Register([FromBody] User registerModel) to register into database
- GetMyLoans() to get user specific loans

## LoanService

implents basic CRUD operations along with ApproveLoan(int id) and RejectLoan(int id) which are used to either approve or reject loan by an accountant

## Roles

two roles: Accountant, User. accountant has access to pretty much everything while user has restrictions

## Database

Entity framework to connect and work with database implemented in infrastructure layer

## Logging

Serilog for logging

## Testing

Mock tests for the three services
