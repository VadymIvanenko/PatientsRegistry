`git clone https://github.com/VadymIvanenko/PatientsRegistry.git`  
`cd PatientsRegistry`  
`docker-compose pull`  
`docker-compose up`  

UI:
http://localhost:5500
- На UI реализовано Добавление/Удаление/Поиск
- Остальное только в API (Обновление пациента, Добавление/Удаление контактов)
- Поиск состоит из ФИО, телефона, даты рождения
- Поиск по ФИО реализован через MultiMatch по полям Name, Lastname, Patronymic c TextQueryType - CrossFields и Operator - And. Части ФИО должны быть полные, порядок не важен.
- Поиск по телефону и дате рождения - частичное вхождение (Prefix)


Back-end:
http://localhost:5000
Структура решения:
PatientsRegistry project:
- PatientsRegistry.Domain - models
- PatientsRegistry.Domain.Repositories - repository contract
- PatientsRegistry.Search - search service contract
- PatientsRegistry.Registry - registry service contract (aggregates search service and repo)  
PatientsRegistry.Search project - ElasticSearch implementation of PatientsRegistry.Search contract  
PatientsRegistry.Domain.Repositories project - MongoDB implementation of PatientsRegistry.Domain.Repositories contract  
PatientsRegistry.Registry project - implementation of PatientsRegistry.Registry contract  
PatientsRegistry.API project - controllers, entry-point  
PatientsRegistry.Web - quick&dirty UI client on React

API:  
`GET http://localhost:5000/api/patients?name=&phone=&birthdate=`

`GET http://localhost:5000/api/patients/{id}`

`POST http://localhost:5000/api/patients
{
    "name": "",
    "lastname": "",
    "patronymic": "",
    "birthdate": "1999-01-01",
    "gender": "Male", // Male, Female, Other
    "phone": "+380681152244"
}`

`PUT http://localhost:5000/api/patients/{id}
{
    "id": "",
    "name": "",
    "lastname": "",
    "patronymic": "",
    "birthdate": "1990-01-01",
    "gender": "Male"
}`

`DELETE http://localhost:5000/api/patients/{id}`

`PUT http://localhost:5000/api/patients/{id}/contacts
{
	"type": "Phone", // Phone, Other
	"kind": "Work", // Work, Main, Emergency
	"value": "+380441112255"
}`

`DELETE http://localhost:5000/api/patients/{id}/contacts?type=Phone&kind=Work`
