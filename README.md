# MVC-Net-Shop

This is a shop application designed by using ASP.NET with MVC pattern. To manage data I used Entity Framework and MySQL database. 
To add authorization and authentication options I chose Identity package.

The application has 3 roles with diffrent functionalities:
* admin - is responsible for articles and categories management. He has a preview of users orders but he can not submit orders on his own
* logged user - can add products to cart and submit orders
* not logged user - can add products to cart but need to login or create an account to place order

<img src="https://user-images.githubusercontent.com/69191839/152976737-ada63b0c-04da-4848-8c6a-007e6cff44d3.png" width="700" height="500">
