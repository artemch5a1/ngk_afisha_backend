# 🎭 НГК Афиша — Backend

**НГК Афиша (Backend)** — серверная часть приложения для организации и просмотра мероприятий в колледже НГК.  
Репозиторий содержит только backend и инфраструктуру, клиентская часть вынесена отдельно.

Проект реализован в виде набора изолированных микросервисов на **ASP.NET Core**, разворачиваемых через **Docker**.

---

## 📁 Структура репозитория

```
.
├── documentation          # Проектная и техническая документация
│   ├── Docker.docx
│   ├── Архитектурные принципы.docx
│   ├── Документация к EventService.rtf
│   ├── Документация к IdentityService.docx
│   └── Тестирование.docx
├── README.md
└── services
    ├── compose             # Docker Compose файлы для запуска сервисов
    │   ├── all.yml          # Общее окружение (все сервисы)
    │   ├── eventservice.yml
    │   └── identityservice.yml
    ├── docker              # Dockerfile'ы и инфраструктура
    │   ├── EventService.Dockerfile
    │   ├── IdentityService.Dockerfile
    │   └── minio
    │       ├── certs
    │       ├── commands
    │       └── docker-compose.yml
    └── src                 # Исходный код сервисов
        ├── EventService
        │   ├── EventService.API
        │   ├── EventService.Application
        │   ├── EventService.Domain
        │   ├── EventService.Infrastructure
        │   ├── EventService.UnitTests
        │   └── EventService.sln
        └── IdentityService
            ├── IdentityService.API
            ├── IdentityService.Application
            ├── IdentityService.Domain
            ├── IdentityService.Infrastructure
            ├── IdentityService.UnitTests
            └── IdentityService.sln
```

---

## 🚀 Backend-сервисы

| Сервис | Описание |
|------|---------|
| **Identity Service** | Регистрация, авторизация, работа с пользователями. В перспективе — разделение на Auth Service и User Service |
| **Event Service** | Управление событиями, участниками и приглашениями |
| **MinIO** | S3-совместимое хранилище файлов |
| **PostgreSQL** | Основная база данных сервисов |

---

## 🐳 Запуск через Docker

Все сервисы запускаются через **Docker Compose**.

### 🔹 Запуск всего окружения

```
docker compose -f services/compose/all.yml up --build
```

Запускает:
- PostgreSQL
- MinIO
- Identity Service
- Event Service

---

### 🔹 MinIO (S3-хранилище)

Папка:
```
services/docker/minio
```

Запуск:
```
docker compose up --build
```

После старта:
- **MinIO API:** http://localhost:9000
- **MinIO Console:** http://localhost:9001

Bucket создаётся автоматически:
```
ngkafisha
```

---

### 🔹 Identity Service

```
docker compose -f services/compose/identityservice.yml up --build
```

---

### 🔹 Event Service

```
docker compose -f services/compose/eventservice.yml up --build
```

---

## 📦 Хранилище файлов — MinIO

MinIO используется как полная замена Yandex Object Storage.

Пример переменных окружения:
```
S3Storage__ServiceUrl=http://minio:9000
S3Storage__AccessKey=minio
S3Storage__SecretKey=minio123
S3Storage__Bucket=ngkafisha
```

---

## 🛠️ Технологии

### Backend
- ASP.NET Core 9
- Entity Framework Core
- PostgreSQL
- JWT (RSA)
- MinIO (S3)
- AWS SDK for S3
- Docker / Docker Compose

---

## 📘 Документация

Документация проекта находится в папке:
```
documentation/
```

Содержит:
- архитектурные принципы
- описание сервисов
- материалы по тестированию
