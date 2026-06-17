# 🎭 НГК Афиша — Backend

**НГК Афиша (Backend)** — серверная часть приложения для организации и просмотра мероприятий в НГК.  
Этот репозиторий содержит **только backend и инфраструктуру**; клиентская часть вынесена в отдельный репозиторий.

Архитектура построена на наборе изолированных **микросервисов** на **ASP.NET Core (.NET 9)**, которые разворачиваются через **Docker / Docker Compose**.

> **⚠️ Прод-сервер временно не поддерживается.** Автодеплой через GitHub Actions отключён, продакшн-окружение недоступно. Разработка и локальный запуск через Docker Compose работают как обычно.

---

## 📁 Структура репозитория

```text
.
├── documentation              # Проектная и техническая документация
│   ├── Архитектурные принципы.docx
│   ├── Документация к EventService.rtf
│   ├── Документация к IdentityService.docx
│   └── Тестирование.docx
├── services
│   ├── .env.example           # Пример файла окружения для docker-compose
│   ├── compose                # Docker Compose-файлы для запуска сервисов
│   │   ├── all.yml            # Общее окружение (все сервисы)
│   │   ├── eventservice.yml
│   │   └── identityservice.yml
│   ├── docker                 # Dockerfile'ы и инфраструктура
│   │   ├── EventService.Dockerfile
│   │   ├── IdentityService.Dockerfile
│   │   └── minio
│   │       ├── certs
│   │       ├── commands
│   │       └── docker-compose.yml
│   └── src                    # Исходный код сервисов
│       ├── EventService
│       │   ├── EventService.API
│       │   ├── EventService.Application
│       │   ├── EventService.Domain
│       │   ├── EventService.Infrastructure
│       │   ├── EventService.UnitTests
│       │   └── EventService.sln
│       └── IdentityService
│           ├── IdentityService.API
│           ├── IdentityService.Application
│           ├── IdentityService.Domain
│           ├── IdentityService.Infrastructure
│           ├── IdentityService.UnitTests
│           └── IdentityService.sln
└── README.md
```

---

## 🧩 Архитектура и сервисы

### Микросервисы

| Сервис              | Назначение |
|---------------------|-----------|
| **Identity Service** | Регистрация, авторизация, управление пользователями. В перспективе возможное разделение на Auth Service и User Service. |
| **Event Service**    | Управление событиями, участниками, приглашениями и связанными файлами (афиши, вложения и т.п.). |
| **MinIO**            | S3-совместимое хранилище файлов. |
| **PostgreSQL**       | Основная база данных для сервисов. |

### Слойная структура сервисов

Каждый сервис реализован по схеме:

- `*.API` — HTTP API (контроллеры, конфигурация хостинга)
- `*.Application` — бизнес-логика приложения (use-cases, handlers)
- `*.Domain` — доменная модель, сущности, value objects
- `*.Infrastructure` — доступ к БД, интеграции, реализация репозиториев
- `*.UnitTests` — модульные тесты

### Диаграмма архитектуры (прод-окружение)

Ниже представлена схема взаимодействия сервисов в продакшене и маршрутизация запросов через nginx reverse proxy:

```mermaid
graph TB
    subgraph "Внешние клиенты"
        Client[Клиентские приложения<br/>Web / Mobile]
        Admin[Администраторы]
    end

    subgraph "Nginx Reverse Proxy"
        Nginx[Nginx<br/>Маршрутизация по доменам]
    end

    subgraph "Docker Network (прод-окружение)"
        subgraph "Identity Service"
            IdentityAPI[Identity Service API<br/>:8080]
            IdentityDB[(PostgreSQL<br/>Identity DB)]
        end

        subgraph "Event Service"
            EventAPI[Event Service API<br/>:8080]
            EventDB[(PostgreSQL<br/>Event DB)]
        end

        subgraph "MinIO (S3)"
            MinIOAPI[MinIO API<br/>:9000]
            MinIOConsole[MinIO Console<br/>:9001]
            MinIOStorage[(S3 Storage<br/>Bucket: ngkafisha)]
        end
    end

    %% Внешние запросы через nginx
    Client -->|identity.ngkapi.ru| Nginx
    Client -->|events.ngkapi.ru| Nginx
    Admin -->|console.storage.ngkapi.ru| Nginx
    Admin -->|storage.ngkapi.ru/login| Nginx

    %% Маршрутизация nginx
    Nginx -->|/ → Identity Service| IdentityAPI
    Nginx -->|/ → Event Service| EventAPI
    Nginx -->|/ → MinIO Console| MinIOConsole
    Nginx -->|/ → MinIO API| MinIOAPI

    %% Взаимодействие сервисов
    IdentityAPI <-->|Чтение/запись| IdentityDB
    EventAPI <-->|Чтение/запись| EventDB
    EventAPI -->|Валидация JWT<br/>публичный ключ| IdentityAPI
    EventAPI <-->|Загрузка/скачивание файлов| MinIOAPI
    MinIOAPI <-->|Хранение| MinIOStorage

    %% Стили
    classDef external fill:#e1f5ff,stroke:#01579b,stroke-width:2px
    classDef proxy fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef service fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef database fill:#e8f5e9,stroke:#1b5e20,stroke-width:2px
    classDef storage fill:#fff9c4,stroke:#f57f17,stroke-width:2px

    class Client,Admin external
    class Nginx proxy
    class IdentityAPI,EventAPI,MinIOAPI,MinIOConsole service
    class IdentityDB,EventDB database
    class MinIOStorage storage
```

