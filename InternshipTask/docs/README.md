# Крестики нолики

## Ключевые особенности
Основные архитектурные решения

- Onion-архитектура с четким разделением слоев:
    - Domain - доменные сущности
    - Application - игровая логика
    - Infrastructure - работа с БД и внешними сервисами
    - API - HTTP-контроллеры и DTO

- Хранение данных
    - PostgreSQL как основное хранилище состояния игр
    - Dapper для высокопроизводительных запросов
    - FluentMigrator для управления миграциями БД
- Надежность
    - ExceptionMiddleware для централизованной обработки ошибок
    - ETag-механизм обеспечивает идемпотентность операций
    - Восстановление состояния после перезапуска сервиса
    - <details>
      <summary>Негативные тесты</summary>
      
      ### HTTP/1.1 400 Bad Request
      
        ```
        {
            "type": "https://http.dog/400",
            "title": "Validation error",
            "status": 400,
            "detail": "One or more validation errors occurred.",
            "instance": "/api/games/create",
            "errors": {
                "dto": [
                "The dto field is required."
                ],
                "$.playerIdO": [
                "The JSON value could not be converted to System.Guid. Path: $.playerIdO | LineNumber: 2 | BytePositionInLine: 22."
                ]
            }
        }
        ```
    </details>
    
- Возможность конфигурации
    - Размер игрового поля
    - Условия победы
    - Вероятность смены знака
- [Тестирование 80%+](pics/Снимок%20экрана%202025-07-11%20040624.png)
    - xUnit 
    - FluentAssertions
    - Moq

## Запуск
1. Перейдите в директорию проекта
```bash
cd InternshipTask
```
2. Запуск сервиса
```bash
docker-compose up
```
3. Проверить работоспособность сервиса: `http://localhost:8080/health`

## Документация к API

- Swagger: `http://localhost:8080/swagger/index.html`
- [Json](swagger.json)

### Контакты

- Telegram [@yrulewet](https://t.me/yrulewet)