re: down build start

rc: down clean build start

start:
	docker-compose up -d

build:
	docker-compose build

down:
	docker-compose down --remove-orphans

clean:
	if [ -n "$$(docker volume ls -q)" ]; then docker volume rm $$(docker volume ls -q); fi
	docker volume prune -f
	docker network prune -f
	docker image prune -f
	docker system prune -f

psql:
	docker exec -it pg_container psql -U ipi -W ipi_jva320_web

pg:
	docker exec -it pg_container sh

pgadmin4:
	docker exec -it pgadmin4_container sh

addr:
	docker inspect pg_container | grep IPv4Address
	docker inspect pgadmin4_container | grep IPv4Address*

back:
	docker-compose restart backend

init:
	docker exec prj-perso-server-1 bash -c "/app/script.sh"