run:
	dotnet watch run --launch-profile https

cert:
	dotnet dev-certs https --trust

gen:
ifeq ($(model),)
	@echo "Usage: make gen model=<model_name>"
else
	dotnet aspnet-codegenerator controller -name $(model)Controller -async -api -m $(model) -dc ApiContext -outDir Controllers
endif

add:
ifeq ($(package),)
	@echo "Usage: make add package=<package_name>"
else
	dotnet add package $(package)
endif

remove:
ifeq ($(package),)
	@echo "Usage: make remove package=<package_name>"
else
	dotnet remove package $(package)
endif

migrate_db:
ifeq ($(migration),)
	@echo "Usage: make mirate_db migration=<migration_name>"
else
	dotnet ef migrations add $(migration) --no-build
endif

update_db:
	dotnet ef database update --no-build

init_db:
	dotnet ef migrations add InitialCreate