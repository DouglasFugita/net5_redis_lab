# Redis Lab

## Docker
```
docker run --name redis-cache -p 6379:6379 -d redis
```
## Redis GUI
Para visualizacao dos dados no Redis foi utilizada a ferramenta RedisInsight, disponivel em https://redislabs.com/

## Basic Caching Demo
Baseado no artigo "Distributed Caching in ASP.NET Core with Redis" disponivel em https://sahansera.dev/distributed-caching-aspnet-core-redis/


First Terminal:
```
dotnet run --project basic_demo/Receive/
```

## Resources:
- https://redis.io/
- https://reqres.in
- https://redislabs.com/