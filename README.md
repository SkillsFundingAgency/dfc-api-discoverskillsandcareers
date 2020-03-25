# dfc-api-discoverskillsandcareers

## Introduction

This project provides an API for Discover Your Skills and Careers and Match Skills, currently providing support for creating a new assessment

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (web application, service and repository layers, with associated unit test and integration test projects).

### Installing

Clone the project and open the solution in Visual Studio 2019.

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Azure Cosmos DB | Document storage |
|Session Client | Session storage and management |

## Local Config Files

Once you have cloned the public repo you need to rename the local.settings files by removing the -template part from the configuration file names listed below.

| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.Api.DiscoverSkillsAndCareers | local.settings-template.json | local.settings.json |
| DFC.Api.DiscoverSkillsAndCareers.IntegrationTests | appsettings-template.json | appsettings.json |

## Configuring to run locally

The project contains a number of "local.settings-template.json" files which contain sample local.settings for the web app and the integration test projects. To use these files, rename them to "local.settings.json" and edit and replace the configuration item values with values suitable for your environment.

By default, the local.settings include a local Azure Cosmos Emulator configuration using the well known configuration values. These may be changed to suit your environment if you are not using the Azure Cosmos Emulator. 

## Running locally

To run this product locally, you will need to configure the list of dependencies. Once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS.

To run the project, start the JobProfileFunction Azure function app. On your local environment, swagger documentation is available at http://localhost:7071/swagger/ui

The API function app has 3 endpoints:
- GET GetUserSession - /assessment/session/{sessionId}
- POST CreateNewShortAssessment - /assessment/short
- POST CreateNewSkillsAssessment - /assessment/skills

## Deployments

This Discover Your Skills and Careers Api function app will be deployed as an individual deployment for consumption by Match Skills and Discover Your Skills and Careers apps running under Composite UI.

## Assets

CSS, JS, images and fonts used in this site can found in the following repository https://github.com/SkillsFundingAgency/dfc-digital-assets

## Built With

* Microsoft Visual Studio 2019
* .Net Core 3.1

## References

Please refer to https://github.com/SkillsFundingAgency/dfc-digital for additional instructions on configuring individual components like Sitefinity and Cosmos.