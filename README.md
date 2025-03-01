# TalentFlow - HR Management API

## Описание

TalentFlow API - это серверная часть системы для управления подбором сотрудников в компанию. Система позволяет управлять вакансиями, отслеживать статус кандидатов и координировать работу HR-специалистов.
## Технологии
- **.NET 8**  
- **ASP.NET Core**
- **MS SQL Server**
- **Entity Framework Core** 
- **FluentValidation**
- **MediatR**
- **Swagger/OpenAPI**
- **Unit-тестирование (xUnit)**
## Архитектура

Проект построен с использованием чистой архитектуры и DDD (Domain-Driven Design). Проект разделен на следующие слои:
- `TalentFlow.API` - слой представления, содержащий контроллеры и конфигурацию API
- `TalentFlow.Application` - слой бизнес-логики
- `TalentFlow.Domain` - слой доменной модели
- `TalentFlow.Infrastructure` - слой инфраструктуры и доступа к данным
### Принципы и паттерны
- `Repository Pattern` -	Абстракция доступа к данным через интерфейсы в Domain слое
- `Envelope Pattern` - Унифицированный формат ответов API
- `CQRS` - Разделение запросов (Queries) и команд (Commands) в Application слое
- `Result Pattern` -	Обработка операций с возвратом Result<T>

## Основные сущности (Entities)

**1. Department (Отдел)**

- **Атрибуты**:    
  - `Name` (название отдела)        
  - `Description` (описание)        

**2. Candidate (Кандидат)**

- **Атрибуты**:    
  - `FullName` (ФИО) – Value Object        
  - `ContactInfo` (контакты) – Value Object     
  - `ResumeUrl` (ссылка на резюме)

**3. HrSpecialist (HR-специалист)**

- **Атрибуты**:    
  - `FullName` (ФИО) – Value Object   
  - `ContactInfo` (контакты) – Value Object          
  - `AssignedVacancies` (назначенные вакансии) - ReadOnlyCollection
        
- **Методы**:    
  - `AssignToVacancy()` – назначение вакансии (макс. 5)        
  - `RemoveVacancy()` – удаление вакансии

**4. Vacancy (Вакансия)**

- **Атрибуты**:    
  - `Title` (название),
  - `Description` (описание)        
  - `Status` (open/closed)        
  - `DepartmentId` (id департамента)
  - `HrSpecialistId` (id специалиста)
  - `OpeningDate` (дата открытия)   
  - `ClosingDate` (дата закрытия)
      
- **Методы**:    
  - `Close()` – закрытие вакансии при успешном испытательном сроке

**5. TestAssignment (Тестовое задание)**

- **Атрибуты**:    
  - `Description` (описание задания)
  - `AssignedDate` (назначенная дата)       
  - `SubmissionDeadline` (дедлайн)      
  - `Status` – статус выполнения (Pending, Submitted, Approved/Rejected)
        
- **Методы**:    
  - `Submit()` – отправка решения        
  - `Approve()` - подтвердить решения
  - `Reject()` - отклонить решение

**6. RecruitmentProcess (Процесс найма)**

- **Атрибуты**:    
    - `VacancyId`, `CandidateId`, `TestAssignment`        
    - `CurrentStage` – текущий этап (PhoneInterview,TechnicalInterview,TestAssignment,Probation)        
    - `Stages` (история этапов) - ReadOnlyCollection        
    - `ProbationPassed` – флаг прохождения испытательного срока
        
- **Методы**:    
    - `AddStage()` – добавление этапа (валидация дубликатов)        
    - `CompleteStage()` – завершение этапа с результатом (Passed/Rejected)
    - `AssignTestAssignment()` - добавление тестового задания

## Основные функции API
**Реализовано 4 ключевых метода для управления HR-процессами:**

- **1. Получение отделов с сортировкой**:
  
  *GET* `/api/departments`  
    Параметры:    
    - `SortBy` (name, description)        
    - `Order` (asc/desc)

- **2. Удаление вакансии у HR-специалиста**:

  *DELETE* `/api/hr-specialists/{id}/remove-vacancy`
  
	Тело запроса:
  ``` json   
  {
    "vacancyId": "uuid"
  }
  ```

- **3. Создание процесса найма**:

  *POST* `/api/recruitment-processes`
  
  Тело запроса:  
  ``` json
  {
    "vacancyId": "uuid",
    "candidateId": "uuid",
    "testAssignmentId": "uuid"
  }
  ```
  
- **4. Изменение статуса тестового задания**:
  
  *PUT* `/api/test-assignments/{id}/status`

  Тело запроса:

  ``` json
  {
    "status": "Approved",
    "SubmissionUrl": ""
  }
  ```
           
  
