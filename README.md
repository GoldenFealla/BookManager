# Book Manager

A backend using Dotnet Core as Web API and using PostgreSQL as database with JWT for Authenication and Authorization

## Installation

- Create RSA key pair in keys folder using command

```bash
# Create directory for keys
mkdir -p keys

# Generate private key
openssl genrsa -out keys/private_key.pem 2048

# Generate public key
openssl rsa -in keys/private_key.pem -pubout -out keys/public_key.pem
```

- Add Environment using .env file

```
POSTGRESQL_STRING="Server=<host>;Port=<port>;Database=<Database>;User ID=<user>;Password=<Password>;"
ISSUER="http://localhost:5229/"
AUDIENCE="http://localhost:5229/"
```

- Start the Server

```
dotnet run
```

## Future

This project is still being developed.

- Add swagger
- Register/Logout user
- Create/Update/Delete Book
- Borrow book system
- Admin system
  - Ban/Unban user
  - Change user information
