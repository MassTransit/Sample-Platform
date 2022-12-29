
# MassTransit Platform
The goal is to migrate Boilerplat configration into a dedicated platform. 
in doing so, we ensure common configration and company requirements are focused to a small foot print. 

## Docker
To build and run 

```
docker-compose build --no-cache
docker-compose up
```

## K8S 
Use Skaffold to build and deploy. The ##repo## should be your development repo where images are stored and are avilible to your K8S instance.

NOTE: You must already have a K8S cluster/instance availible. For local development you may wish to look into Minikube.

### Check
Make sure we have everything required

```shell
minikube install 
minikube run 
kubectl get all
docker ps
```

### Skaffold
Use Skaffold to build and locally test
```shell
skaffold build --default-repo ##repo##
skaffold dev --default-repo ##repo##
```