**Описание взаимодействия:**

1. **Внешний доступ:**
   - Все запросы извне проходят через **nginx reverse proxy**, который маршрутизирует по доменам:
     - `identity.ngkapi.ru` → Identity Service
     - `events.ngkapi.ru` → Event Service
     - `console.storage.ngkapi.ru` → MinIO Console
     - `storage.ngkapi.ru` → MinIO API

2. **Взаимодействие сервисов:**
   - **Identity Service** управляет пользователями и выдаёт JWT-токены
   - **Event Service** валидирует JWT-токены через публичный ключ Identity Service (без прямых HTTP-вызовов)
   - **Event Service** использует MinIO для хранения файлов (афиши, вложения). При этом он отвечает лишь за генерацию preSigned url. Клиент сам загружает и скачивает файл, поток байтов не проходит через EventService. EventService отвечает только за удаление файла (когда удаляется событие)
   - Каждый сервис имеет свою изолированную PostgreSQL БД

3. **Внутренняя сеть:**
   - Все сервисы находятся в одной Docker-сети и обращаются друг к другу по внутренним именам (например, `minio:9000`)

---

## ✅ Требования

Для запуска через Docker:

- **Docker** 24+ (или совместимая версия)
- **Docker Compose** (v2, включён в современные Docker Desktop / docker-cli)
- Открытые порты:
  - `9000`, `9001` — MinIO
  - порты API-сервисов (см. соответствующие `docker-compose`/`all.yml`)

Для локальной разработки без Docker (опционально):

- **.NET SDK 9.0** (для проектов `*.API`, `*.Application`, `*.Infrastructure`, `*.Domain`, `*.UnitTests`)
- PostgreSQL и MinIO, поднятые отдельно или через Docker

---

## 🚀 Быстрый старт (Docker Compose)

Все основные сервисы разворачиваются через `docker compose` из папки `services`.

Перед первым запуском:

1. **Создайте `.env` из примера**:

   ```bash
   cd services
   cp .env.example .env
   ```

2. При необходимости измените пароли, ключи и e-mail администратора в `.env`.  
   **Важно:** значения из `.env.example` нельзя использовать в продакшене как есть.

---

### 🔹 Запуск всего окружения

Из корня репозитория или из папки `services`:

```bash
docker compose -f services/compose/all.yml up --build
```

Это поднимет:

- PostgreSQL (БД Identity и Event)
- MinIO
- Identity Service
- Event Service

Остановка:

```bash
docker compose -f services/compose/all.yml down
```

(При необходимости можно добавить `-v` для удаления томов.)

---

### 🔹 MinIO (S3-хранилище)

Папка с инфраструктурой MinIO:

```text
services/docker/minio
```

Локальный запуск только MinIO:

```bash
cd services/docker/minio
docker compose up --build
```

После старта:

- **MinIO API:** `http://localhost:9000`
- **MinIO Console:** `http://localhost:9001`

Bucket для проекта создаётся автоматически:

```text
ngkafisha
```

---

### 🔹 Отдельный запуск сервисов

**Identity Service:**

```bash
docker compose -f services/compose/identityservice.yml up --build
```

**Event Service:**

```bash
docker compose -f services/compose/eventservice.yml up --build
```

---

## ⚙️ Конфигурация и переменные окружения

Основной пример конфигурации находится в файле:

```text
services/.env.example
```

Рекомендуемый порядок действий:

```bash
cd services
cp .env.example .env
# затем отредактировать .env под свою среду
```

### Пример ключевых переменных

**Базы данных:**

```env
IDENTITY_POSTGRES_DB=IdentityService
IDENTITY_POSTGRES_USER=postgres
IDENTITY_POSTGRES_PASSWORD=identity_postgres_password

EVENT_POSTGRES_DB=EventService
EVENT_POSTGRES_USER=postgres
EVENT_POSTGRES_PASSWORD=event_postgres_password
```

**JWT для Identity / Event:**

