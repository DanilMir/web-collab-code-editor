# Сollab Сode Editor

## API

### Управление проектами

- **GET /projects**
  - Получение всех проектов пользователя
  - *Параметры запроса:*
    - `offset` (необязательный) - смещение результатов запроса
    - `limit` (необязательный) - ограничение количества возвращаемых проектов
   

- **GET /projects/{id}**
  - Получение информации о конкретном проекте по его идентификатору

- **POST /projects**
  - Создание нового проекта

- **PUT /projects/{id}**
  - Обновление информации о проекте по его идентификатору

- **DELETE /projects/{id}**
  - Удаление проекта по его идентификатору


### Доступы

- **GET /projects/{projectId}/accesses**
  - Получение всех разрешений доступа проекта
  - *Параметры запроса:*
    - `offset` (необязательный) - смещение результатов запроса
    - `limit` (необязательный) - ограничение количества возвращаемых проектов

- **GET /projects/{projectId}/accesses/{accessId}**
  - Получение информации о конкретном доступе по его идентификатору

- **POST /projects/{projectId}/accesses**
  - Создание нового доступа

- **PUT /projects/{projectId}/accesses/{accessId}**
  - Обновление информации о доступе по его идентификатору

- **DELETE /projects/{projectId}/accesses/{accessId}**
  - Удаление доступа по его идентификатору


### Файлы


1. **Загрузка файла в бакет:**
   - **POST /buckets/{bucketName}/files**
     - Загружает новый файл в указанный бакет S3.
     - *Параметры запроса:*
       - `prefix` (необязательный) - префикс для фильтрации файлов
       - `file` - файл для загрузки

2. **Получение списка файлов в бакете:**
   - **GET /buckets/{bucketName}/files**
     - Возвращает список всех файлов в указанном бакете S3.
     - *Параметры запроса:*
       - `prefix` (необязательный) - префикс для фильтрации файлов
       - `limit` (необязательный) - ограничение количества возвращаемых файлов
       - `sort` (необязательный) - сортировка файлов

3. **Получение информации о файле в бакете:**
   - **GET /buckets/{bucketName}/files/{fileId}**
     - Возвращает файл конкретного файла в указанном бакете S3 по его идентификатору.

4. **Удаление файла из бакета:**
   - **DELETE /buckets/{bucketName}/files/{fileId}**
     - Удаляет конкретный файл из указанного бакета S3 по его идентификатору.