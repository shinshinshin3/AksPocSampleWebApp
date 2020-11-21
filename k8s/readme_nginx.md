# nginx ingress controller を利用したsample app 配備

## ImagePush
### ACRへのログイン

~~~
# ACRname=funcregistry
# az acr login --name $ACRname
# az acr repository list --name $ACRname --output table
~~~

### ACRとk8sクラスタの紐づけ

~~~
# az aks update -n func-aks -g aks-func-poc --attach-acr  funcregistry
~~~

### ImageのPush

~~~
# docker-compose build

# docker tag akspocsamplewebapp_web funcregistry.azurecr.io/poc_web:latest
# docker tag pocstubappo1 funcregistry.azurecr.io/poc_app:latest

# docker push funcregistry.azurecr.io/poc_web:latest
# docker push funcregistry.azurecr.io/poc_app:latest
~~~

---
## k8s セットアップ

### namespaceの作成

~~~
# kubectl create namespace poc-application-nginxgw
~~~

### nginx ingress controller のインストール
https://github.com/kubernetes/ingress-nginx/blob/master/docs/deploy/index.md#azure

~~~
# kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.40.2/deploy/static/provider/cloud/deploy.yaml
# kubectl get pods -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx --watch
NAME                                        READY   STATUS      RESTARTS   AGE
ingress-nginx-admission-create-zg9jc        0/1     Completed   0          61m
ingress-nginx-admission-patch-7xhmp         0/1     Completed   0          61m
ingress-nginx-controller-7778487745-6cjrx   1/1     Running     0          61m  ★Runningになってる
~~~



### secretsの作成

~~~
# kubectl -n poc-application-nginxgw create secret generic aks-poc-secret \
        --from-literal=dbConnectionString='your db connection string' \
        --from-literal=ApplicationInsights_InstrumentationKey=<your application insights key>

secrets(作り直すとき)
# kubectl -n poc-application-nginxgw delete secret aks-poc-secret
~~~

### nginx/appの作成

~~~
# kubectl apply -n poc-application-nginxgw -f nginxgw/aks-poc-sample-app-deployment.yaml
# kubectl apply -n poc-application-nginxgw -f nginxgw/aks-poc-sample-nginx-deployment.yaml
# kubectl apply -n poc-application-nginxgw -f nginxgw/aks-poc-sample-ingress.yaml
~~~

### 外部IPの確認

~~~
# kubectl --namespace ingress-nginx get services -o wide
NAME                                 TYPE           CLUSTER-IP    EXTERNAL-IP      PORT(S)                      AGE   SELECTOR
ingress-nginx-controller             LoadBalancer   10.1.200.62   20.194.xxx.xxx   80:32678/TCP,443:32411/TCP   84m   app.kubernetes.io/component=controller,app.kubernetes.io/instance=ingress-nginx,app.kubernetes.io/name=ingress-nginx
ingress-nginx-controller-admission   ClusterIP      10.1.200.32   <none>           443/TCP                      84m   app.kubernetes.io/component=controller,app.kubernetes.io/instance=ingress-nginx,app.kubernetes.io/name=ingress-nginx
~~~