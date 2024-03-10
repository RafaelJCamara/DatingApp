# DatingApp
Base work developed as part of this [course](https://www.udemy.com/course/build-an-app-with-aspnet-core-and-angular-from-scratch/).

### Create local certificates

The local certificates will help us run our Angular client app with https.

On the client folder root, create an ssl folder, place yourself there and run the following commands:

- mkcert -install
- mkcert localhost

Then we need to add this to your angular.json file, under serve:
```json
"options": {
            "ssl": true,
            "sslKey": "./ssl/localhost-key.pem",
            "sslCert": "./ssl/localhost.pem"
          }
```