```env
IDENTITY_JWT_PRIVATE_KEY="-----BEGIN PRIVATE KEY----- ... -----END PRIVATE KEY-----"
IDENTITY_JWT_PUBLIC_KEY="-----BEGIN PUBLIC KEY----- ... -----END PUBLIC KEY-----"
IDENTITY_JWT_ISSUER=IdentityService
IDENTITY_JWT_EXPIRES=120

# EventService использует те же ключи и настройки:
EVENT_JWT_PRIVATE_KEY=${IDENTITY_JWT_PRIVATE_KEY}
EVENT_JWT_PUBLIC_KEY=${IDENTITY_JWT_PUBLIC_KEY}
EVENT_JWT_ISSUER=${IDENTITY_JWT_ISSUER}
EVENT_JWT_EXPIRES=${IDENTITY_JWT_EXPIRES}
```

**S3-хранилище для EventService:**

```env
EVENT_S3_BUCKET_NAME=ngkafisha
EVENT_S3_ACCESS_KEY=minio
EVENT_S3_SECRET_KEY=minio123
EVENT_S3_SERVICE_URL=http://localhost:9000        # адрес MinIO с точки зрения хоста
EVENT_S3_SERVICE_URL_BOUNDED=http://minio:9000    # адрес MinIO внутри docker-сети
```

**Администраторы по умолчанию:**

```env
IDENTITY_ADMIN_EMAIL=admin@mail.ru
IDENTITY_ADMIN_PASSWORD=ZSE$5rdx

EVENT_ADMIN_EMAIL=admin@mail.ru
EVENT_ADMIN_PASSWORD=ZSE$5rdx
```

⚠️ **Важно:** все секреты в `.env.example` — исключительно для примеров.  
В продакшене их необходимо **обязательно заменить** на безопасные значения.

---

## 🛠️ Технологии

### Backend

- **ASP.NET Core 9 / .NET 9** (`net9.0`)
- **Entity Framework Core 9**
- **PostgreSQL**
- **JWT (RSA)**
- **MinIO (S3)**
- **AWS SDK for S3** (`AWSSDK.S3`)
- **Docker / Docker Compose**

### Тестирование

- Unit-тесты для основных сервисов:
  - `EventService.UnitTests`
  - `IdentityService.UnitTests`

Пример запуска тестов:

```bash
cd services/src/EventService/EventService.UnitTests
dotnet test

cd ../../IdentityService/IdentityService.UnitTests
dotnet test
```

---

## 🚢 Деплой

> **Статус:** прод-сервер временно не поддерживается — автодеплой **не выполняется**.

В репозитории настроены GitHub Actions-workflow’ы в `.github/workflows`:

- `deploy.yml` — деплой на прод-сервер по SSH с запуском `docker compose` из `services/compose/all.yml` (**сейчас отключён**).
- `dokcer-build.yml` — сборка Docker-образов и healthcheck на pull request.
- `dotnet-desktop.yml` — сборка и unit-тесты на push и pull request.

Когда прод-сервер снова будет доступен, деплой рассчитан на окружение, где:

- на сервере уже клонирован репозиторий;
- установлены `docker` и `docker compose`;
- настроены секреты GitHub (SSH-ключ, хост, пользователь и т.д.).

---

## 🌐 Прод-окружение (актуальные хосты)

> **Статус:** прод-окружение **временно недоступно** — сервер не поддерживается, сервисы по адресам ниже не работают.

Ранее продакшн-версии сервисов были доступны по следующим адресам:

- **Identity Service (Swagger UI):** [identity.ngkapi.ru](https://identity.ngkapi.ru/)
- **Event Service (Swagger UI):** [events.ngkapi.ru](https://events.ngkapi.ru/)
- **MinIO Console:** [console.storage.ngkapi.ru](https://console.storage.ngkapi.ru/)
- **S3-хранилище (авторизация / работа с файлами):** [storage.ngkapi.ru/login](https://storage.ngkapi.ru/login)

Эти адреса могут меняться при переносе инфраструктуры, поэтому для актуальной информации стоит сверяться с документацией и CI/CD-конфигурацией.

---

## 📘 Документация

Подробная документация находится в папке:

```text
documentation/
```

Содержит:

- архитектурные принципы;
- описание сервисов (Identity и Event);
- материалы по тестированию.

---

## 🤝 Участие в разработке

1. Форкните репозиторий или создайте новую ветку.
2. Настройте окружение через `.env` на основе `.env.example`.
3. Запустите сервисы (Docker или локально).
4. Перед созданием PR:
   - убедитесь, что тесты проходят (`dotnet test`);
   - проверьте, что Docker-сборка не ломается.

При необходимости можно добавить отдельный раздел с руководством по контрибьютингу (`CONTRIBUTING.md`).
