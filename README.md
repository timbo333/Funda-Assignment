# Readme

## Introduction

This application was created to return the top 10 real estate agents, that are selling houses with a garden in Amsterdam.
The application uses a console app that calls a funda api and returns the top 10.
The output will look something like this:

![output](.img/output.png)

## Structure

The project is structured as follows

```
src
|_ Abstractions
   Has object that match the funda api returned json
|_ Api
   A console app which call the domain layer
|_ Domain
   The layer that is responsible for creating the real estate agents and return information as requested.
|_ Domain.Test.Unit
   Project responsible for testing the domain layer
|_ Infrastructure
   The layer that is responsible for talking to the funda API.
|_ Infrastructure.Test.Unit
   Project responsible for testing the infrastructure layer
```

## How to use

This project can be run by opening the project in visual studio and pressing F5. 
This will open a console app which will query the funda api for about 2 seconds.