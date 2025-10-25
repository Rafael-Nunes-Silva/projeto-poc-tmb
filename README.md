# projeto-poc-tmb

Projeto completo desafio POC TMB

```
projeto-poc-tmb/
├── backend/
│   ├── api-poc-tmb/...
│   ├── worker-orders/...
│   └── docker-compose.yml
├── frontend/
│   ├── public/...
│   ├── src/...
│   ├── package.json
│   └── tailwind.config.js
├── README.md
└── .gitignore
```

# Instruções para execução

## Pré-requisitos

* Git
* Docker & Docker Compose (Docker Desktop com WSL2 recomendado no Windows)
* (Opcional, para rodar sem Docker) .NET 8 SDK e Node.js 18+ / npm

### Estrutura (esperada)
```
/backend
  /api-poc-tmb        -> projeto API (.NET)
  /worker-orders      -> projeto Worker (.NET)
frontend/             -> React + Vite app
docker-compose.yml
```

## Variáveis de ambiente / configuração

### Frontend
Coloque frontend/.env com:
```
VITE_BASE_API_URL='URL do backend'
```

### Backend

Configurações (connection strings, service bus, etc.) estão em backend/api-poc-tmb/appsettings.json para desenvolvimento.

## Execução com Docker Compose

* Na pasta frontend/ crie/configure o .env
* um .env do Docker com a connection string do DB / Service Bus.

### Execute:
```
docker-compose up --build
```

### Acesse:

API: http://localhost:< porta-do-backend >

Frontend: http://localhost:< porta-do-frontend >
