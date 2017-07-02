# Wallet API

How to run ?

You have to install de .net Core SDK

~~~~
https://www.microsoft.com/net/download/core
~~~~

You mus configure your connection string at Wallet.Api.appsetings.json

~~~~
default value is Server=localhost;Database=Wallet;Trusted_Connection=True;MultipleActiveResultSets=true
~~~~

Open the api folder

~~~
cd Wallet.Api
~~~~

First, you need to restore the packages

~~~~
dotnet restore
~~~~

After that, compile ir

~~~~
dotnet build
~~~~

...And finally: run !!

~~~~
dotnet run
~~~~

The ef core will create a blank database for you called Wallet and a local server will be available to make requests

~~~~
default: localhost:5000
~~~~

Open your browser e see the Swagger documentation at

~~~
localhost:5000/swagger/
~~~~

How to make a request ?

Every request needs a header, unless /Login and /AddUser

~~~
X-IDENTITY-USER
~~~~

This header contains de Guid related to the user. We get the user using this.

Every api saves the information based on this user.

Enjoy the code !
