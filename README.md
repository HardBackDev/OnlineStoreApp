# OnlineStoreApp
To start the project, first open and run OnlineStoreServer.sln, this will create the database and populate all the tables. Then, through Visual Studio Code, open the OnlineStoreClient folder and enter "npm install" command in the terminal, this will install all the dependencies. After this you can launch the client application and view the application at url "http://localhost:4200/". in app you can login with existing account with username: "Admin" and password: "Password1000" to view administrator abilities or login as user with username: "User" and password: "Password1000".
# Docker
To start project in docker, in docker-compose.yml file, replace value of ASPNETCORE_Kestrel__Certificates__Default__Password on your certificate password, if you dont know your certificate password, open cmd and write these command one by one 
"dotnet dev-certs https --clean", 
"dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\cert.pfx -p awesomepass", 
"dotnet dev-certs https --trust"
then just open cmd in root folder and write "docker-compose up --build" command. After container will started, you can view app by url "http://localhost:8082/". 
# Postman
you can use Postman to test api requests.

Requests:

------------------------------------------------------------------------------------------------------------------------------------
  [POST] - https://localhost:5001/api/auth
  Description: register newly user with Administrator role.
  Example body for request:
  { 
    "username": "Jonh",
    "password": "Password1000",
    "email": "johndoe@mail.com",
    "phonenumber": "589-654"
  }
  
------------------------------------------------------------------------------------------------------------------------------------
  [POST] - https://localhost:5001/api/auth/login
  Description: gives bearer token, then in postman need switch to authorization and choose type "Bearer Token" and place given token to field
  Example body for request:
  {
    "username": "Jonh",
    "password": "Password1000"
  }

------------------------------------------------------------------------------------------------------------------------------------
  [POST] - https://localhost:5001/api/token/refresh
  Description: refreshes the authorization token
  Example body for request:
  {
    "AccessToken": Acess Token from https://localhost:5001/api/auth/login response,
    "RefreshToken": Refresh Token from https://localhost:5001/api/auth/login response
  }
  
------------------------------------------------------------------------------------------------------------------------------------
  [GET] - https://localhost:5001/api/products
  Description: returns all products
  query parameters:
  pageSize - defines the size of fetched rows
  pageNumber - defines the page number
  searchTitle - Returns products whose titles contain the searchTitle value
  minPrice - defines the minimal price for fetched products
  maxPrice - defines the maximal price for fetched products
  OrderBy - Defines the column by which to order the results. After specifying the column type, leave a space and indicate the order direction ('desc' for descending or 'asc' for ascending). For example, 'OrderBy=Title desc.' If the     direction is not specified, it defaults to 'desc'.
  Example path: https://localhost:5001/api/products?pageSize=5&searchTitle=iphone

------------------------------------------------------------------------------------------------------------------------------------
  [GET] - https://localhost:5001/api/products/{category name}
  Description: returns products of specified category
  query parameters:
  contains all parameters for https://localhost:5001/api/products request and parameters specific to this category.
  categories parameters:
    smartphones:
      searchModel - Returns smartphones whose model contain the searchModel value 
      minMemory - defines the minimal memory for fetched smartphones
      maxMemory - defines the maximal memory for fetched smartphones
    meats:
      searchMeatType - Returns meats whose meatType contain the searchMeatType value 
      minDateCreated - defines the minimal date time for fetched meats
      maxDateCreated - defines the maximal date time for fetched meats
    wheatProducts:
      searchFlourType - Returns wheatProducts whose FlourType contain the searchFlourType value 
      minDateCreated - defines the minimal date time for fetched wheatProducts
      maxDateCreated - defines the maximal date time for fetched wheatProducts
  Example path: https://localhost:5001/api/products/smartphones?minPrice=600&pageSize=5&pageNumber=2

------------------------------------------------------------------------------------------------------------------------------------
  [GET] - https://localhost:5001/api/products/{category name}/{product id}
  Description: returns product with specified id and category-specific properties
  Example path: https://localhost:5001/api/products/smartphones/65703da3-162e-46e3-b98e-b7fa4756ab81

------------------------------------------------------------------------------------------------------------------------------------
  [POST] - https://localhost:5001/api/products/{category name}
  Requered Authorization with "Administrator" role
  Description: Creates a new product of the specified category. The 'ProductPhoto' column specifies the path to the image file in the 'wwwroot' directory for the product
  Example body for request:
  The body of the request depends on the category of the product being created.
  Example body for smartphones category:
  {
    "RAM": 16,
    "Memory": 528,
    "Model": "iPhone 103 Pro",
    "CPU": "Apple A150 Bionic",
    "OperatingSystem": "iOS 150",
    "Price": 2000,
    "Title": "iPhone 103 Pro"
  }

  Example body for meats category:
  {
    "DateCreated": "2023-08-05T00:00:00",
    "MeatType": "venison",
    "Price": 8,
    "Title": "Pork Chops",
    "ProductPhoto": null
  }

  Example body for wheatproducts category:
  {
    "DateCreated": "2023-08-05T00:00:00",
    "FlourType": "Whole wheat flour",
    "Price": 649,
    "Title": "Whole Wheat Pancake Mix",
    "ProductPhoto": null
  }
  
------------------------------------------------------------------------------------------------------------------------------------
  [PUT] - https://localhost:5001/api/products/{category name}/{product id}
  Requered Authorization with "Administrator" role
  Description: Updates a product of the specified category with a given ID
  Example body for request: 
  Example body for smartphones category:
  {
    "RAM": 16,
    "Memory": 528,
    "Model": "iPhone 103 Pro",
    "CPU": "Apple A150 Bionic",
    "OperatingSystem": "iOS 150",
    "Price": 2000,
    "Title": "iPhone 103 Pro"
  }

  Example body for meats category:
  {
    "DateCreated": "2023-08-05T00:00:00",
    "MeatType": "venison",
    "Price": 8,
    "Title": "Pork Chops",
    "ProductPhoto": null
  }

  Example body for wheatproducts category:
  {
    "DateCreated": "2023-08-05T00:00:00",
    "FlourType": "Whole wheat flour",
    "Price": 649,
    "Title": "Whole Wheat Pancake Mix",
    "ProductPhoto": null
  }

------------------------------------------------------------------------------------------------------------------------------------
  [DELETE] - https://localhost:5001/api/products/{product id}
  Requered Authorization with "Administrator" role
  Description: Deletes a product of the specified category with a given ID
  Example path: https://localhost:5001/api/products/smartphones/65703da3-162e-46e3-b98e-b7fa4756ab81

------------------------------------------------------------------------------------------------------------------------------------
  [GET] - https://localhost:5001/api/cart
  Requered Authorization with any role
  Description: Returns products from cart of authenticated user

------------------------------------------------------------------------------------------------------------------------------------
  [POST] - https://localhost:5001/api/cart/{product id}
  Requered Authorization with any role
  Description: Adds product to cart of authenticated user
  Example path: https://localhost:5001/api/cart/65703da3-162e-46e3-b98e-b7fa4756ab81
