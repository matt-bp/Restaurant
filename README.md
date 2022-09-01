# Restaurant

## Purpose

Find open restaurants by a specific date and time.

## Project Structure

The project `Website` contains the ASP.NET core web server. It includes a UI to check for open restaurants from a specified date and time.

The project `Lib` contains services and models used by the web server for parsing availabilities and finding open restaurants.

The project `Lib.UnitTests` contains unit tests for the `Lib` project to verify that parsing and other services work as intended.

## How to run

Please install [dotnet](https://dotnet.microsoft.com/en-us/). The `global.json` file should ensure you have the correct version installed.

To start the server, run the following from the project root (or by using your IDE of choice):

```shell
dotnet run --project .\Website\Website.csproj
```

To run the unit tests, run the following from the project root:

```shell
dotnet test
```
