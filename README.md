### Agency
----

Last summer, I was asked to create an application which, among other things, manages a employees contact list and let you to modify them, synchronizing local changes with the server database. I have actually never built something for Android (and I did not want to start to do it either), and surely I had to look for a cross-platform solution. Turned out Xamarin.Forms (Xamarin.iOS too, since some components have been customized for iOS) was a great experience, which by the way, at the end AOT compiles in native ARM assembly (on iOS), the JITter translates IL bytecodes to assembly at runtime on Android instead. 

Also, I evaluated C# as a highly productive language, especially the way you can query datas, relational models, XMLs etc., via LINQ. Not surprisingly Swift is being influenced by C# a lot.

The (partial) project consists of the application itself, and a Web API, an HTTPS service which provides REST-like APIs based on the main HTTP verbs (corresponding to the CRUD operations). Access to the service is limited by specific controllers so that a user must be authenticated through a token signed by SHA-256 HMAC signature. Once a user logins, a request to `/Token` to generate the token is carried out; it sends a particular payload in form of `Content-Type: application/x-www-form-urlencoded` in the body, like this:

```
echo 'username=admin@admin.com&password=password&grant_type=password' | curl -k -X POST --data @- https://localhost:44396/Token
```

So, if everything went right, a one-time generic bearer token with its lifetime should be received as response (pretty similar to a JWT). It should be something like this:

```
{  
"access_token": "HsSGlFEpTIrkJGW9Cs7cPBPHlYotrBZgz2j7BOnNtoPWjKT84mZn_guhjBXhkJrucD1QtE1J_zqaTR-5diNred6HeSomuNicyVLNQmrLDuI--rhx9SprzewwlAV271mzeWSL5lj2seBp7AFb4n8AUc2pAJc42n_FZLa7A5jFqNU7afNdJL6c-lfBXOHEHImttAPyYE8Ni4gaH9CXs2JX6DfQ3qsEAnkSJs6CKa7xVPtdtPoUL3nYPOy1--aUl37AxW1Wiy5ZPeQuWwfOMXUUfVMY8MBhvmvaBWzRHuF2nk0Jm3C04KQ7tqlGae7usRvsSKONvlNj258rR-9Bbh_wlijoEm_OTOHo2Cy4WHPtgQr3ffrG19IDAzLysq8M7vzn8Gjagd-xohu3yBZrrAIh14s3dcWdeO1EZVCmDaGgkHdWfDP-R_f8h0X-MZnUwsHfoiE2GHAZ1QyUs40XemmqEgVBEhHx-3E92E4TK7cntuU",
"token_type":"bearer",
"expires_in":1209599,
"userName":"admin@admin.com",
".issued":"Wed, 27 Jul 2016 13:57:25 GMT",
".expires":"Wed, 10 Aug 2016 13:57:25 GMT"
}
```

Eventually, a GET request embeds the access token, it is then decrypted and a filter is responsible to validate it, thus identifying the user. 

While the architectural pattern typically used on iOS is the MVC one, the most fitting design pattern on Xamarin is the MVVM (Model-View-ViewModel), accomplished by the separation between XAML (View) and the data model. Views do not know (almost) nothing about each other, especially what is going on in the ViewModel, which contains the workflow of the app and connects the UI with the business logic itself through data binding, hence achieving the very last goal of MVVM, which is, indeed, the separation of concerns (SoC).

As example, datas (on the server) are retrieved from Northwind by establishing a connection to SQL Server. The app then treats them with (de)serializing JSON arrays, after having queried authorized APIs. If the Internet connection is missing, then it attempts to store datas in the local SQLite database.

#### Screenshots
Code is kinda clean. Gonna attach some screenshots so you can have a whole idea of it.

<img src="https://www.dropbox.com/s/0m4hf87du2d8rvb/login_agency.png?raw=1" width="250px"/>
<img src="https://www.dropbox.com/s/jt1fwtmi86vstls/list_agency.png?raw=1" width="250px"/>
<img src="https://www.dropbox.com/s/qby8dztwqfh8qo7/update_agency.png?raw=1" width="250px"/>

#### License
You may wish to reuse, adapt, modify and whatever.
