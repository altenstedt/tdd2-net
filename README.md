What is this?
-------------

This is the source for the TDD 2 course for C#.  You will need .NET
Core 2 installed to build the sources.

* https://www.microsoft.com/net/core

Step into the Source folder and run the following command from a
terminal to run all the tests:

    dotnet test

MongoDB
-------

Add NuGet package `MongoDB.Driver`.

Products service
----------------

This solution depends on an HTTP resource that will return a product
list as a JSON structure.  You can setup Azure Functions to do this:

```json
{
{
    "$schema": "http://json.schemastore.org/proxies",
    "proxies": {
        "Products": {
            "matchCondition": {
                "route": "/products",
                "methods": [
                    "GET"
                ]
            },
            "responseOverrides": {
                "response.statusCode": "200",
                "response.body": "[{ \"id\": 1, \"name\": \"Apple\", \"cost\": { \"units\": 1376, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}, { \"id\": 2, \"name\": \"Banana\", \"cost\": { \"units\": 4455, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}]",
                "response.headers.Content-Type": "application/json"
            }
        },
        "ProductById": {
            "matchCondition": {
                "route": "/products/{id}",
                "methods": [
                    "GET"
                ]
            },
            "responseOverrides": {
                "response.statusCode": "200",
                "response.body": "{ \"id\": 1, \"name\": \"Apple\", \"cost\": { \"units\": 1376, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}",
                "response.headers.Content-Type": "application/json"
            }
        }
    }
}
```

See the documentation on Azure Functions for more information:

* https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-serverless-api