az login --username <user> --password <password>
az group create --name <resource_group_name> --location eastus
az acr create --resource-group <resource_group_name> --name <registry_name> --sku Standard
az acr login --name <registry_name>
docker-compose up --build -d
docker-compose push
docker login azure --username <user> --password <password>
docker context create aci myacicontext
docker context use myacicontext
docker compose up