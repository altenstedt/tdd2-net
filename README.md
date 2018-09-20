What is this?
-------------

This is the source for the C# variant of the course in Test Driven
Development, Part 2.  You will need .NET Core installed to build and
run the sources.

Tests are divided into three different categories:

  1. Level 0 (L0) - tests without dependencies (unit tests)
  2. Level 1 (L1) - tests that include the database only
  3. Level 2 (L2) - tests that runs against the hosted API

# Building

Run the `build` command from the CLI:

```shell
dotnet build
```

or simply use your IDE to build normally.

# Running the REST API

The REST API can be started by using the following command:

```
dotnet run --urls=http://localhost:5000 --project ./Source/Commerce.Host
```

or use your IDE to start the project `Commerce.Host`.

The application needs to be configured with settings for the MongoDB
instance.  This is done using standard practice for ASP.NET
[configuration][1].  In order, settings are read from:

  1. File `appsettings.json`
  2. File `appsettings.<Environment>.json`
  3. Environment variables

where `Environment` is set to the value of the environment variable
`ASPNETCORE_ENVIRONMENT`.  See the [documentation][2] for details.

So in order to specify the MongoDB settings, you would typically
create a new file `appsettings.<Environment>.json` and just override
the values that needs to be different from the existing
`appsettings.json` file.

# Swagger documentation

The REST API is documented using Swagger/OpenAPI.  Once you have
started the REST API, you will find the documentation at path
`/swagger/index.html`.  So if you are running on port 5000, you can
point your browser to http://localhost:5000/swagger/index.html.

# Running tests

All tests are categorized according to the test level described above.
The categories are "L0", "L1" and "L2".  You can use the test runner
to filter tests if you want to run a subset of the tests only.

## Level 0 tests

If you want to run the unit tests only, you can do so from a command
prompt:

```
dotnet test --filter Category=L0
```

* https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests

## Level 1 tests

When running the database integration tests, you need to specify the
credentials for the MongoDB database first.  You can do this in a
number of different ways:

  1. File `testsettings.json`
  2. File `testsettings.<Environment>.json`
  3. Environment variables

Once you have created settings for your MongoDB instance, you can run
the level 1 tests only from the command prompt:

```
dotnet test --filter Category=L1 .\Source\Tests\Test.Level1.Commerce\
```

## Level 2 tests

The full system tests require a running service, so you need to start
the service first:

```
dotnet run --urls=http://localhost:5000 --project ./Source/Commerce.Host --configuration Release
```

Notice that we select the `Release` configuration in the example
above, so that the assemblies that we are running are not interfering
with building the tests.

Then you can run the tests:

```
dotnet test --filter Category=L2 .\Source\Tests\Test.Level2.Commerce\
```

Note that you also need to update the application settings with values
for your MongoDB instance and product service as described above.

Sadly, there is no standard way to run all the tests from the CLI for
.NET Core 2.1, and get a summarized result.  There are various
workarounds, but as it stands at the time of this writing, you are
probably better off using an IDE to run your tests.

* https://github.com/Microsoft/vstest/issues/1129
