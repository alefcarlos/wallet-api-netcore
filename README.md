# Wallet API

### How to run

 1. You have to [install](https://www.microsoft.com/net/download/core) de .net Core SDK

 2. You must configure your connection string at Wallet.Api/appsetings.json

~~~~
default value is Server=localhost;Database=Wallet;Trusted_Connection=True;MultipleActiveResultSets=true
~~~~

 3. Open the api folder

~~~cmd
cd Wallet.Api
~~~~

 4. First, you need to restore the packages

~~~~
dotnet restore
~~~~

 5. After that, compile it

~~~~
dotnet build
~~~~

 6. ...And finally: run !!

~~~~
dotnet run
~~~~

>The ef core will create a blank database for you, called Wallet, and a [local server](http://localhost:5000) will be available to make requests

>See the documentation at [here](http://localhost:5000/swagger).


### How to make a request ?

Every request needs a header, unless POST api/v1/user/Login and POST api/v1/user

~~~
X-IDENTITY-USER
~~~~

>This header contains de Guid related to and user. Every method saves the information based on this user.

*When you make a login, you get a Guid in the response body*

~~~~
{
    "code": 200,
    "done": true,
    "data": "bc1cb656-a4d9-4f72-a8c7-779fdaf62edd"
}
~~~~

Every response has this pattern result

~~~~
{
    "code": int,
    "done": bool,
    "data": object
}
~~~~

Enjoy the code !
