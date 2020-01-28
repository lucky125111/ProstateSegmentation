# Prostate segmentation app
Implementation of the application supporting cancer diagnostics prostate using the DICOM standard

![https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Editing/16.png](https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Editing/16.png)

## Architecture
Application has microservice architecture (runs on docker), the main components are:
* Presentation layer - which comunicates with PACS system
* REST API server
* Volume calculation service
* Image segmentation service
* SQL database
![https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/Architektura.png](https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/Architektura.png)

## Features
## Loading patients
![https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Main/15.png](https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Main/15.png)

## Calculating prostate volume
![https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Patient/343.png](https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Patient/343.png)
### Editing mask calculated by AI
![https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Editing/117.png](https://raw.githubusercontent.com/lucky125111/InzynierkaDocs/master/FrontScreen/Editing/117.png)

## Contributors
* [Łukasz Berwid](https://www.linkedin.com/in/lukasz-berwid/)
* [Rafał Buzun](https://www.linkedin.com/in/rafa%C5%82-buzun/)
