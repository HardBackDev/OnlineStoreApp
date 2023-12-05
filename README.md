# OnlineStoreApp
To start the project, first open and run OnlineStoreServer.sln, this will create the database and populate all the tables. Then, through Visual Studio Code, open the OnlineStoreClient folder and enter "npm install" command in the terminal, this will install all the dependencies. After this you can launch the client application and view the application at url "http://localhost:4200/". in app you can login with existing account with username: "Admin" and password: "Password1000" to view administrator abilities or login as user with username: "User" and password: "Password1000".
# Docker
To start project in docker, in docker-compose.yml file, replace value of ASPNETCORE_Kestrel__Certificates__Default__Password on your certificate password, if you dont know your certificate password, open cmd and write these command one by one 
"dotnet dev-certs https --clean", <br>
"dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\cert.pfx -p awesomepass", <br>
"dotnet dev-certs https --trust" <br>
then just open cmd in root folder and write "docker-compose up --build" command. After container will started, you can view app by url "http://localhost:8082/". 
# Endpoints
## (GET) https://localhost:5001/api/products - gets all products	
### parameters:  
### 	- searchTitle - filtering by title  
###  	- minPrice - filtering by price  
### 	- maxPrice - filtering by price  
## (GET) https://localhost:5001/api/products/{category} - gets all products of specified category
### parameters:
### 	parameters depend on the selected category  
###  	by "https://localhost:5001/api/categories/{category}/parameters" request, you can get information about parameters of specified category  
## (GET) https://localhost:5001/api/products/{category}/{id} - gets product of specified category by id. When entering a category, consider case.
## (POST) https://localhost:5001/api/products/{category} - creates new product for specified category. To get information about category properties, get this request: "https://localhost:5001/api/categories/{category}/manipulatingMetadata"  
## (DELETE) https://localhost:5001/api/products/{category}/{id} - deletes product of specified category by its id 
## (PUT) https://localhost:5001/api/products/{category}/{id} - updates product of specified category by its id
## (GET) https://localhost:5001/api/categories - gets all categories of products
## (GET) https://localhost:5001/api/categories/{category}/parameters - gets information about parameters of specified category
## (GET) https://localhost:5001/api/categories/{category}/manipulatingMetadata - gets information about parameters of specified category
## (GET) (Authorized) https://localhost:5001/api/cart - gets user cart.
### parameters:
### 	- searchTitle - filtering by title  
###  	- minPrice - filtering by price  
### 	- maxPrice - filtering by price  
## (GET) (Authorized) https://localhost:5001/api/cart - gets user cart.
## (GET) (Authorized) https://localhost:5001/api/cart/checkInCart{productId} - checks whether the speecified product has in cart
## (POST) (Authorized) https://localhost:5001/api/cart/{productId} - adds product to cart
## (DELETE) (Authorized) https://localhost:5001/api/cart/{productId} - deletes product from cart
## (POST) (Authorized) https://localhost:5001/api/auth - registers user
### request body:
### {
###			"userName":"", 
###		 	"password":"", 
###			"email":"", 
###		 	"phoneNumber":""
### }
## (POST) (Authorized) https://localhost:5001/api/auth - gets auth token and refresh token
## (POST) (Authorized) https://localhost:5001/api/token/refresh - refreshes token, requires auth token and refresh token as body
